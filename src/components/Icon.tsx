import type { ComponentType, SVGProps } from "react";
import {
  Archive,
  Camera,
  Check,
  Computer,
  Copy,
  Download,
  ElectronicsChip,
  Erase,
  Filter,
  Folder,
  HelpCircle,
  Import,
  InfoCircle,
  Journal,
  Key,
  List,
  Lock,
  LockSlash,
  Maximize,
  Minus,
  NavArrowDown,
  Package,
  PlaySolid,
  QrCode,
  RefreshDouble,
  Reduce,
  Search,
  Settings,
  Shield,
  Square,
  Star,
  SwitchOff,
  SystemRestart,
  TaskList,
  Terminal,
  Trash,
  Upload,
  Usb,
  View360,
  WarningCircle,
  WarningTriangle,
  Wifi,
  Xmark,
} from "iconoir-react";

/** Semantic names used across the UI, mapped to Iconoir glyphs. */
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

type Glyph = ComponentType<SVGProps<SVGSVGElement> & { color?: string; strokeWidth?: number }>;

const GLYPHS: Record<IconName, Glyph> = {
  refresh: RefreshDouble,
  filter: Filter,
  trash: Trash,
  box: Package,
  key: Key,
  star: Star,
  search: Search,
  download: Download,
  extract: Archive,
  terminal: Terminal,
  logs: Journal,
  tasks: TaskList,
  screen: Computer,
  settings: Settings,
  help: HelpCircle,
  close: Xmark,
  min: Minus,
  max: Maximize,
  restore: Reduce,
  chevron: NavArrowDown,
  play: PlaySolid,
  check: Check,
  x: Xmark,
  usb: Usb,
  wifi: Wifi,
  power: SwitchOff,
  camera: Camera,
  rotate: View360,
  upload: Upload,
  shield: Shield,
  info: InfoCircle,
  warning: WarningTriangle,
  error: WarningCircle,
  folder: Folder,
  chip: ElectronicsChip,
  lock: Lock,
  unlock: LockSlash,
  boot: SystemRestart,
  wipe: Erase,
  install: Import,
  list: List,
  copy: Copy,
  stop: Square,
  qr: QrCode,
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
      width={size}
      height={size}
      strokeWidth={1.6}
      aria-hidden="true"
    />
  );
}
