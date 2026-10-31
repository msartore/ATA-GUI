/**
 * Tab completion for the detached terminal. The catalogue is the surface most
 * people actually type: `adb`/`fastboot` verbs plus the fixed arguments of the
 * ones that take them. File paths are left to the user — the terminal has no
 * completion protocol back to the device.
 */

export type Tool = "adb" | "fastboot";

const ADB = [
  "backup",
  "bugreport",
  "connect",
  "devices",
  "disable-verity",
  "disconnect",
  "emu",
  "enable-verity",
  "exec-out",
  "forward",
  "get-devpath",
  "get-serialno",
  "get-state",
  "help",
  "install",
  "jdwp",
  "keygen",
  "logcat",
  "pair",
  "pull",
  "push",
  "reboot",
  "remount",
  "restore",
  "reverse",
  "root",
  "shell",
  "sideload",
  "sync",
  "tcpip",
  "track-devices",
  "uninstall",
  "unroot",
  "usb",
  "version",
  "wait-for-device",
];

const FASTBOOT = [
  "boot",
  "continue",
  "create-partition",
  "delete-partition",
  "devices",
  "erase",
  "fetch",
  "flash",
  "flashall",
  "format",
  "getvar",
  "help",
  "oem",
  "partition",
  "reboot",
  "reboot-bootloader",
  "reboot-fastboot",
  "reboot-recovery",
  "resize",
  "set_active",
  "slots",
  "snapshot-update",
  "stage",
  "unlock",
  "update",
  "verify",
  "version",
];

/** Arguments that are a closed set, keyed by command. */
const ADB_ARGS: Record<string, string[]> = {
  reboot: ["bootloader", "recovery", "sideload", "system", "userspace"],
  logcat: ["-c", "-d", "-e", "-s", "-t", "-v", "-w", "--pid"],
  install: ["-d", "-g", "-r", "-t", "--user"],
  uninstall: ["-k", "--user"],
  forward: ["--list", "--no-rebind", "--remove", "--remove-all"],
  reverse: ["--list", "--no-rebind", "--remove", "--remove-all"],
  connect: [],
  tcpip: ["5555"],
};

const FASTBOOT_ARGS: Record<string, string[]> = {
  oem: ["device-info", "edl", "frp", "lock", "unlock"],
  getvar: ["all", "current-slot", "product", "serialno", "version"],
  set_active: ["a", "b"],
  reboot: ["bootloader", "fastboot", "recovery", "sideload"],
  flash: ["boot", "dtbo", "init_boot", "recovery", "super", "system", "vbmeta", "vendor"],
};

function pool(tool: Tool, tokens: string[]): string[] {
  // Accept "adb devices" as well as "devices": the binary name is optional
  // because the mode is already selected in the status bar.
  const rest = tokens[0] === tool ? tokens.slice(1) : tokens;
  if (rest.length === 0) return tool === "adb" ? ADB : FASTBOOT;
  if (rest.length === 1) {
    const args = tool === "adb" ? ADB_ARGS[rest[0]] : FASTBOOT_ARGS[rest[0]];
    return args ?? [];
  }
  return [];
}

export function suggest(tool: Tool, tokens: string[], partial: string): string[] {
  const lower = partial.toLowerCase();
  return pool(tool, tokens).filter((entry) => entry.toLowerCase().startsWith(lower));
}

/** Longest common prefix — a shell completes to it before listing. */
export function commonPrefix(values: string[]): string {
  if (values.length === 0) return "";
  let prefix = values[0];
  for (const value of values.slice(1)) {
    let index = 0;
    while (index < prefix.length && index < value.length && prefix[index] === value[index]) {
      index += 1;
    }
    prefix = prefix.slice(0, index);
    if (!prefix) break;
  }
  return prefix;
}
