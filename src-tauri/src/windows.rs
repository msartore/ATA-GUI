use tauri::{AppHandle, Manager, WebviewUrl, WebviewWindowBuilder, Window};

#[tauri::command]
pub fn window_minimize(window: Window) -> Result<(), String> {
    window.minimize().map_err(|e| e.to_string())
}

#[tauri::command]
pub fn window_toggle_maximize(window: Window) -> Result<bool, String> {
    let maximized = window.is_maximized().map_err(|e| e.to_string())?;
    if maximized {
        window.unmaximize().map_err(|e| e.to_string())?;
    } else {
        window.maximize().map_err(|e| e.to_string())?;
    }
    Ok(!maximized)
}

#[tauri::command]
pub fn window_is_maximized(window: Window) -> Result<bool, String> {
    window.is_maximized().map_err(|e| e.to_string())
}

#[tauri::command]
pub fn window_close(window: Window) -> Result<(), String> {
    window.close().map_err(|e| e.to_string())
}

#[tauri::command]
pub fn window_start_drag(window: Window) -> Result<(), String> {
    window.start_dragging().map_err(|e| e.to_string())
}

fn encode(value: &str) -> String {
    let mut out = String::with_capacity(value.len());
    for byte in value.bytes() {
        match byte {
            b'A'..=b'Z' | b'a'..=b'z' | b'0'..=b'9' | b'-' | b'_' | b'.' | b':' => {
                out.push(byte as char)
            }
            _ => out.push_str(&format!("%{byte:02X}")),
        }
    }
    out
}

/// Open the detached windows the WinForms app showed with `Form.Show()`:
/// Device Logs, Task Manager and the terminal.
#[tauri::command]
pub async fn open_sub_window(app: AppHandle, kind: String, serial: String, user: String) -> Result<(), String> {
    let (label, title, width, height, min_width, min_height) = match kind.as_str() {
        "tasks" => ("tasks", "ATA-GUI — Task Manager", 1010.0, 700.0, 760.0, 520.0),
        "terminal" => ("terminal", "ATA-GUI — Terminal", 900.0, 600.0, 560.0, 380.0),
        _ => ("logs", "ATA-GUI — Device Logs", 1010.0, 700.0, 760.0, 520.0),
    };

    if let Some(existing) = app.get_webview_window(label) {
        let _ = existing.show();
        let _ = existing.unminimize();
        let _ = existing.set_focus();
        return Ok(());
    }

    let url = format!(
        "index.html?window={label}&serial={}&user={}",
        encode(&serial),
        encode(&user)
    );
    WebviewWindowBuilder::new(&app, label, WebviewUrl::App(url.into()))
        .title(title)
        .inner_size(width, height)
        .min_inner_size(min_width, min_height)
        .decorations(false)
        .resizable(true)
        .visible(true)
        .build()
        .map_err(|e| e.to_string())?;

    Ok(())
}
