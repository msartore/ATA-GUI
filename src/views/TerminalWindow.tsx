import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "../lib/api";
import type { TermExit, TermOutput } from "../lib/types";
import { commonPrefix, suggest } from "../lib/completion";
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

/** Open completion list. `before + item + suffix` is the preview text. */
interface Popup {
  items: string[];
  index: number;
  before: string;
  partial: string;
  suffix: string;
}

const MAX_LINES = 600;

/** Prompt host: the address without the adb port, like a shell prompt. */
function promptHost(serial: string): string {
  if (!serial) return "local";
  const host = serial.split(":")[0];
  return host.length > 24 ? `${host.slice(0, 22)}…` : host;
}

/** The whitespace-delimited token around the caret and the words before it. */
function tokenAt(text: string, caret: number) {
  const before = text.slice(0, caret);
  const match = before.match(/(\S*)$/);
  const partial = match ? match[1] : "";
  const start = before.length - partial.length;
  const tail = text.slice(caret).match(/^(\S*)/);
  const end = caret + (tail ? tail[1].length : 0);
  const tokens = before.slice(0, start).trim().split(/\s+/).filter(Boolean);
  return { partial, start, end, tokens };
}

export function TerminalWindow({ serial }: { serial: string }) {
  const { t } = useI18n();
  const [tool, setTool] = useState<"adb" | "fastboot">("adb");
  const [command, setCommand] = useState("");
  const [entries, setEntries] = useState<Entry[]>([]);
  const [running, setRunning] = useState(false);
  const [history, setHistory] = useState<string[]>([]);
  const [cursor, setCursor] = useState(-1);
  const [popup, setPopup] = useState<Popup | null>(null);

  const scrollRef = useRef<HTMLDivElement | null>(null);
  const inputRef = useRef<HTMLInputElement | null>(null);
  const termRef = useRef<HTMLDivElement | null>(null);
  const lineRef = useRef<HTMLDivElement | null>(null);
  const popupRef = useRef<HTMLDivElement | null>(null);
  const [popupBox, setPopupBox] = useState<{ top: number; below: boolean }>({ top: 0, below: false });
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

  // The prompt sits wherever the scrollback ends, so the list is anchored to
  // the input line rather than the panel: above it when there is room, below
  // it when the session is still empty.
  useLayoutEffect(() => {
    if (!popup) return;
    const measure = () => {
      const term = termRef.current;
      const line = lineRef.current;
      if (!term || !line) return;
      const host = term.getBoundingClientRect();
      const rect = line.getBoundingClientRect();
      const top = rect.top - host.top;
      const height = popupRef.current?.offsetHeight ?? 0;
      const above = top - height - 6 >= 4;
      setPopupBox({
        top: above ? top - 6 : rect.bottom - host.top + 6,
        below: !above,
      });
    };
    measure();
    window.addEventListener("resize", measure);
    return () => window.removeEventListener("resize", measure);
  }, [popup]);

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
    setPopup(null);
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

  /* -------------------------------------------------------- completion */

  const focusEnd = (value: string) => {
    setCommand(value);
    requestAnimationFrame(() => {
      const input = inputRef.current;
      if (input) {
        input.focus();
        input.setSelectionRange(value.length, value.length);
      }
    });
  };

  const accept = (list: Popup, item: string) => {
    const needsSpace = list.suffix.trim() === "";
    focusEnd(`${list.before}${item}${needsSpace ? " " : list.suffix}`);
    setPopup(null);
  };

  const preview = (list: Popup, index: number) => {
    const item = list.items[index] ?? list.partial;
    focusEnd(`${list.before}${item}${list.suffix}`);
    setPopup({ ...list, index });
  };

  const complete = () => {
    if (liveRef.current) return;
    const input = inputRef.current;
    if (!input) return;

    if (popup) {
      const next = popup.index < 0 ? 0 : (popup.index + 1) % popup.items.length;
      preview(popup, next);
      return;
    }

    const caret = input.selectionStart ?? command.length;
    const { partial, start, end, tokens } = tokenAt(command, caret);
    const items = suggest(tool, tokens, partial);

    if (items.length === 0) return;

    if (items.length === 1) {
      const before = command.slice(0, start);
      const suffix = command.slice(end);
      focusEnd(`${before}${items[0]}${suffix.trim() === "" ? " " : suffix}`);
      return;
    }

    // Several matches: complete to the longest common prefix, then list them.
    const prefix = commonPrefix(items);
    const before = command.slice(0, start);
    const suffix = command.slice(end);
    const grown = prefix.length > partial.length;
    if (grown) focusEnd(`${before}${prefix}${suffix}`);
    setPopup({
      items,
      index: -1,
      before,
      partial: grown ? prefix : partial,
      suffix,
    });
  };

  const keyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === "Tab") {
      event.preventDefault();
      complete();
      return;
    }

    if (popup) {
      if (event.key === "Escape") {
        event.preventDefault();
        focusEnd(`${popup.before}${popup.partial}${popup.suffix}`);
        setPopup(null);
        return;
      }
      if (event.key === "ArrowDown" || event.key === "ArrowUp") {
        event.preventDefault();
        const step = event.key === "ArrowDown" ? 1 : -1;
        const size = popup.items.length;
        const index =
          popup.index < 0
            ? step === 1
              ? 0
              : size - 1
            : (popup.index + step + size) % size;
        preview(popup, index);
        return;
      }
      if (event.key === "Enter") {
        event.preventDefault();
        if (popup.index >= 0) accept(popup, popup.items[popup.index]);
        else {
          setPopup(null);
          void run();
        }
        return;
      }
    }

    if (event.key === "c" && event.ctrlKey) {
      event.preventDefault();
      if (running) void interrupt();
      else setCommand("");
      return;
    }
    if (event.key === "Enter") {
      event.preventDefault();
      void run();
      return;
    }
    if (event.key === "ArrowUp") {
      event.preventDefault();
      recall(-1);
      return;
    }
    if (event.key === "ArrowDown") {
      event.preventDefault();
      recall(1);
      return;
    }
    if (event.key === "l" && (event.ctrlKey || event.metaKey)) {
      event.preventDefault();
      setEntries([]);
      setPopup(null);
      return;
    }
    if (event.key === "u" && event.ctrlKey) {
      event.preventDefault();
      setCommand("");
    }
  };

  return (
    <div className="app terminal-window">
      <TitleBar tools={null} compact title={t("terminal.title")} />

      <div className="term" ref={termRef} onClick={() => inputRef.current?.focus()}>
        <div className="term-scroll" ref={scrollRef}>
          {entries.map((entry) => (
            <div key={entry.session}>
              <div className={`term-cmd${entry.state === "running" ? " is-running" : ""}`}>
                <span className="term-prompt">{prompt}</span>
                {entry.command}
                {entry.state === "stopped" ? <span className="term-note"> ^C</span> : null}
              </div>
              {entry.output ? (
                <pre className={entry.state === "failed" ? "term-out is-error" : "term-out"}>
                  {entry.output}
                </pre>
              ) : null}
            </div>
          ))}

          <div className={running ? "term-line is-busy" : "term-line"} ref={lineRef}>
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
              onChange={(event) => {
                setCommand(event.target.value);
                if (popup) setPopup(null);
              }}
              onKeyDown={keyDown}
              onBlur={() => setPopup(null)}
            />
            {running ? <span className="term-running">›</span> : null}
          </div>
        </div>

        {popup ? (
          <div
            ref={popupRef}
            className={popupBox.below ? "term-completions is-below" : "term-completions"}
            role="listbox"
            aria-label={t("terminal.completions")}
            style={{ top: popupBox.top }}
          >
            {popup.items.map((item, index) => (
              <button
                key={item}
                type="button"
                role="option"
                aria-selected={index === popup.index}
                className={index === popup.index ? "is-active" : ""}
                onMouseDown={(event) => {
                  event.preventDefault();
                  accept(popup, item);
                }}
              >
                {item}
              </button>
            ))}
          </div>
        ) : null}

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
