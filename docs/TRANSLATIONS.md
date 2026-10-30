# Translating ATA-GUI

The UI is a flat `key -> text` JSON catalogue. Every bundled language lives in
`src/i18n/locales/*.json` and is compiled into the binary, so the app always has
a working translation. On top of that, **any `locales/*.json` file found at
runtime overrides or adds a language** — no rebuild needed.

## Where the app looks for translations

Highest priority first:

1. `<executable folder>/locales/*.json` — portable build, next to the exe
2. `<executable folder>/resources/locales/*.json` — bundle resource layout
3. `<user data dir>/locales/*.json` — e.g. `%APPDATA%\dev.msartore.atagui\locales`
4. `<config dir>/locales/*.json`

The path is shown in **Settings → Translations → Open locales folder**, together
with **Reload languages** (re-scan without restarting) and **Export template**
(writes `_template.json`).

## The file format

```json
{
  "_meta.name": "Nederlands",
  "tab.system": "Systeem",
  "button.sync": "Smartphone synchroniseren",
  "log.installed": "{name} geïnstalleerd"
}
```

Rules:

- The file name is the language code (`nl.json` → `nl`). Files starting with `_`
  are ignored (`_template.json` is only a template).
- Keys must match the English catalogue exactly.
- `_meta.name` is the label shown in the language picker — write it in the
  language itself.
- Keep `{placeholders}` untouched: `{count} {name} {ip} {version} {path}
  {display} {kind} {image} {partition}`.
- Missing keys fall back to English, so a partial translation is valid and can
  be completed over time.
- Do not translate product names: ATA-GUI, ATA-Bridge, adb, fastboot, scrcpy,
  SDK Platform Tools, Logcat, GetEvent, Universal Android Debloater, Icons8,
  Google Play.

## Workflow for a new language

1. Settings → Translations → **Export template** (or copy
   `src/i18n/locales/_template.json`).
2. Rename it to your language code, e.g. `nl.json`.
3. Set `_meta.name` to `Nederlands`.
4. Translate the values.
5. Put the file in the `locales` folder, then **Reload languages** and pick it in
   the language selector.
6. Validate the JSON (`python -m json.tool nl.json`) — invalid files are listed
   in Settings and ignored.

## Adding a language to the repository (bundled)

1. Add `src/i18n/locales/<code>.json` (same keys as `en.json`).
2. Add it to the `BUNDLED` table in `src-tauri/src/locales.rs`
   (`("<code>", include_str!("../../src/i18n/locales/<code>.json"))`).
3. Rebuild. The catalogue is now available even with no external files.

`cargo test` checks that all bundled catalogues are flat string maps with
identical key sets, so a forgotten key fails the build instead of shipping a
half-translated language.

## Overriding a bundled language

Inexpensive way to fix a wording without rebuilding: copy the bundled file into
the `locales` folder, edit it, reload. The external file wins; delete it to fall
back to the bundled version.

## Current languages

| Code | Language |
| --- | --- |
| `en` | English |
| `it` | Italiano |
| `es` | Español |
| `de` | Deutsch |
| `fr` | Français |
| `pt-BR` | Português (Brasil) |
| `ru` | Русский |
| `zh-CN` | 简体中文 |
