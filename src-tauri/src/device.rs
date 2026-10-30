use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;
use std::collections::BTreeMap;
use std::net::Ipv4Addr;
use std::collections::HashMap;
use std::sync::LazyLock;
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::thread;
use std::time::{Duration, Instant};

#[derive(Serialize, Clone, Debug, PartialEq)]
#[serde(rename_all = "camelCase")]
pub struct DeviceEntry {
    pub id: String,
    pub name: String,
    pub mode: String,
    pub connection: String,
}

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct UserEntry {
    pub id: String,
    pub name: String,
}

/// Device states `adb devices` reports. Anything else on stdout is daemon noise
/// ("could not read ok from ADB Server", "adb.exe: failed to ...") from a server
/// that was killed a moment earlier, and must not become a fake device.
const DEVICE_STATES: &[&str] = &[
    "device",
    "offline",
    "unauthorized",
    "authorizing",
    "recovery",
    "sideload",
    "bootloader",
    "fastboot",
    "fastbootd",
    "connecting",
    "rescue",
    "host",
];

/// Parse `adb devices` / `fastboot devices` output into (id, state) pairs.
pub fn parse_devices(text: &str) -> Vec<(String, String)> {
    text.lines()
        .filter_map(|line| {
            let line = line.trim();
            if line.is_empty()
                || line.starts_with("List of devices")
                || line.starts_with("attached")
                || line.starts_with("*")
            {
                return None;
            }
            let mut parts = line.split_whitespace();
            let id = parts.next()?.to_string();
            if line.contains("no permissions") {
                return Some((id, "unauthorized".to_string()));
            }
            let state = parts.next().unwrap_or("");
            if !DEVICE_STATES.contains(&state) {
                return None;
            }
            Some((id, state.to_string()))
        })
        .collect()
}

/// `ro.serialno` + model per transport, cached: the watcher polls every second
/// and a shell round-trip per device per tick is wasted work.
#[derive(Clone)]
struct Identity {
    serial: String,
    model: String,
    at: Instant,
}

static IDENTITIES: LazyLock<Mutex<HashMap<String, Identity>>> =
    LazyLock::new(|| Mutex::new(HashMap::new()));

fn identity(paths: &AppPaths, id: &str) -> Identity {
    if let Ok(cache) = IDENTITIES.lock() {
        if let Some(hit) = cache.get(id) {
            if hit.at.elapsed() < Duration::from_secs(60) {
                return hit.clone();
            }
        }
    }

    let text = tools::adb_text(
        paths,
        Some(id),
        &[
            "shell".into(),
            "getprop ro.serialno; getprop ro.product.model".into(),
        ],
    );
    let mut lines = text.lines().map(|line| line.trim().to_string());
    let serial = lines.next().unwrap_or_default();
    let model = lines.next().unwrap_or_default();

    let identity = Identity {
        serial,
        model,
        at: Instant::now(),
    };
    if let Ok(mut cache) = IDENTITIES.lock() {
        cache.insert(id.to_string(), identity.clone());
    }
    identity
}

pub fn mode_for(state: &str) -> &'static str {
    match state {
        "recovery" => "RECOVERY",
        "sideload" => "SIDELOAD",
        "device" => "SYSTEM",
        "unauthorized" | "authorizing" => "UNAUTHORIZED",
        "fastboot" | "fastbootd" | "bootloader" => "FASTBOOT",
        _ => "UNKNOWN",
    }
}

pub fn is_valid_ip(text: &str) -> bool {
    text.trim().parse::<Ipv4Addr>().is_ok()
}

/// Parse an `adb devices` line's connection type from the serial.
pub fn connection_for(id: &str) -> &'static str {
    let host = id.split(':').next().unwrap_or(id);
    if id.contains("tcp") || id.contains("_adb-tls") || is_valid_ip(host) || id.contains(':') {
        "WIRELESS"
    } else {
        "CABLE"
    }
}

pub fn list(paths: &AppPaths, is_adb: bool) -> Result<Vec<DeviceEntry>, String> {
    let args = vec!["devices".to_string()];
    let out = if is_adb {
        tools::adb(paths, None, &args)?
    } else {
        tools::fastboot(paths, None, &args)?
    };

    let mut found: Vec<(String, DeviceEntry)> = Vec::new();
    for (id, state) in parse_devices(&out.merged()) {
        let identity = if is_adb && state == "device" {
            identity(paths, &id)
        } else {
            Identity {
                serial: String::new(),
                model: String::new(),
                at: Instant::now(),
            }
        };

        let entry = DeviceEntry {
            id: id.clone(),
            name: identity.model,
            mode: mode_for(&state).to_string(),
            connection: if is_adb {
                connection_for(&id).to_string()
            } else {
                "CABLE".into()
            },
        };
        found.push((identity.serial, entry));
    }

    Ok(dedupe(found))
}

/// adb happily exposes one phone twice: its mDNS service name next to the
/// resolved `ip:port`, or the wireless-debugging port next to legacy 5555.
/// Keep one row per physical device (`ro.serialno`), preferring a numeric
/// endpoint on a non-5555 port that is actually `device`.
fn dedupe(found: Vec<(String, DeviceEntry)>) -> Vec<DeviceEntry> {
    fn score(serial: &str, entry: &DeviceEntry) -> i32 {
        let mut score = 0;
        if !serial.is_empty() {
            score += 1;
        }
        if entry.mode == "SYSTEM" {
            score += 4;
        }
        // A cabled transport is the faster, steadier one: when the same phone
        // is also on Wi-Fi, keep the cable row.
        if entry.connection == "CABLE" {
            score += 3;
        }
        let host = entry.id.split(':').next().unwrap_or(&entry.id);
        if is_valid_ip(host) {
            score += 2;
        }
        if entry.id.ends_with(":5555") {
            score -= 1;
        }
        score
    }

    let mut kept: Vec<(String, DeviceEntry)> = Vec::new();
    for (serial, entry) in found {
        let key = if serial.is_empty() {
            entry.id.clone()
        } else {
            serial.clone()
        };
        match kept.iter().position(|(known_serial, known)| {
            let known_key = if known_serial.is_empty() {
                known.id.clone()
            } else {
                known_serial.clone()
            };
            known_key == key
        }) {
            Some(index) => {
                if score(&serial, &entry) > score(&kept[index].0, &kept[index].1) {
                    kept[index] = (serial, entry);
                }
            }
            None => kept.push((serial, entry)),
        }
    }

    kept.into_iter().map(|(_, entry)| entry).collect()
}

/// Full `getprop` dump as a key/value map (single adb round-trip instead of
/// the seven separate `getprop` calls the WinForms app made).
pub fn getprops(paths: &AppPaths, serial: &str) -> BTreeMap<String, String> {
    let text = tools::adb_text(
        paths,
        Some(serial),
        &["shell".into(), "getprop".into()],
    );
    parse_getprop(&text)
}

static GETPROP_RE: LazyLock<regex::Regex> = LazyLock::new(|| {
    regex::Regex::new(r"^\[(?P<key>[^\]]+)\]:\s*\[(?P<value>.*)\]\s*$")
        .expect("getprop regex is valid")
});

pub fn parse_getprop(text: &str) -> BTreeMap<String, String> {
    let mut map = BTreeMap::new();
    for line in text.lines() {
        if let Some(caps) = GETPROP_RE.captures(line.trim()) {
            map.insert(caps["key"].to_string(), caps["value"].to_string());
        }
    }
    map
}

pub fn users(paths: &AppPaths, serial: &str) -> Vec<UserEntry> {
    let text = tools::adb_text(
        paths,
        Some(serial),
        &["shell".into(), "pm".into(), "list".into(), "users".into()],
    );
    parse_users(&text)
}

static USERS_RE: LazyLock<regex::Regex> =
    LazyLock::new(|| regex::Regex::new(r"UserInfo\{(\d+):([^:}]*)").expect("users regex is valid"));

pub fn parse_users(text: &str) -> Vec<UserEntry> {
    let re = &*USERS_RE;
    let mut users: Vec<UserEntry> = re
        .captures_iter(text)
        .map(|caps| UserEntry {
            id: caps[1].to_string(),
            name: caps[2].to_string(),
        })
        .collect();

    // Prefer the "running" user, exactly like the original implementation.
    if let Some(running_line) = text.lines().find(|l| l.contains("running")) {
        if let Some(caps) = re.captures(running_line) {
            let id = caps[1].to_string();
            let name = caps[2].to_string();
            users.retain(|u| u.id != id);
            users.insert(0, UserEntry { id, name });
        }
    }
    users
}

pub fn primary_user(paths: &AppPaths, serial: &str) -> String {
    users(paths, serial)
        .first()
        .map(|u| u.id.clone())
        .unwrap_or_else(|| "0".to_string())
}

#[derive(Serialize, Clone, Debug, Default)]
#[serde(rename_all = "camelCase")]
pub struct DeviceInfo {
    pub serial: String,
    pub android_version: String,
    pub sdk: i32,
    pub build_user: String,
    pub cpu_abilist: String,
    pub manufacturer: String,
    pub model: String,
    pub board: String,
    pub device: String,
    pub ip: String,
    pub user: String,
    pub wireless: bool,
    pub rotation_free: bool,
    pub max_display: i32,
    pub supported: bool,
    /// i18n key when the device cannot be used (see src/i18n dictionaries).
    pub error: Option<String>,
}

fn prop_or_unknown(props: &BTreeMap<String, String>, key: &str) -> String {
    match props.get(key).map(|s| s.trim()).filter(|s| !s.is_empty()) {
        Some(value) => value.to_string(),
        None => String::new(),
    }
}

pub fn info(paths: &AppPaths, serial: &str) -> Result<DeviceInfo, String> {
    let props = getprops(paths, serial);
    let sdk_raw = prop_or_unknown(&props, "ro.build.version.sdk");
    let mut info = DeviceInfo {
        serial: serial.to_string(),
        android_version: prop_or_unknown(&props, "ro.build.version.release"),
        sdk: sdk_raw.parse().unwrap_or(0),
        build_user: prop_or_unknown(&props, "ro.build.user"),
        cpu_abilist: prop_or_unknown(&props, "ro.product.cpu.abilist"),
        manufacturer: prop_or_unknown(&props, "ro.product.manufacturer"),
        model: prop_or_unknown(&props, "ro.product.model"),
        board: prop_or_unknown(&props, "ro.product.board"),
        device: prop_or_unknown(&props, "ro.product.device"),
        ..Default::default()
    };

    if sdk_raw.trim().is_empty() {
        info.error = Some("errors.androidVersionNotFound".into());
        return Ok(info);
    }
    if !sdk_raw.chars().any(|c| c.is_ascii_digit()) {
        info.error = Some("errors.deviceNotSupported".into());
        return Ok(info);
    }
    if info.sdk <= 20 {
        info.error = Some("errors.androidTooOld".into());
        return Ok(info);
    }

    // Local IP, taken from the routing table like the original.
    let route = tools::adb_text(paths, Some(serial), &["shell".into(), "ip".into(), "route".into()]);
    let ip = route
        .split_whitespace()
        .rfind(|token| is_valid_ip(token))
        .unwrap_or("")
        .to_string();
    info.ip = ip;

    info.user = primary_user(paths, serial);
    info.rotation_free = rotation_get(paths, serial);
    info.max_display = max_display(paths, serial);
    info.wireless = tools::adb_text(
        paths,
        Some(serial),
        &["shell".into(), "getprop".into(), "service.adb.tcp.port".into()],
    )
    .trim()
    .parse::<u32>()
    .map(|port| port > 0)
    .unwrap_or(false);
    info.supported = true;
    Ok(info)
}

/// Highest display id reported by the device (`cmd display get-displays`).
pub fn max_display(paths: &AppPaths, serial: &str) -> i32 {
    let text = tools::adb_text(
        paths,
        Some(serial),
        &[
            "shell".into(),
            "cmd".into(),
            "display".into(),
            "get-displays".into(),
        ],
    );
    parse_max_display(&text)
}

static DISPLAY_RE: LazyLock<regex::Regex> =
    LazyLock::new(|| regex::Regex::new(r"(?i)display\s*id[:\s]+(\d+)").expect("display regex is valid"));

pub fn parse_max_display(text: &str) -> i32 {
    if let Ok(value) = text.trim().parse::<i32>() {
        return value.max(0);
    }
    let mut max = 0;
    for caps in DISPLAY_RE.captures_iter(text) {
        if let Ok(value) = caps[1].parse::<i32>() {
            max = max.max(value);
        }
    }
    max
}

pub fn rotation_get(paths: &AppPaths, serial: &str) -> bool {
    tools::adb_text(
        paths,
        Some(serial),
        &[
            "shell".into(),
            "wm".into(),
            "get-ignore-orientation-request".into(),
        ],
    )
    .to_ascii_lowercase()
    .contains("true")
}

pub fn rotation_set(
    paths: &AppPaths,
    serial: &str,
    enabled: bool,
    display: Option<i32>,
) -> Result<bool, String> {
    let mut args = vec![
        "shell".to_string(),
        "wm".to_string(),
        "set-ignore-orientation-request".to_string(),
    ];
    if let Some(display) = display {
        args.push("-d".into());
        args.push(display.to_string());
    }
    args.push(if enabled { "true" } else { "false" }.to_string());
    tools::adb(paths, Some(serial), &args)?;
    Ok(rotation_get(paths, serial))
}

const BOOTLOADER_KEYS: &[&str] = &[
    "DP",
    "token",
    "crc",
    "parallel-download-flash",
    "anti",
    "cpuid",
    "board_version",
    "hw-revision",
    "unlocked",
    "off-mode-charge",
    "charger-screen-enabled",
    "battery-soc-ok",
    "battery-voltage",
    "erase-block-size",
    "logical-block-size",
    "variant",
    "secure",
    "serialno",
    "product",
    "max-download-size",
    "kernel",
];

static BOOTLOADER_RE: LazyLock<regex::Regex> = LazyLock::new(|| {
    regex::Regex::new(r"\(bootloader\) (?P<key>[^:]+):(?P<value>.+)").expect("bootloader regex is valid")
});

/// Port of `DeviceData.ExtractBootloaderInfo`.
pub fn extract_bootloader_info(input: &str) -> String {
    let mut out = String::new();
    for line in input.lines() {
        if let Some(caps) = BOOTLOADER_RE.captures(line) {
            let key = caps["key"].trim();
            if BOOTLOADER_KEYS.contains(&key) {
                out.push_str(&format!("{}: {}\n", key, caps["value"].trim()));
            }
        }
    }
    out.trim_end().to_string()
}

pub struct WatcherHandle {
    pub stop: Arc<AtomicBool>,
    pub join: Option<thread::JoinHandle<()>>,
}

impl WatcherHandle {
    pub fn stop(mut self) {
        self.stop.store(true, Ordering::Relaxed);
        if let Some(join) = self.join.take() {
            let _ = join.join();
        }
    }
}

/// Cross-platform replacement for the WMI `Win32_DeviceChangeEvent` watcher:
/// poll `adb devices` every two seconds and emit `devices:changed` on diffs.
pub fn start_watcher(app: tauri::AppHandle, paths: AppPaths, paused: Arc<AtomicBool>) -> WatcherHandle {
    use tauri::Emitter;

    let stop = Arc::new(AtomicBool::new(false));
    let stop_thread = stop.clone();

    let join = thread::spawn(move || {
        let mut last: Vec<DeviceEntry> = Vec::new();
        while !stop_thread.load(Ordering::Relaxed) {
            // "Kill ADB" pauses polling: any `adb devices` would otherwise
            // start a fresh daemon two seconds later.
            if paused.load(Ordering::Relaxed) {
                sleep_unless_stopped(&stop_thread, Duration::from_millis(250));
                continue;
            }
            // Poll both transports so switching to fastboot is picked up.
            let mut current = list(&paths, true).unwrap_or_default();
            if let Ok(mut fastboot) = list(&paths, false) {
                current.append(&mut fastboot);
            }
            if current != last {
                last = current.clone();
                let _ = app.emit("devices:changed", &current);
            }
            // Poll faster while nothing is attached so a freshly plugged phone
            // shows up almost immediately; relax to 2 s once a device is live.
            let interval = if current.is_empty() { 1000 } else { 2000 };
            sleep_unless_stopped(&stop_thread, Duration::from_millis(interval));
        }
    });

    WatcherHandle {
        stop,
        join: Some(join),
    }
}

/// Sleep in small slices so `WatcherHandle::stop` does not wait a full poll.
fn sleep_unless_stopped(stop: &AtomicBool, total: Duration) {
    let step = Duration::from_millis(50);
    let mut left = total;
    while left > Duration::ZERO {
        if stop.load(Ordering::Relaxed) {
            return;
        }
        let slice = left.min(step);
        thread::sleep(slice);
        left -= slice;
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parses_adb_devices() {
        let text = "List of devices attached\nR58M1234\tdevice\n192.168.1.5:5555\tdevice\n\n";
        let devices = parse_devices(text);
        assert_eq!(devices.len(), 2);
        assert_eq!(devices[0], ("R58M1234".into(), "device".into()));
        assert_eq!(devices[1].0, "192.168.1.5:5555");
    }

    #[test]
    fn ignores_daemon_noise_after_a_kill() {
        let text = "* daemon not running; starting now at tcp:5037\n\
                    * daemon started successfully\n\
                    List of devices attached\n\
                    could not read ok from ADB Server\n\
                    * failed to start daemon\n\
                    adb.exe: failed to check server version: cannot connect to daemon\n\
                    R58M1234\tdevice\n";
        assert_eq!(parse_devices(text), vec![("R58M1234".to_string(), "device".to_string())]);
    }

    #[test]
    fn collapses_the_same_phone_reported_twice() {
        let make = |id: &str, mode: &str| DeviceEntry {
            id: id.to_string(),
            name: "21051182G".into(),
            mode: mode.into(),
            connection: "WIRELESS".into(),
        };
        let found = vec![
            (
                "SN123".to_string(),
                make("adb-414d8918._adb-tls-connect._tcp", "SYSTEM"),
            ),
            ("SN123".to_string(), make("192.168.1.81:44599", "SYSTEM")),
            ("SN123".to_string(), make("192.168.1.81:5555", "SYSTEM")),
        ];
        let devices = dedupe(found);
        assert_eq!(devices.len(), 1);
        assert_eq!(devices[0].id, "192.168.1.81:44599");
    }

    #[test]
    fn prefers_the_cable_transport_for_one_phone() {
        let found = vec![
            (
                "SN9".to_string(),
                DeviceEntry {
                    id: "192.168.1.81:44599".into(),
                    name: "Pixel".into(),
                    mode: "SYSTEM".into(),
                    connection: "WIRELESS".into(),
                },
            ),
            (
                "SN9".to_string(),
                DeviceEntry {
                    id: "R58M1234".into(),
                    name: "Pixel".into(),
                    mode: "SYSTEM".into(),
                    connection: "CABLE".into(),
                },
            ),
        ];
        let devices = dedupe(found);
        assert_eq!(devices.len(), 1);
        assert_eq!(devices[0].id, "R58M1234");
    }

    #[test]
    fn keeps_different_phones() {
        let make = |id: &str| DeviceEntry {
            id: id.to_string(),
            name: String::new(),
            mode: "SYSTEM".into(),
            connection: "CABLE".into(),
        };
        let found = vec![
            ("SN1".to_string(), make("R58M1234")),
            ("SN2".to_string(), make("R58M9999")),
        ];
        assert_eq!(dedupe(found).len(), 2);
    }

    #[test]
    fn detects_wireless_serials() {
        assert_eq!(connection_for("192.168.1.20:5555"), "WIRELESS");
        assert_eq!(connection_for("adb-1234-_adb-tls-connect._tcp"), "WIRELESS");
        assert_eq!(connection_for("R58M1234"), "CABLE");
    }

    #[test]
    fn parses_getprop_dump() {
        let text = "[ro.build.version.sdk]: [34]\n[ro.product.model]: [Pixel 8]\n";
        let map = parse_getprop(text);
        assert_eq!(map.get("ro.build.version.sdk").unwrap(), "34");
        assert_eq!(map.get("ro.product.model").unwrap(), "Pixel 8");
    }

    #[test]
    fn parses_users() {
        let text = "Users:\n\tUserInfo{0:Owner:c13} running\n\tUserInfo{10:Guest:404}\n";
        let users = parse_users(text);
        assert_eq!(users[0].id, "0");
        assert_eq!(users[1].name, "Guest");
    }

    #[test]
    fn extracts_bootloader_info() {
        let text = "(bootloader) unlocked: yes\n(bootloader) token: abc\n(bootloader) junk: nope\n";
        let info = extract_bootloader_info(text);
        assert!(info.contains("unlocked: yes"));
        assert!(info.contains("token: abc"));
        assert!(!info.contains("junk"));
    }

    #[test]
    fn parses_display_ids() {
        assert_eq!(parse_max_display("2"), 2);
        assert_eq!(
            parse_max_display("Display Devices:\n  Display Id: 0\n  Display Id: 3"),
            3
        );
        assert_eq!(parse_max_display("garbage"), 0);
    }
}
