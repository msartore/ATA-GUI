# Build

> Toolchain, development loop and checks. What the app does: [tour](FEATURES.md).

## Prerequisites

- Node.js 22+ and npm.
- Rust stable 1.80+ with the platform toolchain.
- Windows: MSVC build tools and the WebView2 runtime (preinstalled on Windows 11).
- Linux: `libwebkit2gtk-4.1-dev`, `libgtk-3-dev`, `librsvg2-dev`,
  `libayatana-appindicator3-dev`, `patchelf`.
- macOS: Xcode command line tools.

## Commands

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

## Checks

```bash
cargo test --manifest-path src-tauri/Cargo.toml
cargo clippy --manifest-path src-tauri/Cargo.toml --all-targets
npx tsc --noEmit
npm run check:locales     # parity across the 9 catalogues, duplicate and unused keys
```

## CI

`.github/workflows/build.yml` builds Windows, Linux and macOS on pushes and pull requests.
`.github/workflows/release.yml` builds and publishes the bundles for a `v*` tag. See
[`RELEASING.md`](RELEASING.md).

## UI inspection during development

`scripts/inspect.mjs` drives the running webview over the Chrome DevTools Protocol:

```bash
WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS="--remote-debugging-port=9222" ./ATA-GUI.exe
node scripts/inspect.mjs eval "document.querySelector('.modal h3')?.textContent"
node scripts/inspect.mjs shot shot.png
```

Set `CDP_MATCH` (substring, or `re:<regex>`) to target a specific window when the detached Logs,
Tasks or Terminal windows are open.
