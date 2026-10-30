use crate::state::AppPaths;
use serde::{Deserialize, Serialize};
use std::path::{Path, PathBuf};
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::Arc;
use std::time::{Duration, Instant};
use std::process::{Command, Stdio};

#[derive(Serialize, Deserialize, Clone, Debug, Default)]
pub struct CmdOut {
    pub stdout: String,
    pub stderr: String,
    pub code: i32,
}

impl CmdOut {
    /// Fastboot prints `(bootloader) key: value` on stderr while adb prints on
    /// stdout; the WinForms app merged both, so we do the same.
    pub fn merged(&self) -> String {
        let out = self.stdout.trim_end_matches(['\r', '\n']);
        let err = self.stderr.trim_end_matches(['\r', '\n']);
        match (out.is_empty(), err.is_empty()) {
            (true, _) => err.to_string(),
            (_, true) => out.to_string(),
            _ => format!("{out}\n{err}"),
        }
    }

    pub fn success(&self) -> bool {
        self.code == 0
    }
}

pub fn tool_file(name: &str) -> String {
    if cfg!(windows) {
        format!("{name}.exe")
    } else {
        name.to_string()
    }
}

pub fn find_tool(paths: &AppPaths, name: &str) -> Option<PathBuf> {
    let file = tool_file(name);

    for dir in paths.tool_dirs() {
        let candidate = dir.join(&file);
        if candidate.is_file() {
            return Some(candidate);
        }
    }

    if let Some(path) = std::env::var_os("PATH") {
        for dir in std::env::split_paths(&path) {
            let candidate = dir.join(&file);
            if candidate.is_file() {
                return Some(candidate);
            }
        }
    }

    None
}

pub fn adb_path(paths: &AppPaths) -> Result<PathBuf, String> {
    find_tool(paths, "adb").ok_or_else(|| "adb not found".to_string())
}

pub fn fastboot_path(paths: &AppPaths) -> Result<PathBuf, String> {
    find_tool(paths, "fastboot").ok_or_else(|| "fastboot not found".to_string())
}

/// `CREATE_NO_WINDOW` — a console child of a GUI process must never flash a
/// window over the UI (adb, fastboot, taskkill, tasklist, …).
pub fn hide_console(cmd: &mut Command) {
    #[cfg(windows)]
    {
        use std::os::windows::process::CommandExt;
        cmd.creation_flags(0x0800_0000);
    }
    #[cfg(not(windows))]
    {
        let _ = cmd;
    }
}

fn base_command(program: &Path) -> Command {
    let mut cmd = Command::new(program);
    cmd.stdin(Stdio::null());
    hide_console(&mut cmd);
    cmd
}

/// Default watchdog for ordinary adb/fastboot queries. Long operations (flash,
/// sideload, install) pass their own, larger budget.
pub const DEFAULT_TIMEOUT: Duration = Duration::from_secs(300);

/// Cancellation + timeout shared by a long-running operation.
#[derive(Clone)]
pub struct Guard {
    cancel: Arc<AtomicBool>,
    timeout: Duration,
}

impl Guard {
    pub fn new(cancel: Arc<AtomicBool>, timeout: Duration) -> Self {
        cancel.store(false, Ordering::Relaxed);
        Self { cancel, timeout }
    }

    pub fn timeout(&self) -> Duration {
        self.timeout
    }

    pub fn flag(&self) -> &AtomicBool {
        &self.cancel
    }
}


/// Kill the child **and its descendants** — `cmd /C ping` otherwise leaves the
/// grandchild holding the pipes open and the drain threads never finish.
fn kill_tree(child: &mut std::process::Child) {
    #[cfg(windows)]
    {
        let mut cmd = std::process::Command::new("taskkill");
        cmd.args(["/F", "/T", "/PID", &child.id().to_string()]);
        hide_console(&mut cmd);
        let _ = cmd.status();
    }
    let _ = child.kill();
    let _ = child.wait();
}

/// Run a process, killing it when it outlives `timeout` or `cancel` is raised.
/// stdout/stderr are drained on separate threads so a full pipe can never
/// deadlock the child.
pub fn run_guarded(
    program: &Path,
    args: &[String],
    timeout: Option<Duration>,
    cancel: Option<&AtomicBool>,
) -> Result<CmdOut, String> {
    use std::io::Read;

    let mut child = base_command(program)
        .args(args)
        .stdout(Stdio::piped())
        .stderr(Stdio::piped())
        .spawn()
        .map_err(|e| format!("failed to start {}: {e}", program.display()))?;

    let stdout_pipe = child.stdout.take();
    let stderr_pipe = child.stderr.take();

    let drain = |pipe: Option<std::process::ChildStdout>| {
        pipe.map(|mut pipe| {
            std::thread::spawn(move || {
                let mut buffer = Vec::new();
                let _ = pipe.read_to_end(&mut buffer);
                buffer
            })
        })
    };
    let drain_err = |pipe: Option<std::process::ChildStderr>| {
        pipe.map(|mut pipe| {
            std::thread::spawn(move || {
                let mut buffer = Vec::new();
                let _ = pipe.read_to_end(&mut buffer);
                buffer
            })
        })
    };

    let out_thread = drain(stdout_pipe);
    let err_thread = drain_err(stderr_pipe);

    let deadline = timeout.map(|limit| Instant::now() + limit);
    let status = loop {
        match child.try_wait() {
            Ok(Some(status)) => break Some(status),
            Ok(None) => {}
            Err(error) => return Err(error.to_string()),
        }

        let cancelled = cancel.map(|flag| flag.load(Ordering::Relaxed)).unwrap_or(false);
        let expired = deadline.map(|limit| Instant::now() >= limit).unwrap_or(false);
        if cancelled || expired {
            kill_tree(&mut child);
            if let Some(handle) = out_thread {
                let _ = handle.join();
            }
            if let Some(handle) = err_thread {
                let _ = handle.join();
            }
            return Err(if cancelled {
                "errors.cancelled".to_string()
            } else {
                "errors.timeout".to_string()
            });
        }

        std::thread::sleep(Duration::from_millis(40));
    };

    let stdout = out_thread.and_then(|handle| handle.join().ok()).unwrap_or_default();
    let stderr = err_thread.and_then(|handle| handle.join().ok()).unwrap_or_default();

    Ok(CmdOut {
        stdout: String::from_utf8_lossy(&stdout).to_string(),
        stderr: String::from_utf8_lossy(&stderr).to_string(),
        code: status.and_then(|status| status.code()).unwrap_or(-1),
    })
}

pub fn run(program: &Path, args: &[String]) -> Result<CmdOut, String> {
    run_guarded(program, args, Some(DEFAULT_TIMEOUT), None)
}

/// Build `adb [ -s serial ] args...` and execute it.
pub fn adb(paths: &AppPaths, serial: Option<&str>, args: &[String]) -> Result<CmdOut, String> {
    adb_guarded(paths, serial, args, None, None)
}

/// Same as [`adb`] but with an explicit timeout and cancellation flag.
pub fn adb_guarded(
    paths: &AppPaths,
    serial: Option<&str>,
    args: &[String],
    timeout: Option<Duration>,
    cancel: Option<&AtomicBool>,
) -> Result<CmdOut, String> {
    let program = adb_path(paths)?;
    let mut full: Vec<String> = Vec::with_capacity(args.len() + 2);
    if let Some(serial) = serial {
        if !serial.is_empty() {
            full.push("-s".into());
            full.push(serial.to_string());
        }
    }
    full.extend_from_slice(args);
    run_guarded(&program, &full, timeout, cancel)
}

/// Same as [`fastboot`] but with an explicit timeout and cancellation flag.
pub fn fastboot_guarded(
    paths: &AppPaths,
    serial: Option<&str>,
    args: &[String],
    timeout: Option<Duration>,
    cancel: Option<&AtomicBool>,
) -> Result<CmdOut, String> {
    let program = fastboot_path(paths)?;
    let mut full: Vec<String> = Vec::with_capacity(args.len() + 2);
    if let Some(serial) = serial {
        if !serial.is_empty() {
            full.push("-s".into());
            full.push(serial.to_string());
        }
    }
    full.extend_from_slice(args);
    run_guarded(&program, &full, timeout, cancel)
}

/// Build `fastboot [ -s serial ] args...` and execute it.
pub fn fastboot(paths: &AppPaths, serial: Option<&str>, args: &[String]) -> Result<CmdOut, String> {
    let program = fastboot_path(paths)?;
    let mut full: Vec<String> = Vec::with_capacity(args.len() + 2);
    if let Some(serial) = serial {
        if !serial.is_empty() {
            full.push("-s".into());
            full.push(serial.to_string());
        }
    }
    full.extend_from_slice(args);
    run(&program, &full)
}

pub fn adb_text(paths: &AppPaths, serial: Option<&str>, args: &[String]) -> String {
    adb(paths, serial, args)
        .map(|o| o.merged())
        .unwrap_or_default()
}

/// Spawn a process without waiting (scrcpy, updater scripts).
pub fn spawn_detached(mut cmd: Command) -> Result<(), String> {
    #[cfg(windows)]
    {
        use std::os::windows::process::CommandExt;
        // DETACHED_PROCESS | CREATE_NEW_PROCESS_GROUP
        cmd.creation_flags(0x0000_0008 | 0x0000_0200);
    }
    #[cfg(not(windows))]
    {
        cmd.stdin(Stdio::null())
            .stdout(Stdio::null())
            .stderr(Stdio::null());
    }
    cmd.spawn().map(|_| ()).map_err(|e| e.to_string())
}

pub fn open_external(url: &str) -> Result<(), String> {
    #[cfg(windows)]
    {
        let mut cmd = Command::new("cmd");
        cmd.args(["/C", "start", "", url]);
        spawn_detached(cmd)
    }
    #[cfg(target_os = "macos")]
    {
        let mut cmd = Command::new("open");
        cmd.arg(url);
        spawn_detached(cmd)
    }
    #[cfg(all(unix, not(target_os = "macos")))]
    {
        let mut cmd = Command::new("xdg-open");
        cmd.arg(url);
        spawn_detached(cmd)
    }
}

/// Kill leftover adb / fastboot processes (replaces `taskkill /f /im`).
pub fn kill_tool_processes() {
    #[cfg(windows)]
    {
        for exe in ["adb.exe", "fastboot.exe"] {
            let mut cmd = Command::new("taskkill");
            cmd.args(["/F", "/IM", exe]);
            hide_console(&mut cmd);
            let _ = cmd.output();
        }
    }
    #[cfg(not(windows))]
    {
        for name in ["adb", "fastboot"] {
            let mut cmd = Command::new("pkill");
            cmd.args(["-f", name]);
            let _ = cmd.output();
        }
    }
}

pub fn platform_tools_url() -> &'static str {
    if cfg!(windows) {
        "https://dl.google.com/android/repository/platform-tools-latest-windows.zip"
    } else if cfg!(target_os = "macos") {
        "https://dl.google.com/android/repository/platform-tools-latest-darwin.zip"
    } else {
        "https://dl.google.com/android/repository/platform-tools-latest-linux.zip"
    }
}

pub fn ping_online() -> bool {
    use std::net::{TcpStream, ToSocketAddrs};
    use std::time::Duration;

    let targets = [("1.1.1.1", 53u16), ("8.8.8.8", 53u16)];
    for (host, port) in targets {
        if let Ok(mut addrs) = (host, port).to_socket_addrs() {
            if let Some(addr) = addrs.next() {
                if TcpStream::connect_timeout(&addr, Duration::from_millis(1200)).is_ok() {
                    return true;
                }
            }
        }
    }
    false
}

#[cfg(test)]
mod tests {
    use super::*;

    #[cfg(windows)]
    fn slow_command() -> (PathBuf, Vec<String>) {
        (
            PathBuf::from("cmd"),
            vec!["/C".into(), "ping -n 8 127.0.0.1 > nul".into()],
        )
    }

    #[cfg(not(windows))]
    fn slow_command() -> (PathBuf, Vec<String>) {
        (PathBuf::from("sleep"), vec!["8".into()])
    }

    #[test]
    fn captures_output() {
        let (program, args) = if cfg!(windows) {
            (PathBuf::from("cmd"), vec!["/C".into(), "echo hello".into()])
        } else {
            (PathBuf::from("echo"), vec!["hello".into()])
        };
        let out = run_guarded(&program, &args, Some(Duration::from_secs(10)), None).unwrap();
        assert!(out.stdout.to_lowercase().contains("hello"));
    }

    #[test]
    fn kills_a_cancelled_child() {
        let (program, args) = slow_command();
        let cancel = Arc::new(AtomicBool::new(false));
        let flag = cancel.clone();
        std::thread::spawn(move || {
            std::thread::sleep(Duration::from_millis(200));
            flag.store(true, Ordering::Relaxed);
        });

        let started = Instant::now();
        let result = run_guarded(&program, &args, Some(Duration::from_secs(30)), Some(&cancel));
        assert_eq!(result.err().as_deref(), Some("errors.cancelled"));
        assert!(started.elapsed() < Duration::from_secs(5), "cancel must not wait for the child");
    }

    #[test]
    fn kills_a_child_that_outlives_the_timeout() {
        let (program, args) = slow_command();
        let started = Instant::now();
        let result = run_guarded(&program, &args, Some(Duration::from_millis(200)), None);
        assert_eq!(result.err().as_deref(), Some("errors.timeout"));
        assert!(started.elapsed() < Duration::from_secs(5));
    }
}
