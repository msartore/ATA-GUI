use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;
use std::io::{BufRead, BufReader, Read};
use std::process::{Child, Command, Stdio};
use std::sync::{Arc, Mutex};
use std::time::{Duration, Instant};

/// One chunk of output. Chunks are batched (60 ms or 8 KB) so a command that
/// prints thousands of lines per second — logcat — cannot flood the webview.
#[derive(Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct TermOutput {
    pub session: u64,
    pub stream: String,
    pub text: String,
}

#[derive(Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct TermExit {
    pub session: u64,
    pub code: i32,
}

pub struct TermHandle {
    child: Arc<Mutex<Child>>,
}

impl TermHandle {
    pub fn stop(self) {
        if let Ok(mut child) = self.child.lock() {
            let _ = child.kill();
            let _ = child.wait();
        }
    }
}

#[derive(Default)]
pub struct TermState {
    pub current: Mutex<Option<TermHandle>>,
    /// Every run gets a generation. Ctrl+C records the generation it wants
    /// stopped, so a press that lands while the child is still spawning is
    /// honoured without poisoning the next command.
    pub generation: std::sync::atomic::AtomicU64,
    pub kill_generation: std::sync::atomic::AtomicU64,
}

const BATCH: usize = 8 * 1024;
const FLUSH: Duration = Duration::from_millis(60);

fn pump(app: tauri::AppHandle, target: String, session: u64, stream: &'static str, reader: impl Read) {
    use tauri::Emitter;

    let mut reader = BufReader::new(reader);
    let mut line = String::new();
    let mut buffer = String::new();
    let mut last = Instant::now();

    loop {
        line.clear();
        match reader.read_line(&mut line) {
            Ok(0) => break,
            Ok(_) => {
                buffer.push_str(&line);
                if buffer.len() >= BATCH || last.elapsed() >= FLUSH {
                    let _ = app.emit_to(
                        &target,
                        "term:output",
                        TermOutput {
                            session,
                            stream: stream.to_string(),
                            text: std::mem::take(&mut buffer),
                        },
                    );
                    last = Instant::now();
                }
            }
            Err(_) => break,
        }
    }

    if !buffer.is_empty() {
        let _ = app.emit_to(
            &target,
            "term:output",
            TermOutput {
                session,
                stream: stream.to_string(),
                text: buffer,
            },
        );
    }
}

pub struct Run<'a> {
    pub target: &'a str,
    pub session: u64,
    pub tool: &'a str,
    pub serial: Option<&'a str>,
    pub args: &'a [String],
}

/// Spawn `adb|fastboot [ -s serial ] args…` and stream its output to the
/// requesting window. Returns once the process is running, so the caller can
/// still type and interrupt while it prints (logcat never exits on its own).
pub fn run(
    app: tauri::AppHandle,
    state: &TermState,
    paths: &AppPaths,
    request: Run<'_>,
) -> Result<(), String> {
    let Run {
        target,
        session,
        tool,
        serial,
        args,
    } = request;

    use tauri::Emitter;

    // One command at a time per terminal: a new run replaces the old process.
    if let Some(previous) = state.current.lock().map_err(|e| e.to_string())?.take() {
        previous.stop();
    }

    let generation = state
        .generation
        .fetch_add(1, std::sync::atomic::Ordering::SeqCst)
        + 1;

    let program = if tool == "fastboot" {
        tools::fastboot_path(paths)?
    } else {
        tools::adb_path(paths)?
    };

    let mut cmd = Command::new(program);
    if let Some(serial) = serial.filter(|s| !s.is_empty()) {
        cmd.arg("-s").arg(serial);
    }
    cmd.args(args)
        .stdin(Stdio::null())
        .stdout(Stdio::piped())
        .stderr(Stdio::piped());
    tools::hide_console(&mut cmd);

    let mut child = cmd.spawn().map_err(|e| e.to_string())?;
    let stdout = child.stdout.take().ok_or("no stdout")?;
    let stderr = child.stderr.take().ok_or("no stderr")?;
    let child = Arc::new(Mutex::new(child));

    let target_out = target.to_string();
    let app_out = app.clone();
    std::thread::spawn(move || pump(app_out, target_out, session, "out", stdout));

    let target_err = target.to_string();
    let app_err = app.clone();
    std::thread::spawn(move || pump(app_err, target_err, session, "err", stderr));

    // Reaper: polls try_wait so `term_kill` can still lock the child.
    let child_ref = child.clone();
    let app_exit = app.clone();
    let target_exit = target.to_string();
    std::thread::spawn(move || loop {
        std::thread::sleep(Duration::from_millis(120));
        let status = match child_ref.lock() {
            Ok(mut child) => child.try_wait(),
            Err(_) => return,
        };
        match status {
            Ok(Some(status)) => {
                let code = status.code().unwrap_or(-1);
                let _ = app_exit.emit_to(
                    &target_exit,
                    "term:exit",
                    TermExit { session, code },
                );
                return;
            }
            Ok(None) => continue,
            Err(_) => return,
        }
    });

    state
        .current
        .lock()
        .map_err(|e| e.to_string())?
        .replace(TermHandle { child });

    // Ctrl+C pressed while this command was starting.
    if state.kill_generation.load(std::sync::atomic::Ordering::SeqCst) == generation {
        kill(state)?;
    }

    Ok(())
}

/// Ctrl+C in the terminal: kill whatever is running, keep the window alive.
pub fn kill(state: &TermState) -> Result<bool, String> {
    let generation = state.generation.load(std::sync::atomic::Ordering::SeqCst);
    state
        .kill_generation
        .store(generation, std::sync::atomic::Ordering::SeqCst);
    let handle = state.current.lock().map_err(|e| e.to_string())?.take();
    match handle {
        Some(handle) => {
            handle.stop();
            Ok(true)
        }
        None => Ok(false),
    }
}
