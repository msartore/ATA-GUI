//! Translation catalogs.
//!
//! Every locale ships inside the binary so the app is always translated, but
//! any `locales/*.json` file found next to the executable (portable build) or
//! in the user data/config directories overrides/adds a language at runtime.
//! Adding a language never requires a rebuild.

use crate::state::AppPaths;
use serde::Serialize;
use std::collections::BTreeMap;
use std::fs;
use std::path::PathBuf;

/// Compiled-in catalogs. Keep in sync with `src/i18n/locales/*.json`.
const BUNDLED: &[(&str, &str)] = &[
    ("en", include_str!("../../src/i18n/locales/en.json")),
    ("it", include_str!("../../src/i18n/locales/it.json")),
    ("es", include_str!("../../src/i18n/locales/es.json")),
    ("de", include_str!("../../src/i18n/locales/de.json")),
    ("fr", include_str!("../../src/i18n/locales/fr.json")),
    ("pt-BR", include_str!("../../src/i18n/locales/pt-BR.json")),
    ("ru", include_str!("../../src/i18n/locales/ru.json")),
    ("zh-CN", include_str!("../../src/i18n/locales/zh-CN.json")),
];

const TEMPLATE: &str = "_template.json";
const README: &str = "README.txt";

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct LocaleMeta {
    pub code: String,
    pub name: String,
    /// "bundled" or "external"
    pub source: String,
    pub path: Option<String>,
    pub valid: bool,
}

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct LocaleBundle {
    pub code: String,
    pub name: String,
    pub source: String,
    pub json: String,
}

/// Directories scanned for translations, highest priority first.
pub fn external_dirs(paths: &AppPaths) -> Vec<PathBuf> {
    vec![
        paths.runtime_dir.join("locales"),
        paths.exe_dir.join("resources").join("locales"),
        paths.data_dir.join("locales"),
        paths.config_dir.join("locales"),
    ]
}

fn parse_object(json: &str) -> Option<serde_json::Map<String, serde_json::Value>> {
    match serde_json::from_str::<serde_json::Value>(json) {
        Ok(serde_json::Value::Object(map)) => Some(map),
        _ => None,
    }
}

fn is_valid(json: &str) -> bool {
    parse_object(json)
        .map(|map| map.values().all(|value| value.is_string()))
        .unwrap_or(false)
}

fn display_name(json: &str, fallback: &str) -> String {
    parse_object(json)
        .and_then(|map| map.get("_meta.name").and_then(|value| value.as_str()).map(str::to_string))
        .map(|name| name.trim().to_string())
        .filter(|name| !name.is_empty())
        .unwrap_or_else(|| fallback.to_string())
}

fn external_files(paths: &AppPaths) -> Vec<(String, PathBuf, String, bool)> {
    let mut files = Vec::new();
    for dir in external_dirs(paths) {
        let Ok(entries) = fs::read_dir(&dir) else {
            continue;
        };
        for entry in entries.flatten() {
            let path = entry.path();
            if !path.is_file() {
                continue;
            }
            if path
                .extension()
                .and_then(|ext| ext.to_str())
                .map(|ext| !ext.eq_ignore_ascii_case("json"))
                .unwrap_or(true)
            {
                continue;
            }
            let Some(code) = path.file_stem().and_then(|stem| stem.to_str()) else {
                continue;
            };
            if code.starts_with('_') {
                continue; // _template.json and friends
            }
            let Ok(json) = fs::read_to_string(&path) else {
                continue;
            };
            let valid = is_valid(&json);
            files.push((code.to_string(), path, json, valid));
        }
    }
    files
}

/// Bundled catalogs merged with external overrides. External files win.
pub fn bundles(paths: &AppPaths) -> Vec<LocaleBundle> {
    let mut merged: BTreeMap<String, LocaleBundle> = BTreeMap::new();

    for (code, json) in BUNDLED {
        merged.insert(
            (*code).to_string(),
            LocaleBundle {
                code: (*code).to_string(),
                name: display_name(json, code),
                source: "bundled".into(),
                json: (*json).to_string(),
            },
        );
    }

    for (code, _path, json, valid) in external_files(paths) {
        if !valid {
            continue;
        }
        merged.insert(
            code.clone(),
            LocaleBundle {
                code: code.clone(),
                name: display_name(&json, &code),
                source: "external".into(),
                json,
            },
        );
    }

    let mut out: Vec<LocaleBundle> = merged.into_values().collect();
    out.sort_by(|a, b| {
        let left = (a.code != "en", a.name.to_lowercase());
        let right = (b.code != "en", b.name.to_lowercase());
        left.cmp(&right)
    });
    out
}

/// Lightweight metadata for the settings UI (includes invalid external files so
/// a broken translation can be reported instead of silently ignored).
pub fn catalog(paths: &AppPaths) -> Vec<LocaleMeta> {
    let mut catalog: Vec<LocaleMeta> = bundles(paths)
        .into_iter()
        .map(|bundle| LocaleMeta {
            code: bundle.code,
            name: bundle.name,
            source: bundle.source,
            path: None,
            valid: true,
        })
        .collect();

    for (code, path, json, valid) in external_files(paths) {
        if valid {
            if let Some(entry) = catalog.iter_mut().find(|entry| entry.code == code) {
                entry.path = Some(path.to_string_lossy().to_string());
                entry.source = "external".into();
                entry.name = display_name(&json, &code);
            }
        } else {
            catalog.push(LocaleMeta {
                code,
                name: path
                    .file_name()
                    .map(|name| name.to_string_lossy().to_string())
                    .unwrap_or_default(),
                source: "external".into(),
                path: Some(path.to_string_lossy().to_string()),
                valid: false,
            });
        }
    }

    catalog
}

fn readme_text() -> String {
    r#"ATA-GUI translations
=====================

Every *.json file in this folder is a language. The file name is the language
code shown in Settings (for example "it.json" -> "it"), and the "_meta.name"
key is the label shown in the language picker.

To translate the app:
  1. copy _template.json to <code>.json (for example nl.json);
  2. set "_meta.name" to the language name, in that language;
  3. translate the values, never the keys, and keep {placeholders} intact;
  4. choose the language in Settings, or press "Reload languages" if the app is
     already open.

Keys missing from a file fall back to English, so a partial translation works.
The bundled languages (en, it, es, de, fr, pt-BR, ru, zh-CN) can be overridden by
placing a file with the same code here. Where this folder lives:

  * portable build: next to the ATA-GUI executable (this folder);
  * installed build: the path shown by Settings -> Translations.

Invalid JSON is reported in Settings and ignored.
"#
    .to_string()
}

/// Create the locales folder next to the app (if missing) with a template and a
/// short README. Returns the folder path.
pub fn ensure_dir(paths: &AppPaths) -> Result<PathBuf, String> {
    let dir = paths.runtime_dir.join("locales");
    fs::create_dir_all(&dir).map_err(|error| error.to_string())?;

    let template = dir.join(TEMPLATE);
    if !template.is_file() {
        let english = BUNDLED
            .iter()
            .find(|(code, _)| *code == "en")
            .map(|(_, json)| *json)
            .unwrap_or("{}");
        fs::write(&template, english).map_err(|error| error.to_string())?;
    }

    let readme = dir.join(README);
    if !readme.is_file() {
        fs::write(&readme, readme_text()).map_err(|error| error.to_string())?;
    }

    Ok(dir)
}

/// Seed the compiled-in catalogues next to the executable when the folder is
/// missing. A 3.x install upgraded by the old WinForms updater cannot move
/// directories (`move *` only moves files), so the app restores `locales/`
/// itself on first start. Best effort: read-only install dirs are ignored.
pub fn seed(paths: &AppPaths) {
    let dir = paths.runtime_dir.join("locales");
    if dir.exists() || fs::create_dir_all(&dir).is_err() {
        return;
    }

    for (code, json) in BUNDLED {
        let _ = fs::write(dir.join(format!("{code}.json")), json);
    }
    if let Some((_, english)) = BUNDLED.iter().find(|(code, _)| *code == "en") {
        let _ = fs::write(dir.join(TEMPLATE), english);
    }
    let _ = fs::write(dir.join(README), readme_text());
}

pub fn open_dir(paths: &AppPaths) -> Result<String, String> {
    let dir = ensure_dir(paths)?;
    crate::tools::open_external(&dir.to_string_lossy())?;
    Ok(dir.to_string_lossy().to_string())
}

pub fn export_template(paths: &AppPaths) -> Result<String, String> {
    let dir = ensure_dir(paths)?;
    Ok(dir.join(TEMPLATE).to_string_lossy().to_string())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn bundled_catalogs_are_valid_and_translated() {
        assert!(BUNDLED.len() >= 8);
        for (code, json) in BUNDLED {
            assert!(is_valid(json), "{code} is not a flat string map");
            let name = display_name(json, code);
            assert!(!name.is_empty(), "{code} has no display name");
        }
    }

    #[test]
    fn bundled_catalogs_share_the_same_keys() {
        let reference = parse_object(BUNDLED[0].1).unwrap();
        let reference_keys: Vec<&String> = reference.keys().collect();
        for (code, json) in BUNDLED.iter().skip(1) {
            let map = parse_object(json).unwrap();
            let keys: Vec<&String> = map.keys().collect();
            assert_eq!(reference_keys, keys, "{code} key set differs from en");
        }
    }

    #[test]
    fn rejects_non_string_values() {
        assert!(!is_valid(r#"{"a": 1}"#));
        assert!(!is_valid("[]"));
        assert!(is_valid(r#"{"_meta.name":"Test","a":"b"}"#));
    }
}
