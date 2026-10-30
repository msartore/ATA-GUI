use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct NetResult {
    pub ok: bool,
    pub message: String,
    /// Machine-readable failure reason so the UI can show an actionable hint
    /// instead of the raw adb text ("refused", "timeout", "unreachable", "auth").
    #[serde(skip_serializing_if = "Option::is_none")]
    pub code: Option<String>,
}

fn classify(text: &str) -> Option<String> {
    let lower = text.to_ascii_lowercase();
    let code = if lower.contains("refused") || lower.contains("10061") {
        "refused"
    } else if lower.contains("timed out")
        || lower.contains("timeout")
        || lower.contains("10060")
        || lower.contains("did not properly respond")
        || lower.contains("failed to respond")
    {
        "timeout"
    } else if lower.contains("unreachable") || lower.contains("10051") || lower.contains("10065") {
        "unreachable"
    } else if lower.contains("failed to authenticate") || lower.contains("wrong password") {
        "auth"
    } else {
        return None;
    };
    Some(code.to_string())
}

pub fn connect(
    paths: &AppPaths,
    ip: &str,
    port: &str,
    tcpip_serial: Option<String>,
) -> NetResult {
    if ip.trim().len() < 2 {
        return NetResult {
            ok: false,
            message: "errors.ipRequired".into(),
            code: None,
        };
    }

    if let Some(serial) = tcpip_serial.filter(|s| !s.is_empty()) {
        let _ = tools::adb(
            paths,
            Some(&serial),
            &["tcpip".into(), port.to_string()],
        );
    }

    let endpoint = format!("{ip}:{port}");
    match tools::adb(paths, None, &["connect".into(), endpoint.clone()]) {
        Ok(out) => {
            let text = out.merged();
            let lower = text.to_ascii_lowercase();
            let code = classify(&text);
            NetResult {
                // adb exits 0 even when it prints "cannot connect", and a broken
                // adb (missing DLL) prints nothing at all: require both a clean
                // exit and the success sentence.
                ok: out.success() && (lower.contains("connected to") || lower.contains("already connected")),
                message: text,
                code,
            }
        }
        Err(err) => NetResult {
            ok: false,
            message: err,
            code: None,
        },
    }
}

pub fn disconnect(paths: &AppPaths, ip: &str, port: &str) -> NetResult {
    let endpoint = format!("{ip}:{port}");
    match tools::adb(paths, None, &["disconnect".into(), endpoint]) {
        Ok(out) => {
            let text = out.merged();
            let code = classify(&text);
            NetResult {
                ok: out.success() && text.contains("disconnected"),
                message: text,
                code,
            }
        }
        Err(err) => NetResult {
            ok: false,
            message: err,
            code: None,
        },
    }
}

pub fn pair(paths: &AppPaths, ip: &str, port: &str, code: &str) -> NetResult {
    let endpoint = format!("{ip}:{port}");
    match tools::adb(paths, None, &["pair".into(), endpoint, code.to_string()]) {
        Ok(out) => {
            let text = out.merged();
            let code = classify(&text);
            NetResult {
                ok: out.success() && text.contains("Successfully paired"),
                message: text,
                code,
            }
        }
        Err(err) => NetResult {
            ok: false,
            message: err,
            code: None,
        },
    }
}

/// Reconnect endpoints that are already reachable: mDNS-advertised wireless
/// debugging services plus saved manual IPs whose port answers. Avoids the
/// 20-second wait `adb connect` incurs on a silent host.
pub fn auto_connect(paths: &AppPaths, ips: Vec<String>) -> Vec<String> {
    use std::net::{TcpStream, ToSocketAddrs};

    fn port_open(ip: &str, port: u16) -> bool {
        let Ok(mut addrs) = (ip, port).to_socket_addrs() else {
            return false;
        };
        match addrs.next() {
            Some(addr) => TcpStream::connect_timeout(&addr, std::time::Duration::from_millis(400)).is_ok(),
            None => false,
        }
    }

    fn reachable(paths: &AppPaths, endpoint: &str) -> bool {
        match tools::adb(paths, None, &["connect".into(), endpoint.to_string()]) {
            Ok(out) => {
                let text = out.merged().to_ascii_lowercase();
                !text.contains("cannot connect") && !text.contains("failed")
            }
            Err(_) => false,
        }
    }

    let mut connected: Vec<String> = Vec::new();

    for service in crate::wireless::services(paths)
        .into_iter()
        .filter(|service| service.service.contains("_adb-tls-connect"))
    {
        let endpoint = format!("{}:{}", service.ip, service.port);
        if reachable(paths, &endpoint) {
            connected.push(endpoint);
        }
    }

    for ip in ips.into_iter().take(4) {
        if connected.iter().any(|known| known.starts_with(&ip)) || !port_open(&ip, 5555) {
            continue;
        }
        let endpoint = format!("{ip}:5555");
        if reachable(paths, &endpoint) {
            connected.push(endpoint);
        }
    }

    connected
}

pub fn kill_adb(paths: &AppPaths) -> String {
    let out = tools::adb_text(paths, None, &["kill-server".into()]);
    tools::kill_tool_processes();
    out
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn classifies_connection_failures() {
        let refused = "cannot connect to 192.168.1.81:5555: No connection could be made because the target machine actively refused it. (10061)";
        assert_eq!(classify(refused).as_deref(), Some("refused"));
        assert_eq!(classify("failed to connect to 10.0.0.2:5555: timed out").as_deref(), Some("timeout"));
        let silent = "cannot connect to 192.168.1.250:5555: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond. (10060)";
        assert_eq!(classify(silent).as_deref(), Some("timeout"));
        assert_eq!(classify("failed to connect: network is unreachable").as_deref(), Some("unreachable"));
        assert_eq!(classify("Failed to authenticate: wrong password").as_deref(), Some("auth"));
        assert_eq!(classify("connected to 192.168.1.20:5555"), None);
        assert_eq!(classify("Successfully paired to 192.168.1.20:41234"), None);
    }


    #[test]
    fn detects_success_payloads() {
        // The parsing rules are shared with the manual flow above.
        assert!("Successfully paired to 192.168.1.4:41234".contains("Successfully paired"));
        assert!("disconnected 192.168.1.4:5555".contains("disconnected"));
        assert!(!"cannot connect to 10.0.0.2:5555".contains("Successfully paired"));
    }
}
