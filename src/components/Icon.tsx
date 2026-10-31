import type { ComponentType } from "react";
import {
  IconAlertCircle,
  IconAlertTriangle,
  IconArchive,
  IconArrowsDiagonalMinimize2,
  IconCamera,
  IconCheck,
  IconChecklist,
  IconChevronDown,
  IconCopy,
  IconCpu,
  IconDeviceDesktop,
  IconDownload,
  IconEraser,
  IconFilter,
  IconFolder,
  IconHelpCircle,
  IconInfoCircle,
  IconKey,
  IconList,
  IconLock,
  IconLockOpen,
  IconMaximize,
  IconMinus,
  IconNotes,
  IconPackage,
  IconPackageImport,
  IconPlayerPlayFilled,
  IconPlayerStop,
  IconPower,
  IconQrcode,
  IconRefresh,
  IconReload,
  IconRotate360,
  IconSearch,
  IconSettings,
  IconShield,
  IconStar,
  IconTerminal2,
  IconTrash,
  IconUpload,
  IconUsb,
  IconWifi,
  IconX,
} from "@tabler/icons-react";

/** Semantic names used across the UI, mapped to Tabler glyphs. */
export type IconName =
  | "refresh"
  | "filter"
  | "trash"
  | "box"
  | "key"
  | "star"
  | "search"
  | "download"
  | "extract"
  | "terminal"
  | "logs"
  | "tasks"
  | "screen"
  | "settings"
  | "help"
  | "close"
  | "min"
  | "max"
  | "restore"
  | "chevron"
  | "play"
  | "check"
  | "x"
  | "usb"
  | "wifi"
  | "power"
  | "camera"
  | "rotate"
  | "upload"
  | "shield"
  | "info"
  | "warning"
  | "error"
  | "folder"
  | "chip"
  | "lock"
  | "unlock"
  | "boot"
  | "wipe"
  | "install"
  | "list"
  | "copy"
  | "stop"
  | "qr";

type Glyph = ComponentType<{
  size?: number;
  className?: string;
  strokeWidth?: number;
  color?: string;
  "aria-hidden"?: boolean | "true" | "false";
}>;

const GLYPHS: Record<IconName, Glyph> = {
  refresh: IconRefresh,
  filter: IconFilter,
  trash: IconTrash,
  box: IconPackage,
  key: IconKey,
  star: IconStar,
  search: IconSearch,
  download: IconDownload,
  extract: IconArchive,
  terminal: IconTerminal2,
  logs: IconNotes,
  tasks: IconChecklist,
  screen: IconDeviceDesktop,
  settings: IconSettings,
  help: IconHelpCircle,
  close: IconX,
  min: IconMinus,
  max: IconMaximize,
  restore: IconArrowsDiagonalMinimize2,
  chevron: IconChevronDown,
  play: IconPlayerPlayFilled,
  check: IconCheck,
  x: IconX,
  usb: IconUsb,
  wifi: IconWifi,
  power: IconPower,
  camera: IconCamera,
  rotate: IconRotate360,
  upload: IconUpload,
  shield: IconShield,
  info: IconInfoCircle,
  warning: IconAlertTriangle,
  error: IconAlertCircle,
  folder: IconFolder,
  chip: IconCpu,
  lock: IconLock,
  unlock: IconLockOpen,
  boot: IconReload,
  wipe: IconEraser,
  install: IconPackageImport,
  list: IconList,
  copy: IconCopy,
  stop: IconPlayerStop,
  qr: IconQrcode,
};

export function Icon({
  name,
  size = 16,
  className,
}: {
  name: IconName;
  size?: number;
  className?: string;
}) {
  const Glyph = GLYPHS[name];

  return (
    <Glyph
      className={className ? `icon ${className}` : "icon"}
      size={size}
      strokeWidth={1.75}
      aria-hidden="true"
    />
  );
}
