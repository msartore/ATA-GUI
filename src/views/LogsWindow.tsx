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
  level: string;
}

let lineId = 0;

/** Kept in memory for filtering and copy; only the tail is rendered. */
const MAX_LINES = 6000;
const RENDER_LIMIT = 400;

/**
 * Default `threadtime` output already carries the level letter between the
 * tid and the tag. The list shows that letter in its own coloured column, so
 * drop it here instead of printing it twice.
 */
const THREADTIME = /^(\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d{3}\s+\d+\s+\d+)\s+[VDIWEFA]\s/;

function tidy(line: string): string {
  return line.replace(THREADTIME, "$1 ");
}

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
  const [running, setRunning] = useState<"logcat" | "getevent" | null>(null);
  const [filter, setFilter] = useState("");
  const [keepScrolling, setKeepScrolling] = useState(true);
  const [version, setVersion] = useState(0);
  const [chars, setChars] = useState(0);
  const scrollRef = useRef<HTMLDivElement | null>(null);
  // The buffer is mutated in place: React only needs to know that it changed,
  // not a copy of six thousand strings sixty times a second.
  const buffer = useRef<Line[]>([]);
  const charsRef = useRef(0);
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

        const target = buffer.current;
        for (const text of batch) {
          const level = levelOf(text);
          const cleaned = tidy(text);
          target.push({ id: ++lineId, text: cleaned, level });
          charsRef.current += cleaned.length + 1;
        }
        if (target.length > MAX_LINES) {
          const dropped = target.splice(0, target.length - MAX_LINES);
          for (const line of dropped) charsRef.current -= line.text.length + 1;
        }

        setChars(charsRef.current);
        setVersion((value) => value + 1);
      };

      const append = (text: string) => {
        if (disposed) return;
        pending.current.push(text);
        if (frame.current === null) frame.current = requestAnimationFrame(flush);
      };

      try {
        // Lines arrive in batches from the reader thread; a busy device can
        // print thousands per second and one state update each would stall.
        stops.push(
          await listen<string[]>("logcat:lines", (event) => {
            for (const line of event.payload) append(line);
          }),
        );
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

  const clear = () => {
    buffer.current = [];
    charsRef.current = 0;
    pending.current = [];
    setChars(0);
    setVersion((value) => value + 1);
  };

  useEffect(() => {
    if (!keepScrolling) return;
    const node = scrollRef.current;
    if (node) node.scrollTop = node.scrollHeight;
  }, [version, keepScrolling]);

  const { visible, total } = useMemo(() => {
    const all = buffer.current;
    const needle = filter.trim().toLowerCase();
    const matched = needle ? all.filter((line) => line.text.toLowerCase().includes(needle)) : all;
    return {
      total: matched.length,
      visible: matched.length > RENDER_LIMIT ? matched.slice(-RENDER_LIMIT) : matched,
    };
    // `all` is mutated in place; `version` is the change signal.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [version, filter]);

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
                  clear();
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
                const text = buffer.current.map((line) => line.text).join("\n");
                if (!text) return;
                await api.clipboardWrite(text);
                ui.notify(t("logs.copied"), "ok");
              }}
            >
              {t("logs.copy")}
            </Btn>
            <Btn small variant="ghost" icon="trash" onClick={clear}>
              {t("logs.clear")}
            </Btn>
            {total > visible.length ? (
              <span className="hint">
                {t("logs.showing", { shown: visible.length, total })}
              </span>
            ) : null}
            <span className="hint right">
              {t("logs.charCount")}: {chars}
            </span>
          </div>

          <div className="log" ref={scrollRef}>
            {visible.map((line) => (
              <div key={line.id} className="log-line">
                <span className={LEVEL_CLASS[line.level] ?? "log-UNKNOWN"}>{line.level}</span>{" "}
                {line.text}
              </div>
            ))}
          </div>
        </Panel>

        <Panel title={t("label.status")}>
          <Field label={t("label.device")} value={serial} mono />
          <Field label={t("label.status")} value={running ?? t("value.none")} />
          <Field label={t("logs.charCount")} value={chars} mono />
        </Panel>
      </div>

      <ResizeHandles />
    </div>
  );
}
