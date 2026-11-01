import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { api } from "../lib/api";
import { broadcastTheme } from "../lib/theme";
import { devicesForTab, tabUsesAdb, type Tab } from "../lib/devices";
import { DEFAULT_CONFIG } from "../lib/config";
import { useAppBootstrap } from "../lib/use-bootstrap";
import { useLogBuffer } from "../lib/use-log-buffer";
import { useI18n } from "../i18n";
import { useUi } from "../components/ui";
import { MainContext, type MainApi } from "../lib/main-context";
import { TitleBar, ResizeHandles } from "../components/TitleBar";
import { Dock } from "../components/Dock";
import { SystemTab } from "./SystemTab";
import { FastbootTab } from "./FastbootTab";
import { RecoveryTab } from "./RecoveryTab";
import { ToolsTab } from "./ToolsTab";
import {
  AboutDialog,
  BloatwareDialog,
  DefaultAppDialog,
  DisclaimerDialog,
  FeedbackDialog,
  LoadingDialog,
  MissingDialog,
  OemDriversDialog,
  PackageMenuDialog,
  PairingDialog,
  ScrcpyDialog,
  ScrollableDialog,
  SettingsDialog,
  ShellCommandsDialog,
  UpdateDialog,
} from "../components/dialogs";
import { BootloaderDialog } from "../components/BootloaderDialog";
import { QrPairingDialog } from "../components/QrPairingDialog";
import { basename, errorKey, errorText } from "../lib/util";
import type {
  AppEntry,
  AppMode,
  Config,
  DeviceEntry,
  DeviceInfo,
  DownloadProgress,
  ToolStatus,
} from "../lib/types";


export function MainWindow({ config, setConfig }: { config: Config; setConfig: (config: Config) => void }) {
  const { t } = useI18n();
  const ui = useUi();

  const [tab, setTab] = useState<Tab>("system");
  const [tools, setTools] = useState<ToolStatus | null>(null);
  const [allDevices, setAllDevices] = useState<DeviceEntry[]>([]);
  const [deviceIndex, setDeviceIndex] = useState(0);
  const [info, setInfo] = useState<DeviceInfo | null>(null);
  const [user, setUser] = useState("0");
  const [fastbootInfo, setFastbootInfo] = useState("");
  const [apps, setApps] = useState<AppEntry[]>([]);
  const [appsMode, setAppsMode] = useState<AppMode>("nonSystem");
  const [bridge, setBridge] = useState(false);
  const [selectedApps, setSelectedApps] = useState<Set<string>>(new Set());
  const [search, setSearch] = useState("");
  const [rotationFree, setRotationFree] = useState(false);
  const [maxDisplay, setMaxDisplay] = useState(0);
  const [ips, setIps] = useState<string[]>(config.ips);
  const [busy, setBusy] = useState<string | null>(null);
  const [cancelable, setCancelable] = useState(false);
  const [progress, setProgress] = useState<DownloadProgress | null>(null);
  const [dialog, setDialog] = useState<{ name: string; props?: Record<string, unknown> } | null>(null);
  const [missing, setMissing] = useState<"tools" | "scrcpy" | null>(null);
  const [disclaimer, setDisclaimer] = useState(!config.disclaimerAccepted);

  const tabRef = useRef<Tab>(tab);
  tabRef.current = tab;
  const devicesRef = useRef<DeviceEntry[]>([]);
  const deviceIndexRef = useRef(deviceIndex);
  deviceIndexRef.current = deviceIndex;
  const appsModeRef = useRef<AppMode>(appsMode);
  appsModeRef.current = appsMode;
  const devicesChangedRef = useRef<(list: DeviceEntry[]) => void>(() => {});
  // Guards against loading the same device twice: the bootstrap, the watcher's
  // first "devices:changed" and the tab switch can all ask for a sync.
  const syncInFlight = useRef<string | null>(null);
  const lastSync = useRef<{ id: string; at: number }>({ id: "", at: 0 });
  // The same phone answers on several transports (wireless debugging port vs
  // legacy 5555): sync once per host, not once per endpoint.
  const syncKey = (entry: DeviceEntry) => entry.id.split(":")[0];

  const devices = useMemo(() => devicesForTab(allDevices, tab), [allDevices, tab]);
  devicesRef.current = devices;
  const device = devices[deviceIndex] ?? null;

  const { logs, log, clear: clearLogs } = useLogBuffer();

  /** Clear everything that belongs to the currently selected device. */
  const resetDeviceView = useCallback(() => {
    setInfo(null);
    setApps([]);
    setSelectedApps(new Set());
  }, []);

  const runBusy = useCallback(
    async <T,>(
      label: string,
      task: () => Promise<T>,
      options?: { cancelable?: boolean },
    ): Promise<T | undefined> => {
      setBusy(label);
      setCancelable(Boolean(options?.cancelable));
      setProgress(null);
      try {
        return await task();
      } catch (error) {
        const text = errorText(error);
        const key = errorKey(text);
        const message = key ? t(key) : text;
        log("ERROR", message);
        await ui.alert(message);
        return undefined;
      } finally {
        setBusy(null);
        setCancelable(false);
        setProgress(null);
      }
    },
    [log, t, ui],
  );

  const updateConfig = useCallback(
    (next: Config) => {
      setConfig(next);
      setIps(next.ips);
      void api.configSave(next).catch(() => {});
      void broadcastTheme(next.theme);
    },
    [setConfig],
  );

  const loadAppsFor = useCallback(
    async (target: DeviceEntry, targetUser: string, mode: AppMode) => {
      log("INFO", t("log.loadingApps"));
      try {
        const result = await api.appsLoad(target.id, targetUser, mode);
        setApps(result.apps);
        setBridge(result.bridge);
        setSelectedApps(new Set());
        setSearch("");
        setAppsMode(mode);
        log("OK", t("log.appsLoaded"));
      } catch (error) {
        log("ERROR", `${t("log.appsLoadFailed")}: ${errorText(error)}`);
      }
    },
    [log, t],
  );

  const syncFor = useCallback(
    async (target: DeviceEntry, options?: { force?: boolean }) => {
      const now = Date.now();
      const key = syncKey(target);
      if (!options?.force) {
        if (syncInFlight.current === key) return;
        if (lastSync.current.id === key && now - lastSync.current.at < 1500) return;
      }
      syncInFlight.current = key;
      lastSync.current = { id: key, at: now };

      try {
        if (tabRef.current === "fastboot") {
          try {
            const text = await api.bootloaderInfo(target.id);
            setFastbootInfo(text);
            log("OK", t("log.fastbootInfoExtracted"));
          } catch (error) {
            log("ERROR", errorText(error));
          }
          return;
        }
        if (tabRef.current !== "system") return;

        log("INFO", t("log.checkingDevice"));
        try {
          const result = await api.deviceInfo(target.id);
          if (result.error) {
            setInfo(null);
            log("ERROR", t("log.deviceInfoFailed"));
            await ui.alert(
              `${t("log.deviceInfoFailed")}\n\n${t(errorKey(result.error) || "error.generic")}`,
            );
            return;
          }
          setInfo(result);
          setUser(result.user || "0");
          setRotationFree(result.rotationFree);
          setMaxDisplay(result.maxDisplay);
          log("OK", t("log.deviceInfoExtracted"));
          await loadAppsFor(target, result.user || "0", appsModeRef.current);
        } catch (error) {
          log("ERROR", errorText(error));
        }
      } finally {
        syncInFlight.current = null;
      }
    },
    [loadAppsFor, log, t, ui],
  );

  const refreshDevices = useCallback(async () => {
    const activeTab = tabRef.current;
    const isAdb = tabUsesAdb(activeTab);
    log("INFO", t("log.searchingDevices"));
    try {
      const list = await api.devices(isAdb);
      setAllDevices(list);
      const filtered = devicesForTab(list, activeTab);
      if (deviceIndexRef.current >= filtered.length) setDeviceIndex(0);
      if (filtered.length === 0) {
        resetDeviceView();
        log("ERROR", t("log.noDevices"));
      } else {
        log("OK", filtered.length === 1 ? t("log.oneDevice") : t("log.moreDevices"));
      }
      return filtered;
    } catch (error) {
      log("ERROR", errorText(error));
      return [];
    }
  }, [log, t]);

  /**
   * React to `devices:changed`: keep the selected device if it is still there,
   * otherwise attach the first available one and load its data (plug & play).
   */
  const handleDevicesChanged = useCallback(
    (list: DeviceEntry[]) => {
      setAllDevices(list);
      const filtered = devicesForTab(list, tabRef.current);
      const current = devicesRef.current[deviceIndexRef.current];

      if (current) {
        const index = filtered.findIndex((entry) => entry.id === current.id);
        if (index >= 0) {
          if (index !== deviceIndexRef.current) setDeviceIndex(index);
          return;
        }
        // Same phone, different transport: keep the data we already loaded.
        const sameHost = filtered.findIndex((entry) => syncKey(entry) === syncKey(current));
        if (sameHost >= 0) {
          setDeviceIndex(sameHost);
          return;
        }
      }

      setDeviceIndex(0);
      resetDeviceView();
      const next = filtered[0];
      if (!next) {
        const pending = list[0];
        if (pending) log("WARNING", t("log.deviceNotReady", { id: pending.id }));
        return;
      }
      log("OK", t("log.deviceAttached", { id: next.id }));
      void syncFor(next);
    },
    [log, syncFor, t],
  );
  devicesChangedRef.current = handleDevicesChanged;

  const selectDevice = useCallback(
    (index: number) => {
      const target = devicesRef.current[index];
      setDeviceIndex(index);
      resetDeviceView();
      if (target) void syncFor(target, { force: true });
    },
    [syncFor],
  );

  const syncDevice = useCallback(async () => {
    const target = devicesRef.current[deviceIndex];
    if (!target) return;
    await syncFor(target, { force: true });
  }, [deviceIndex, syncFor]);

  const loadApps = useCallback(
    async (mode?: AppMode) => {
      const target = devicesRef.current[deviceIndex];
      if (!target) return;
      await loadAppsFor(target, user, mode ?? appsModeRef.current);
    },
    [deviceIndex, loadAppsFor, user],
  );

  const addIp = useCallback(
    async (ip: string) => {
      const list = await api.ipAdd(ip).catch(() => ips);
      setIps(list);
      updateConfig({ ...config, ips: list });
    },
    [config, ips, updateConfig],
  );

  const installApks = useCallback(
    async (paths: string[], downgrade: boolean) => {
      const target = devicesRef.current[deviceIndex];
      if (!target) {
        await ui.alert(t("error.noDeviceSelected"), "warning");
        return;
      }
      const apks = paths.filter((path) => path.toLowerCase().endsWith(".apk"));
      if (!apks.length) return;

      await runBusy(t("log.installing", { name: basename(apks[0]) }), async () => {
        for (const file of apks) {
          log("INFO", t("log.installing", { name: basename(file) }));
          const results = await api.appsInstall(target.id, user, [file], downgrade);
          for (const result of results) {
            log(result.ok ? "OK" : "ERROR",
              result.ok
                ? t("log.installed", { name: basename(result.target) })
                : `${t("log.installFailed", { name: basename(result.target) })}\n${result.output}`);
          }
        }
        if (tabRef.current === "system") await loadAppsFor(target, user, appsModeRef.current);
      }, { cancelable: true });
    },
    [deviceIndex, loadAppsFor, log, runBusy, t, ui, user],
  );

  const transferFiles = useCallback(
    async (paths: string[]) => {
      const target = devicesRef.current[deviceIndex];
      if (!target) {
        await ui.alert(t("error.noDeviceSelected"), "warning");
        return;
      }
      await runBusy(t("log.fileTransferStarting", { count: paths.length }), async () => {
        log("INFO", t("log.fileTransferStarting", { count: paths.length }));
        const results = await api.filesPush(target.id, paths);
        const failed = results.filter((result) => !result.ok);
        if (failed.length) {
          log("ERROR", `${t("log.genericFailed")}\n${failed.map((item) => item.target).join("\n")}`);
        } else {
          log("OK", t("log.fileTransferDone"));
        }
      }, { cancelable: true });
    },
    [deviceIndex, log, runBusy, t, ui],
  );

  const extractApks = useCallback(
    async (packages: string[]) => {
      const target = devicesRef.current[deviceIndex];
      if (!target) return;
      await runBusy(t("button.extract"), async () => {
        const results = await api.apksExtract(target.id, packages);
        const failed = results.filter((result) => !result.ok);
        const saved = results.length - failed.length;

        if (failed.length) log("ERROR", failed.map((item) => item.target).join("\n"));
        else log("OK", t("log.apksExtracted", { path: tools?.apksDir ?? "APKS" }));

        if (saved > 0 && tools?.apksDir) {
          const folder = tools.apksDir;
          ui.notify(t("toast.apksExtracted", { count: saved }), "ok", {
            label: t("toast.showInFolder"),
            run: () => void api.revealPath(folder).catch(() => undefined),
          });
        }
      }, { cancelable: true });
    },
    [deviceIndex, log, runBusy, t, tools?.apksDir, ui],
  );

  const setRotation = useCallback(
    async (enabled: boolean, display: number | null) => {
      const target = devicesRef.current[deviceIndex];
      if (!target) return;
      try {
        const updated = await api.rotationSet(target.id, enabled, display);
        setRotationFree(updated);
        setInfo((current) => (current ? { ...current, rotationFree: updated } : current));
        log("INFO", updated ? t("log.freeRotationSet") : t("log.freeRotationUnset"));
      } catch (error) {
        log("ERROR", errorText(error));
      }
    },
    [deviceIndex, log, t],
  );

  const openTerminal = useCallback(() => {
    const target = devicesRef.current[deviceIndex];
    void api.openSubWindow("terminal", target?.id ?? "", user);
  }, [deviceIndex, user]);

  const ensureScrcpy = useCallback(async () => {
    const status = await api.toolStatus().catch(() => null);
    if (status) setTools(status);
    if (status?.scrcpy) return true;
    setMissing("scrcpy");
    return false;
  }, []);

  const shareScreen = useCallback(async () => {
    const target = devicesRef.current[deviceIndex];
    if (!target) return;
    if (!(await ensureScrcpy())) return;
    setDialog({ name: "scrcpy" });
  }, [deviceIndex, ensureScrcpy]);

  const openLogs = useCallback(() => {
    const target = devicesRef.current[deviceIndex];
    if (!target) return;
    void api.openSubWindow("logs", target.id, user);
  }, [deviceIndex, user]);

  const openTasks = useCallback(() => {
    const target = devicesRef.current[deviceIndex];
    if (!target) return;
    void api.openSubWindow("tasks", target.id, user);
  }, [deviceIndex, user]);

  const checkUpdates = useCallback(
    async (interactive: boolean) => {
      log("INFO", t("log.updateChecking"));
      try {
        const update = await api.checkUpdate();
        if (update.newVersion) {
          log("WARNING", t("log.updateAvailable", { version: update.latest }));
          const accepted = await ui.confirm(
            `${t("log.updateAvailable", { version: update.latest })}\n${t("label.currentRelease")}: ${update.current}\n\n${t("button.update")}?`,
            t("button.update"),
          );
          if (accepted) {
            if (update.htmlUrl) void api.openUrl(update.htmlUrl);
            setDialog({ name: "update", props: { url: update.assetUrl } });
          }
        } else {
          log("OK", t("log.updateNone"));
          if (interactive) ui.notify(t("state.upToDate"), "ok");
        }
      } catch (error) {
        log("WARNING", t("state.offline"));
        if (interactive) await ui.alert(errorText(error));
      }
    },
    [log, t, ui],
  );

  const completeDisclaimer = useCallback(() => {
    setDisclaimer(false);
    updateConfig({ ...config, disclaimerAccepted: true });
    if (config.feedbackPopup) setDialog({ name: "feedback" });
  }, [config, updateConfig]);

  /* ------------------------------------------------------------ bootstrap */

  useAppBootstrap({
    t,
    log,
    checkUpdates,
    refreshDevices,
    syncFor,
    onTools: setTools,
    onMissing: setMissing,
    onProgress: setProgress,
    onDevices: (list) => devicesChangedRef.current(list),
  });

  /* ------------------------------------------------------------ tab change */

  const changeTab = useCallback(
    async (next: Tab) => {
      if (next === tabRef.current) return;
      setTab(next);
      tabRef.current = next;
      setAllDevices([]);
      setDeviceIndex(0);
      resetDeviceView();
      setFastbootInfo("");
      setRotationFree(false);
      if (next === "tools") return;
      const found = await refreshDevices();
      if (found.length) await syncFor(found[0], { force: true });
    },
    [refreshDevices, syncFor],
  );

  /* ------------------------------------------------------------ shortcuts */

  useEffect(() => {
    const onKey = (event: KeyboardEvent) => {
      const mod = event.ctrlKey || event.metaKey;

      if (event.key === "Escape") {
        setDialog(null);
        return;
      }
      if (mod && event.key.toLowerCase() === "f") {
        const search = document.querySelector<HTMLInputElement>("[data-search]");
        if (search) {
          event.preventDefault();
          search.focus();
          search.select();
        }
        return;
      }
      if (event.key === "F5") {
        event.preventDefault();
        void refreshDevices();
        return;
      }
      if (mod && event.key.toLowerCase() === "l") {
        event.preventDefault();
        clearLogs();
        return;
      }
      if (mod && ["1", "2", "3", "4"].includes(event.key)) {
        event.preventDefault();
        const order: Tab[] = ["system", "fastboot", "recovery", "tools"];
        void changeTab(order[Number(event.key) - 1]);
      }
    };

    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [changeTab, clearLogs, refreshDevices]);

  /* ------------------------------------------------------------ downloads */

  const downloadTools = useCallback(async () => {
    await runBusy(t("log.toolsDownloading"), async () => {
      await api.downloadPlatformTools();
      const status = await api.toolStatus();
      setTools(status);
      log("OK", t("log.toolsReady"));
      // Only dismiss the blocking prompt when both tools really exist — a
      // failed download must not leave a working-looking UI without adb.
      if (!status.adb || !status.fastboot) return;
      setMissing(null);
      // No restart needed: reconnect the endpoints that were unreachable while
      // adb was missing and load whatever is attached now.
      const reconnected = await api.autoConnect().catch(() => [] as string[]);
      for (const endpoint of reconnected) {
        log("OK", t("log.connected", { ip: endpoint }));
      }
      const found = await refreshDevices();
      if (found.length) await syncFor(found[0], { force: true });
    });
  }, [log, refreshDevices, runBusy, syncFor, t]);

  const downloadScrcpy = useCallback(async () => {
    await runBusy(t("log.scrcpyDownloading"), async () => {
      await api.downloadScrcpy();
      const status = await api.toolStatus();
      setTools(status);
      if (!status.scrcpy) {
        // The archive unpacked but no binary was found: say so instead of
        // leaving the prompt on screen as if nothing happened.
        log("ERROR", t("error.scrcpyMissing"));
        await ui.alert(t("error.scrcpyMissing"));
        return;
      }
      log("OK", t("log.scrcpyReady"));
      setMissing(null);
    });
  }, [log, runBusy, t, ui]);

  const requestClose = useCallback(async () => {
    const running = await api.adbRunning().catch(() => false);
    if (running) {
      const kill = await ui.confirm(t("confirm.killAdbBody"), t("confirm.killAdbTitle"));
      await api.exit(kill).catch(() => {});
    } else {
      await api.exit(false).catch(() => {});
    }
  }, [t, ui]);

  /* ------------------------------------------------------------ context */

  const context: MainApi = {
    t,
    log,
    tools,
    platform: tools?.platform ?? "",
    devices,
    device,
    deviceIndex,
    selectDevice,
    refreshDevices: async () => {
      await refreshDevices();
    },
    info,
    user,
    fastbootInfo,
    syncDevice,
    apps,
    appsMode,
    bridge,
    selectedApps,
    setSelectedApps,
    toggleApp: (pkg: string) => {
      setSelectedApps((current) => {
        const next = new Set(current);
        if (next.has(pkg)) next.delete(pkg);
        else next.add(pkg);
        return next;
      });
    },
    search,
    setSearch,
    loadApps,
    rotationFree,
    maxDisplay,
    setRotation,
    ips,
    addIp,
    busy,
    runBusy,
    openDialog: (name, props) => setDialog({ name, props }),
    closeDialog: () => setDialog(null),
    installApks,
    transferFiles,
    extractApks,
    logs,
    clearLogs,
    openTerminal,
    openLogs,
    openTasks,
    shareScreen,
  };

  return (
    <MainContext.Provider value={context}>
      <div className={`app ${tools?.platform === "macos" ? "is-macos" : ""}`}>
        <TitleBar
          tools={tools}
          onSettings={() => setDialog({ name: "settings" })}
          onFeedback={() => setDialog({ name: "feedback" })}
          onOemDrivers={() => setDialog({ name: "oemDrivers" })}
          onKillAdb={async () => {
            const output = await api.adbKill();
            log("OK", t("log.adbKilled"));
            if (output.trim()) log("INFO", output);
            resetDeviceView();
            setAllDevices([]);
            setDeviceIndex(0);
            log("INFO", t("log.adbStopped"));
          }}
          onVideoTutorial={() => void api.openUrl("https://github.com/MassimilianoSartore/ATA-GUI/wiki")}
          onClose={() => void requestClose()}
        />

        <nav className="tabs">
          {(
            [
              ["system", "tab.system"],
              ["fastboot", "tab.fastboot"],
              ["recovery", "tab.recovery"],
              ["tools", "tab.tools"],
            ] as Array<[Tab, string]>
          ).map(([key, label]) => (
            <button
              key={key}
              type="button"
              className={`tab ${tab === key ? "is-active" : ""}`}
              onClick={() => void changeTab(key)}
            >
              <span className="tab-dot" />
              {t(label)}
            </button>
          ))}
        </nav>

        <div className="workspace">
          {tab === "system" ? <SystemTab /> : null}
          {tab === "fastboot" ? <FastbootTab /> : null}
          {tab === "recovery" ? <RecoveryTab /> : null}
          {tab === "tools" ? <ToolsTab /> : null}
        </div>

        <Dock />
        <ResizeHandles />

        {disclaimer ? (
          <DisclaimerDialog
            onAccept={completeDisclaimer}
            onDecline={() => void api.exit(false)}
          />
        ) : null}

        {missing && !disclaimer && !busy ? (
          <MissingDialog
            title={missing === "tools" ? t("dialog.missingTitle") : t("dialog.scrcpyMissingTitle")}
            body={
              missing === "tools" ? t("dialog.platformToolsBody") : t("dialog.scrcpyBody")
            }
            license={missing === "tools" ? t("dialog.licenseTools") : t("dialog.licenseScrcpy")}
            licenseUrl={
              missing === "tools"
                ? "https://developer.android.com/license"
                : "https://raw.githubusercontent.com/Genymobile/scrcpy/master/LICENSE"
            }
            onYes={() => void (missing === "tools" ? downloadTools() : downloadScrcpy())}
            onNo={() => setMissing(null)}
          />
        ) : null}

        {dialog?.name === "settings" ? (
          <SettingsDialog config={config} tools={tools} onChange={updateConfig} onClose={() => setDialog(null)} />
        ) : null}
        {dialog?.name === "about" ? (
          <AboutDialog version={tools?.version ?? ""} onClose={() => setDialog(null)} />
        ) : null}
        {dialog?.name === "feedback" && !disclaimer && !missing ? (
          <FeedbackDialog onClose={() => setDialog(null)} />
        ) : null}
        {dialog?.name === "oemDrivers" ? <OemDriversDialog onClose={() => setDialog(null)} /> : null}
        {dialog?.name === "scrollable" ? (
          <ScrollableDialog title={String(dialog.props?.title ?? "")} onClose={() => setDialog(null)} wide>
            <pre className="pre-block">{String(dialog.props?.text ?? "")}</pre>
          </ScrollableDialog>
        ) : null}
        {dialog?.name === "pairing" ? (
          <PairingDialog
            ip={String(dialog.props?.ip ?? "")}
            port={String(dialog.props?.port ?? "5555")}
            onDone={(ok) => {
              if (ok) log("OK", t("log.paired", { ip: String(dialog.props?.ip ?? "") }));
              else log("ERROR", t("log.pairFailed", { ip: String(dialog.props?.ip ?? "") }));
              setDialog(null);
            }}
            onClose={() => setDialog(null)}
          />
        ) : null}
        {dialog?.name === "packageMenu" && device ? (
          <PackageMenuDialog
            serial={device.id}
            user={user}
            packages={(dialog.props?.packages as string[]) ?? []}
            onClose={() => setDialog(null)}
            onDone={() => void loadApps(appsMode)}
          />
        ) : null}
        {dialog?.name === "defaultApp" && device ? (
          <DefaultAppDialog serial={device.id} pkg={String(dialog.props?.pkg ?? "")} onClose={() => setDialog(null)} />
        ) : null}
        {dialog?.name === "shellCommands" && device ? (
          <ShellCommandsDialog
            serial={device.id}
            user={user}
            pkg={String(dialog.props?.pkg ?? "")}
            onClose={() => setDialog(null)}
          />
        ) : null}
        {dialog?.name === "bloatware" && device ? (
          <BloatwareDialog
            serial={device.id}
            user={user}
            onClose={() => setDialog(null)}
            onApplied={() => void loadApps(appsMode)}
          />
        ) : null}
        {dialog?.name === "qr" ? <QrPairingDialog onClose={() => setDialog(null)} /> : null}
        {dialog?.name === "scrcpy" && device ? (
          <ScrcpyDialog serial={device.id} onClose={() => setDialog(null)} />
        ) : null}
        {dialog?.name === "bootloader" && device ? (
          <BootloaderDialog
            serial={device.id}
            onClose={() => setDialog(null)}
            onLog={(type, message) => log(type, message)}
          />
        ) : null}
        {dialog?.name === "update" ? (
          <UpdateDialog url={String(dialog.props?.url ?? "")} onClose={() => setDialog(null)} />
        ) : null}

        {busy ? (
          <LoadingDialog
            label={busy}
            onCancel={cancelable ? () => void api.cancelOperation() : undefined}
            detail={progress?.detail}
            progress={progress ? progress.percent : null}
          />
        ) : null}
      </div>
    </MainContext.Provider>
  );
}
