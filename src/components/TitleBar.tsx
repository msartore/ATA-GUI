import { useEffect, useState } from "react";
import { getCurrentWindow } from "@tauri-apps/api/window";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { Dropdown, MenuItem } from "./ui";
import { Icon } from "./Icon";
import type { ToolStatus } from "../lib/types";

const RESIZE_HANDLES: Array<[string, string]> = [
  ["n", "resize-n"],
  ["s", "resize-s"],
  ["e", "resize-e"],
  ["w", "resize-w"],
  ["ne", "resize-ne"],
  ["nw", "resize-nw"],
  ["se", "resize-se"],
  ["sw", "resize-sw"],
];

const RESIZE_DIRECTIONS: Record<string, "North" | "South" | "East" | "West" | "NorthEast" | "NorthWest" | "SouthEast" | "SouthWest"> = {
  n: "North",
  s: "South",
  e: "East",
  w: "West",
  ne: "NorthEast",
  nw: "NorthWest",
  se: "SouthEast",
  sw: "SouthWest",
};

export function ResizeHandles() {
  return (
    <>
      {RESIZE_HANDLES.map(([key, className]) => (
        <div
          key={key}
          className={`resize-handle ${className}`}
          onMouseDown={(event) => {
            event.preventDefault();
            getCurrentWindow()
              .startResizeDragging(RESIZE_DIRECTIONS[key])
              .catch(() => {});
          }}
        />
      ))}
    </>
  );
}

export function TitleBar({
  tools,
  onSettings,
  onFeedback,
  onOemDrivers,
  onKillAdb,
  onVideoTutorial,
  compact,
  onClose,
  title,
}: {
  tools: ToolStatus | null;
  onSettings?: () => void;
  onFeedback?: () => void;
  onOemDrivers?: () => void;
  onKillAdb?: () => void;
  onVideoTutorial?: () => void;
  compact?: boolean;
  onClose?: () => void;
  title?: string;
}) {
  const { t } = useI18n();
  const [maximized, setMaximized] = useState(false);

  useEffect(() => {
    api
      .windowIsMaximized()
      .then(setMaximized)
      .catch(() => {});
  }, []);

  const toggleMaximize = async () => {
    try {
      setMaximized(await api.windowToggleMaximize());
    } catch {
      /* window may be closing */
    }
  };

  return (
    <header
      className="titlebar"
      onMouseDown={(event) => {
        if (event.button !== 0) return;
        const target = event.target as HTMLElement;
        if (target.closest("button, .dropdown, img, input, select")) return;
        api.windowStartDrag().catch(() => {});
      }}
      onDoubleClick={(event) => {
        const target = event.target as HTMLElement;
        if (target.closest("button, .dropdown, input")) return;
        void toggleMaximize();
      }}
    >
      <div className="brand is-plain">
        <img src="/resources/logo.png" alt="ATA" draggable={false} />
      </div>

      {!compact ? (
        <nav className="topnav">
          {onSettings ? (
            <Dropdown label={t("menu.settings")} icon="settings">
              <MenuItem icon="settings" onClick={onSettings}>
                {t("dialog.settingsTitle")}
              </MenuItem>
              <MenuItem icon="download" onClick={() => void api.openUrl("https://msartore.dev/projects/ata-gui")}>
                {t("about.repo")}
              </MenuItem>
            </Dropdown>
          ) : null}

          {onVideoTutorial || onFeedback || onOemDrivers ? (
            <Dropdown label={t("menu.help")} icon="help">
              {onVideoTutorial ? (
                <MenuItem icon="play" onClick={onVideoTutorial}>
                  {t("menu.videoTutorial")}
                </MenuItem>
              ) : null}
              {onFeedback ? (
                <MenuItem icon="star" onClick={onFeedback}>
                  {t("menu.submitFeedback")}
                </MenuItem>
              ) : null}
              {onOemDrivers ? (
                <MenuItem icon="usb" onClick={onOemDrivers}>
                  {t("menu.oemDrivers")}
                </MenuItem>
              ) : null}
            </Dropdown>
          ) : null}

          {onKillAdb ? (
            <Dropdown label={t("menu.tools")} icon="terminal">
              <MenuItem icon="x" onClick={onKillAdb}>
                {t("menu.killAdb")}
              </MenuItem>
            </Dropdown>
          ) : null}
        </nav>
      ) : (
        <span className="product">{title ?? (tools ? tools.platform : "")}</span>
      )}

      <div className="window-controls">
        <button type="button" className="wc" title={t("menu.minimize")} onClick={() => api.windowMinimize().catch(() => {})}>
          <Icon name="min" size={14} />
        </button>
        <button type="button" className="wc" title={t("menu.maximize")} onClick={() => void toggleMaximize()}>
          <Icon name={maximized ? "restore" : "max"} size={13} />
        </button>
        <button
          type="button"
          className="wc close"
          title={t("menu.close")}
          onClick={() => (onClose ? onClose() : api.windowClose().catch(() => {}))}
        >
          <Icon name="close" size={14} />
        </button>
      </div>
    </header>
  );
}
