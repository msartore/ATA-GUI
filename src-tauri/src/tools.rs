use crate::state::AppPaths;
use serde::{Deserialize, Serialize};
use std::path::{Path, PathBuf};
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

pub fn run(program: &Path, args: &[String]) -> Result<CmdOut, String> {
    let output = base_command(program)
        .args(args)
        .output()
        .map_err(|e| format!("failed to start {}: {e}", program.display()))?;

    Ok(CmdOut {
        stdout: String::from_utf8_lossy(&output.stdout).to_string(),
        stderr: String::from_utf8_lossy(&output.stderr).to_string(),
        code: output.status.code().unwrap_or(-1),
    })
}

/// Build `adb [ -s serial ] args...` and execute it.
pub fn adb(paths: &AppPaths, serial: Option<&str>, args: &[String]) -> Result<CmdOut, String> {
    let program = adb_path(paths)?;
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
