use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;
use std::collections::HashSet;
use std::fs;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct BloatEntry {
    pub package: String,
    pub description: String,
    pub removal: String,
    pub list: String,
    pub system: bool,
}

/// Android package-name validation (port of `IsValidPackageName`) — also the
/// command-injection guard before anything is passed to `pm`.
pub fn is_valid_package_name(name: &str) -> bool {
    if name.trim().is_empty() {
        return false;
    }
    for part in name.split('.') {
        if part.is_empty() {
            return false;
        }
        if part.chars().next().map(|c| c.is_ascii_digit()).unwrap_or(true) {
            return false;
        }
        if !part.chars().all(|c| c.is_alphanumeric() || c == '_') {
            return false;
        }
    }
    true
}

fn packages_with_flags(paths: &AppPaths, serial: &str, user: &str, flags: &[&str]) -> HashSet<String> {
    let mut args: Vec<String> = vec!["shell".into(), "pm".into(), "list".into(), "packages".into()];
    args.extend(flags.iter().map(|f| f.to_string()));
    args.push("--user".into());
    args.push(user.to_string());
    let text = tools::adb_text(paths, Some(serial), &args);
    crate::apps::parse_package_list(&text).into_iter().collect()
}

fn installed_sets(
    paths: &AppPaths,
    serial: &str,
    user: &str,
) -> (HashSet<String>, HashSet<String>) {
    let system = packages_with_flags(paths, serial, user, &["-s"]);
    let non_system = packages_with_flags(paths, serial, user, &["-3"]);
    (system, non_system)
}

pub fn list_path(paths: &AppPaths) -> std::path::PathBuf {
    paths.runtime_dir.join("uad_lists.json")
}

pub fn scan(
    paths: &AppPaths,
    serial: &str,
    user: &str,
    level: &str,
) -> Result<Vec<BloatEntry>, String> {
    let text =
        fs::read_to_string(list_path(paths)).map_err(|_| "errors.bloatwareMissing".to_string())?;
    let json: serde_json::Value =
        serde_json::from_str(&text).map_err(|_| "errors.bloatwareInvalid".to_string())?;
    let object = json
        .as_object()
        .ok_or_else(|| "errors.bloatwareInvalid".to_string())?;

    let (system_set, non_system_set) = installed_sets(paths, serial, user);
    let mut entries = Vec::new();

    for (package, value) in object {
        if !is_valid_package_name(package) {
            continue;
        }
        if !system_set.contains(package) && !non_system_set.contains(package) {
            continue;
        }

        let removal = value["removal"].as_str().unwrap_or("").to_string();
        let lower = removal.to_ascii_lowercase();
        let include = match level {
            "basic" => lower == "recommended",
            "medium" => lower == "recommended" || lower == "advanced",
            "advanced" | "advance" => lower != "expert",
            "expert" => true,
            _ => lower == "recommended",
        };

        if !include {
            continue;
        }

        entries.push(BloatEntry {
            package: package.clone(),
            description: value["description"].as_str().unwrap_or("").to_string(),
            removal,
            list: value["list"].as_str().unwrap_or("").to_string(),
            system: system_set.contains(package),
        });
    }

    entries.sort_by(|a, b| a.package.cmp(&b.package));
    Ok(entries)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn validates_package_names() {
        assert!(is_valid_package_name("com.example.app"));
        assert!(is_valid_package_name("com.example.app_2"));
        assert!(!is_valid_package_name(""));
        assert!(!is_valid_package_name("com..app"));
        assert!(!is_valid_package_name("com.2example"));
        assert!(!is_valid_package_name("com.example;rm -rf /"));
    }
}
