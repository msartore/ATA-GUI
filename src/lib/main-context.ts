import { createContext, useContext } from "react";
import type { TFunction } from "../i18n";
import type {
  AppEntry,
  AppMode,
  DeviceEntry,
  DeviceInfo,
  LogEntry,
  LogType,
  ToolStatus,
} from "./types";

export interface MainApi {
  t: TFunction;
  log: (type: LogType, message: string) => void;
  tools: ToolStatus | null;
  platform: string;

  devices: DeviceEntry[];
  device: DeviceEntry | null;
  deviceIndex: number;
  selectDevice: (index: number) => void;
  refreshDevices: () => Promise<void>;

  info: DeviceInfo | null;
  user: string;
  fastbootInfo: string;
  syncDevice: () => Promise<void>;

  apps: AppEntry[];
  appsMode: AppMode;
  bridge: boolean;
  selectedApps: Set<string>;
  setSelectedApps: (next: Set<string>) => void;
  toggleApp: (pkg: string) => void;
  search: string;
  setSearch: (value: string) => void;
  loadApps: (mode?: AppMode) => Promise<void>;

  rotationFree: boolean;
  maxDisplay: number;
  setRotation: (enabled: boolean, display: number | null) => Promise<void>;

  ips: string[];
  addIp: (ip: string) => Promise<void>;

  busy: string | null;
  progress: number | null;
  runBusy: <T>(label: string, task: () => Promise<T>) => Promise<T | undefined>;

  openDialog: (name: string, props?: Record<string, unknown>) => void;
  closeDialog: () => void;

  installApks: (paths: string[], downgrade: boolean) => Promise<void>;
  transferFiles: (paths: string[]) => Promise<void>;
  extractApks: (packages: string[]) => Promise<void>;

  logs: LogEntry[];
  clearLogs: () => void;
  openTerminal: () => void;
  openLogs: () => void;
  openTasks: () => void;
  shareScreen: () => Promise<void>;
}

export const MainContext = createContext<MainApi | null>(null);

export function useMain(): MainApi {
  const value = useContext(MainContext);
  if (!value) {
    throw new Error("useMain must be used inside MainContext");
  }
  return value;
}
