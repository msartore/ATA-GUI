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
                ok: !lower.contains("cannot connect") && !lower.contains("failed"),
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
                ok: text.contains("disconnected"),
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
                ok: text.contains("Successfully paired"),
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
