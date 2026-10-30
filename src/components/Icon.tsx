import type { JSX } from "react";

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

const PATHS: Record<IconName, JSX.Element> = {
  refresh: (
    <path d="M20 11a8 8 0 1 0-2.5 6.4M20 5v6h-6" />
  ),
  filter: <path d="M3 5h18l-7 8v6l-4 2v-8L3 5z" />,
  trash: <path d="M4 7h16M9 7V4h6v3M6 7l1 13h10l1-13M10 11v6M14 11v6" />,
  box: <path d="M12 3l8 4.5v9L12 21l-8-4.5v-9L12 3zM4 7.5l8 4.5 8-4.5M12 12v9" />,
  key: <path d="M14 10a4 4 0 1 1-4 4l-6 6H2v-3l6-6a4 4 0 0 1 6-1zM15 9h.01" />,
  star: <path d="M12 3l2.7 5.6 6.1.9-4.4 4.3 1 6.1L12 17l-5.4 2.9 1-6.1L3.2 9.5l6.1-.9L12 3z" />,
  search: <path d="M11 4a7 7 0 1 0 0 14 7 7 0 0 0 0-14zM20 20l-4-4" />,
  download: <path d="M12 3v12m0 0l-4-4m4 4l4-4M4 19h16" />,
  extract: <path d="M12 21V9m0 0L8 13m4-4l4 4M4 5h16" />,
  terminal: <path d="M3 5h18v14H3zM7 10l3 3-3 3M13 16h4" />,
  logs: <path d="M4 4h16v16H4zM8 8h8M8 12h8M8 16h5" />,
  tasks: <path d="M4 6h10M4 12h10M4 18h7M17 15l2 2 3-3" />,
  screen: <path d="M3 5h18v11H3zM8 20h8M12 16v4" />,
  settings: (
    <path d="M12 9a3 3 0 1 0 0 6 3 3 0 0 0 0-6zM19.4 13.5a7.6 7.6 0 0 0 0-3l2-1.5-2-3.4-2.3 1a7.7 7.7 0 0 0-2.6-1.5L14 2h-4l-.5 2.6a7.7 7.7 0 0 0-2.6 1.5l-2.3-1-2 3.4 2 1.5a7.6 7.6 0 0 0 0 3l-2 1.5 2 3.4 2.3-1a7.7 7.7 0 0 0 2.6 1.5L10 22h4l.5-2.6a7.7 7.7 0 0 0 2.6-1.5l2.3 1 2-3.4-2-1.5z" />
  ),
  help: <path d="M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18zM9.5 9a2.5 2.5 0 1 1 3.7 2.2c-.8.5-1.2 1-1.2 1.8M12 17h.01" />,
  close: <path d="M6 6l12 12M18 6L6 18" />,
  min: <path d="M5 12h14" />,
  max: <path d="M5 5h14v14H5z" />,
  restore: <path d="M8 8V4h12v12h-4M4 8h12v12H4z" />,
  chevron: <path d="M6 9l6 6 6-6" />,
  play: <path d="M8 5l11 7-11 7V5z" />,
  check: <path d="M4 12l5 5L20 6" />,
  x: <path d="M6 6l12 12M18 6L6 18" />,
  usb: <path d="M12 21V7m0 0L9 10m3-3l3 3M12 17l-4-3M12 17l4-3M6 14v-4M18 14v-4" />,
  wifi: <path d="M2 8a15 15 0 0 1 20 0M5 12a10 10 0 0 1 14 0M8.5 15.5a5 5 0 0 1 7 0M12 19h.01" />,
  power: <path d="M12 3v9M7.5 6a8 8 0 1 0 9 0" />,
  camera: <path d="M3 8h4l2-2h6l2 2h4v11H3zM12 12a3.5 3.5 0 1 0 0 7 3.5 3.5 0 0 0 0-7z" />,
  rotate: <path d="M4 9a8 8 0 0 1 14-2M20 15a8 8 0 0 1-14 2M18 3v4h-4M6 21v-4h4" />,
  upload: <path d="M12 21V9m0 0L8 13m4-4l4 4M4 5h16" />,
  shield: <path d="M12 3l8 3v6c0 5-3.5 8-8 9-4.5-1-8-4-8-9V6l8-3z" />,
  info: <path d="M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18zM12 11v6M12 8h.01" />,
  warning: <path d="M12 3l10 18H2L12 3zM12 10v5M12 18h.01" />,
  error: <path d="M12 3a9 9 0 1 0 0 18 9 9 0 0 0 0-18zM8 8l8 8M16 8l-8 8" />,
  folder: <path d="M3 6h6l2 2h10v11H3z" />,
  chip: <path d="M7 7h10v10H7zM3 10h2M3 14h2M19 10h2M19 14h2M10 3v2M14 3v2M10 19v2M14 19v2" />,
  lock: <path d="M6 11h12v10H6zM9 11V7a3 3 0 0 1 6 0v4" />,
  unlock: <path d="M6 11h12v10H6zM9 11V7a3 3 0 0 1 6 0" />,
  boot: <path d="M12 3v6M8 5l4-2 4 2M5 12h14v8H5zM9 16h6" />,
  wipe: <path d="M4 7h16M6 7l1 13h10l1-13M9 11l6 6M15 11l-6 6" />,
  install: <path d="M12 3v10m0 0l-4-4m4 4l4-4M4 17v3h16v-3" />,
  list: <path d="M8 6h13M8 12h13M8 18h13M3 6h.01M3 12h.01M3 18h.01" />,
  copy: <path d="M8 8h12v12H8zM4 16V4h12" />,
  stop: <path d="M6 6h12v12H6z" />,
  qr: (
    <path d="M4 4h6v6H4zM14 4h6v6h-6zM4 14h6v6H4zM14 14h2v2h-2zM18 14h2v2h-2zM14 18h2v2h-2zM18 18h2v2h-2z" />
  ),
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
  return (
    <svg
      className={className ? `icon ${className}` : "icon"}
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.6"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      {PATHS[name]}
    </svg>
  );
}
