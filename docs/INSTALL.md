# Install and upgrade

> Packaging, portable layout and upgrades. New here? Start with the [tour](FEATURES.md) or the [README](../README.md).

## Bundles

| Platform | Recommended | Also provided |
| --- | --- | --- |
| Windows 10/11 x64 | `ATA-GUI_<version>_x64-setup.exe` (per-user NSIS) | `.msi`, portable `.zip` |
| Linux x64 | `.AppImage` or `.deb` | `.rpm`, portable `.tar.gz` / `.zip` |
| macOS 10.15+ | `.dmg` (Apple silicon or Intel) | portable `.zip` containing `ATA-GUI.app` |

All downloads are on the [releases page](https://github.com/msartore/ATA-GUI/releases).

## Portable builds

The portable archive is self-contained: unpack it anywhere and run `ATA-GUI`
(`ATA-GUI.exe` on Windows). Settings, IP history and licence flags go to `config.json` next to the
executable while that folder is writable, otherwise to the per-user config directory.

SDK Platform Tools and scrcpy are not bundled. The app offers to download them on first start
(or **Settings → Tools**) and keeps them beside the executable.

## macOS

The `.dmg` is ad-hoc signed (no Apple Developer ID), so the first launch needs
**right-click → Open**, or:

```bash
xattr -dr com.apple.quarantine /Applications/ATA-GUI.app
```

The in-app updater is disabled on macOS: download the new `.dmg` from the releases page (the app
still tells you when a release exists).

## Upgrading from 3.x

The 3.x updater is still supported. It downloads the **first asset** of the latest release and
expects a ZIP whose root contains `ATA-GUI.exe`. The release workflow therefore publishes
`ATA-GUI-<version>-portable-windows-x64.zip` first, and the 4.x binary keeps the `ATA-GUI.exe`
name, so an existing 3.x install upgrades in place and relaunches the new app.

Settings, IP history and the licence/disclaimer flags are migrated from the old per-user location
on first start.

The old updater moves files only (`move /y *`), so its `locales/` folder is not copied. The app
recreates `locales/` from the compiled-in catalogues on first start, so the UI stays translated; an
empty `ATAUpdate` folder may be left behind and can be deleted.

## Updates after 4.0

The built-in updater queries the repository that produced the build (`ATA_RELEASES_URL`), picks
the portable ZIP for the running platform and replaces the install in place.
