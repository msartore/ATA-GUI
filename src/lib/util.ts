/** Split a command line the way a POSIX shell would, honouring quotes. */
export function splitArgs(input: string): string[] {
  const args: string[] = [];
  let current = "";
  let quote: '"' | "'" | null = null;
  let hasContent = false;

  for (let index = 0; index < input.length; index += 1) {
    const char = input[index];

    if (quote) {
      if (char === quote) {
        quote = null;
      } else if (char === "\\" && quote === '"' && index + 1 < input.length) {
        index += 1;
        current += input[index];
      } else {
        current += char;
      }
      continue;
    }

    if (char === '"' || char === "'") {
      quote = char;
      hasContent = true;
      continue;
    }

    if (/\s/.test(char)) {
      if (hasContent) {
        args.push(current);
        current = "";
        hasContent = false;
      }
      continue;
    }

    current += char;
    hasContent = true;
  }

  if (hasContent) args.push(current);
  return args;
}

export function basename(path: string): string {
  const normalized = path.replace(/\\/g, "/");
  return normalized.slice(normalized.lastIndexOf("/") + 1);
}

export function timeNow(): string {
  return new Date().toLocaleTimeString(undefined, { hour12: false });
}

export function errorText(error: unknown): string {
  if (typeof error === "string") return error;
  if (error instanceof Error) return error.message;
  return String(error);
}

/** Map internal error keys ("errors.x") to a translation key ("error.x"). */
export function errorKey(message: string): string {
  if (message.startsWith("errors.")) return `error.${message.slice("errors.".length)}`;
  if (message.startsWith("error.")) return message;
  return "";
}

export const PARTITION_TYPES = [
  "boot",
  "bootloader",
  "cache",
  "dtbo",
  "init_boot",
  "radio",
  "recovery",
  "super",
  "system",
  "update",
  "vendor",
  "vendor_boot",
  "vendor_kernel_boot",
];

export const PARTITION_SLOTS = ["a", "b", "all", "auto"];

export const IMAGE_PARTITIONS = [
  "abl_a",
  "abl_b",
  "aop_a",
  "aop_b",
  "boot",
  "cmnlib_a",
  "cmnlib_b",
  "cmnlib64_a",
  "cmnlib64_b",
  "devcfg_a",
  "devcfg_b",
  "dtbo",
  "hyp_a",
  "hyp_b",
  "keymaster_a",
  "keymaster_b",
  "modem",
  "recovery",
  "system",
  "tz_a",
  "tz_b",
  "vbmeta",
  "vendor",
  "xbl_a",
  "xbl_b",
  "xbl_config_a",
  "xbl_config_b",
];

export const CAMERA_MODES = ["front", "back", "external"];
