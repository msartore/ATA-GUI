#!/usr/bin/env node
/**
 * Locale health check.
 *
 *   node scripts/check-locales.mjs          # report + fail on missing keys
 *   node scripts/check-locales.mjs --strict # also fail on unused keys
 *
 * Rules enforced:
 *  - every catalogue has exactly the keys of en.json (no missing, no extras)
 *  - no duplicates inside a file (JSON.parse already dedupes, so we re-read raw)
 *  - every key is referenced from src/ (except error.* which errorKey() builds
 *    from backend codes, and _meta.name which the Rust loader reads)
 */
import fs from "node:fs";
import path from "node:path";

const root = path.resolve(import.meta.dirname, "..");
const localesDir = path.join(root, "src", "i18n", "locales");
const strict = process.argv.includes("--strict");

const files = fs
  .readdirSync(localesDir)
  .filter((name) => name.endsWith(".json"))
  .sort();

const catalogues = new Map();
for (const file of files) {
  const raw = fs.readFileSync(path.join(localesDir, file), "utf-8");
  try {
    catalogues.set(file, { raw, data: JSON.parse(raw) });
  } catch (error) {
    console.error(`${file}: invalid JSON — ${error.message}`);
    process.exit(1);
  }
}

const base = "en.json";
if (!catalogues.has(base)) {
  console.error("en.json is missing");
  process.exit(1);
}
const baseKeys = new Set(Object.keys(catalogues.get(base).data));

let failed = false;

// ---------------------------------------------------------------- parity
for (const [file, { data }] of catalogues) {
  if (file.startsWith("_")) continue;
  const keys = new Set(Object.keys(data));
  const missing = [...baseKeys].filter((key) => !keys.has(key));
  const extra = [...keys].filter((key) => !baseKeys.has(key));
  if (missing.length || extra.length) {
    failed = true;
    console.error(`${file}: ${missing.length} missing, ${extra.length} extra`);
    if (missing.length) console.error(`  missing: ${missing.slice(0, 8).join(", ")}`);
    if (extra.length) console.error(`  extra:   ${extra.slice(0, 8).join(", ")}`);
  }
}

// -------------------------------------------------------- duplicate keys
for (const [file, { raw }] of catalogues) {
  const seen = new Map();
  for (const match of raw.matchAll(/^\s*"([^"]+)"\s*:/gm)) {
    const key = match[1];
    seen.set(key, (seen.get(key) ?? 0) + 1);
  }
  const duplicates = [...seen].filter(([, count]) => count > 1).map(([key]) => key);
  if (duplicates.length) {
    failed = true;
    console.error(`${file}: duplicate keys — ${duplicates.join(", ")}`);
  }
}

// ------------------------------------------------------------ unused keys
const sourceFiles = [];
const walk = (dir) => {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walk(full);
    else if (/\.(ts|tsx)$/.test(entry.name)) sourceFiles.push(full);
  }
};
walk(path.join(root, "src"));
const source = sourceFiles.map((file) => fs.readFileSync(file, "utf-8")).join("\n");

const unused = [...baseKeys].filter((key) => {
  if (key.startsWith("error.")) return false; // errorKey() composes these at runtime
  if (key === "_meta.name") return false; // read by the Rust locale loader
  return !source.includes(`"${key}"`) && !source.includes(`'${key}'`);
});

if (unused.length) {
  const message = `unused keys: ${unused.length}\n  ${unused.join("\n  ")}`;
  if (strict) {
    failed = true;
    console.error(message);
  } else {
    console.warn(message);
  }
}

if (failed) process.exit(1);
console.log(
  `${files.length} catalogues, ${baseKeys.size} keys each, ${unused.length} unused — ok`,
);
