use crate::state::AppPaths;
use crate::tools;
use std::io::{BufRead, BufReader};
use std::process::{Child, Command, Stdio};
use std::sync::{Arc, Mutex};

/// Emit at most `BATCH_LINES` per message, or everything older than
/// `BATCH_WINDOW` — whichever comes first.
const BATCH_LINES: usize = 300;
const BATCH_WINDOW: std::time::Duration = std::time::Duration::from_millis(60);

pub struct LogcatHandle {
    child: Arc<Mutex<Child>>,
    join: Option<std::thread::JoinHandle<()>>,
}

impl LogcatHandle {
    pub fn stop(mut self) {
        if let Ok(mut child) = self.child.lock() {
            let _ = child.kill();
            let _ = child.wait();
        }
        if let Some(join) = self.join.take() {
            let _ = join.join();
        }
    }
}

/// Start streaming `adb shell logcat` (or `getevent`) as `logcat:line` events
/// targeted at the requesting window. Port of `DeviceLogsForm`.
pub fn start(
    app: tauri::AppHandle,
    paths: &AppPaths,
    serial: &str,
    mode: &str,
    target: &str,
) -> Result<LogcatHandle, String> {
    use tauri::Emitter;

    let program = tools::adb_path(paths)?;
    let mut cmd = Command::new(program);
    cmd.arg("-s")
        .arg(serial)
        .arg("shell")
        .arg(if mode == "getevent" { "getevent" } else { "logcat" })
        .stdin(Stdio::null())
        .stdout(Stdio::piped())
        .stderr(Stdio::piped());

    #[cfg(windows)]
    {
        use std::os::windows::process::CommandExt;
        cmd.creation_flags(0x0800_0000);
    }

    let mut child = cmd.spawn().map_err(|e| e.to_string())?;
    let stdout = child.stdout.take().ok_or("no stdout")?;
    let stderr = child.stderr.take().ok_or("no stderr")?;
    let child = Arc::new(Mutex::new(child));

    // stdout reader — the actual log stream. Lines are batched: a busy device
    // prints thousands per second and one IPC message each would swamp the
    // webview before React gets a chance to paint.
    let child_ref = child.clone();
    let app_out = app.clone();
    let target_out = target.to_string();
    let join = std::thread::spawn(move || {
        let reader = BufReader::new(stdout);
        let mut batch: Vec<String> = Vec::with_capacity(BATCH_LINES);
        let mut last = std::time::Instant::now();

        for line in reader.lines() {
            match line {
                Ok(line) => {
                    batch.push(line);
                    if batch.len() >= BATCH_LINES || last.elapsed() >= BATCH_WINDOW {
                        let _ = app_out.emit_to(
                            &target_out,
                            "logcat:lines",
                            std::mem::take(&mut batch),
                        );
                        last = std::time::Instant::now();
                    }
                }
                Err(_) => break,
            }
        }

        if !batch.is_empty() {
            let _ = app_out.emit_to(&target_out, "logcat:lines", batch);
        }

        if let Ok(mut child) = child_ref.lock() {
            let _ = child.kill();
        }
    });

    // stderr drainer — keeps the pipe from filling up and surfaces adb errors.
    let app_err = app.clone();
    let target_err = target.to_string();
    std::thread::spawn(move || {
        let reader = BufReader::new(stderr);
        for line in reader.lines().map_while(Result::ok) {
            let _ = app_err.emit_to(&target_err, "logcat:stderr", &line);
        }
    });

    Ok(LogcatHandle {
        child,
        join: Some(join),
    })
}
