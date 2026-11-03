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
/// `streamed` marks a phase whose `received` is a real counter (sideload),
/// so the panel can follow it instead of using a fixed weight.
#[derive(Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct FlashPhase {
    pub phase: String,
    pub received: u64,
    pub total: Option<u64>,
    pub streamed: bool,
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
                            FlashPhase { phase: phase.into(), received: 0, total, streamed: false },
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

/// `serving: '<file>'  (~47%)` is the only byte counter a sideload gives us:
/// adb reports how much of the archive it has pushed to recovery. The number
/// is a percentage of the file, not a running byte count.
pub fn sideload_percent(line: &str) -> Option<u8> {
    let rest = &line[line.find("serving:")?..];
    let open = rest.rfind("(~")?;
    let digits = &rest[open + 2..];
    let end = digits.find('%')?;
    digits[..end].trim().parse::<u8>().ok().filter(|percent| *percent <= 100)
}

/// adb writes its progress with a bare `\r` so the same line is rewritten, and
/// only the final trailer ends in `\n`. A reader that splits on newlines alone
/// would see the entire transfer as one line, which is why the percentage was
/// never usable here.
struct Segments<R: Read> {
    reader: BufReader<R>,
    buffer: Vec<u8>,
    byte: [u8; 1],
    done: bool,
}

impl<R: Read> Segments<R> {
    fn new(reader: R) -> Self {
        Self { reader: BufReader::new(reader), buffer: Vec::new(), byte: [0u8; 1], done: false }
    }
}

impl<R: Read> Iterator for Segments<R> {
    type Item = String;

    fn next(&mut self) -> Option<String> {
        if self.done {
            return None;
        }
        self.buffer.clear();
        loop {
            match self.reader.read(&mut self.byte) {
                Ok(0) => {
                    self.done = true;
                    break;
                }
                Ok(_) if self.byte[0] == b'\r' || self.byte[0] == b'\n' => break,
                Ok(_) => self.buffer.push(self.byte[0]),
                Err(_) => {
                    self.done = true;
                    break;
                }
            }
        }
        let text = String::from_utf8_lossy(&self.buffer).trim().to_string();
        if self.done && text.is_empty() {
            None
        } else {
            Some(text)
        }
    }
}

fn pump_sideload(app: tauri::AppHandle, target: String, reader: impl Read, size: u64) {
    let mut last = 101u8;
    let mut handed_off = false;

    for text in Segments::new(reader) {
        if text.is_empty() {
            continue;
        }
        if let Some(percent) = sideload_percent(&text) {
            // One event per percent, not per line: adb repeats the same figure
            // for every 4 KB it manages to write.
            if percent != last {
                last = percent;
                emit(
                    &app,
                    &target,
                    "flash:phase",
                    FlashPhase {
                        phase: "sending".into(),
                        received: size * u64::from(percent) / 100,
                        total: Some(size),
                        streamed: true,
                    },
                );
            }
        }
        // Transfer over: recovery is applying the archive, and only the device
        // can say how far that has got.
        if !handed_off && text.contains("Total xfer") {
            handed_off = true;
            emit(
                &app,
                &target,
                "flash:phase",
                FlashPhase {
                    phase: "installing".into(),
                    received: size,
                    total: Some(size),
                    streamed: false,
                },
            );
        }
        emit(&app, &target, "flash:output", FlashLine { line: text });
    }
}

/// `adb sideload <file>`. Recovery owns the bar for the apply step, but the
/// host does report its own transfer: `serving: '<file>'  (~47%)` is a real
/// percentage, so the panel follows the bytes until `Total xfer` marks the
/// handoff and the device takes over.
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
        FlashPhase { phase: "sending".into(), received: 0, total: Some(size), streamed: true },
    );

    let mut child = cmd.spawn().map_err(|e| e.to_string())?;
    let stdout = child.stdout.take().ok_or("no stdout")?;
    let stderr = child.stderr.take().ok_or("no stderr")?;
    let child = Arc::new(Mutex::new(child));
    state.attach(child.clone());

    let out_thread = {
        let app = app.clone();
        let target = target.to_string();
        std::thread::spawn(move || pump_sideload(app, target, stdout, size))
    };
    let err_thread = {
        let app = app.clone();
        let target = target.to_string();
        std::thread::spawn(move || pump_sideload(app, target, stderr, size))
    };

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
            FlashPhase { phase: "done".into(), received: size, total: Some(size), streamed: false },
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
    use super::{classify, sideload_percent, Segments};
    use std::io::Cursor;

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

    /// Real output taken from a LineageOS sideload on Windows.
    #[test]
    fn reads_the_sideload_percentage() {
        let zip = r"C:\Users\Max\Downloads\lineage-22.2-20261103-nightly-beryllium-signed.zip";
        assert_eq!(sideload_percent(&format!("serving: '{zip}'  (~0%)    ")), Some(0));
        assert_eq!(sideload_percent(&format!("serving: '{zip}'  (~47%)    ")), Some(47));
        assert_eq!(sideload_percent(&format!("serving: '{zip}'  (~100%)    ")), Some(100));
        // No number, a trailer, or an out-of-range figure must not become a phase.
        assert_eq!(sideload_percent(&format!("serving: '{zip}'")), None);
        assert_eq!(sideload_percent("Total xfer: 1.00x"), None);
        assert_eq!(sideload_percent(&format!("serving: '{zip}'  (~180%)")), None);
    }

    /// adb rewrites one line with `\r`; a newline-only reader sees the whole
    /// transfer as a single line and loses every percentage.
    #[test]
    fn splits_on_carriage_returns_and_newlines() {
        let raw = "serving: 'a.zip'  (~0%)    \rserving: 'a.zip'  (~1%)    \r\r\nTotal xfer: 1.00x\n";
        let segments: Vec<String> = Segments::new(Cursor::new(raw.as_bytes())).collect();
        assert_eq!(
            segments,
            vec![
                "serving: 'a.zip'  (~0%)".to_string(),
                "serving: 'a.zip'  (~1%)".to_string(),
                // `\r\r\n` is two empty segments; the pump skips them.
                String::new(),
                String::new(),
                "Total xfer: 1.00x".to_string(),
            ]
        );
    }

    #[test]
    fn keeps_a_trailing_segment_without_a_delimiter() {
        let segments: Vec<String> =
            Segments::new(Cursor::new(b"one\rtwo".as_slice())).collect();
        assert_eq!(segments, vec!["one".to_string(), "two".to_string()]);
        assert!(Segments::new(Cursor::new(b"".as_slice())).next().is_none());
    }
}
