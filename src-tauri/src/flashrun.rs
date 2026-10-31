use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;
use std::fs;
use std::io::{BufRead, BufReader, Read};
use std::process::{Child, Command, Stdio};
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::time::Duration;

/// One line of tool output, forwarded to the flashing panel.
#[derive(Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct FlashLine {
    pub line: String,
}

/// Phase and byte counters. `total` is `None` while fastboot is writing: the
/// tool never reports how many bytes it has pushed, only when it is done.
#[derive(Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct FlashPhase {
    pub phase: String,
    pub received: u64,
    pub total: Option<u64>,
}

pub struct FlashState {
    child: Mutex<Option<Arc<Mutex<Child>>>>,
    cancel: Arc<AtomicBool>,
}

impl Default for FlashState {
    fn default() -> Self {
        Self {
            child: Mutex::new(None),
            cancel: Arc::new(AtomicBool::new(false)),
        }
    }
}

impl FlashState {
    fn reset(&self) -> Arc<AtomicBool> {
        self.cancel.store(false, Ordering::SeqCst);
        self.cancel.clone()
    }

    fn attach(&self, child: Arc<Mutex<Child>>) {
        *self.child.lock().unwrap() = Some(child);
    }

    fn detach(&self) {
        *self.child.lock().unwrap() = None;
    }
}

/// Map a fastboot line to a phase. `Sending 'boot' (65536 KB)...` also carries
/// the transfer size, which is the only real number fastboot gives us.
pub fn classify(line: &str) -> Option<(&'static str, Option<u64>)> {
    if let Some(start) = line.find("Sending '") {
        let size = line[start..]
            .split('(')
            .nth(1)
            .and_then(|rest| rest.split_whitespace().next())
            .and_then(|kb| kb.parse::<u64>().ok())
            .map(|kb| kb * 1024);
        return Some(("sending", size));
    }
    if line.contains("Writing '") {
        return Some(("writing", None));
    }
    if line.starts_with("Finished.") {
        return Some(("done", None));
    }
    None
}

fn emit<T: Serialize + Clone>(app: &tauri::AppHandle, target: &str, event: &str, payload: T) {
    use tauri::Emitter;
    let _ = app.emit_to(target, event, payload);
}

fn pump_lines(
    app: tauri::AppHandle,
    target: String,
    reader: impl Read,
    track_phase: bool,
) {
    let mut reader = BufReader::new(reader);
    let mut line = String::new();
    let mut total: Option<u64> = None;

    loop {
        line.clear();
        match reader.read_line(&mut line) {
            Ok(0) => break,
            Ok(_) => {
                let text = line.trim_end().to_string();
                if track_phase {
                    if let Some((phase, size)) = classify(&text) {
                        if let Some(bytes) = size {
                            total = Some(bytes);
                        }
                        emit(
                            &app,
                            &target,
                            "flash:phase",
                            FlashPhase { phase: phase.into(), received: 0, total },
                        );
                    }
                }
                emit(&app, &target, "flash:output", FlashLine { line: text });
            }
            Err(_) => break,
        }
    }
}

/// `fastboot flash <partition> <image>` (or `fastboot boot <image>`), streamed
/// line by line. Fastboot has no byte counter, so the panel shows the transfer
/// size it announces and the phase that is running.
pub struct FlashRequest<'a> {
    pub target: &'a str,
    pub serial: &'a str,
    pub kind: &'a str,
    pub image: &'a str,
    pub partition: &'a str,
}

pub fn flash_step(
    app: tauri::AppHandle,
    state: &FlashState,
    paths: &AppPaths,
    request: FlashRequest<'_>,
) -> Result<i32, String> {
    let FlashRequest {
        target,
        serial,
        kind,
        image,
        partition,
    } = request;
    let program = tools::fastboot_path(paths)?;

    if !std::path::Path::new(image).is_file() {
        return Err("errors.imageMissing".into());
    }

    let mut cmd = Command::new(program);
    cmd.arg("-s").arg(serial);
    if kind == "boot" {
        cmd.arg("boot").arg(image);
    } else {
        cmd.arg("flash").arg(partition).arg(image);
    }
    cmd.stdin(Stdio::null())
        .stdout(Stdio::piped())
        .stderr(Stdio::piped());
    tools::hide_console(&mut cmd);

    let cancel = state.reset();
    let mut child = cmd.spawn().map_err(|e| e.to_string())?;
    let stdout = child.stdout.take().ok_or("no stdout")?;
    let stderr = child.stderr.take().ok_or("no stderr")?;
    let child = Arc::new(Mutex::new(child));
    state.attach(child.clone());

    let app_out = app.clone();
    let target_out = target.to_string();
    let out_thread = std::thread::spawn(move || pump_lines(app_out, target_out, stdout, true));

    let app_err = app.clone();
    let target_err = target.to_string();
    let err_thread = std::thread::spawn(move || {
        let reader = BufReader::new(stderr);
        for line in reader.lines().map_while(Result::ok) {
            emit(&app_err, &target_err, "flash:output", FlashLine { line });
        }
    });

    // Honour Cancel while the transfer runs.
    let wait_child = child.clone();
    let code = loop {
        if cancel.load(Ordering::SeqCst) {
            if let Ok(mut guard) = wait_child.lock() {
                let _ = guard.kill();
                let _ = guard.wait();
            }
            break -1;
        }
        let status = wait_child.lock().ok().and_then(|mut guard| guard.try_wait().ok().flatten());
        match status {
            Some(status) => break status.code().unwrap_or(-1),
            None => std::thread::sleep(Duration::from_millis(80)),
        }
    };

    let _ = out_thread.join();
    let _ = err_thread.join();
    state.detach();
    Ok(code)
}

/// `adb sideload <file>`. The host never learns how many bytes reached the
/// device (recovery owns the progress bar), so the panel reports the phases
/// the tool announces — sending, then installing — with the archive size.
pub fn sideload(
    app: tauri::AppHandle,
    state: &FlashState,
    paths: &AppPaths,
    target: &str,
    serial: &str,
    zip: &str,
) -> Result<i32, String> {
    let program = tools::adb_path(paths)?;
    let path = std::path::Path::new(zip);
    let size = fs::metadata(path).map_err(|e| e.to_string())?.len();
    if size == 0 {
        return Err("errors.imageMissing".into());
    }

    let cancel = state.reset();

    let mut cmd = Command::new(program);
    cmd.arg("-s").arg(serial).arg("sideload").arg(zip);
    cmd.stdin(Stdio::null())
        .stdout(Stdio::piped())
        .stderr(Stdio::piped());
    tools::hide_console(&mut cmd);

    emit(
        &app,
        target,
        "flash:phase",
        FlashPhase { phase: "sending".into(), received: 0, total: Some(size) },
    );

    let mut child = cmd.spawn().map_err(|e| e.to_string())?;
    let stdout = child.stdout.take().ok_or("no stdout")?;
    let stderr = child.stderr.take().ok_or("no stderr")?;
    let child = Arc::new(Mutex::new(child));
    state.attach(child.clone());

    let pump = |reader: Box<dyn Read + Send>, app: tauri::AppHandle, target: String, mark: bool| {
        std::thread::spawn(move || {
            let mut reader = BufReader::new(reader);
            let mut line = String::new();
            loop {
                line.clear();
                match reader.read_line(&mut line) {
                    Ok(0) => break,
                    Ok(_) => {
                        let text = line.trim_end().to_string();
                        // The archive has been handed to recovery; what follows
                        // is the device applying it.
                        if mark && (text.contains("Total xfer") || text.contains("serving")) {
                            emit(
                                &app,
                                &target,
                                "flash:phase",
                                FlashPhase { phase: "installing".into(), received: 0, total: Some(size) },
                            );
                        }
                        emit(&app, &target, "flash:output", FlashLine { line: text });
                    }
                    Err(_) => break,
                }
            }
        })
    };

    let out_thread = pump(Box::new(stdout), app.clone(), target.to_string(), true);
    let err_thread = pump(Box::new(stderr), app.clone(), target.to_string(), true);

    let wait_child = child.clone();
    let code = loop {
        if cancel.load(Ordering::SeqCst) {
            if let Ok(mut guard) = wait_child.lock() {
                let _ = guard.kill();
                let _ = guard.wait();
            }
            break -1;
        }
        let status = wait_child.lock().ok().and_then(|mut guard| guard.try_wait().ok().flatten());
        match status {
            Some(status) => break status.code().unwrap_or(-1),
            None => std::thread::sleep(Duration::from_millis(100)),
        }
    };

    let _ = out_thread.join();
    let _ = err_thread.join();
    state.detach();

    if code == 0 {
        emit(
            &app,
            target,
            "flash:phase",
            FlashPhase { phase: "done".into(), received: size, total: Some(size) },
        );
    }

    Ok(code)
}

pub fn cancel(state: &FlashState) -> bool {
    state.cancel.store(true, Ordering::SeqCst);
    true
}

#[cfg(test)]
mod tests {
    use super::classify;

    #[test]
    fn reads_fastboot_phases() {
        assert_eq!(
            classify("Sending 'boot_a' (65536 KB)                         OKAY [  1.512s]"),
            Some(("sending", Some(65536 * 1024)))
        );
        assert_eq!(
            classify("Writing 'boot_a'                                    OKAY [  0.845s]"),
            Some(("writing", None))
        );
        assert_eq!(classify("Finished. Total time: 2.371s"), Some(("done", None)));
        assert_eq!(classify("OKAY [  0.004s]"), None);
        assert_eq!(classify("FAILED (remote: 'unknown command')"), None);
    }

    #[test]
    fn ignores_lines_without_a_phase() {
        assert_eq!(classify(""), None);
        assert_eq!(classify("fastboot: error: cannot load 'x.img'"), None);
    }
}
