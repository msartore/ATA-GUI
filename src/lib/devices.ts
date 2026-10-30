import type { DeviceEntry } from "./types";

export type Tab = "system" | "fastboot" | "recovery" | "tools";

/**
 * Devices a tab can act on. Offline / unauthorized entries are reported as
 * `UNKNOWN` by the backend and therefore never match a working tab.
 */
export function devicesForTab(list: DeviceEntry[], tab: Tab): DeviceEntry[] {
  return list.filter((device) => {
    if (tab === "fastboot") return device.mode === "FASTBOOT";
    if (tab === "recovery") return device.mode === "RECOVERY" || device.mode === "SIDELOAD";
    return device.mode === "SYSTEM";
  });
}

/** Fastboot talks to the bootloader; every other tab goes through adb. */
export function tabUsesAdb(tab: Tab): boolean {
  return tab !== "fastboot";
}
