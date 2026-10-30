import { useEffect, useRef } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "./api";
import type { TFunction } from "../i18n";
import type { DeviceEntry, LogType, ToolStatus } from "./types";

export interface BootstrapHooks {
  t: TFunction;
  log: (type: LogType, message: string) => void;
  checkUpdates: (interactive: boolean) => Promise<void>;
  refreshDevices: () => Promise<DeviceEntry[]>;
  syncFor: (device: DeviceEntry) => Promise<void>;
  onTools: (tools: ToolStatus | null) => void;
  onMissing: (what: "tools" | null) => void;
  onProgress: (value: number | null) => void;
  onDevices: (devices: DeviceEntry[]) => void;
}

/**
 * Owns the IPC side of the main window: tool check, update probe, device
 * watcher subscription, download progress and the initial device sync.
 * Callbacks are read through a ref so the effect runs exactly once and never
 * captures a stale closure.
 */
export function useAppBootstrap(hooks: BootstrapHooks): void {
  const latest = useRef(hooks);
  latest.current = hooks;

  useEffect(() => {
    let disposed = false;
    const unlisteners: Array<() => void> = [];

    const subscribe = async <T,>(event: string, handler: (payload: T) => void) => {
      try {
        const stop = await listen<T>(event, (message) => handler(message.payload));
        if (disposed) stop();
        else unlisteners.push(stop);
      } catch {
        /* events unavailable (e.g. plain browser preview) */
      }
    };

    const start = async () => {
      const current = latest.current;
      current.log("INFO", current.t("log.connectionChecking"));

      const status = await api.toolStatus().catch(() => null);
      if (disposed) return;
      current.onTools(status);
      if (!status?.adb || !status?.fastboot) current.onMissing("tools");

      const online = await api.onlineCheck().catch(() => false);
      if (disposed) return;
      if (online) void current.checkUpdates(false);
      else current.log("WARNING", current.t("log.offline"));

      current.log("OK", current.t("log.connectionChecked"));
      current.log("INFO", current.t("log.adbStarting"));

      void api.watcherStart().catch(() => {});

      await subscribe<DeviceEntry[]>("devices:changed", (payload) => {
        latest.current.onDevices(payload ?? []);
      });

      await subscribe<{ phase: string; received: number; total: number | null }>(
        "download:progress",
        ({ received, total }) => {
          if (total && total > 0) {
            latest.current.onProgress(Math.min(99, Math.round((received / total) * 100)));
          }
        },
      );

      if (disposed) return;
      const found = await latest.current.refreshDevices();
      if (found.length) await latest.current.syncFor(found[0]);
    };

    void start();

    return () => {
      disposed = true;
      unlisteners.forEach((stop) => stop());
      void api.watcherStop().catch(() => {});
    };
  }, []);
}
