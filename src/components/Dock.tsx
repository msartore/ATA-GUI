import { useEffect, useRef } from "react";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, IconBtn, Panel } from "../components/ui";
import type { LogEntry } from "../lib/types";

const URL_REGEX = /(https?:\/\/[^\s]+)/g;

function LogLine({ entry }: { entry: LogEntry }) {
  const parts = entry.message.split(URL_REGEX);

  return (
    <div className="log-line">
      <span className="ts">{entry.time}</span>
      <span className={`log-tag log-${entry.type}`}>[{entry.type}]</span>{" "}
      {parts.map((part, index) =>
        index % 2 === 1 ? (
          <a key={index} className="log-link" href={part} target="_blank" rel="noreferrer">
            {part}
          </a>
        ) : (
          <span key={index}>{part}</span>
        ),
      )}
    </div>
  );
}

export function LogPanel() {
  const { t } = useI18n();
  const m = useMain();
  const scrollRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const node = scrollRef.current;
    if (node) node.scrollTop = node.scrollHeight;
  }, [m.logs]);

  return (
    <Panel
      title={t("section.log")}
      className="log-panel"
      flush
      actions={
        <div className="row is-tight">
          <Btn
            small
            variant="ghost"
            icon="trash"
            className="is-collapsible"
            title={t("button.clearLog")}
            onClick={m.clearLogs}
          >
            {t("button.clearLog")}
          </Btn>
          <Btn
            small
            variant="ghost"
            icon="logs"
            className="is-collapsible"
            title={t("button.deviceLogs")}
            disabled={!m.device}
            onClick={m.openLogs}
          >
            {t("button.deviceLogs")}
          </Btn>
          <Btn
            small
            variant="ghost"
            icon="tasks"
            className="is-collapsible"
            title={t("button.taskManager")}
            disabled={!m.device}
            onClick={m.openTasks}
          >
            {t("button.taskManager")}
          </Btn>
          <Btn
            small
            variant="ghost"
            icon="screen"
            className="is-collapsible"
            title={t("button.screenShare")}
            disabled={!m.device}
            onClick={() => void m.shareScreen()}
          >
            {t("button.screenShare")}
          </Btn>
        </div>
      }
    >
      <div className="log" ref={scrollRef}>
        {m.logs.map((entry) => (
          <LogLine key={entry.id} entry={entry} />
        ))}
      </div>
    </Panel>
  );
}

export function Dock() {
  const { t } = useI18n();
  const m = useMain();

  return (
    <div className="dock">
      <div className="dock-left">
        <div className="terminal-launcher">
          <span className="launcher-label">{t("section.terminal")}</span>
          <Btn
            small
            variant="ghost"
            icon="terminal"
            className="is-collapsible"
            title={t("terminal.open")}
            onClick={m.openTerminal}
          >
            {t("terminal.open")}
          </Btn>
        </div>
        <Panel title={t("section.deviceSync")} className="sync-panel">
          <Btn
            variant="primary"
            icon="refresh"
            className="is-block"
            disabled={!m.device}
            onClick={() => void m.syncDevice()}
          >
            {t("button.sync")}
          </Btn>
          <div className="control">
            <span>{t("label.device")}</span>
            <div className="sync-pick">
              <select
                className="select"
                value={m.deviceIndex}
                onChange={(event) => m.selectDevice(Number(event.target.value))}
              >
                {m.devices.length === 0 ? <option value={-1}>{t("value.noDevice")}</option> : null}
                {m.devices.map((device, index) => (
                  <option key={`${device.id}-${index}`} value={index}>
                    {device.name || device.id} [{device.mode}]
                  </option>
                ))}
              </select>
              <IconBtn
                icon="usb"
                label={t("button.refreshDevices")}
                onClick={() => void m.refreshDevices()}
              />
            </div>
          </div>
        </Panel>
      </div>

      <div className="dock-right">
        <LogPanel />
      </div>
    </div>
  );
}
