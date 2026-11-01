# Releasing

> How a version becomes a release and which asset names matter. Feature overview: [tour](FEATURES.md).

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

## Pre-releases

A tag push publishes a **pre-release**, so the build is only visible to people who look for it:

- `https://api.github.com/repos/msartore/ATA-GUI/releases/latest` ignores pre-releases, so the
  3.x WinForms updater keeps offering the last stable build and does not move everyone to 4.x.
- The in-app updater reads the `/releases` list, so 4.x test builds still notice newer test builds.
- Testers install manually from the release page.

To publish a stable release, run the workflow with `workflow_dispatch` and set
`prerelease: false`. That release then becomes `latest` for the 3.x updater too.

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
