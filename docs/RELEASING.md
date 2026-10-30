# Releasing

## Version bump

Set the same version in `package.json`, `src-tauri/Cargo.toml` and `src-tauri/tauri.conf.json`.
The workflow refuses to build when the tag and the three fields disagree.

## Tag

```bash
git tag -a v4.0.0 -m "ATA-GUI 4.0.0"
git push origin v4.0.0
```

`.github/workflows/release.yml` runs the backend tests, builds on `windows-latest`,
`ubuntu-22.04`, `macos-latest` (Apple silicon) and `macos-13` (Intel), packages the portable
archives plus installers, verifies each platform's expected files and publishes a GitHub release
with generated notes. `workflow_dispatch` rebuilds an existing tag without moving it.

## Asset contract

```
ATA-GUI-<version>-portable-windows-x64.zip     first asset — the 3.x updater downloads this one
ATA-GUI_<version>_x64-setup.exe                NSIS installer
ATA-GUI_<version>_x64_en-US.msi                MSI installer
ATA-GUI-<version>-portable-linux-x64.zip       Linux portable (used by the 4.x updater)
ATA-GUI-<version>-portable-linux-x64.tar.gz    Linux portable (manual installs)
ATA-GUI_<version>_amd64.deb / .rpm / .AppImage Linux packages
ATA-GUI_<version>_aarch64.dmg / _x64.dmg       macOS installers
ATA-GUI-<version>-portable-darwin-arm64.zip    macOS portable (.app bundle)
```

The Windows portable ZIP must be uploaded first and must contain `ATA-GUI.exe` at its root: the
3.x updater copies the archive root into the install folder and relaunches that name. The workflow
fails the release when the layout does not match.
