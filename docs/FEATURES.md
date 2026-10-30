# Features

## Device management

- Manage system and user apps without root: uninstall, disable/enable, clear data, restore.
- Install APKs, including split bundles, and push files to the device.
- Inspect and grant/revoke runtime permissions; set default app roles.
- Detect and remove bloatware with the Universal Android Debloater list.
- Reboot to system, recovery or fastboot; unlock/lock the bootloader on supported devices.
- Flash images, wipe partitions, erase data/cache, sideload ZIPs.
- Extract and repack `payload.bin` / images.
- Screen and camera mirroring via scrcpy, with encoder discovery.
- Device info in system and fastboot modes, with a multi-device selector.
- Logcat / `getevent` streaming in a detached, scrollable window.
- Task manager (process list, force-stop), shell command runner, text injection.
- Terminal in its own window: adb/fastboot commands with history (↑/↓) and Enter to run.

## Wireless ADB

- 6-digit pairing and QR pairing (see below).
- mDNS discovery listing the real `ip:port` endpoints — Android 11+ uses a random port instead of
  5555, which is the usual reason manual connects fail.
- `adb tcpip`, connect/disconnect with readable failure hints, IP history.
- One row per physical device: adb reporting the same phone as `ip:port`, as an mDNS service name
  and over a cable is collapsed by `ro.serialno`, preferring the cable.
- Auto-reconnect on start for wireless endpoints that are already reachable, without stalling on
  dead addresses.

## Interface

- Dark, light and **System** themes. The choice is applied before the first paint (no flash),
  mirrored onto the native window, broadcast to the detached windows and, in System mode, follows
  OS changes live.
- Eight bundled languages loaded at runtime; see [`TRANSLATIONS.md`](TRANSLATIONS.md).
- Virtualised package list: only the visible rows of an 800+ package list are rendered.
- Toolbar actions appear only when they can act instead of sitting greyed out.

## Desktop capabilities

Things the WinForms app could not do without freezing or blocking:

- **Cancellable long operations** — installs, pushes, image extraction, flashing and sideloading
  run through a guarded process wrapper: every child has a watchdog (20–45 min depending on the
  task), stdout/stderr are drained on separate threads so a full pipe cannot deadlock, the whole
  process tree is killed on cancel, and the busy overlay shows **Cancel** where supported.
- **The UI never blocks** — all adb/fastboot commands run off the main thread.
- **Single instance** — launching the app twice focuses the running window instead of failing on
  the shared WebView2 profile.
- **Remembered window** — size, position and maximised state are restored, clamped to the minimum.
- **Strict CSP** — the webview only loads bundled assets; remote fetches and inline scripts fail.
- **No console window** — the binary is GUI-subsystem and every child tool is spawned hidden.

## Keyboard shortcuts

| Keys | Action |
| --- | --- |
| `Ctrl+F` | Focus the app search box |
| `F5` | Rescan devices |
| `Ctrl+L` | Clear the log |
| `Ctrl+1…4` | System / Fastboot / Recovery / Tools |
| `Esc` | Close the open dialog |

## QR pairing

Android 11+ can pair over Wi-Fi without typing a code. **System → ADB over network → Wireless →
QR** renders the official payload:

```text
WIFI:T:ADB;S:<service-name>;P:<password>;;
```

The phone (Wireless debugging → *Pair device with QR code*) scans it, advertises
`_adb-tls-pairing._tcp`, and the app discovers it through `adb mdns services`, pairs, connects
through `_adb-tls-connect._tcp` and refreshes the device list. The dialog shows live progress and
can regenerate the code at any time. Both devices must be on the same Wi-Fi network.
