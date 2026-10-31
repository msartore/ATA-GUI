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

### Terminal

- The detached terminal is a real terminal surface now: dark scrollback, a
  shell prompt (`ata@<host>:~$`) with the command typed inline, output kept
  verbatim (errors in red), ↑/↓ history, Ctrl+L / Cmd+K to clear, Ctrl+U or
  Ctrl+C to drop the current line, and a status bar that toggles adb/fastboot.
  The old card-per-command layout, textarea and Run button are gone.
- Commands stream: output arrives as it is printed (batched at 60 ms), so
  `logcat`, `getevent` and long pushes report live instead of freezing the
  window until exit. Ctrl+C kills the child and returns the prompt; closing the
  terminal window kills it too.
- Tab completion for the adb and fastboot verbs, plus the closed argument sets
  of `reboot`, `logcat`, `install`, `oem`, `getvar`, `set_active` and `flash`.
  One match completes inline, several complete to the shared prefix and open a
  list navigable with the arrow keys; the list flips below the prompt when the
  scrollback is still empty. A process that dies without printing anything
  (a missing DLL, a broken binary) reports its exit code instead of leaving an
  empty block.

### Downloads

- scrcpy and the SDK Platform Tools report every phase — resolving, download
  percentage, unpacking, installing — instead of sitting at 100% while the
  archive is extracted.
- Downloads have connect/read deadlines (15 s / 45 s) rather than one global
  timeout that aborted large files on slow links, and retry once.
- A download that unpacks without producing the binary now says so instead of
  silently leaving the prompt on screen.

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
