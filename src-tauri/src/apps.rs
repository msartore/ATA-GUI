use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;
use std::fs;
use std::time::Duration;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct AppEntry {
    pub package: String,
    /// `None` when the package name is unknown (ATA-Bridge not installed).
    pub name: Option<String>,
}

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct AppsResult {
    pub apps: Vec<AppEntry>,
    pub bridge: bool,
    pub mode: String,
    pub total: usize,
}

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct OpResult {
    pub target: String,
    pub ok: bool,
    pub output: String,
}

fn pm_list(paths: &AppPaths, serial: &str, user: &str, flags: &[&str]) -> Vec<String> {
    let mut args: Vec<String> = vec!["shell".into(), "pm".into(), "list".into(), "packages".into()];
    for flag in flags {
        args.push((*flag).to_string());
    }
    args.push("--user".into());
    args.push(user.to_string());

    let text = tools::adb_text(paths, Some(serial), &args);
    parse_package_list(&text)
}

pub fn parse_package_list(text: &str) -> Vec<String> {
    let mut packages: Vec<String> = text
        .lines()
        .filter_map(|line| line.trim().strip_prefix("package:").map(|p| p.trim().to_string()))
        .filter(|p| !p.is_empty())
        .collect();
    packages.sort();
    packages.dedup();
    packages
}

pub fn bridge_installed(paths: &AppPaths, serial: &str) -> bool {
    let args = vec![
        "shell".to_string(),
        "pm".to_string(),
        "list".to_string(),
        "packages".to_string(),
        "dev.msartore.atabridge".to_string(),
    ];
    !tools::adb_text(paths, Some(serial), &args).trim().is_empty()
}

/// Ask ATA-Bridge to export the human readable app names and pull the JSON.
fn bridge_apps(paths: &AppPaths, serial: &str) -> Vec<AppEntry> {
    let _ = tools::adb(
        paths,
        Some(serial),
        &[
            "shell".into(),
            "am".into(),
            "start".into(),
            "-a".into(),
            "dev.msartore.ACTION_SHARE_APP_LIST".into(),
            "-c".into(),
            "android.intent.category.DEFAULT".into(),
            "-n".into(),
            "dev.msartore.atabridge/dev.msartore.atabridge.MainActivity".into(),
        ],
    );

    std::thread::sleep(Duration::from_millis(1200));

    let destination = paths.runtime_dir.join("installed_apps.json");
    let _ = fs::remove_file(&destination);
    let _ = tools::adb(
        paths,
        Some(serial),
        &[
            "pull".into(),
            "sdcard/Android/data/dev.msartore.atabridge/cache/installed_apps.json".into(),
            destination.to_string_lossy().to_string(),
        ],
    );

    let Ok(text) = fs::read_to_string(&destination) else {
        return Vec::new();
    };

    #[derive(serde::Deserialize)]
    struct BridgeApp {
        #[serde(default)]
        name: String,
        #[serde(default)]
        package: String,
    }

    match serde_json::from_str::<Vec<BridgeApp>>(&text) {
        Ok(list) => list
            .into_iter()
            .filter(|entry| !entry.package.is_empty())
            .map(|entry| AppEntry {
                package: entry.package,
                name: if entry.name.trim().is_empty() { None } else { Some(entry.name) },
            })
            .collect(),
        Err(_) => Vec::new(),
    }
}

/// Port of `MainForm.loadApps` — returns the package list for the requested
/// filter, enriched with app names when ATA-Bridge is available.
pub fn load(
    paths: &AppPaths,
    serial: &str,
    user: &str,
    mode: &str,
) -> Result<AppsResult, String> {
    let bridge = bridge_installed(paths, serial);
    let bridge_entries = if bridge { bridge_apps(paths, serial) } else { Vec::new() };

    let packages: Vec<String> = match mode {
        "system" => pm_list(paths, serial, user, &["-s"]),
        "nonSystem" => pm_list(paths, serial, user, &["-3"]),
        "disabled" => pm_list(paths, serial, user, &["-d"]),
        "uninstalled" => {
            let installed = pm_list(paths, serial, user, &[]);
            let known = pm_list(paths, serial, user, &["-u"]);
            known
                .into_iter()
                .filter(|pkg| !installed.contains(pkg))
                .collect()
        }
        _ => pm_list(paths, serial, user, &[]),
    };

    let apps = packages
        .into_iter()
        .map(|package| {
            let name = bridge_entries
                .iter()
                .find(|entry| entry.package == package)
                .and_then(|entry| entry.name.clone());
            AppEntry { package, name }
        })
        .collect::<Vec<_>>();

    Ok(AppsResult {
        total: apps.len(),
        apps,
        bridge,
        mode: mode.to_string(),
    })
}

fn looks_failed(output: &str) -> bool {
    let lower = output.to_ascii_lowercase();
    lower.contains("failure")
        || lower.contains("failed")
        || lower.contains("error")
        || lower.contains("not found")
        || lower.contains("doesn't exist")
        || lower.contains("unknown package")
        || lower.contains("exception")
}

/// enable / disable / clear-data / uninstall / restore for a batch of packages.
pub fn apply(
    paths: &AppPaths,
    serial: &str,
    user: &str,
    action: &str,
    packages: &[String],
) -> Vec<OpResult> {
    if !matches!(action, "enable" | "disable" | "clear" | "uninstall" | "restore") {
        // An unknown action used to fall through to an empty argv: adb printed
        // its usage text (containing no failure words) and every package was
        // reported as successfully processed.
        return packages
            .iter()
            .map(|package| OpResult {
                target: package.clone(),
                ok: false,
                output: format!("errors.unknownAction: {action}"),
            })
            .collect();
    }

    packages
        .iter()
        .map(|package| {
            let args: Vec<String> = match action {
                "enable" => vec![
                    "shell".into(), "pm".into(), "enable".into(), "--user".into(), user.into(), package.clone(),
                ],
                "disable" => vec![
                    "shell".into(), "pm".into(), "disable-user".into(), "--user".into(), user.into(), package.clone(),
                ],
                "clear" => vec![
                    "shell".into(), "pm".into(), "clear".into(), "--user".into(), user.into(), package.clone(),
                ],
                "uninstall" => vec![
                    "shell".into(), "pm".into(), "uninstall".into(), "-k".into(), "--user".into(), user.into(), package.clone(),
                ],
                "restore" => vec![
                    "shell".into(), "cmd".into(), "package".into(), "install-existing".into(), "--user".into(), user.into(), package.clone(),
                ],
                _ => vec![],
            };

            match tools::adb(paths, Some(serial), &args) {
                Ok(out) => {
                    let output = out.merged();
                    OpResult {
                        target: package.clone(),
                        ok: !looks_failed(&output) && !output.trim().is_empty(),
                        output,
                    }
                }
                Err(err) => OpResult {
                    target: package.clone(),
                    ok: false,
                    output: err,
                },
            }
        })
        .collect()
}

pub fn grant(
    paths: &AppPaths,
    serial: &str,
    user: &str,
    package: &str,
    permission: &str,
    method: &str,
) -> Result<tools::CmdOut, String> {
    let args = vec![
        "shell".into(),
        "pm".into(),
        method.to_string(),
        package.to_string(),
        format!("android.permission.{permission}"),
        "--user".into(),
        user.to_string(),
    ];
    tools::adb(paths, Some(serial), &args)
}

pub fn install_apks(
    paths: &AppPaths,
    serial: &str,
    user: &str,
    files: &[String],
    downgrade: bool,
) -> Vec<OpResult> {
    files
        .iter()
        .filter(|file| file.to_ascii_lowercase().ends_with(".apk"))
        .map(|file| {
            let mut args: Vec<String> = vec!["install".into(), "-r".into()];
            if downgrade {
                args.push("-d".into());
            }
            args.push("--user".into());
            args.push(user.to_string());
            args.push(file.clone());

            match tools::adb(paths, Some(serial), &args) {
                Ok(out) => {
                    let output = out.merged();
                    OpResult {
                        target: file.clone(),
                        ok: output.contains("Success"),
                        output,
                    }
                }
                Err(err) => OpResult {
                    target: file.clone(),
                    ok: false,
                    output: err,
                },
            }
        })
        .collect()
}

pub fn push_files(paths: &AppPaths, serial: &str, files: &[String]) -> Vec<OpResult> {
    let _ = tools::adb(
        paths,
        Some(serial),
        &["shell".into(), "mkdir".into(), "-p".into(), "sdcard/ATA".into()],
    );

    files
        .iter()
        .map(|file| {
            if !std::path::Path::new(file).exists() {
                return OpResult {
                    target: file.clone(),
                    ok: false,
                    output: "file not found".into(),
                };
            }
            let args = vec!["push".to_string(), file.clone(), "sdcard/ATA".to_string()];
            match tools::adb(paths, Some(serial), &args) {
                Ok(out) => {
                    let output = out.merged();
                    OpResult {
                        target: file.clone(),
                        ok: !looks_failed(&output),
                        output,
                    }
                }
                Err(err) => OpResult {
                    target: file.clone(),
                    ok: false,
                    output: err,
                },
            }
        })
        .collect()
}

/// Pull every split APK of the selected packages into `APKS/`.
pub fn extract_apks(paths: &AppPaths, serial: &str, packages: &[String]) -> Vec<OpResult> {
    let dir = paths.apks_dir();
    let _ = fs::create_dir_all(&dir);

    let mut results = Vec::new();
    for package in packages {
        let text = tools::adb_text(
            paths,
            Some(serial),
            &["shell".into(), "pm".into(), "path".into(), package.clone()],
        );

        let apk_paths: Vec<String> = text
            .lines()
            .filter(|line| line.contains("package:"))
            .map(|line| line.replace("package:", "").trim().to_string())
            .filter(|line| !line.is_empty())
            .collect();

        if apk_paths.is_empty() {
            results.push(OpResult {
                target: package.clone(),
                ok: false,
                output: text,
            });
            continue;
        }

        for apk in apk_paths {
            let file_name = apk.rsplit('/').next().unwrap_or("base.apk");
            let destination = dir.join(format!("{package}_{file_name}"));
            let args = vec![
                "pull".to_string(),
                apk.clone(),
                destination.to_string_lossy().to_string(),
            ];
            match tools::adb(paths, Some(serial), &args) {
                Ok(out) => {
                    let output = out.merged();
                    results.push(OpResult {
                        target: destination.to_string_lossy().to_string(),
                        ok: output.contains("file pulled") || output.contains("1 file"),
                        output,
                    });
                }
                Err(err) => results.push(OpResult {
                    target: apk,
                    ok: false,
                    output: err,
                }),
            }
        }
    }
    results
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parses_package_list() {
        let text = "package:com.android.chrome\npackage:com.example.app\n";
        let packages = parse_package_list(text);
        assert_eq!(packages, vec!["com.android.chrome", "com.example.app"]);
    }

    #[test]
    fn detects_failures() {
        assert!(looks_failed("Failure [not installed for 0]"));
        assert!(looks_failed("Error: unknown package"));
        assert!(!looks_failed("Success"));
    }
}
