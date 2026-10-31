import { useEffect, useMemo, useRef, useState } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { Btn, Field, IconBtn, Panel, useUi } from "../components/ui";
import { TitleBar, ResizeHandles } from "../components/TitleBar";
import { errorText } from "../lib/util";

interface Line {
  id: number;
  text: string;
}

let lineId = 0;

function levelOf(line: string): string {
  const tokens = line.split(/\s+/).filter(Boolean);
  const candidate = tokens[4];
  if (candidate && candidate.length === 1 && "VDIWEFA".includes(candidate)) return candidate;
  const match = line.match(/\s([VDIWEFA])\s/);
  return match ? match[1] : "?";
}

const LEVEL_CLASS: Record<string, string> = {
  D: "log-D",
  E: "log-ERROR",
  I: "log-INFO",
  W: "log-WARNING",
  V: "log-V",
  F: "log-F",
  A: "log-A",
  "?": "log-UNKNOWN",
};

export function LogsWindow({ serial }: { serial: string }) {
  const { t } = useI18n();
  const ui = useUi();
  const [lines, setLines] = useState<Line[]>([]);
  const [running, setRunning] = useState<"logcat" | "getevent" | null>(null);
  const [filter, setFilter] = useState("");
  const [keepScrolling, setKeepScrolling] = useState(true);
  const scrollRef = useRef<HTMLDivElement | null>(null);
  // logcat can push hundreds of lines per second: buffer them and flush once
  // per frame so React re-renders at most 60 times a second.
  const pending = useRef<string[]>([]);
  const frame = useRef<number | null>(null);

  useEffect(() => {
    let disposed = false;
    const stops: Array<() => void> = [];

    (async () => {
      const flush = () => {
        frame.current = null;
        const batch = pending.current;
        pending.current = [];
        if (disposed || batch.length === 0) return;
        setLines((current) => {
          const next = [...current];
          for (const text of batch) next.push({ id: ++lineId, text });
          return next.length > 6000 ? next.slice(next.length - 6000) : next;
        });
      };

      const append = (text: string) => {
        if (disposed) return;
        pending.current.push(text);
        if (frame.current === null) frame.current = requestAnimationFrame(flush);
      };

      try {
        stops.push(await listen<string>("logcat:line", (event) => append(event.payload)));
        stops.push(
          await listen<string>("logcat:stderr", (event) => {
            if (event.payload.trim()) append(event.payload);
          }),
        );
      } catch {
        /* ignore */
      }
    })();

    return () => {
      disposed = true;
      if (frame.current !== null) cancelAnimationFrame(frame.current);
      pending.current = [];
      stops.forEach((stop) => stop());
      void api.logcatStop().catch(() => {});
    };
  }, []);

  useEffect(() => {
    if (!keepScrolling) return;
    const node = scrollRef.current;
    if (node) node.scrollTop = node.scrollHeight;
  }, [lines, keepScrolling]);

  const visible = useMemo(() => {
    const needle = filter.trim().toLowerCase();
    if (!needle) return lines;
    return lines.filter((line) => line.text.toLowerCase().includes(needle));
  }, [lines, filter]);

  const text = useMemo(() => lines.map((line) => line.text).join("\n"), [lines]);

  const start = async (mode: "logcat" | "getevent") => {
    try {
      await api.logcatStart(serial, mode, "logs");
      setRunning(mode);
    } catch (error) {
      await ui.alert(errorText(error));
    }
  };

  const stop = async () => {
    await api.logcatStop().catch(() => {});
    setRunning(null);
  };

  return (
    <div className="app">
      <TitleBar tools={null} compact title={t("logs.title")} />
      <div className="window-head">
        <h1>{t("logs.title")}</h1>
        <span className="badge badge-info mono" title={serial}>{serial}</span>
      </div>

      <div className="workspace is-window logs-layout">
        <Panel
          title={t("logs.logcat")}
          flush
          actions={
            <div className="row is-tight">
              <Btn
                small
                variant={running === "logcat" ? "danger" : "primary"}
                icon={running === "logcat" ? "stop" : "play"}
                onClick={() => void (running === "logcat" ? stop() : start("logcat"))}
              >
                {running === "logcat" ? t("logs.stopLogcat") : t("logs.startLogcat")}
              </Btn>
              <Btn small icon="terminal" disabled={Boolean(running)} onClick={() => void start("getevent")}>
                {t("logs.getEvent")}
              </Btn>
              <Btn
                small
                variant="ghost"
                icon="trash"
                onClick={async () => {
                  await api.logcatClear(serial).catch(() => {});
                  setLines([]);
                }}
              >
                {t("logs.clearLogcat")}
              </Btn>
            </div>
          }
        >
          <div className="log-toolbar">
            <input
              className="input input-sm w-filter"
              placeholder={t("logs.filter")}
              value={filter}
              onChange={(event) => setFilter(event.target.value)}
            />
            <IconBtn
              icon="refresh"
              label={keepScrolling ? t("button.stopScrolling") : t("button.keepScrolling")}
              active={!keepScrolling}
              onClick={() => setKeepScrolling((value) => !value)}
            />
            <Btn
              small
              variant="ghost"
              icon="copy"
              onClick={async () => {
                if (!text) return;
                await api.clipboardWrite(text);
                ui.notify(t("logs.copied"), "ok");
              }}
            >
              {t("logs.copy")}
            </Btn>
            <Btn small variant="ghost" icon="trash" onClick={() => setLines([])}>
              {t("logs.clear")}
            </Btn>
            <span className="hint right">
              {t("logs.charCount")}: {text.length}
            </span>
          </div>

          <div className="log" ref={scrollRef}>
            {visible.map((line) => (
              <div key={line.id} className="log-line">
                <span className={LEVEL_CLASS[levelOf(line.text)] ?? "log-UNKNOWN"}>
                  {levelOf(line.text)}
                </span>{" "}
                {line.text}
              </div>
            ))}
          </div>
        </Panel>

        <Panel title={t("label.status")}>
          <Field label={t("label.device")} value={serial} mono />
          <Field label={t("label.status")} value={running ?? t("value.none")} />
          <Field label={t("logs.charCount")} value={text.length} mono />
        </Panel>
      </div>

      <ResizeHandles />
    </div>
  );
}
