import { useEffect, useRef, useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { Btn, IconBtn } from "../components/ui";
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

/** Standalone terminal: the dock only holds the button that opens this window. */
export function TerminalWindow({ serial }: { serial: string }) {
  const { t } = useI18n();
  const [tool, setTool] = useState<"adb" | "fastboot">("adb");
  const [command, setCommand] = useState("");
  const [entries, setEntries] = useState<Entry[]>([]);
  const [busy, setBusy] = useState(false);
  const [history, setHistory] = useState<string[]>([]);
  const [cursor, setCursor] = useState(-1);

  const scrollRef = useRef<HTMLDivElement | null>(null);
  const inputRef = useRef<HTMLTextAreaElement | null>(null);

  useEffect(() => {
    inputRef.current?.focus();
  }, []);

  useEffect(() => {
    const node = scrollRef.current;
    if (node) node.scrollTop = node.scrollHeight;
  }, [entries]);

  const run = async () => {
    const value = command.trim();
    if (!value || busy) return;

    const args = splitArgs(value);
    if (args.length === 0) return;

    setBusy(true);
    setHistory((current) => [...current, value]);
    setCursor(-1);

    try {
      const out = await api.runTool(tool, serial || null, args);
      const text = `${out.stdout}${out.stderr}`.trim();
      setEntries((current) => [
        ...current,
        { id: ++entryId, tool, command: value, output: text || t("terminal.noOutput"), ok: out.code === 0 },
      ]);
      setCommand("");
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

      <div className="window-head">
        <h1>{t("terminal.title")}</h1>
        <span className="badge badge-info mono" title={serial || undefined}>
          {serial || t("value.noDevice")}
        </span>
        <div className="row is-tight right">
          <div className="radio-row">
            <label className="check">
              <input type="radio" checked={tool === "adb"} onChange={() => setTool("adb")} />
              adb
            </label>
            <label className="check">
              <input type="radio" checked={tool === "fastboot"} onChange={() => setTool("fastboot")} />
              fastboot
            </label>
          </div>
          <IconBtn
            icon="trash"
            label={t("terminal.clear")}
            disabled={entries.length === 0}
            onClick={() => setEntries([])}
          />
        </div>
      </div>

      <div className="terminal-body" ref={scrollRef}>
        {entries.length === 0 ? (
          <p className="hint">{t("terminal.empty")}</p>
        ) : (
          entries.map((entry) => (
            <div key={entry.id} className="term-entry">
              <div className="term-cmd">
                <span className="term-prompt">$</span>
                {entry.tool} {entry.command}
              </div>
              <pre className={entry.ok ? "term-out" : "term-out is-error"}>{entry.output}</pre>
            </div>
          ))
        )}
      </div>

      <div className="terminal-foot">
        <textarea
          ref={inputRef}
          className="textarea terminal-input"
          rows={1}
          value={command}
          placeholder={t("terminal.placeholder")}
          onChange={(event) => setCommand(event.target.value)}
          onKeyDown={(event) => {
            if (event.key === "Enter" && !event.shiftKey && !event.altKey) {
              event.preventDefault();
              void run();
            } else if (event.key === "ArrowUp") {
              event.preventDefault();
              recall(-1);
            } else if (event.key === "ArrowDown") {
              event.preventDefault();
              recall(1);
            }
          }}
        />
        <Btn variant="primary" icon="play" disabled={busy || !command.trim()} onClick={() => void run()}>
          {t("button.run")}
        </Btn>
      </div>

      <ResizeHandles />
    </div>
  );
}
