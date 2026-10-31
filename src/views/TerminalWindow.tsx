import { useEffect, useRef, useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { TitleBar, ResizeHandles } from "../components/TitleBar";
import { errorText, splitArgs } from "../lib/util";

interface Entry {
  id: number;
  tool: "adb" | "fastboot";
  command: string;
  output: string;
  ok: boolean;
}

let entryId = 0;

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
  const [busy, setBusy] = useState(false);
  const [history, setHistory] = useState<string[]>([]);
  const [cursor, setCursor] = useState(-1);

  const scrollRef = useRef<HTMLDivElement | null>(null);
  const inputRef = useRef<HTMLInputElement | null>(null);

  const prompt = `ata@${promptHost(serial)}:~$`;

  useEffect(() => {
    inputRef.current?.focus();
  }, []);

  useEffect(() => {
    const node = scrollRef.current;
    if (node) node.scrollTop = node.scrollHeight;
  }, [entries, busy]);

  const run = async () => {
    const value = command.trim();
    if (!value || busy) return;

    const args = splitArgs(value);
    if (args.length === 0) return;

    setBusy(true);
    setHistory((current) => [...current, value]);
    setCursor(-1);
    setCommand("");

    try {
      const out = await api.runTool(tool, serial || null, args);
      const text = `${out.stdout}${out.stderr}`.replace(/\r\n/g, "\n").trim();
      setEntries((current) => [
        ...current,
        { id: ++entryId, tool, command: value, output: text || t("terminal.noOutput"), ok: out.code === 0 },
      ]);
    } catch (error) {
      setEntries((current) => [
        ...current,
        { id: ++entryId, tool, command: value, output: errorText(error), ok: false },
      ]);
    } finally {
      setBusy(false);
      inputRef.current?.focus();
    }
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
            <div key={entry.id}>
              <div className="term-cmd">
                <span className="term-prompt">{prompt}</span>
                {entry.command}
              </div>
              <pre className={entry.ok ? "term-out" : "term-out is-error"}>{entry.output}</pre>
            </div>
          ))}

          {busy ? (
            <div className="term-cmd">
              <span className="term-prompt">{prompt}</span>
              <span className="term-running">…</span>
            </div>
          ) : null}

          <div className="term-line">
            <span className="term-prompt">{prompt}</span>
            <input
              ref={inputRef}
              className="term-input"
              value={command}
              spellCheck={false}
              autoComplete="off"
              autoCapitalize="off"
              aria-label={t("terminal.title")}
              onChange={(event) => setCommand(event.target.value)}
              onKeyDown={(event) => {
                if (event.key === "Enter") {
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
                } else if ((event.key === "u" || event.key === "c") && event.ctrlKey) {
                  event.preventDefault();
                  setCommand("");
                }
              }}
            />
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
