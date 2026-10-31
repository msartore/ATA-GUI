# Changelog

## 4.0.0

The WinForms 3.15 code base is now a Tauri 2 + Rust + React application. The
C# sources stay available on the `legacy` branch, and the Windows portable ZIP
keeps the layout the 3.x auto-updater expects (`ATA-GUI.exe` at the archive
root), so existing installations upgrade in place.

### Platforms

- Windows, Linux and macOS from one code base; CI builds NSIS/MSI, `.deb`,
  `.rpm`, AppImage, `.dmg`/`.app` and portable archives on every `v*` tag.
- macOS runs with the native overlay title bar (traffic lights); the detached
  Logs, Tasks and Terminal windows keep the app's own chrome.

### Languages

Eight catalogues ship with the binary (`en`, `it`, `es`, `de`, `fr`, `pt-BR`,
`ru`, `zh-CN`). External `locales/*.json` files are merged *over* the bundled
ones and reloaded at runtime, so an older export can never hide a newer key.

### Device handling

- One row per physical device: adb reporting the same phone as `ip:port`, as an
  mDNS service name and over a cable is collapsed by `ro.serialno`, preferring
  the cable transport.
- "Kill ADB" really stops it: the device watcher pauses instead of polling
  `adb devices` (which used to respawn the daemon two seconds later). Asking for
  the device list is the way back.
- Plug-and-play attach, wireless reconnects, QR pairing (`WIFI:T:ADB;S:…;P:…;;`)
  with mDNS discovery, and connection failures translated into readable hints.

### Interface

- Dark, light and follow-system themes with live repaint and no flash on start.
- Tabler Icons set throughout.
- The terminal lives in its own window (history with ↑/↓, Enter to run, full
  scrollback) instead of a cramped field in the dock.
- The layout holds together at the 1040×720 minimum: the dock shrinks, columns
  narrow rather than stack, device columns scroll instead of clipping, and
  toolbar buttons keep only their icon when space runs out.

### Robustness

- Every long operation has a timeout and a working Cancel (installs, pushes,
  flashes, image extraction).
- Single instance, remembered window size, strict CSP, virtualised package list.

### Issue tracker

- **Closes #34** — language files for multiple languages: the UI strings live in
  JSON catalogues, ship in eight languages and can be extended without a rebuild.
- **Closes #37** — Linux version: the release workflow produces `.deb`, `.rpm`,
  AppImage and a portable Linux archive from the same code base.
- **Refs #35** — paying to remove ATA-Bridge ads. The ads live in ATA-Bridge, the
  Android companion app; ATA-GUI itself has no ads or paid tier, so this request
  is out of scope for this repository.
