import { emit } from "@tauri-apps/api/event";
import type { ThemeMode } from "./types";

export type ResolvedTheme = "dark" | "light";

const lightQuery = () => window.matchMedia("(prefers-color-scheme: light)");

/** Resolve a stored preference to the theme that should actually be painted. */
export function resolveTheme(mode: ThemeMode): ResolvedTheme {
  if (mode === "system") return lightQuery().matches ? "light" : "dark";
  return mode === "light" ? "light" : "dark";
}

/**
 * Paint the UI in the given mode.
 *
 * The stylesheet keys off `data-theme` on `<html>`; the native window theme is
 * set too so the title bar / scrollbars / native menus match. `null` hands the
 * window back to the OS default, which is what "system" means.
 */
export async function applyTheme(mode: ThemeMode): Promise<void> {
  const resolved = resolveTheme(mode);
  document.documentElement.dataset.theme = resolved;
  try {
    // Remember the resolved theme so the first paint after a restart is
    // already correct instead of flashing the dark default.
    localStorage.setItem("ata.theme", resolved);
  } catch {
    /* storage unavailable */
  }
  try {
    const { getCurrentWindow } = await import("@tauri-apps/api/window");
    await getCurrentWindow().setTheme(mode === "system" ? null : mode);
  } catch {
    /* not running inside Tauri (vite preview) */
  }
}

/** Apply locally and tell the other windows (logs, tasks) to follow. */
export async function broadcastTheme(mode: ThemeMode): Promise<void> {
  await applyTheme(mode);
  try {
    await emit("theme:changed", mode);
  } catch {
    /* events unavailable */
  }
}

/** Re-paint when the OS switches scheme while "system" is selected. */
export function watchTheme(mode: ThemeMode, onChange: (theme: ResolvedTheme) => void): () => void {
  const query = lightQuery();
  const handler = () => onChange(resolveTheme(mode));
  query.addEventListener("change", handler);
  return () => query.removeEventListener("change", handler);
}
