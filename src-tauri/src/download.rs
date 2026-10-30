use crate::state::AppPaths;
use crate::tools;
use futures_util::StreamExt;
use serde::Serialize;
use std::fs;
use std::io::Write;
use std::path::Path;

const LINKS_URL: &str = "https://ata.msartore.dev/api/links.json";
const RELEASES_FALLBACK: &str = "https://api.github.com/repos/msartore/ATA-GUI/releases";

/// Where the in-app updater looks for releases. The release workflow compiles
/// `ATA_RELEASES_URL` in, so a build always checks the repository it came from;
/// the runtime variable wins for local testing, then the hosted `links.json`.
fn releases_url() -> String {
    if let Ok(value) = std::env::var("ATA_RELEASES_URL") {
        if !value.trim().is_empty() {
            return value;
        }
    }
    if let Some(value) = option_env!("ATA_RELEASES_URL") {
        if !value.trim().is_empty() {
            return value.to_string();
        }
    }
    RELEASES_FALLBACK.to_string()
}
const UAD_LISTS_URL: &str = "https://raw.githubusercontent.com/Universal-Debloater-Alliance/universal-android-debloater-next-generation/refs/heads/main/resources/assets/uad_lists.json";
const SCRCPY_RELEASES: &str = "https://api.github.com/repos/Genymobile/scrcpy/releases/latest";

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct Progress {
    pub phase: String,
    pub received: u64,
    pub total: Option<u64>,
}

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct UpdateInfo {
    pub current: String,
    pub latest: String,
    pub body: String,
    pub html_url: String,
    pub asset_url: String,
    pub new_version: bool,
}

pub fn client() -> Result<reqwest::Client, String> {
    reqwest::Client::builder()
        .user_agent("ATA-GUI")
        .timeout(std::time::Duration::from_secs(120))
        .build()
        .map_err(|e| e.to_string())
}

async fn fetch_text(url: &str) -> Result<String, String> {
    let client = client()?;
    let response = client.get(url).send().await.map_err(|e| e.to_string())?;
    if !response.status().is_success() {
        return Err(format!("HTTP {} for {url}", response.status()));
    }
    response.text().await.map_err(|e| e.to_string())
}

async fn download_file(
    app: &tauri::AppHandle,
    url: &str,
    destination: &Path,
    phase: &str,
) -> Result<(), String> {
    use tauri::Emitter;

    let client = client()?;
    let response = client.get(url).send().await.map_err(|e| e.to_string())?;
    if !response.status().is_success() {
        return Err(format!("HTTP {} for {url}", response.status()));
    }

    let total = response.content_length();
    let mut file = fs::File::create(destination).map_err(|e| e.to_string())?;
    let mut received: u64 = 0;
    let mut stream = response.bytes_stream();

    while let Some(chunk) = stream.next().await {
        let chunk = chunk.map_err(|e| e.to_string())?;
        file.write_all(&chunk).map_err(|e| e.to_string())?;
        received += chunk.len() as u64;
        let _ = app.emit(
            "download:progress",
            Progress {
                phase: phase.to_string(),
                received,
                total,
            },
        );
    }

    file.flush().map_err(|e| e.to_string())?;
    Ok(())
}

pub fn extract_zip(zip_path: &Path, destination: &Path) -> Result<(), String> {
    let file = fs::File::open(zip_path).map_err(|e| e.to_string())?;
    let mut archive = zip::ZipArchive::new(file).map_err(|e| e.to_string())?;

    for index in 0..archive.len() {
        let mut entry = archive.by_index(index).map_err(|e| e.to_string())?;
        let Some(relative) = entry.enclosed_name() else {
            continue;
        };
        let out_path = destination.join(relative);

        if entry.is_dir() {
            let _ = fs::create_dir_all(&out_path);
            continue;
        }

        if let Some(parent) = out_path.parent() {
            let _ = fs::create_dir_all(parent);
        }

        let mut out = fs::File::create(&out_path).map_err(|e| e.to_string())?;
        std::io::copy(&mut entry, &mut out).map_err(|e| e.to_string())?;

        #[cfg(unix)]
        {
            use std::os::unix::fs::PermissionsExt;
            let mode = entry.unix_mode().unwrap_or(0o755);
            let _ = fs::set_permissions(&out_path, fs::Permissions::from_mode(mode | 0o700));
        }
    }

    Ok(())
}

fn chmod_executable(path: &Path) {
    #[cfg(unix)]
    {
        use std::os::unix::fs::PermissionsExt;
        let _ = fs::set_permissions(path, fs::Permissions::from_mode(0o755));
    }
    #[cfg(not(unix))]
    {
        let _ = path;
    }
}

/// Retry a filesystem operation a few times: on Windows, antivirus scanners
/// briefly hold sharing locks on freshly written executables and DLLs.
fn retry_io<T>(mut operation: impl FnMut() -> std::io::Result<T>) -> std::io::Result<T> {
    let mut last = None;
    for attempt in 0..6u64 {
        match operation() {
            Ok(value) => return Ok(value),
            Err(error) => {
                last = Some(error);
                std::thread::sleep(std::time::Duration::from_millis(150 * (attempt + 1)));
            }
        }
    }
    Err(last.expect("retry_io ran at least once"))
}

fn copy_dir_contents(from: &Path, to: &Path) -> Result<(), String> {
    for entry in fs::read_dir(from).map_err(|e| e.to_string())? {
        let entry = entry.map_err(|e| e.to_string())?;
        let source = entry.path();
        let target = to.join(entry.file_name());
        if source.is_dir() {
            retry_io(|| fs::create_dir_all(&target)).map_err(|e| e.to_string())?;
            copy_dir_contents(&source, &target)?;
        } else {
            retry_io(|| fs::copy(&source, &target))
                .map_err(|e| format!("{}: {e}", source.display()))?;
            chmod_executable(&target);
        }
    }
    Ok(())
}

/// Download SDK Platform Tools for the running platform, unpack them next to
/// the executable and verify adb/fastboot exist. Port of `ADBDownload`.
pub async fn platform_tools(app: tauri::AppHandle, paths: AppPaths) -> Result<(), String> {
    use tauri::Emitter;

    let zip_path = paths.runtime_dir.join("sdkplatformtool.zip");
    let _ = fs::remove_file(&zip_path);

    download_file(
        &app,
        tools::platform_tools_url(),
        &zip_path,
        "platformTools",
    )
    .await?;

    let staging = paths.runtime_dir.join("platform-tools-tmp");
    let _ = fs::remove_dir_all(&staging);
    fs::create_dir_all(&staging).map_err(|e| e.to_string())?;
    extract_zip(&zip_path, &staging)?;

    let _ = app.emit(
        "download:progress",
        Progress {
            phase: "unpacking".into(),
            received: 1,
            total: Some(1),
        },
    );

    let extracted = staging.join("platform-tools");
    let source = if extracted.is_dir() { extracted } else { staging.clone() };

    let existing = paths.runtime_dir.join("platform-tools");
    let _ = fs::remove_dir_all(&existing);

    // adb/fastboot may still be running and hold file locks on Windows.
    tools::kill_tool_processes();

    // Flatten the archive into the runtime directory (the WinForms app moved
    // every file out of platform-tools/).
    copy_dir_contents(&source, &paths.runtime_dir)?;
    let _ = fs::remove_dir_all(&staging);
    let _ = fs::remove_file(&zip_path);

    for binary in ["adb", "fastboot"] {
        if let Some(path) = tools::find_tool(&paths, binary) {
            chmod_executable(&path);
        }
    }

    if tools::find_tool(&paths, "adb").is_none() || tools::find_tool(&paths, "fastboot").is_none() {
        return Err("errors.toolsMissing".into());
    }

    Ok(())
}

/// Pick the release asset that matches the running OS and CPU architecture.
/// GitHub lists `win32` before `win64`, so a plain "contains win" check would
/// download the 32-bit build on a 64-bit machine.
fn pick_asset(names: &[String]) -> Option<String> {
    let is_64 = cfg!(target_pointer_width = "64");
    let is_arm = cfg!(target_arch = "aarch64");
    let want_linux = cfg!(all(unix, not(target_os = "macos")));
    let want_mac = cfg!(target_os = "macos");

    let mut best: Option<(i32, &String)> = None;
    for name in names {
        let lower = name.to_ascii_lowercase();

        let mut score = 0;
        if want_linux {
            if !lower.contains("linux") {
                continue;
            }
            if lower.contains("aarch64") || lower.contains("arm64") {
                score += if is_arm { 30 } else { -20 };
            } else if lower.contains("x86_64") || lower.contains("amd64") || lower.contains("x64") {
                score += if is_arm { -20 } else { 20 };
            }
            score += 1;
        } else if want_mac {
            if !(lower.contains("macos") || lower.contains("darwin")) {
                continue;
            }
            score += 10;
        } else {
            if lower.contains("linux") || lower.contains("macos") || lower.contains("darwin") {
                continue;
            }
            if !(lower.contains("win") || lower.contains("windows")) {
                continue;
            }
            if lower.contains("aarch64") || lower.contains("arm64") {
                score += if is_arm { 30 } else { -20 };
            } else if lower.contains("win64") || lower.contains("x64") || lower.contains("amd64") {
                score += if is_64 && !is_arm { 30 } else { 0 };
            } else if lower.contains("win32") || lower.contains("x86") {
                score += if is_64 || is_arm { -10 } else { 20 };
            }
            score += 1;
        }

        // The updater unpacks a ZIP on every platform: when a release ships both
        // a .zip and a .tar.gz for the same target, prefer the ZIP.
        if lower.ends_with(".zip") {
            score += 2;
        } else if lower.ends_with(".tar.gz") || lower.ends_with(".tgz") {
            score -= 1;
        }

        if best.map(|(best_score, _)| score > best_score).unwrap_or(true) {
            best = Some((score, name));
        }
    }

    best.map(|(_, name)| name.clone())
}

/// Release asset (file name, download URL) for the running platform.
fn pick_asset_url(assets: &[serde_json::Value]) -> Option<(String, String)> {
    let names: Vec<String> = assets
        .iter()
        .filter_map(|asset| asset["name"].as_str().map(str::to_string))
        .collect();
    let name = pick_asset(&names)?;
    let url = assets
        .iter()
        .find(|asset| asset["name"].as_str() == Some(name.as_str()))
        .and_then(|asset| asset["browser_download_url"].as_str())?
        .to_string();
    Some((name, url))
}

/// Extract a zip or a tar.gz into `destination` (scrcpy ships a tarball on
/// Linux, everything else is a zip).
fn extract_archive(archive: &Path, destination: &Path) -> Result<(), String> {
    let name = archive
        .file_name()
        .map(|name| name.to_string_lossy().to_ascii_lowercase())
        .unwrap_or_default();

    if name.ends_with(".tar.gz") || name.ends_with(".tgz") {
        let status = std::process::Command::new("tar")
            .arg("-xzf")
            .arg(archive)
            .arg("-C")
            .arg(destination)
            .status()
            .map_err(|error| error.to_string())?;
        if !status.success() {
            return Err("errors.extractFailed".into());
        }
        Ok(())
    } else {
        extract_zip(archive, destination)
    }
}

/// Download scrcpy for the running platform. Windows ships a zip, Linux a
/// tarball — both are flattened into the runtime directory.
pub async fn scrcpy(app: tauri::AppHandle, paths: AppPaths) -> Result<(), String> {
    let text = fetch_text(SCRCPY_RELEASES).await?;
    let json: serde_json::Value = serde_json::from_str(&text).map_err(|e| e.to_string())?;
    let assets = json["assets"].as_array().ok_or("no assets")?;

    let (asset_name, url) = pick_asset_url(assets).ok_or("errors.scrcpyAssetMissing")?;
    let is_tar = asset_name.ends_with(".tar.gz") || asset_name.ends_with(".tgz");
    let archive = paths
        .runtime_dir
        .join(if is_tar { "scrcpy.tar.gz" } else { "scrcpy.zip" });
    download_file(&app, &url, &archive, "scrcpy").await?;

    let staging = paths.runtime_dir.join("scrcpy-tmp");
    let _ = fs::remove_dir_all(&staging);
    fs::create_dir_all(&staging).map_err(|e| e.to_string())?;
    extract_archive(&archive, &staging)?;

    // The archive contains a single scrcpy-<platform>-vX folder.
    let mut root = staging.clone();
    if let Ok(entries) = fs::read_dir(&staging) {
        if let Some(dir) = entries
            .flatten()
            .map(|entry| entry.path())
            .find(|path| path.is_dir())
        {
            root = dir;
        }
    }

    copy_dir_contents(&root, &paths.runtime_dir)?;
    // Cleanup is best effort: a file may still be scanned by the OS.
    let _ = retry_io(|| fs::remove_dir_all(&staging));
    let _ = retry_io(|| fs::remove_file(&archive));

    let binary = tools::find_tool(&paths, "scrcpy").ok_or("errors.scrcpyMissing")?;
    chmod_executable(&binary);
    Ok(())
}

pub async fn bloatware_list(paths: AppPaths) -> Result<String, String> {
    let text = fetch_text(UAD_LISTS_URL).await?;
    let destination = paths.runtime_dir.join("uad_lists.json");
    fs::write(&destination, &text).map_err(|e| e.to_string())?;
    Ok(destination.to_string_lossy().to_string())
}

fn releases_url_from_links(text: &str) -> Option<String> {
    let json: serde_json::Value = serde_json::from_str(text).ok()?;
    json.get(0)?.get("url")?.as_str().map(|s| s.to_string())
}

pub async fn check_update(paths: AppPaths) -> Result<UpdateInfo, String> {
    let _ = &paths;
    let fallback = releases_url();
    let releases_url = match fetch_text(LINKS_URL).await {
        Ok(text) => releases_url_from_links(&text).unwrap_or(fallback),
        Err(_) => fallback,
    };

    let text = fetch_text(&releases_url).await?;
    let json: serde_json::Value = serde_json::from_str(&text).map_err(|e| e.to_string())?;
    let latest_release = json
        .as_array()
        .and_then(|list| list.first())
        .ok_or("errors.noRelease")?;

    let latest = latest_release["tag_name"].as_str().unwrap_or("").to_string();
    let body = latest_release["body"].as_str().unwrap_or("").to_string();
    let html_url = latest_release["html_url"].as_str().unwrap_or("").to_string();

    // Pick the asset that matches the running platform, falling back to the first.
    let asset_url = latest_release["assets"]
        .as_array()
        .map(|assets| {
            pick_asset_url(assets)
                .map(|(_, url)| url)
                .or_else(|| {
                    assets
                        .first()
                        .and_then(|asset| asset["browser_download_url"].as_str())
                        .map(str::to_string)
                })
                .unwrap_or_default()
        })
        .unwrap_or_default();

    let current = current_version();
    Ok(UpdateInfo {
        new_version: compare_versions(&latest, &current) > 0,
        latest,
        current,
        body,
        html_url,
        asset_url,
    })
}

/// Configured app version. Kept in sync with Cargo.toml / tauri.conf.json.
pub fn current_version() -> String {
    format!("v{}", env!("CARGO_PKG_VERSION"))
}

/// Port of `Utils.Version.CompareVersions`.
pub fn compare_versions(a: &str, b: &str) -> i32 {
    let split = |version: &str| -> (Vec<i32>, bool) {
        let trimmed = version.trim().trim_start_matches('v');
        let mut parts = trimmed.splitn(2, "_Pre-release");
        let numbers: Vec<i32> = parts
            .next()
            .unwrap_or("")
            .split('.')
            .map(|part| part.trim().parse::<i32>().unwrap_or(0))
            .collect();
        let pre_release = parts.next().is_some();
        (numbers, pre_release)
    };

    let (mut left, left_pre) = split(a);
    let (mut right, right_pre) = split(b);
    let len = left.len().max(right.len());
    left.resize(len, 0);
    right.resize(len, 0);

    for index in 0..len {
        if left[index] != right[index] {
            return if left[index] < right[index] { -1 } else { 1 };
        }
    }

    match (left_pre, right_pre) {
        (false, true) => 1,
        (true, false) => -1,
        _ => 0,
    }
}

/// Download the release archive, stage it, spawn the platform updater and quit.
pub async fn apply_update(
    app: tauri::AppHandle,
    paths: AppPaths,
    url: String,
) -> Result<(), String> {
    let zip_path = paths.runtime_dir.join("ATAUpdate.zip");
    let staging = paths.runtime_dir.join("ATAUpdate");
    let _ = fs::remove_dir_all(&staging);
    let _ = fs::remove_file(&zip_path);

    download_file(&app, &url, &zip_path, "update").await?;
    extract_zip(&zip_path, &staging)?;

    let exe = std::env::current_exe().map_err(|e| e.to_string())?;
    let exe_dir = exe.parent().ok_or("no exe dir")?.to_path_buf();
    let exe_name = exe
        .file_name()
        .map(|name| name.to_string_lossy().to_string())
        .unwrap_or_else(|| "ATA-GUI".into());
    #[cfg(windows)]
    let _ = &exe_dir;

    #[cfg(windows)]
    {
        let script = paths.runtime_dir.join("ATAUpdater.bat");
        let body = format!(
            "@echo off\r\nTITLE ATA-GUI Updater\r\nping 127.0.0.1 -n 3 >nul\r\ntaskkill /f /im \"{exe_name}\" >nul 2>&1\r\nxcopy /s /y /q \"%~dp0ATAUpdate\\*\" \"%~dp0\" >nul\r\nrmdir /s /q \"%~dp0ATAUpdate\"\r\ndel /q \"%~dp0ATAUpdate.zip\" >nul 2>&1\r\nstart \"\" \"%~dp0{exe_name}\"\r\nexit\r\n"
        );
        fs::write(&script, body).map_err(|e| e.to_string())?;
        let mut cmd = std::process::Command::new("cmd");
        cmd.args(["/C", "start", "", &script.to_string_lossy()]);
        tools::spawn_detached(cmd)?;
    }

    #[cfg(not(windows))]
    {
        let script = paths.runtime_dir.join("ATAUpdater.sh");
        let body = format!(
            "#!/bin/sh\nsleep 2\nDIR=\"{}\"\nSTAGE=\"{}\"\ncp -a \"$STAGE/ATAUpdate/.\" \"$DIR/\"\nchmod +x \"$DIR/{exe_name}\"\nrm -rf \"$STAGE/ATAUpdate\" \"$STAGE/ATAUpdate.zip\" \"$STAGE/ATAUpdater.sh\"\n\"$DIR/{exe_name}\" &\n",
            exe_dir.to_string_lossy(),
            paths.runtime_dir.to_string_lossy()
        );
        fs::write(&script, body).map_err(|e| e.to_string())?;
        chmod_executable(&script);
        let mut cmd = std::process::Command::new("sh");
        cmd.arg(&script);
        tools::spawn_detached(cmd)?;
    }

    app.exit(0);
    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn prefers_zip_when_a_release_ships_both_archives() {
        let names = vec![
            "ATA-GUI-3.15.0-portable-windows-x64.tar.gz".to_string(),
            "ATA-GUI-3.15.0-portable-windows-x64.zip".to_string(),
        ];
        assert_eq!(
            pick_asset(&names).as_deref(),
            Some("ATA-GUI-3.15.0-portable-windows-x64.zip")
        );
    }

    #[test]
    fn ignores_installers_the_updater_cannot_unpack() {
        // setup.exe contains no "windows"/"win" marker, so it is not selected.
        let names = vec![
            "ATA-GUI_3.15.0_x64-setup.exe".to_string(),
            "ATA-GUI-3.15.0-portable-windows-x64.zip".to_string(),
        ];
        assert_eq!(
            pick_asset(&names).as_deref(),
            Some("ATA-GUI-3.15.0-portable-windows-x64.zip")
        );
    }

    #[cfg(all(unix, not(target_os = "macos")))]
    #[test]
    fn linux_picks_the_zip_portable_archive() {
        let names = vec![
            "ATA-GUI-3.15.0-portable-linux-x64.tar.gz".to_string(),
            "ATA-GUI-3.15.0-portable-linux-x64.zip".to_string(),
            "ATA-GUI_3.15.0_amd64.deb".to_string(),
        ];
        assert_eq!(
            pick_asset(&names).as_deref(),
            Some("ATA-GUI-3.15.0-portable-linux-x64.zip")
        );
    }

    #[test]
    fn compares_versions() {
        assert_eq!(compare_versions("v3.15.0", "v3.15.0"), 0);
        assert_eq!(compare_versions("v3.16.0", "v3.15.0"), 1);
        assert_eq!(compare_versions("v3.9.9", "v3.15.0"), -1);
        assert_eq!(compare_versions("v3.15.0", "v3.15.0_Pre-release"), 1);
        assert_eq!(compare_versions("v3.15.0_Pre-release", "v3.15.0"), -1);
    }

    #[test]
    fn picks_platform_asset() {
        let names = vec![
            "ATA-GUI-linux.tar.gz".to_string(),
            "ATA-GUI-windows.zip".to_string(),
        ];
        let picked = pick_asset(&names).unwrap();
        if cfg!(windows) {
            assert!(picked.contains("windows"));
        } else {
            assert!(picked.contains("linux"));
        }
    }

    #[test]
    fn prefers_64_bit_scrcpy_assets() {
        let names = vec![
            "scrcpy-win32-v5.0.1.zip".to_string(),
            "scrcpy-win64-v5.0.1.zip".to_string(),
        ];
        if cfg!(windows) && cfg!(target_pointer_width = "64") {
            assert_eq!(pick_asset(&names).unwrap(), "scrcpy-win64-v5.0.1.zip");
        }
        // Linux builds must never pick a Windows archive.
        let linux_names = vec![
            "scrcpy-win64-v5.0.1.zip".to_string(),
            "scrcpy-linux-x86_64-v5.0.1.tar.gz".to_string(),
        ];
        if cfg!(all(unix, not(target_os = "macos"))) {
            assert_eq!(pick_asset(&linux_names).unwrap(), "scrcpy-linux-x86_64-v5.0.1.tar.gz");
        }
    }

    #[test]
    fn reads_links_json() {
        let text = r#"[{"url":"https://api.github.com/repos/msartore/ATA-GUI/releases","adbv":3103}]"#;
        assert_eq!(
            releases_url_from_links(text).unwrap(),
            "https://api.github.com/repos/msartore/ATA-GUI/releases"
        );
    }
}
