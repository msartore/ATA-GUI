//! Wireless debugging pairing over a QR code.
//!
//! Android 11+ can pair by scanning a Wi-Fi style QR payload:
//!
//! ```text
//! WIFI:T:ADB;S:<service-name>;P:<password>;;
//! ```
//!
//! The phone then advertises `_adb-tls-pairing._tcp` with the service name from
//! the payload; the desktop discovers it through `adb mdns services`, pairs with
//! `adb pair <ip>:<port> <password>` and finally connects through the
//! `_adb-tls-connect._tcp` service. This module generates the code and parses
//! the mDNS table; the state machine lives in the frontend.

use crate::state::AppPaths;
use crate::tools;
use rand::distr::Alphanumeric;
use rand::RngExt;
use serde::Serialize;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct QrPairing {
    /// `_adb-tls-pairing._tcp` instance the phone will advertise.
    pub service: String,
    pub password: String,
    /// Exact payload encoded in the QR image.
    pub payload: String,
    /// Self-contained SVG markup (dark modules on white).
    pub svg: String,
}

#[derive(Serialize, Clone, Debug, PartialEq)]
#[serde(rename_all = "camelCase")]
pub struct MdnsService {
    pub name: String,
    pub service: String,
    pub address: String,
    pub ip: String,
    pub port: String,
}

fn random_chars(count: usize, lowercase: bool) -> String {
    let generated: String = rand::rng()
        .sample_iter(&Alphanumeric)
        .take(count)
        .map(char::from)
        .collect();
    if lowercase {
        generated.to_lowercase()
    } else {
        generated
    }
}

/// Build the QR payload. Service and password are ASCII alphanumerics, so no
/// WPA-style escaping is needed.
pub fn build_payload(service: &str, password: &str) -> String {
    format!("WIFI:T:ADB;S:{service};P:{password};;")
}

fn render_svg(payload: &str) -> Result<String, String> {
    use qrcode::render::svg;

    let code = qrcode::QrCode::new(payload.as_bytes()).map_err(|error| error.to_string())?;
    Ok(code
        .render::<svg::Color>()
        .min_dimensions(280, 280)
        .quiet_zone(true)
        .dark_color(svg::Color("#0b0f10"))
        .light_color(svg::Color("#ffffff"))
        .build())
}

pub fn pairing_code() -> Result<QrPairing, String> {
    let service = format!("adb-{}", random_chars(6, true));
    let password = random_chars(10, false);
    let payload = build_payload(&service, &password);
    let svg = render_svg(&payload)?;

    Ok(QrPairing {
        service,
        password,
        payload,
        svg,
    })
}

/// Parse `adb mdns services`:
///
/// ```text
/// List of discovered mdns services
/// adb-R58M1234-abcde    _adb-tls-pairing._tcp    192.168.1.20:37099
/// adb-R58M1234-abcde    _adb-tls-connect._tcp    192.168.1.20:5555
/// ```
pub fn parse_mdns(text: &str) -> Vec<MdnsService> {
    text.lines()
        .filter(|line| line.contains('\t'))
        .filter_map(|line| {
            let mut columns = line.split('\t');
            let name = columns.next()?.trim();
            let service = columns.next()?.trim();
            let address = columns.next()?.trim();

            if name.is_empty() || !service.starts_with('_') {
                return None;
            }
            let (ip, port) = address.rsplit_once(':')?;
            if ip.is_empty() || port.is_empty() {
                return None;
            }

            Some(MdnsService {
                name: name.to_string(),
                service: service.to_string(),
                address: address.to_string(),
                ip: ip.to_string(),
                port: port.to_string(),
            })
        })
        .collect()
}

pub fn services(paths: &AppPaths) -> Vec<MdnsService> {
    let text = tools::adb_text(paths, None, &["mdns".into(), "services".into()]);
    parse_mdns(&text)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn payload_matches_the_official_format() {
        let payload = build_payload("adb-abc123", "Secret1234");
        assert_eq!(payload, "WIFI:T:ADB;S:adb-abc123;P:Secret1234;;");
        assert!(!payload.contains('\\'), "no escaping needed for alphanumerics");
    }

    #[test]
    fn pairing_code_is_scannable() {
        let code = pairing_code().expect("qr generation");
        assert!(code.service.starts_with("adb-"));
        assert_eq!(code.service.len(), 10);
        assert_eq!(code.password.len(), 10);
        assert_eq!(code.payload, format!("WIFI:T:ADB;S:{};P:{};;", code.service, code.password));
        assert!(code.svg.starts_with("<?xml") || code.svg.starts_with("<svg"));
        assert!(code.svg.contains("viewBox"));
    }

    #[test]
    fn parses_mdns_services() {
        let text = "\
List of discovered mdns services
adb-R58M1234-abcde\t_adb-tls-pairing._tcp\t192.168.1.20:37099
adb-R58M1234-abcde\t_adb-tls-connect._tcp\t192.168.1.20:5555
";
        let services = parse_mdns(text);
        assert_eq!(services.len(), 2);
        assert_eq!(services[0].name, "adb-R58M1234-abcde");
        assert_eq!(services[0].service, "_adb-tls-pairing._tcp");
        assert_eq!(services[0].ip, "192.168.1.20");
        assert_eq!(services[0].port, "37099");
        assert_eq!(services[1].port, "5555");
    }

    #[test]
    fn ignores_headers_and_garbage() {
        assert!(parse_mdns("List of discovered mdns services\n").is_empty());
        assert!(parse_mdns("garbage line without tabs\n").is_empty());
        assert!(parse_mdns("\t_adb-tls-pairing._tcp\t\n").is_empty());
    }
}
