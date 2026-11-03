export type DeviceMode =
  | "SYSTEM"
  | "RECOVERY"
  | "FASTBOOT"
  | "SIDELOAD"
  | "UNAUTHORIZED"
  | "UNKNOWN";

export type AppMode = "all" | "system" | "nonSystem" | "disabled" | "uninstalled";

export type LogType = "ERROR" | "OK" | "WARNING" | "INFO" | "UNKNOWN";

export interface ToolStatus {
  adb: string | null;
  fastboot: string | null;
  scrcpy: string | null;
  platform: string;
  version: string;
  runtimeDir: string;
  dataDir: string;
  configDir: string;
  imgDir: string;
  apksDir: string;
}

export interface FlashPhase {
  phase: "sending" | "writing" | "installing" | "done";
  received: number;
  total: number | null;
  /** `received` is a real byte counter (sideload), not just the announced size. */
  streamed?: boolean;
}

export interface FlashLine {
  line: string;
}

export interface TermOutput {
  session: number;
  stream: "out" | "err";
  text: string;
}

export interface TermExit {
  session: number;
  code: number;
}

export interface CmdOut {
  stdout: string;
  stderr: string;
  code: number;
}

export interface DeviceEntry {
  id: string;
  name: string;
  mode: DeviceMode;
  connection: "CABLE" | "WIRELESS";
}

export interface DeviceInfo {
  serial: string;
  androidVersion: string;
  sdk: number;
  buildUser: string;
  cpuAbilist: string;
  manufacturer: string;
  model: string;
  board: string;
  device: string;
  ip: string;
  user: string;
  wireless: boolean;
  rotationFree: boolean;
  maxDisplay: number;
  supported: boolean;
  error: string | null;
}

export interface AppEntry {
  package: string;
  name: string | null;
}

export interface AppsResult {
  apps: AppEntry[];
  bridge: boolean;
  mode: string;
  total: number;
}

export interface OpResult {
  target: string;
  ok: boolean;
  output: string;
}

export interface FlashStep {
  command: string;
  output: string;
  ok: boolean;
}

export interface ImageExtraction {
  ok: boolean;
  path: string;
  log: string[];
  error: string | null;
}

export interface NetResult {
  ok: boolean;
  message: string;
  /** "refused" | "timeout" | "unreachable" | "auth" when the failure is known. */
  code?: string | null;
}

export interface EncoderData {
  codec: string;
  encoder: string;
}

export interface ScrcpyMedia {
  video: EncoderData[];
  audio: EncoderData[];
}

export interface UpdateInfo {
  current: string;
  latest: string;
  body: string;
  htmlUrl: string;
  assetUrl: string;
  newVersion: boolean;
}

export interface BloatEntry {
  package: string;
  description: string;
  removal: string;
  list: string;
  system: boolean;
}

export type ThemeMode = "dark" | "light" | "system";

export interface Config {
  language: string;
  feedbackPopup: boolean;
  disclaimerAccepted: boolean;
  ips: string[];
  theme: ThemeMode;
}

export interface LogEntry {
  id: number;
  type: LogType;
  message: string;
  time: string;
}

export type MessageKind = "error" | "warning" | "info";

export interface DialogApi {
  alert: (message: string, kind?: MessageKind) => Promise<void>;
  confirm: (message: string, title?: string) => Promise<boolean>;
  open: (name: string, props?: Record<string, unknown>) => void;
  close: () => void;
  notify: (message: string, kind?: MessageKind | "ok") => void;
}

export interface LocaleBundle {
  code: string;
  name: string;
  source: "bundled" | "external" | string;
  json: string;
}

export interface QrPairing {
  service: string;
  password: string;
  payload: string;
  svg: string;
}

export interface MdnsService {
  name: string;
  service: string;
  address: string;
  ip: string;
  port: string;
}

/** Download progress shown by the busy overlay. `percent` is null while the
 *  total size is unknown (the bar then runs indeterminate). */
export interface DownloadProgress {
  percent: number | null;
  detail?: string;
}
