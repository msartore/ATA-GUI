use serde::{Deserialize, Serialize};
use std::fs;
use std::path::{Path, PathBuf};
use std::sync::Mutex;

/// Every path the app needs, resolved once at startup.
///
/// The original WinForms app writes next to the executable (IPList.txt,
/// InfoCache.xml, uad_lists.json, scrcpy.exe, adb.exe).  We keep that contract
/// when the executable directory is writable (typical on Windows and on Linux
/// portable builds) and fall back to the per-user data directory when it is not
/// (e.g. installed under /usr/bin or Program Files).
#[derive(Clone)]
pub struct AppPaths {
    pub exe_dir: PathBuf,
    pub data_dir: PathBuf,
    pub config_dir: PathBuf,
    pub runtime_dir: PathBuf,
}

impl AppPaths {
    pub fn resolve(app: &tauri::AppHandle) -> Self {
        use tauri::Manager;

        let exe_dir = std::env::current_exe()
            .ok()
            .and_then(|p| p.parent().map(Path::to_path_buf))
            .unwrap_or_else(|| PathBuf::from("."));

        let data_dir = app
            .path()
            .app_data_dir()
            .unwrap_or_else(|_| exe_dir.join("data"));
        let config_dir = app
            .path()
            .app_config_dir()
            .unwrap_or_else(|_| exe_dir.clone());

        let _ = fs::create_dir_all(&data_dir);
        let _ = fs::create_dir_all(&config_dir);

        let probe = exe_dir.join(".ata-write-probe");
        let runtime_dir = if fs::write(&probe, b"ok").is_ok() {
            let _ = fs::remove_file(&probe);
            exe_dir.clone()
        } else {
            data_dir.clone()
        };

        AppPaths {
            exe_dir,
            data_dir,
            config_dir,
            runtime_dir,
        }
    }

    /// Directories searched for adb / fastboot / scrcpy.
    pub fn tool_dirs(&self) -> Vec<PathBuf> {
        let mut dirs = vec![
            self.runtime_dir.clone(),
            self.exe_dir.clone(),
            self.runtime_dir.join("platform-tools"),
            self.data_dir.join("platform-tools"),
            self.data_dir.clone(),
        ];
        dirs.dedup();
        dirs
    }

    pub fn img_dir(&self) -> PathBuf {
        self.runtime_dir.join("IMG")
    }

    pub fn apks_dir(&self) -> PathBuf {
        self.runtime_dir.join("APKS")
    }

    /// Configuration lives next to the executable for portable builds (when
    /// that directory is writable) and in the per-user config directory for
    /// installed builds.
    pub fn config_file(&self) -> PathBuf {
        if self.runtime_dir == self.exe_dir {
            self.runtime_dir.join("config.json")
        } else {
            self.config_dir.join("config.json")
        }
    }

    /// Locations searched when loading: the active one first, then the
    /// per-user location (migration from earlier versions/installs).
    pub fn config_candidates(&self) -> Vec<PathBuf> {
        let primary = self.config_file();
        let secondary = self.config_dir.join("config.json");
        if primary == secondary {
            vec![primary]
        } else {
            vec![primary, secondary]
        }
    }
}

#[derive(Serialize, Deserialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct Config {
    /// BCP-47 language tag chosen by the user ("system" = follow the OS).
    #[serde(default = "default_language")]
    pub language: String,
    /// Show the feedback window on start-up.
    #[serde(default = "default_true")]
    pub feedback_popup: bool,
    /// The licence agreement was accepted.
    #[serde(default)]
    pub disclaimer_accepted: bool,
    /// Known wireless debugging endpoints (IP addresses).
    #[serde(default)]
    pub ips: Vec<String>,
    /// UI theme: "dark", "light" or "system".
    #[serde(default = "default_theme")]
    pub theme: String,
}

fn default_theme() -> String {
    "dark".to_string()
}

fn default_language() -> String {
    "system".to_string()
}
fn default_true() -> bool {
    true
}

impl Default for Config {
    fn default() -> Self {
        Self {
            language: default_language(),
            feedback_popup: true,
            disclaimer_accepted: false,
            ips: Vec::new(),
            theme: default_theme(),
        }
    }
}

impl Config {
    /// Load config.json, migrating the legacy WinForms `InfoCache.xml` and
    /// `IPList.txt` files the first time the Rust port runs.
    pub fn load(paths: &AppPaths) -> Self {
        let primary = paths.config_file();
        for candidate in paths.config_candidates() {
            if let Ok(text) = fs::read_to_string(&candidate) {
                match serde_json::from_str::<Config>(&text) {
                    Ok(cfg) => {
                        if candidate != primary {
                            // Migrated from a per-user install: seed the portable
                            // location so the next start is self-contained.
                            cfg.save(paths);
                        }
                        return cfg;
                    }
                    Err(_) => {
                        // Keep the unreadable file instead of overwriting it with
                        // defaults: a schema change or a partial write must not
                        // silently wipe the user's language/theme/history.
                        let backup = candidate.with_extension("json.bak");
                        let _ = fs::rename(&candidate, &backup);
                    }
                }
            }
        }

        let mut cfg = Config::default();

        // Legacy disclaimer / feedback flags.
        let info_cache = paths.runtime_dir.join("InfoCache.xml");
        if let Ok(xml) = fs::read_to_string(&info_cache) {
            if let Some(v) = xml_tag(&xml, "license") {
                cfg.disclaimer_accepted = v.eq_ignore_ascii_case("yes");
            }
            if let Some(v) = xml_tag(&xml, "initialPopUpFeedback") {
                cfg.feedback_popup = v.eq_ignore_ascii_case("yes");
            }
        }

        // Legacy IP history.
        let ip_list = paths.runtime_dir.join("IPList.txt");
        if let Ok(text) = fs::read_to_string(&ip_list) {
            for line in text.lines() {
                let ip = line.trim();
                if !ip.is_empty() && !cfg.ips.iter().any(|known| known == ip) {
                    cfg.ips.push(ip.to_string());
                }
            }
        }

        cfg.save(paths);
        cfg
    }

    pub fn save(&self, paths: &AppPaths) {
        let Ok(text) = serde_json::to_string_pretty(self) else {
            return;
        };
        // Write-then-rename: a crash mid-write must not leave a truncated file
        // that the loader would treat as corrupt on the next start.
        let target = paths.config_file();
        let temp = target.with_extension("json.tmp");
        if fs::write(&temp, text).is_ok() && fs::rename(&temp, &target).is_err() {
            let _ = fs::remove_file(&temp);
        }
    }
}

fn xml_tag(xml: &str, tag: &str) -> Option<String> {
    let open = format!("<{tag}>");
    let close = format!("</{tag}>");
    let start = xml.find(&open)? + open.len();
    let end = xml[start..].find(&close)? + start;
    Some(xml[start..end].trim().to_string())
}

pub struct AppState {
    pub paths: AppPaths,
    pub config: Mutex<Config>,
    pub logcat: Mutex<Option<crate::logcat::LogcatHandle>>,
    pub watcher: Mutex<Option<crate::device::WatcherHandle>>,
    /// Raised by the Cancel button; long operations poll it and kill their child.
    pub cancel: std::sync::Arc<std::sync::atomic::AtomicBool>,
}
