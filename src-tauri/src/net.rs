use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct NetResult {
    pub ok: bool,
    pub message: String,
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
            NetResult {
                ok: !lower.contains("cannot connect") && !lower.contains("failed"),
                message: text,
            }
        }
        Err(err) => NetResult {
            ok: false,
            message: err,
        },
    }
}

pub fn disconnect(paths: &AppPaths, ip: &str, port: &str) -> NetResult {
    let endpoint = format!("{ip}:{port}");
    match tools::adb(paths, None, &["disconnect".into(), endpoint]) {
        Ok(out) => {
            let text = out.merged();
            NetResult {
                ok: text.contains("disconnected"),
                message: text,
            }
        }
        Err(err) => NetResult {
            ok: false,
            message: err,
        },
    }
}

pub fn pair(paths: &AppPaths, ip: &str, port: &str, code: &str) -> NetResult {
    let endpoint = format!("{ip}:{port}");
    match tools::adb(paths, None, &["pair".into(), endpoint, code.to_string()]) {
        Ok(out) => {
            let text = out.merged();
            NetResult {
                ok: text.contains("Successfully paired"),
                message: text,
            }
        }
        Err(err) => NetResult {
            ok: false,
            message: err,
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

    #[test]
    fn detects_success_payloads() {
        // The parsing rules are shared with the manual flow above.
        assert!("Successfully paired to 192.168.1.4:41234".contains("Successfully paired"));
        assert!("disconnected 192.168.1.4:5555".contains("disconnected"));
        assert!(!"cannot connect to 10.0.0.2:5555".contains("Successfully paired"));
    }
}
