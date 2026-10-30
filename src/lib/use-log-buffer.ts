import { useCallback, useRef, useState } from "react";
import type { LogEntry, LogType } from "./types";
import { timeNow } from "./util";

const MAX_ENTRIES = 1200;

/** Bounded log buffer shared by the dock, the tabs and the main window. */
export function useLogBuffer() {
  const [logs, setLogs] = useState<LogEntry[]>([]);
  const counter = useRef(0);

  const log = useCallback((type: LogType, message: string) => {
    if (!message) return;
    setLogs((current) => {
      const next = [...current, { id: ++counter.current, type, message, time: timeNow() }];
      return next.length > MAX_ENTRIES ? next.slice(next.length - MAX_ENTRIES) : next;
    });
  }, []);

  const clear = useCallback(() => setLogs([]), []);

  return { logs, log, clear };
}
