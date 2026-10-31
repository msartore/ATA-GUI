mod apps;
mod bloatware;
mod device;
mod download;
mod flash;
mod locales;
mod logcat;
mod net;
mod flashrun;
mod scrcpy;
mod terminal;
mod state;
mod tools;
mod windows;
mod wireless;

use serde::Serialize;
use state::{AppPaths, AppState, Config};
use tauri::Manager;
use tauri_plugin_dialog::DialogExt;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
struct ToolStatus {
    adb: Option<String>,
    fastboot: Option<String>,
    scrcpy: Option<String>,
    platform: String,
    version: String,
    runtime_dir: String,
    data_dir: String,
    config_dir: String,
    img_dir: String,
    apks_dir: String,
}

#[tauri::command(async)]
fn tool_status(state: tauri::State<'_, AppState>) -> ToolStatus {
    ToolStatus {
        adb: tools::find_tool(&state.paths, "adb").map(|p| p.to_string_lossy().to_string()),
        fastboot: tools::find_tool(&state.paths, "fastboot").map(|p| p.to_string_lossy().to_string()),
        scrcpy: tools::find_tool(&state.paths, "scrcpy").map(|p| p.to_string_lossy().to_string()),
        platform: std::env::consts::OS.to_string(),
        version: download::current_version(),
        runtime_dir: state.paths.runtime_dir.to_string_lossy().to_string(),
        data_dir: state.paths.data_dir.to_string_lossy().to_string(),
        config_dir: state.paths.config_dir.to_string_lossy().to_string(),
        img_dir: state.paths.img_dir().to_string_lossy().to_string(),
        apks_dir: state.paths.apks_dir().to_string_lossy().to_string(),
    }
}

#[tauri::command(async)]
fn run_tool(
    state: tauri::State<'_, AppState>,
    tool: String,
    serial: Option<String>,
    args: Vec<String>,
) -> Result<tools::CmdOut, String> {
    let serial = serial.as_deref().filter(|s| !s.is_empty());
    if tool == "fastboot" {
        tools::fastboot(&state.paths, serial, &args)
    } else {
        tools::adb(&state.paths, serial, &args)
    }
}

#[tauri::command(async)]
fn devices(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
    is_adb: bool,
) -> Result<Vec<device::DeviceEntry>, String> {
    // Asking for the device list is the way back from "Kill ADB".
    if state
        .adb_paused
        .swap(false, std::sync::atomic::Ordering::Relaxed)
    {
        watcher_start(app, state.clone());
    }
    device::list(&state.paths, is_adb)
}

#[tauri::command(async)]
fn device_info(state: tauri::State<'_, AppState>, serial: String) -> Result<device::DeviceInfo, String> {
    device::info(&state.paths, &serial)
}

#[tauri::command(async)]
fn apps_load(
    state: tauri::State<'_, AppState>,
    serial: String,
    user: String,
    mode: String,
) -> Result<apps::AppsResult, String> {
    apps::load(&state.paths, &serial, &user, &mode)
}

#[tauri::command(async)]
fn apps_apply(
    state: tauri::State<'_, AppState>,
    serial: String,
    user: String,
    action: String,
    packages: Vec<String>,
) -> Vec<apps::OpResult> {
    apps::apply(&state.paths, &serial, &user, &action, &packages)
}

#[tauri::command(async)]
fn apps_install(
    state: tauri::State<'_, AppState>,
    serial: String,
    user: String,
    files: Vec<String>,
    downgrade: bool,
) -> Vec<apps::OpResult> {
    let guard = tools::Guard::new(state.cancel.clone(), std::time::Duration::from_secs(20 * 60));
    apps::install_apks(&state.paths, &serial, &user, &files, downgrade, &guard)
}

#[tauri::command(async)]
fn files_push(
    state: tauri::State<'_, AppState>,
    serial: String,
    files: Vec<String>,
) -> Vec<apps::OpResult> {
    let guard = tools::Guard::new(state.cancel.clone(), std::time::Duration::from_secs(30 * 60));
    apps::push_files(&state.paths, &serial, &files, &guard)
}

#[tauri::command(async)]
fn apks_extract(
    state: tauri::State<'_, AppState>,
    serial: String,
    packages: Vec<String>,
) -> Vec<apps::OpResult> {
    apps::extract_apks(&state.paths, &serial, &packages)
}

#[tauri::command(async)]
fn permission_grant(
    state: tauri::State<'_, AppState>,
    serial: String,
    user: String,
    package: String,
    permission: String,
    method: String,
) -> Result<tools::CmdOut, String> {
    apps::grant(&state.paths, &serial, &user, &package, &permission, &method)
}

#[tauri::command(async)]
fn bootloader_info(state: tauri::State<'_, AppState>, serial: String) -> String {
    flash::bootloader_info(&state.paths, &serial)
}

#[tauri::command(async)]
fn fastboot_erase(
    state: tauri::State<'_, AppState>,
    serial: String,
    partitions: Vec<String>,
) -> Vec<flash::FlashStep> {
    flash::erase(&state.paths, &serial, &partitions)
}

#[tauri::command(async)]
fn fastboot_wipe(state: tauri::State<'_, AppState>, serial: String) -> flash::FlashStep {
    flash::wipe(&state.paths, &serial)
}

#[tauri::command(async)]
fn bootloader_ops(
    state: tauri::State<'_, AppState>,
    serial: String,
    op: String,
) -> Vec<flash::FlashStep> {
    flash::bootloader_ops(&state.paths, &serial, &op)
}

#[tauri::command(async)]
fn flash_step(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
    target: String,
    serial: String,
    kind: String,
    image: String,
    partition: String,
) -> Result<i32, String> {
    flashrun::flash_step(
        app,
        &state.flash,
        &state.paths,
        flashrun::FlashRequest {
            target: &target,
            serial: &serial,
            kind: &kind,
            image: &image,
            partition: &partition,
        },
    )
}

#[tauri::command(async)]
fn flash_sideload(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
    target: String,
    serial: String,
    zip_path: String,
) -> Result<i32, String> {
    flashrun::sideload(app, &state.flash, &state.paths, &target, &serial, &zip_path)
}

#[tauri::command(async)]
async fn save_text_file(
    app: tauri::AppHandle,
    name: String,
    content: String,
) -> Result<Option<String>, String> {
    // Automation hook, mirroring ATA_PICK_FILES: answer the save dialog
    // without showing it so CDP tests can verify the written file.
    if let Ok(target) = std::env::var("ATA_SAVE_FILE") {
        if !target.trim().is_empty() {
            std::fs::write(&target, content).map_err(|error| error.to_string())?;
            return Ok(Some(target));
        }
    }

    let (sender, receiver) = std::sync::mpsc::channel();
    app.dialog()
        .file()
        .set_file_name(&name)
        .add_filter("Log", &["txt", "log"])
        .save_file(move |path| {
            let _ = sender.send(path);
        });

    let picked = receiver.recv().map_err(|error| error.to_string())?;
    match picked {
        Some(path) => {
            let target = path
                .as_path()
                .ok_or_else(|| "invalid path".to_string())?
                .to_path_buf();
            std::fs::write(&target, content).map_err(|error| error.to_string())?;
            Ok(Some(target.to_string_lossy().to_string()))
        }
        None => Ok(None),
    }
}

#[tauri::command(async)]
fn file_size(path: String) -> u64 {
    std::fs::metadata(&path).map(|meta| meta.len()).unwrap_or(0)
}

#[tauri::command(async)]
fn flash_cancel(state: tauri::State<'_, AppState>) -> bool {
    flashrun::cancel(&state.flash)
}

#[tauri::command(async)]
fn image_extract(
    state: tauri::State<'_, AppState>,
    serial: String,
    partition: String,
) -> flash::ImageExtraction {
    let guard = tools::Guard::new(state.cancel.clone(), std::time::Duration::from_secs(20 * 60));
    flash::extract_image(&state.paths, &serial, &partition, &guard)
}

#[tauri::command(async)]
fn adb_connect(
    state: tauri::State<'_, AppState>,
    ip: String,
    port: String,
    tcpip_serial: Option<String>,
) -> net::NetResult {
    net::connect(&state.paths, &ip, &port, tcpip_serial)
}

#[tauri::command(async)]
fn adb_disconnect(state: tauri::State<'_, AppState>, ip: String, port: String) -> net::NetResult {
    net::disconnect(&state.paths, &ip, &port)
}

#[tauri::command(async)]
fn adb_pair(
    state: tauri::State<'_, AppState>,
    ip: String,
    port: String,
    code: String,
) -> net::NetResult {
    net::pair(&state.paths, &ip, &port, &code)
}

#[tauri::command(async)]
fn auto_connect(state: tauri::State<'_, AppState>) -> Vec<String> {
    let ips = state
        .config
        .lock()
        .map(|config| config.ips.clone())
        .unwrap_or_default();
    net::auto_connect(&state.paths, ips)
}

#[tauri::command(async)]
fn adb_kill(state: tauri::State<'_, AppState>) -> String {
    // Stop the watcher first: it polls `adb devices`, which would spawn a new
    // daemon right after the kill and make the action look like it failed.
    state
        .adb_paused
        .store(true, std::sync::atomic::Ordering::Relaxed);
    if let Ok(mut guard) = state.watcher.lock() {
        if let Some(handle) = guard.take() {
            handle.stop();
        }
    }
    net::kill_adb(&state.paths)
}

#[tauri::command(async)]
fn adb_running() -> bool {
    #[cfg(windows)]
    {
        let mut cmd = std::process::Command::new("tasklist");
        cmd.args(["/FI", "IMAGENAME eq adb.exe", "/NH"]);
        tools::hide_console(&mut cmd);
        cmd.output()
            .map(|out| String::from_utf8_lossy(&out.stdout).to_ascii_lowercase().contains("adb.exe"))
            .unwrap_or(false)
    }
    #[cfg(not(windows))]
    {
        std::process::Command::new("pgrep")
            .args(["-x", "adb"])
            .output()
            .map(|out| out.status.success())
            .unwrap_or(false)
    }
}

#[tauri::command(async)]
fn rotation_set(
    state: tauri::State<'_, AppState>,
    serial: String,
    enabled: bool,
    display: Option<i32>,
) -> Result<bool, String> {
    device::rotation_set(&state.paths, &serial, enabled, display)
}

#[tauri::command(async)]
fn inject_text(
    state: tauri::State<'_, AppState>,
    serial: String,
    text: String,
) -> Result<tools::CmdOut, String> {
    // `adb shell input text` uses %s for spaces when the value is passed as a
    // single argv entry (the WinForms app relied on shell quoting instead).
    let escaped = text.replace(' ', "%s");
    tools::adb(
        &state.paths,
        Some(&serial),
        &["shell".into(), "input".into(), "text".into(), escaped],
    )
}

#[tauri::command(async)]
fn processes(state: tauri::State<'_, AppState>, serial: String) -> Vec<Vec<String>> {
    let text = tools::adb_text(&state.paths, Some(&serial), &["shell".into(), "ps".into()]);
    text.lines()
        .skip(1)
        .filter(|line| !line.trim().is_empty())
        .map(|line| line.split_whitespace().map(|s| s.to_string()).collect())
        .collect()
}

#[tauri::command(async)]
fn force_stop(state: tauri::State<'_, AppState>, serial: String, names: Vec<String>) -> usize {
    let mut stopped = 0;
    for name in names {
        if tools::adb(
            &state.paths,
            Some(&serial),
            &["shell".into(), "am".into(), "force-stop".into(), name],
        )
        .is_ok()
        {
            stopped += 1;
        }
    }
    stopped
}

#[tauri::command(async)]
fn term_run(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
    target: String,
    session: u64,
    tool: String,
    serial: Option<String>,
    args: Vec<String>,
) -> Result<(), String> {
    terminal::run(
        app,
        &state.term,
        &state.paths,
        terminal::Run {
            target: &target,
            session,
            tool: &tool,
            serial: serial.as_deref(),
            args: &args,
        },
    )
}

#[tauri::command(async)]
fn term_kill(state: tauri::State<'_, AppState>) -> Result<bool, String> {
    terminal::kill(&state.term)
}

#[tauri::command(async)]
fn logcat_start(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
    serial: String,
    mode: String,
    target: String,
) -> Result<(), String> {
    logcat_stop_inner(&state);
    let handle = logcat::start(app, &state.paths, &serial, &mode, &target)?;
    *state.logcat.lock().map_err(|e| e.to_string())? = Some(handle);
    Ok(())
}

fn logcat_stop_inner(state: &tauri::State<'_, AppState>) {
    if let Ok(mut guard) = state.logcat.lock() {
        if let Some(handle) = guard.take() {
            handle.stop();
        }
    }
}

#[tauri::command(async)]
fn logcat_stop(state: tauri::State<'_, AppState>) {
    logcat_stop_inner(&state);
}

#[tauri::command(async)]
fn logcat_clear(state: tauri::State<'_, AppState>, serial: String) -> String {
    tools::adb_text(&state.paths, Some(&serial), &["logcat".into(), "-c".into()])
}

#[tauri::command(async)]
fn scrcpy_version(state: tauri::State<'_, AppState>) -> Result<String, String> {
    scrcpy::version(&state.paths)
}

#[tauri::command(async)]
fn scrcpy_encoders(
    state: tauri::State<'_, AppState>,
    serial: String,
) -> Result<scrcpy::ScrcpyMedia, String> {
    scrcpy::encoders(&state.paths, &serial)
}

#[tauri::command(async)]
fn scrcpy_run(
    state: tauri::State<'_, AppState>,
    serial: String,
    args: Vec<String>,
) -> Result<(), String> {
    scrcpy::run(&state.paths, &serial, &args)
}

#[tauri::command]
async fn download_platform_tools(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
) -> Result<(), String> {
    let paths = state.paths.clone();
    download::platform_tools(app, paths).await
}

#[tauri::command]
async fn download_scrcpy(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
) -> Result<(), String> {
    let paths = state.paths.clone();
    download::scrcpy(app, paths).await
}

#[tauri::command]
async fn download_bloatware_list(state: tauri::State<'_, AppState>) -> Result<String, String> {
    let paths = state.paths.clone();
    download::bloatware_list(paths).await
}

#[tauri::command]
async fn check_update(state: tauri::State<'_, AppState>) -> Result<download::UpdateInfo, String> {
    let paths = state.paths.clone();
    download::check_update(paths).await
}

#[tauri::command]
async fn apply_update(
    app: tauri::AppHandle,
    state: tauri::State<'_, AppState>,
    url: String,
) -> Result<(), String> {
    let paths = state.paths.clone();
    download::apply_update(app, paths, url).await
}

#[tauri::command(async)]
fn bloatware_scan(
    state: tauri::State<'_, AppState>,
    serial: String,
    user: String,
    level: String,
) -> Result<Vec<bloatware::BloatEntry>, String> {
    bloatware::scan(&state.paths, &serial, &user, &level)
}

#[tauri::command(async)]
fn bloatware_available(state: tauri::State<'_, AppState>) -> bool {
    bloatware::list_path(&state.paths).is_file()
}

#[tauri::command(async)]
fn qr_pairing_code() -> Result<wireless::QrPairing, String> {
    wireless::pairing_code()
}

#[tauri::command(async)]
fn mdns_services(state: tauri::State<'_, AppState>) -> Vec<wireless::MdnsService> {
    wireless::services(&state.paths)
}

#[tauri::command(async)]
fn locales_bundles(state: tauri::State<'_, AppState>) -> Vec<locales::LocaleBundle> {
    locales::bundles(&state.paths)
}

#[tauri::command(async)]
fn locales_open_dir(state: tauri::State<'_, AppState>) -> Result<String, String> {
    locales::open_dir(&state.paths)
}

#[tauri::command(async)]
fn locales_export_template(state: tauri::State<'_, AppState>) -> Result<String, String> {
    locales::export_template(&state.paths)
}

#[tauri::command(async)]
fn config_get(state: tauri::State<'_, AppState>) -> Config {
    state.config.lock().map(|c| c.clone()).unwrap_or_default()
}

#[tauri::command(async)]
fn config_save(state: tauri::State<'_, AppState>, config: Config) {
    if let Ok(mut guard) = state.config.lock() {
        *guard = config;
        guard.save(&state.paths);
    }
}

#[tauri::command(async)]
fn ip_add(state: tauri::State<'_, AppState>, ip: String) -> Vec<String> {
    if let Ok(mut guard) = state.config.lock() {
        let ip = ip.trim().to_string();
        if !ip.is_empty() && !guard.ips.iter().any(|known| known == &ip) {
            guard.ips.push(ip);
            guard.save(&state.paths);
        }
        return guard.ips.clone();
    }
    Vec::new()
}

#[tauri::command(async)]
fn ip_clear(state: tauri::State<'_, AppState>) -> Vec<String> {
    if let Ok(mut guard) = state.config.lock() {
        guard.ips.clear();
        guard.save(&state.paths);
    }
    Vec::new()
}

#[tauri::command(async)]
fn online_check() -> bool {
    tools::ping_online()
}

#[tauri::command(async)]
fn open_url(url: String) -> Result<(), String> {
    tools::open_external(&url)
}

#[tauri::command]
async fn pick_files(
    app: tauri::AppHandle,
    kind: String,
    multiple: bool,
) -> Result<Vec<String>, String> {
    // Automation hook: `ATA_PICK_FILES` answers the picker without a dialog so
    // CDP tests can drive the flash flows. It is ignored in normal runs.
    if let Ok(list) = std::env::var("ATA_PICK_FILES") {
        let files: Vec<String> = list
            .split(',')
            .map(str::trim)
            .filter(|entry| !entry.is_empty())
            .map(str::to_string)
            .collect();
        if !files.is_empty() {
            return Ok(if multiple { files } else { vec![files[0].clone()] });
        }
    }

    let (sender, receiver) = std::sync::mpsc::channel();
    let mut dialog = app.dialog().file();
    dialog = match kind.as_str() {
        "apk" => dialog.add_filter("Android package", &["apk"]),
        "img" => dialog.add_filter("Disk image", &["img"]),
        "zip" => dialog.add_filter("Archive", &["zip"]),
        _ => dialog,
    };

    if multiple {
        dialog.pick_files(move |paths| {
            let _ = sender.send(paths);
        });
    } else {
        dialog.pick_file(move |path| {
            let _ = sender.send(path.map(|p| vec![p]));
        });
    }

    let picked = tauri::async_runtime::spawn_blocking(move || receiver.recv().ok().flatten())
        .await
        .map_err(|e| e.to_string())?
        .unwrap_or_default();

    Ok(picked.into_iter().map(|path| path.to_string()).collect())
}


/// Ask the running long operation (install / flash / sideload / push) to stop.
#[tauri::command]
fn cancel_operation(state: tauri::State<'_, AppState>) {
    state
        .cancel
        .store(true, std::sync::atomic::Ordering::Relaxed);
}

#[tauri::command(async)]
fn watcher_start(app: tauri::AppHandle, state: tauri::State<'_, AppState>) {
    let Ok(mut guard) = state.watcher.lock() else {
        return;
    };
    if guard.is_some() {
        return;
    }
    *guard = Some(device::start_watcher(
        app,
        state.paths.clone(),
        state.adb_paused.clone(),
    ));
}

#[tauri::command(async)]
fn watcher_stop(state: tauri::State<'_, AppState>) {
    if let Ok(mut guard) = state.watcher.lock() {
        if let Some(handle) = guard.take() {
            handle.stop();
        }
    }
}

#[tauri::command(async)]
fn exit_app(app: tauri::AppHandle, state: tauri::State<'_, AppState>, kill_adb: bool) {
    if kill_adb {
        let _ = tools::adb(&state.paths, None, &["kill-server".into()]);
        tools::kill_tool_processes();
    }
    if let Ok(mut guard) = state.logcat.lock() {
        if let Some(handle) = guard.take() {
            handle.stop();
        }
    }
    if let Ok(mut guard) = state.watcher.lock() {
        if let Some(handle) = guard.take() {
            handle.stop();
        }
    }
    app.exit(0);
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        // Must be the first plugin: a second launch focuses the running window
        // instead of failing on the shared WebView2 profile.
        .plugin(tauri_plugin_single_instance::init(|app, _argv, _cwd| {
            if let Some(window) = app.get_webview_window("main") {
                let _ = window.unminimize();
                let _ = window.set_focus();
            }
        }))
        .plugin(tauri_plugin_window_state::Builder::default().build())
        .plugin(tauri_plugin_dialog::init())
        .setup(|app| {
            let paths = AppPaths::resolve(app.handle());
            let config = Config::load(&paths);
            // A 3.x install upgraded in place has no locales folder (the old
            // updater cannot move directories) — restore it from the bundle.
            locales::seed(&paths);
            app.manage(AppState {
                paths,
                config: std::sync::Mutex::new(config),
                logcat: std::sync::Mutex::new(None),
                watcher: std::sync::Mutex::new(None),
                term: terminal::TermState::default(),
                flash: flashrun::FlashState::default(),
                cancel: std::sync::Arc::new(std::sync::atomic::AtomicBool::new(false)),
                adb_paused: std::sync::Arc::new(std::sync::atomic::AtomicBool::new(false)),
            });

            // The window-state plugin can restore a size saved by a build with
            // a smaller minimum, leaving the layout cramped.
            if let Some(window) = app.get_webview_window("main") {
                let scale = window.scale_factor().unwrap_or(1.0);
                if let Ok(size) = window.inner_size() {
                    let logical: tauri::LogicalSize<f64> = size.to_logical(scale);
                    if logical.width < 1040.0 || logical.height < 720.0 {
                        let _ = window.set_size(tauri::LogicalSize::new(
                            logical.width.max(1040.0),
                            logical.height.max(720.0),
                        ));
                    }
                }
            }
            Ok(())
        })
        .on_window_event(|window, event| {
            if let tauri::WindowEvent::CloseRequested { .. } = event {
                if window.label() == "terminal" {
                    if let Some(state) = window.app_handle().try_state::<AppState>() {
                        if let Ok(mut guard) = state.term.current.lock() {
                            if let Some(handle) = guard.take() {
                                handle.stop();
                            }
                        }
                    }
                }
                if window.label() == "logs" {
                    if let Some(state) = window.app_handle().try_state::<AppState>() {
                        if let Ok(mut guard) = state.logcat.lock() {
                            if let Some(handle) = guard.take() {
                                handle.stop();
                            }
                        }
                    }
                }
            }
        })
        .invoke_handler(tauri::generate_handler![
            tool_status,
            run_tool,
            devices,
            device_info,
            apps_load,
            apps_apply,
            apps_install,
            files_push,
            apks_extract,
            permission_grant,
            bootloader_info,
            fastboot_erase,
            fastboot_wipe,
            bootloader_ops,
            flash_step,
            flash_sideload,
            flash_cancel,
            file_size,
            save_text_file,
            image_extract,
            adb_connect,
            adb_disconnect,
            adb_pair,
            adb_kill,
            adb_running,
            rotation_set,
            inject_text,
            processes,
            force_stop,
            term_run,
            term_kill,
            logcat_start,
            logcat_stop,
            logcat_clear,
            scrcpy_version,
            scrcpy_encoders,
            scrcpy_run,
            download_platform_tools,
            download_scrcpy,
            download_bloatware_list,
            check_update,
            apply_update,
            bloatware_scan,
            bloatware_available,
            config_get,
            config_save,
            qr_pairing_code,
            mdns_services,
            locales_bundles,
            locales_open_dir,
            locales_export_template,
            ip_add,
            ip_clear,
            online_check,
            open_url,
            pick_files,
            cancel_operation,
            auto_connect,
            watcher_start,
            watcher_stop,
            exit_app,
            windows::window_minimize,
            windows::window_toggle_maximize,
            windows::window_is_maximized,
            windows::window_is_maximized,
            windows::window_close,
            windows::window_start_drag,
            windows::open_sub_window,
        ])
        .run(tauri::generate_context!())
        .expect("error while running ATA-GUI");
}
