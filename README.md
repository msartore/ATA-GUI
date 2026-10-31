# ATA-GUI 4

Advanced Android device manager over ADB and Fastboot, rebuilt as a cross-platform desktop app
with **Tauri 2 + Rust + React**.

[![License](https://img.shields.io/github/license/msartore/ATA-GUI?cacheSeconds=3600)](LICENSE)
[![Release](https://img.shields.io/github/v/release/msartore/ATA-GUI?cacheSeconds=3600)](https://github.com/msartore/ATA-GUI/releases)
![Platforms](https://img.shields.io/badge/platforms-Windows%20%7C%20Linux%20%7C%20macOS-2b3137)

> The 3.x C# sources are on the **`legacy`** branch; `main` is the Tauri rewrite.

## Features

Manage system and user apps without root, install APKs and push files, inspect and grant
permissions, wireless ADB with QR pairing, bloatware removal (Universal Android Debloater),
fastboot flashing and image extraction, scrcpy mirroring, logcat/getevent, task manager and a
terminal in its own window. One row per device, eight languages, dark/light/system themes.

Full list, shortcuts and behaviour: [`docs/FEATURES.md`](docs/FEATURES.md).

## Install

| Platform | Recommended | Also provided |
| --- | --- | --- |
| Windows 10/11 x64 | `ATA-GUI_<version>_x64-setup.exe` (per-user NSIS) | `.msi`, portable `.zip` |
| Linux x64 | `.AppImage` or `.deb` | `.rpm`, portable `.tar.gz` / `.zip` |
| macOS 10.15+ | `.dmg` (Apple silicon or Intel) | portable `.zip` containing `ATA-GUI.app` |

Downloads live on the [releases page](https://github.com/msartore/ATA-GUI/releases). Portable
builds are self-contained. SDK Platform Tools and scrcpy are not bundled: the app downloads them
on first use.

Platform notes, portable settings, the 3.x upgrade path and the updater: [`docs/INSTALL.md`](docs/INSTALL.md).

## Build

Requires Node 22+, Rust 1.80+ and the platform toolchain (MSVC on Windows, GTK/WebKit on Linux).

```bash
npm ci
npm run desktop:dev      # Tauri window with hot reload
npx tauri build          # app + installers
npm run package          # release/ layout: portable archive + installers
```

Prerequisites per platform, checks and CI: [`docs/BUILD.md`](docs/BUILD.md).

## Documentation

| Document | Contents |
| --- | --- |
| [`docs/FEATURES.md`](docs/FEATURES.md) | feature list, keyboard shortcuts, themes, QR pairing |
| [`docs/INSTALL.md`](docs/INSTALL.md) | bundles, portable config, macOS, upgrading from 3.x, updates |
| [`docs/BUILD.md`](docs/BUILD.md) | prerequisites, development loop, tests, packaging |
| [`docs/RELEASING.md`](docs/RELEASING.md) | versioning, tag workflow, release asset contract |
| [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md) | source tree, process layer, conventions |
| [`docs/TRANSLATIONS.md`](docs/TRANSLATIONS.md) | adding or overriding a language |
| [`docs/PORTING.md`](docs/PORTING.md) | mapping from the WinForms forms |
| [`CHANGELOG.md`](CHANGELOG.md) | release notes |

## Credits

Massimiliano Sartore · icons by [Tabler Icons](https://tabler.io/icons) (MIT) · debloat list by the
[Universal Android Debloater Alliance](https://github.com/Universal-Debloater-Alliance/universal-android-debloater-next-generation)
· screen mirroring by [scrcpy](https://github.com/Genymobile/scrcpy) · powered by
[SDK Platform Tools](https://developer.android.com/studio/releases/platform-tools).

## License

Apache-2.0 — see [LICENSE](LICENSE).
