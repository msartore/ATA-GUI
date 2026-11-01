# Architecture

> Source layout and the process layer. Feature overview: [tour](FEATURES.md).


```
src-tauri/                       Rust backend (Tauri 2)
  src/lib.rs                     IPC facade: thin #[tauri::command] wrappers + registry
  src/state.rs                   AppPaths (portable vs installed), Config, AppState
  src/tools.rs                   process layer — every adb/fastboot spawn goes through here
  src/device.rs                  device list, identity/dedupe, getprop/users/rotation parsing
  src/apps.rs                    package listing/apply/install, split-APK extraction
  src/flash.rs                   fastboot flash/erase/wipe/bootloader operations
  src/net.rs                     wireless connect/pair, adb server control
  src/logcat.rs                  streaming logcat/getevent with an owned process handle
  src/download.rs                platform-tools, scrcpy, UAD list, update check/apply
  src/wireless.rs                QR pairing payload + mDNS parsing
  src/locales.rs                 bundled + external translation catalogues
  src/scrcpy.rs                  encoder discovery and launch
  src/bloatware.rs               UAD list matching
  src/windows.rs                 detached Logs/Tasks/Terminal windows

src/                             React 19 + TypeScript frontend
  views/                         MainWindow (composition root), tabs, detached windows
  components/                    ui primitives, TitleBar, Dock, dialogs/ (one per dialog)
  lib/                           api, types, devices, util, theme, hooks
  i18n/locales/*.json            runtime catalogues (8 languages)
```

## Conventions

- Commands never panic: they return `Result<_, String>` with `errors.*` keys that `errorKey()` maps
  to translated messages.
- Every command that spawns adb/fastboot, touches the disk or does network I/O is declared
  `#[tauri::command(async)]` (or `async fn`). Tauri runs those off the main thread; a plain
  `#[tauri::command]` would run on it and freeze the window for the duration of the call. CPU-bound
  helpers inside async commands go through `tauri::async_runtime::spawn_blocking` (see `download::unpack`).
- The webview is never asked to do bulk work either: the package list is virtualised and logcat
  lines are buffered and flushed once per animation frame.
- Child processes are argv-only and hidden; long operations run through `Guard`, which owns the
  timeout, the cancel flag and the process-tree kill.
- Dialogs share `useAction()` for busy state and error alerts; the main window's IPC lifecycle
  lives in `useAppBootstrap()` and its log buffer in `useLogBuffer()`.
- `device::list` is the single source of truth for the device selector: it dedupes transports,
  caches `ro.serialno`/model lookups and emits `devices:changed` from the watcher.
- Adb connections are owned by the app (`ADB_MDNS_AUTO_CONNECT=0` for every child), so pairing and
  reconnect behaviour stays predictable.
