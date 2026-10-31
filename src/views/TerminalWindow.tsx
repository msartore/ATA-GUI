import { useEffect, useRef, useState } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "../lib/api";
import type { TermExit, TermOutput } from "../lib/types";
import { useI18n } from "../i18n";
import { TitleBar, ResizeHandles } from "../components/TitleBar";
import { errorText, splitArgs } from "../lib/util";

type State = "running" | "done" | "failed" | "stopped";

interface Entry {
  session: number;
  command: string;
  output: string;
  state: State;
}

const MAX_LINES = 600;

/** Prompt host: the address without the adb port, like a shell prompt. */
function promptHost(serial: string): string {
  if (!serial) return "local";
  const host = serial.split(":")[0];
  return host.length > 24 ? `${host.slice(0, 22)}…` : host;
}

export function TerminalWindow({ serial }: { serial: string }) {
  const { t } = useI18n();
  const [tool, setTool] = useState<"adb" | "fastboot">("adb");
  const [command, setCommand] = useState("");
  const [entries, setEntries] = useState<Entry[]>([]);
  const [running, setRunning] = useState(false);
  const [history, setHistory] = useState<string[]>([]);
  const [cursor, setCursor] = useState(-1);

  const scrollRef = useRef<HTMLDivElement | null>(null);
  const inputRef = useRef<HTMLInputElement | null>(null);
  const sessionRef = useRef(0);
  const activeRef = useRef<number | null>(null);
  const stoppedRef = useRef(false);
  const liveRef = useRef(false);
  const tRef = useRef(t);
  tRef.current = t;

  const prompt = `ata@${promptHost(serial)}:~$`;

  useEffect(() => {
    inputRef.current?.focus();
  }, []);

  useEffect(() => {
    const node = scrollRef.current;
    if (node) node.scrollTop = node.scrollHeight;
  }, [entries, running]);

  // Output streams in while the command runs: logcat never exits on its own.
  useEffect(() => {
    let disposed = false;
    const stops: Array<() => void> = [];

    const wire = async () => {
      const stopOutput = await listen<TermOutput>("term:output", ({ payload }) => {
        setEntries((list) =>
          list.map((entry) => {
            if (entry.session !== payload.session) return entry;
            // Keep a shell-like tail: logcat can print megabytes, and a DOM
            // node with 60k characters is slow to lay out.
            const lines = (entry.output + payload.text).split("\n");
            const output =
              lines.length > MAX_LINES ? lines.slice(-MAX_LINES).join("\n") : lines.join("\n");
            return { ...entry, output };
          }),
        );
      });
      const stopExit = await listen<TermExit>("term:exit", ({ payload }) => {
        const state: State = stoppedRef.current
          ? "stopped"
          : payload.code === 0
            ? "done"
            : "failed";
        setEntries((list) =>
          list.map((entry) => {
            if (entry.session !== payload.session) return entry;
            // A child that cannot even start (missing DLL, bad binary) prints
            // nothing: surface the code instead of an empty block.
            const output =
              !entry.output && payload.code !== 0
                ? `${tRef.current("terminal.exitCode", { code: payload.code })}\n`
                : entry.output;
            return { ...entry, output, state };
          }),
        );
        if (activeRef.current === payload.session) {
          activeRef.current = null;
          stoppedRef.current = false;
          liveRef.current = false;
          setRunning(false);
          inputRef.current?.focus();
        }
      });
      if (disposed) {
        stopOutput();
        stopExit();
        return;
      }
      stops.push(stopOutput, stopExit);
    };

    void wire();
    return () => {
      disposed = true;
      for (const stop of stops) stop();
    };
  }, []);

  const run = async () => {
    const value = command.trim();
    if (!value || liveRef.current) return;

    const args = splitArgs(value);
    if (args.length === 0) return;

    const session = ++sessionRef.current;
    setHistory((current) => [...current, value]);
    setCursor(-1);
    setCommand("");
    setEntries((current) => [...current, { session, command: value, output: "", state: "running" }]);
    liveRef.current = true;
    setRunning(true);
    stoppedRef.current = false;
    activeRef.current = session;

    try {
      await api.termRun("terminal", session, tool, serial || null, args);
    } catch (error) {
      setEntries((current) =>
        current.map((entry) =>
          entry.session === session
            ? { ...entry, output: `${entry.output}${errorText(error)}\n`, state: "failed" }
            : entry,
        ),
      );
      activeRef.current = null;
      liveRef.current = false;
      setRunning(false);
      inputRef.current?.focus();
    }
  };

  const interrupt = async () => {
    if (!running) return;
    stoppedRef.current = true;
    setEntries((current) =>
      current.map((entry) =>
        entry.session === activeRef.current && entry.output && !entry.output.endsWith("\n")
          ? { ...entry, output: `${entry.output}\n` }
          : entry,
      ),
    );
    await api.termKill().catch(() => false);
  };

  const recall = (direction: -1 | 1) => {
    if (history.length === 0) return;
    const next = cursor === -1 ? (direction === -1 ? history.length - 1 : -1) : cursor + direction;
    if (next < 0) {
      setCursor(-1);
      setCommand("");
      return;
    }
    if (next >= history.length) return;
    setCursor(next);
    setCommand(history[next]);
  };

  return (
    <div className="app terminal-window">
      <TitleBar tools={null} compact title={t("terminal.title")} />

      <div className="term" onClick={() => inputRef.current?.focus()}>
        <div className="term-scroll" ref={scrollRef}>
          {entries.map((entry) => (
            <div key={entry.session}>
              <div className={`term-cmd${entry.state === "running" ? " is-running" : ""}`}>
                <span className="term-prompt">{prompt}</span>
                {entry.command}
                {entry.state === "stopped" ? <span className="term-note"> ^C</span> : null}
              </div>
              {entry.output ? (
                <pre
                  className={entry.state === "failed" ? "term-out is-error" : "term-out"}
                >
                  {entry.output}
                </pre>
              ) : null}
            </div>
          ))}

          <div className={running ? "term-line is-busy" : "term-line"}>
            <span className="term-prompt">{prompt}</span>
            <input
              ref={inputRef}
              className="term-input"
              value={command}
              spellCheck={false}
              autoComplete="off"
              autoCapitalize="off"
              aria-label={t("terminal.title")}
              readOnly={running}
              onChange={(event) => setCommand(event.target.value)}
              onKeyDown={(event) => {
                if (event.key === "c" && event.ctrlKey) {
                  event.preventDefault();
                  if (running) void interrupt();
                  else setCommand("");
                } else if (event.key === "Enter") {
                  event.preventDefault();
                  void run();
                } else if (event.key === "ArrowUp") {
                  event.preventDefault();
                  recall(-1);
                } else if (event.key === "ArrowDown") {
                  event.preventDefault();
                  recall(1);
                } else if (event.key === "l" && (event.ctrlKey || event.metaKey)) {
                  event.preventDefault();
                  setEntries([]);
                } else if (event.key === "u" && event.ctrlKey) {
                  event.preventDefault();
                  setCommand("");
                }
              }}
            />
            {running ? <span className="term-running">›</span> : null}
          </div>
        </div>

        <footer className="term-status">
          <span className="term-mode">
            <button type="button" className={tool === "adb" ? "is-on" : ""} onClick={() => setTool("adb")}>
              adb
            </button>
            <button
              type="button"
              className={tool === "fastboot" ? "is-on" : ""}
              onClick={() => setTool("fastboot")}
            >
              fastboot
            </button>
          </span>
          <span className="term-host">{serial || t("value.noDevice")}</span>
          <span className="term-hint">{t("terminal.keys")}</span>
        </footer>
      </div>

      <ResizeHandles />
    </div>
  );
}
