#!/usr/bin/env node
/**
 * Produce the release layout:
 *
 *   release/
 *     ATA-GUI-<version>-portable-<platform>-<arch>/     portable folder
 *     ATA-GUI-<version>-portable-<platform>-<arch>.zip  portable archive (.tar.gz on Linux)
 *     ATA-GUI_<version>_x64-setup.exe / .msi / .deb / .AppImage ...
 *
 * Run after `npm run desktop:build`.
 */
import { copyFile, cp, mkdir, readdir, readFile, readlink, rm, stat, symlink, writeFile } from "node:fs/promises";
import { existsSync } from "node:fs";
import path from "node:path";
import { execFileSync } from "node:child_process";

const root = process.cwd();
const pkg = JSON.parse(await readFile(path.join(root, "package.json"), "utf8"));
const version = pkg.version;
const platform = process.platform;
const arch = process.arch;
const platformLabel = platform === "win32" ? "windows" : platform;
const exeName = platform === "win32" ? "ATA-GUI.exe" : "ATA-GUI";
const isMac = platform === "darwin";

const releaseDir = path.join(root, "release");
const stageName = `ATA-GUI-${version}-portable-${platformLabel}-${arch}`;
const stageDir = path.join(releaseDir, stageName);

const binary = path.join(root, "src-tauri", "target", "release", exeName);
const appBundle = path.join(root, "src-tauri", "target", "release", "bundle", "macos", "ATA-GUI.app");

if (isMac ? !existsSync(appBundle) : !existsSync(binary)) {
  console.error(`Missing release ${isMac ? "app bundle" : "binary"}: ${isMac ? appBundle : binary}\nRun "npm run desktop:build" first.`);
  process.exit(1);
}

await rm(stageDir, { recursive: true, force: true });
await mkdir(path.join(stageDir, "locales"), { recursive: true });

if (isMac) {
  // macOS users need the .app bundle, not the raw Mach-O binary.
  await cp(appBundle, path.join(stageDir, "ATA-GUI.app"), { recursive: true });
} else {
  await copyFile(binary, path.join(stageDir, exeName));
}
await copyFile(path.join(root, "LICENSE"), path.join(stageDir, "LICENSE"));

const localesDir = path.join(root, "src", "i18n", "locales");
for (const entry of await readdir(localesDir)) {
  if (entry.toLowerCase().endsWith(".json")) {
    await copyFile(path.join(localesDir, entry), path.join(stageDir, "locales", entry));
  }
}

const translationsDoc = path.join(root, "docs", "TRANSLATIONS.md");
if (existsSync(translationsDoc)) {
  await copyFile(translationsDoc, path.join(stageDir, "TRANSLATIONS.md"));
}

await writeFile(
  path.join(stageDir, "README.txt"),
  `ATA-GUI ${version} — portable build (${platformLabel} ${arch})

Run ${exeName}. Everything is self-contained: no installation, no registry keys,
no system Python/Node needed. Settings and the IP history are stored in
config.json next to the executable while that folder is writable.

SDK Platform Tools (adb/fastboot) are downloaded from Google on first use, or you
can drop your own adb/fastboot binaries next to ${exeName}.

Translations
------------
The locales/ folder contains every bundled language. Drop another <code>.json
file there (copy _template.json to start), edit "_meta.name", translate the
values and pick the language in Settings -> Translations -> Reload languages.
No rebuild required: files next to the executable override the built-in ones.

Licence: Apache-2.0 (see LICENSE). SDK Platform Tools (c) Google, scrcpy (c) Genymobile,
bloatware list by the Universal Android Debloater Alliance, icons by Icons8.
`,
  "utf8",
);

/* --------------------------------------------------------------- archives */

const archiveBase = path.join(releaseDir, stageName);
if (isMac) {
  const zip = `${archiveBase}.zip`;
  await rm(zip, { force: true });
  execFileSync("zip", ["-qry", zip, stageName], { cwd: releaseDir, stdio: "inherit" });
} else if (platform === "win32") {
  const zip = `${archiveBase}.zip`;
  await rm(zip, { force: true });
  execFileSync(
    "powershell",
    ["-NoProfile", "-Command", `Compress-Archive -Path '${stageDir}\\*' -DestinationPath '${zip}' -Force`],
    { stdio: "inherit" },
  );
} else {
  await rm(`${archiveBase}.tar.gz`, { force: true });
  execFileSync("tar", ["-czf", `${archiveBase}.tar.gz`, "-C", releaseDir, stageName], { stdio: "inherit" });

  // The in-app updater always unpacks a ZIP, so publish one on Linux as well.
  // Entries are stored at the archive root ("./") to match the Windows layout,
  // otherwise the updater would copy a nested folder into the install dir.
  const zip = `${archiveBase}.zip`;
  await rm(zip, { force: true });
  try {
    execFileSync("zip", ["-qr", zip, "."], { cwd: stageDir, stdio: "inherit" });
  } catch {
    console.warn("zip not available: the portable ZIP needed by the updater was not created");
  }
}

/* -------------------------------------------------------------- installers */

const bundleDir = path.join(root, "src-tauri", "target", "release", "bundle");
const installerExt = new Set([".exe", ".msi", ".deb", ".rpm", ".appimage", ".dmg"]);

async function walk(dir) {
  let entries;
  try {
    entries = await readdir(dir, { withFileTypes: true });
  } catch {
    return;
  }
  for (const entry of entries) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      await walk(full);
    } else if (installerExt.has(path.extname(entry.name).toLowerCase())) {
      await copyFile(full, path.join(releaseDir, entry.name));
    }
  }
}

await walk(bundleDir);

/* ----------------------------------------------------------------- summary */

const built = [];
for (const entry of await readdir(releaseDir, { withFileTypes: true })) {
  const full = path.join(releaseDir, entry.name);
  const info = await stat(full);
  const size = entry.isDirectory()
    ? await (async () => {
        const inner = await readdir(full);
        return `${inner.length} files`;
      })()
    : `${(info.size / 1024 / 1024).toFixed(2)} MiB`;
  built.push(`  ${entry.name.padEnd(52)} ${size}`);
}

console.log(`\nRelease ${version} (${platformLabel} ${arch})\n`);
console.log(built.join("\n"));
console.log(`\nOutput: ${releaseDir}`);
