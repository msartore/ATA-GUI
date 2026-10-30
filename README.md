# ATA-GUI 4

Advanced Android device manager over ADB and Fastboot — now a cross-platform desktop app built
with **Tauri 2 + Rust + React**, replacing the original Windows-only WinForms edition.

[![License](https://img.shields.io/github/license/msartore/ATA-GUI?cacheSeconds=3600)](LICENSE)
[![Release](https://img.shields.io/github/v/release/msartore/ATA-GUI?cacheSeconds=3600)](https://github.com/msartore/ATA-GUI/releases)
![Platforms](https://img.shields.io/badge/platforms-Windows%20%7C%20Linux-2b3137)

> The 3.x C# sources are preserved on the **`legacy`** branch; `main` is the Tauri rewrite.

## What's new in 4.0

- **Rust + Tauri core** — same feature set without a .NET runtime; a ~2.4 MB installer instead of a framework-dependent WinForms build.
- **Windows and Linux** — one codebase, per-platform bundles (NSIS/MSI, `.deb`/`.rpm`/AppImage and portable archives).
- **Dark and light themes**, including "follow system" with live repaint.
- **8 bundled languages** (`en`, `it`, `es`, `de`, `fr`, `pt-BR`, `ru`, `zh-CN`) loaded at runtime — translators can drop a JSON file next to the executable and reload without rebuilding.
- **QR-code wireless pairing** (`WIFI:T:ADB;S:…;P:…;;`) with mDNS discovery, alongside the 6-digit code.
- **No console window, ever**: the binary is GUI-subsystem and every child tool is spawned hidden.

## Features

- Manage system and user apps without root: uninstall, disable/enable, clear data, restore
- Install APKs (including split bundles) and push files
- Inspect and grant/revoke runtime permissions, set default app roles
- ADB over Wi-Fi: 6-digit pairing, QR pairing, `adb tcpip`, connect/disconnect, IP history
- Detect and remove bloatware with the Universal Android Debloater list
- Reboot to system, recovery or fastboot; unlock/lock the bootloader on supported devices
- Flash images, wipe partitions, erase data/cache, sideload ZIPs
- Extract and repack `payload.bin`/images
- Screen and camera mirroring via scrcpy, with encoder discovery
- Device info in system and fastboot modes, multi-device selector
- Logcat / `getevent` streaming in a detached, scrollable window
- Task manager (process list, force-stop), shell command runner, text injection

## Install

| Platform | Recommended | Also provided |
| --- | --- | --- |
| Windows 10/11 x64 | `ATA-GUI_4.0.0_x64-setup.exe` (per-user NSIS) | `.msi`, portable `.zip` |
| Linux x64 | `.AppImage` or `.deb` | `.rpm`, portable `.tar.gz` / `.zip` |

Downloads live on the [releases page](https://github.com/msartore/ATA-GUI/releases).
The portable archive is self-contained: unpack it anywhere and run `ATA-GUI` (`ATA-GUI.exe` on
Windows). Settings and IP history are stored in `config.json` next to the executable while that
folder is writable, otherwise in the per-user config directory.

SDK Platform Tools and scrcpy are not bundled — the app downloads them on first use (startup
prompt or Settings → Tools) and keeps them beside the executable.

### Upgrading from 3.x

The 3.x updater is still supported. It downloads the **first asset** of the latest release and
expects a ZIP whose root contains `ATA-GUI.exe`. The release workflow therefore publishes
`ATA-GUI-<version>-portable-windows-x64.zip` first, and the 4.x binary keeps the `ATA-GUI.exe`
name, so an existing 3.x install upgrades in place and relaunches the new app.

Settings, IP history and the licence/disclaimer flags are read from the old per-user location on
first start and copied to the new one.

The old updater moves files only (`move /y *`), so its `locales/` folder is not copied. The app
recreates `locales/` from the compiled-in catalogues on first start, so the UI stays fully
translated; an empty `ATAUpdate` folder may be left behind and can be deleted.

Upgrades after 4.0 use the built-in updater, which queries the repository that produced the build
(`ATA_RELEASES_URL`), picks the portable ZIP for the running platform and replaces the install.

## Languages

The UI ships with eight catalogues in `src/i18n/locales/`. To add or change a language without
rebuilding:

1. Open **Settings → Open locales folder**.
2. Copy `_template.json` to `<code>.json`, set `_meta.name` and translate the values.
3. Press **Reload languages**.

External catalogues override the bundled ones and missing keys fall back to English. Full guide:
[`docs/TRANSLATIONS.md`](docs/TRANSLATIONS.md). `cargo test` verifies every bundled catalogue has
the same key set as English (438 keys).

## Themes

**Settings → Appearance** offers Dark, Light and System. The choice is stored in `config.json`,
applied before the first paint (no flash), mirrored onto the native window theme, broadcast to the
detached Logs/Tasks windows, and — in System mode — follows OS changes live.

## QR pairing

Android 11+ can pair over Wi-Fi without typing a code. **System → ADB over network → Wireless →
QR** renders the official payload

```text
WIFI:T:ADB;S:<service-name>;P:<password>;;
```

The phone (Wireless debugging → *Pair device with QR code*) scans it, advertises
`_adb-tls-pairing._tcp`, and the app discovers it through `adb mdns services`, pairs, connects
through `_adb-tls-connect._tcp`, and refreshes the device list. The dialog shows live progress and
can regenerate the code at any time. Both devices must be on the same Wi-Fi network.

## Architecture

```
src-tauri/                       Rust backend (Tauri 2)
  src/lib.rs                     IPC façade: thin #[tauri::command] wrappers + registry
  src/state.rs                   AppPaths (portable vs installed), Config, AppState
  src/tools.rs                   process layer — every adb/fastboot spawn goes through here
  src/device.rs                  device list, getprop/users/rotation/bootloader parsing
  src/apps.rs                    package listing/apply/install, split-APK extraction
  src/flash.rs                   fastboot flash/erase/wipe/bootloader operations
  src/net.rs                     wireless connect/pair, adb server control
  src/logcat.rs                  streaming logcat/getevent with an owned process handle
  src/download.rs                platform-tools, scrcpy, UAD list, update check/apply
  src/wireless.rs                QR pairing payload + mDNS parsing
  src/locales.rs                 bundled + external translation catalogues
  src/scrcpy.rs                  encoder discovery and launch
  src/bloatware.rs               UAD list matching
  src/windows.rs                 detached Logs/Tasks windows

src/                             React 19 + TypeScript frontend
  views/                         MainWindow (composition root), tabs, Logs/Tasks windows
  components/                    ui primitives, TitleBar, Dock, dialogs/ (one per dialog)
  lib/                           api, types, devices, util, theme, hooks
  i18n/locales/*.json            runtime catalogues (8 languages, 438 keys each)
```

Conventions: commands never panic — they return `Result<_, String>` with `errors.*` keys that
`errorKey()` maps to translated messages; child processes are argv-only and hidden; dialogs share
`useAction()` for busy state and error alerts; the main window's IPC lifecycle lives in
`useAppBootstrap()` and its log buffer in `useLogBuffer()`.

## Build from source

Prerequisites:

- Node.js 22+ and npm
- Rust stable (1.80+) with the platform toolchain (MSVC on Windows, `build-essential` on Linux)
- Windows: WebView2 runtime (preinstalled on Windows 11) and MSVC build tools
- Linux: `libwebkit2gtk-4.1-dev`, `libgtk-3-dev`, `librsvg2-dev`, `libayatana-appindicator3-dev`, `patchelf`

```bash
npm ci
npm run build            # type-check + bundle the frontend
npx tauri build          # app + installers into src-tauri/target/release/bundle
npm run package          # release/ layout: portable archive + installers
```

Development:

```bash
npm run dev              # Vite only (port 1420)
npm run desktop:dev      # Tauri window with hot reload
```

Checks:

```bash
cargo test --manifest-path src-tauri/Cargo.toml
npx tsc --noEmit
cargo clippy --manifest-path src-tauri/Cargo.toml --all-targets
```

## Releasing

1. Bump the version in `package.json`, `src-tauri/Cargo.toml` and `src-tauri/tauri.conf.json` (they must match).
2. Tag and push:

   ```bash
   git tag -a v4.0.0 -m "ATA-GUI 4.0.0"
   git push origin v4.0.0
   ```

`.github/workflows/release.yml` verifies the tag against the three version fields, builds on
`windows-latest` and `ubuntu-22.04`, runs the backend tests, packages the portable archives plus
installers, and publishes a GitHub release with generated notes. The Windows portable ZIP is
uploaded first so the 3.x updater keeps working; `workflow_dispatch` can rebuild an existing tag.

Asset contract:

```
ATA-GUI-4.0.0-portable-windows-x64.zip     first asset — 3.x updater downloads this one
ATA-GUI_4.0.0_x64-setup.exe                NSIS installer
ATA-GUI_4.0.0_x64_en-US.msi                MSI installer
ATA-GUI-4.0.0-portable-linux-x64.zip       Linux portable (used by the 4.x updater)
ATA-GUI-4.0.0-portable-linux-x64.tar.gz    Linux portable (manual installs)
ATA-GUI_4.0.0_amd64.deb / .rpm / .AppImage  Linux packages
```

## Legacy

The C# WinForms sources (up to 3.15.0) live on the [`legacy`](../../tree/legacy) branch and are no
longer maintained. `main` contains only the Tauri implementation.

## Credits

Massimiliano Sartore · icons by [Icons8](https://icons8.com) · debloat list by the
[Universal Android Debloater Alliance](https://github.com/Universal-Debloater-Alliance/universal-android-debloater-next-generation)
· screen mirroring by [scrcpy](https://github.com/Genymobile/scrcpy) · powered by
[SDK Platform Tools](https://developer.android.com/studio/releases/platform-tools).

## License

Apache-2.0 — see [LICENSE](LICENSE).
