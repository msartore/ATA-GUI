# Porting map — C# WinForms → Rust + React

> The mapping from the 3.x WinForms forms. Feature overview: [tour](FEATURES.md).


This document records how the WinForms 3.15.0 app (now on the `legacy` branch) was converted into the Tauri app on `main`.

## Forms

| C# form | Replacement | Notes |
| --- | --- | --- |
| `MainForm` (2372 LOC) | `src/views/MainWindow.tsx` + `SystemTab`, `FastbootTab`, `RecoveryTab`, `ToolsTab`, `components/Dock.tsx` | Custom title bar, tab strip, always-visible log dock, device sync |
| `BloatwareRemoverForm` | `components/dialogs.tsx` → `BloatwareDialog` + `bloatware.rs` | Scan levels, severity colours, system/non-system aware actions |
| `DeviceLogsForm` | `views/LogsWindow.tsx` + `logcat.rs` | Separate Tauri window (`index.html?window=logs`), logcat/getevent streaming |
| `TaskManager` | `views/TasksWindow.tsx` | Separate window (`?window=tasks`), filter by name/apps, force-stop |
| `SettingsForm` | `SettingsDialog` | Adds the language selector; changelog, SDK update, IP history |
| `AboutForm` | `AboutDialog` | Links to repo, licence, platform tools, scrcpy, Icons8, UAD |
| `FeedbackForm` | `FeedbackDialog` | Feedback file replaced by `config.json` (`feedbackPopup`) |
| `DisclaimerForm` | `DisclaimerDialog` | Apache-2.0 text embedded in `lib/license.ts` |
| `UpdateForm` | `UpdateDialog` + `download::apply_update` | `.bat` on Windows, `.sh` on Linux |
| `exeMissingForm` | `MissingDialog` | Three-way Yes / No / Info prompt |
| `LoadingForm` | `LoadingDialog` | Modal progress while install/push/extract run |
| `PackageMenuForm` | `PackageMenuDialog` | enable / disable / clear data |
| `DefaultAppForm` | `DefaultAppDialog` | `cmd role add-role-holder android.app.role.<role>` |
| `ShellCommandsForm` | `ShellCommandsDialog` | 880-entry permission list in `lib/permissions.ts` |
| `ScrcpyForm` | `ScrcpyDialog` | Encoder discovery, mirroring flags |
| `WirelessPairingForm` | `PairingDialog` | 6-digit validation, `adb pair` |
| — (new) | `QrPairingDialog` + `wireless.rs` | QR pairing: `WIFI:T:ADB;S:;P:;;` payload, SVG QR, `adb mdns services` discovery, pair + connect |
| `BootloaderMenuForm` | `components/BootloaderDialog.tsx` | All OEM/flashing/Vivo operations |
| `ScrollableMessageBoxForm` | `ScrollableDialog` | Used by OEM drivers and fastboot info |
| `WaitingForm` | Bootstrap log lines | Startup is non-blocking, progress goes to the log |

## Classes and utilities

| C# | Rust |
| --- | --- |
| `ATA` (`ATA.cs`) | `state.rs` (`AppPaths`, `Config`), `tools.rs` (path resolution), `lib.rs` (state) |
| `DeviceData` + `ExtractBootloaderInfo` | `device.rs` (`DeviceEntry`, `DeviceInfo`, `extract_bootloader_info`) |
| `AppData` | `apps.rs::AppEntry` |
| `EncoderData` | `scrcpy.rs::EncoderData`, `parse_encoders` |
| `APK.installApk` | `apps.rs::install_apks` |
| `ConsoleProcess` | `tools.rs` (`run`, `adb`, `fastboot`, `spawn_detached`, `open_external`, `kill_tool_processes`) |
| `Network.IsValidIP` | `device.rs::is_valid_ip` (regex port) |
| `Utils.Version.CompareVersions` | `download.rs::compare_versions` (+ unit test) |
| `RichTextBoxExtensions.AppendText` | React log entries + CSS classes (`log-OK`, `log-ERROR`, …) |
| `MainForm.MessageShowBox` | `UiProvider.alert` / `confirm` in `components/ui.tsx` |
| `commandAssemblerF` | `tools::adb/fastboot` add `-s <serial>` automatically |
| `BackgroundWorker`s | `async` commands / `runBusy` + `LoadingDialog` |
| WMI `Win32_DeviceChangeEvent` watcher | `device::start_watcher` polling every 2 s, emits `devices:changed` |

## Original command strings

Commands are now argv vectors instead of shell strings, e.g.

```
C#:   "-s SERIAL shell pm list packages -3 --user 0"
Rust: adb(paths, Some(serial), ["shell","pm","list","packages","-3","--user","0"])
```

The interactive terminal keeps a shell-like splitter (`lib/util.ts::splitArgs`, quote aware) so typed commands still behave like the original.

## Behavioural differences (deliberate)

- `getprop` is read once per device instead of seven separate calls (same data, fewer round-trips).
- Device hotplug is polled on both platforms instead of WMI, so Linux gets the same auto-detection.
- Downloads select the asset for the running platform (Windows zip, Linux tarball/deb/AppImage) instead of always `assets[0]`.
- Config lives in `config.json`; legacy `InfoCache.xml` and `IPList.txt` are migrated on first start.
- The licence dialog is shown over the main window instead of a separate modal form.

## Verified

- `cargo test` — 17 tests: device paths, getprop, users, bootloader info, package validation, encoder parsing, version comparison, platform/arch asset selection.
- `tsc --noEmit` and `vite build` — frontend type-checks and bundles.
- Release build on Windows produced both installers (`ATA-GUI_<version>_x64-setup.exe`, `ATA-GUI_<version>_x64_en-US.msi`).
- Live WebView2 over CDP: every tab, settings/about/disclaimer/feedback dialogs, Italian locale switch, detached Task Manager and Device Logs windows.
- Real network + process paths: SDK Platform Tools downloaded/unpacked (adb 1.0.41 executes, `devices` returns an empty list), bloatware endpoint, update check (the then-current version reported up to date), scrcpy 5.0.1 downloaded for win64, unpacked and executed (`scrcpy --version`).

- Themes: light and dark palettes driven by `data-theme` on `<html>`, persisted as `config.theme`, applied to the native window, broadcast to the detached windows, and "system" verified by emulating `prefers-color-scheme` (light ↔ dark repaint without reload).
- QR pairing: payload format and mDNS table parsing covered by unit tests; the dialog renders a scannable QR (white card, quiet zone) and stays in the waiting state when no phone advertises `_adb-tls-pairing._tcp`; `adb mdns check` reports a working discovery daemon and `adb pair` errors are surfaced in the dialog.
- Translations: 8 bundled catalogs loaded through `locales_bundles` (not static imports), an external `locales/nl.json` added at runtime appeared in the picker after "Reload languages", and a partial catalog correctly fell back to English for missing keys.
- Portable build: `release/ATA-GUI-<version>-portable-windows-x64.zip` unpacked to a clean folder, started, migrated/seeded `config.json` next to the executable, persisted a language change across a restart, and loaded all eight catalogs from the bundled `locales/` folder.

Known caveat: the 64-bit asset preference and the retry-on-sharing-violation copy were added after the first scrcpy attempt downloaded the win32 build and hit a file lock; both are covered by tests and re-verified.
