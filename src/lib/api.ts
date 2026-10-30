import { invoke } from "@tauri-apps/api/core";
import type {
  AppMode,
  AppsResult,
  LocaleBundle,
  LocaleMeta,
  MdnsService,
  BloatEntry,
  CmdOut,
  Config,
  DeviceEntry,
  DeviceInfo,
  FlashStep,
  ImageExtraction,
  NetResult,
  OpResult,
  QrPairing,
  ScrcpyMedia,
  ToolStatus,
  UpdateInfo,
  UserEntry,
} from "./types";

export const api = {
  toolStatus: () => invoke<ToolStatus>("tool_status"),

  runTool: (tool: "adb" | "fastboot", serial: string | null, args: string[]) =>
    invoke<CmdOut>("run_tool", { tool, serial, args }),

  devices: (isAdb: boolean) => invoke<DeviceEntry[]>("devices", { isAdb }),

  deviceInfo: (serial: string) => invoke<DeviceInfo>("device_info", { serial }),

  deviceUsers: (serial: string) => invoke<UserEntry[]>("device_users", { serial }),

  appsLoad: (serial: string, user: string, mode: AppMode) =>
    invoke<AppsResult>("apps_load", { serial, user, mode }),

  appsApply: (serial: string, user: string, action: string, packages: string[]) =>
    invoke<OpResult[]>("apps_apply", { serial, user, action, packages }),

  appsInstall: (serial: string, user: string, files: string[], downgrade: boolean) =>
    invoke<OpResult[]>("apps_install", { serial, user, files, downgrade }),

  filesPush: (serial: string, files: string[]) =>
    invoke<OpResult[]>("files_push", { serial, files }),

  apksExtract: (serial: string, packages: string[]) =>
    invoke<OpResult[]>("apks_extract", { serial, packages }),

  permissionGrant: (
    serial: string,
    user: string,
    pkg: string,
    permission: string,
    method: string,
  ) => invoke<CmdOut>("permission_grant", { serial, user, package: pkg, permission, method }),

  bootloaderInfo: (serial: string) => invoke<string>("bootloader_info", { serial }),

  fastbootErase: (serial: string, partitions: string[]) =>
    invoke<FlashStep[]>("fastboot_erase", { serial, partitions }),

  fastbootWipe: (serial: string) => invoke<FlashStep>("fastboot_wipe", { serial }),

  bootloaderOps: (serial: string, op: string) =>
    invoke<FlashStep[]>("bootloader_ops", { serial, op }),

  flashImage: (
    serial: string,
    kind: "flash" | "boot",
    imagePath: string,
    imageType: string,
    partition: string,
  ) => invoke<FlashStep[]>("flash_image", { serial, kind, imagePath, imageType, partition }),

  sideloadZip: (serial: string, zipPath: string) =>
    invoke<FlashStep>("sideload_zip", { serial, zipPath }),

  imageExtract: (serial: string, partition: string) =>
    invoke<ImageExtraction>("image_extract", { serial, partition }),

  adbConnect: (ip: string, port: string, tcpipSerial: string | null) =>
    invoke<NetResult>("adb_connect", { ip, port, tcpipSerial }),

  adbDisconnect: (ip: string, port: string) =>
    invoke<NetResult>("adb_disconnect", { ip, port }),

  adbPair: (ip: string, port: string, code: string) =>
    invoke<NetResult>("adb_pair", { ip, port, code }),

  qrPairingCode: () => invoke<QrPairing>("qr_pairing_code"),

  mdnsServices: () => invoke<MdnsService[]>("mdns_services"),

  adbKill: () => invoke<string>("adb_kill"),

  adbRunning: () => invoke<boolean>("adb_running"),

  rotationGet: (serial: string) => invoke<boolean>("rotation_get", { serial }),

  rotationSet: (serial: string, enabled: boolean, display: number | null) =>
    invoke<boolean>("rotation_set", { serial, enabled, display }),

  injectText: (serial: string, text: string) =>
    invoke<CmdOut>("inject_text", { serial, text }),

  processes: (serial: string) => invoke<string[][]>("processes", { serial }),

  forceStop: (serial: string, names: string[]) =>
    invoke<number>("force_stop", { serial, names }),

  logcatStart: (serial: string, mode: "logcat" | "getevent", target: string) =>
    invoke<void>("logcat_start", { serial, mode, target }),

  logcatStop: () => invoke<void>("logcat_stop"),

  logcatClear: (serial: string) => invoke<string>("logcat_clear", { serial }),

  scrcpyAvailable: () => invoke<boolean>("scrcpy_available"),

  scrcpyVersion: () => invoke<string>("scrcpy_version"),

  scrcpyEncoders: (serial: string) => invoke<ScrcpyMedia>("scrcpy_encoders", { serial }),

  scrcpyRun: (serial: string, args: string[]) => invoke<void>("scrcpy_run", { serial, args }),

  downloadPlatformTools: () => invoke<void>("download_platform_tools"),

  downloadScrcpy: () => invoke<void>("download_scrcpy"),

  downloadBloatwareList: () => invoke<string>("download_bloatware_list"),

  checkUpdate: () => invoke<UpdateInfo>("check_update"),

  applyUpdate: (url: string) => invoke<void>("apply_update", { url }),

  bloatwareScan: (serial: string, user: string, level: string) =>
    invoke<BloatEntry[]>("bloatware_scan", { serial, user, level }),

  bloatwareAvailable: () => invoke<boolean>("bloatware_available"),

  bloatwareListPath: () => invoke<string>("bloatware_list_path"),

  configGet: () => invoke<Config>("config_get"),

  localesBundles: () => invoke<LocaleBundle[]>("locales_bundles"),

  localesCatalog: () => invoke<LocaleMeta[]>("locales_catalog"),

  localesOpenDir: () => invoke<string>("locales_open_dir"),

  localesExportTemplate: () => invoke<string>("locales_export_template"),

  configSave: (config: Config) => invoke<void>("config_save", { config }),

  ipAdd: (ip: string) => invoke<string[]>("ip_add", { ip }),

  ipClear: () => invoke<string[]>("ip_clear"),

  onlineCheck: () => invoke<boolean>("online_check"),

  openUrl: (url: string) => invoke<void>("open_url", { url }),

  pickFiles: (kind: "apk" | "img" | "zip" | "any", multiple: boolean) =>
    invoke<string[]>("pick_files", { kind, multiple }),

  clipboardWrite: (text: string) => invoke<void>("clipboard_write", { text }),

  watcherStart: () => invoke<void>("watcher_start"),

  watcherStop: () => invoke<void>("watcher_stop"),

  exit: (killAdb: boolean) => invoke<void>("exit_app", { killAdb }),

  windowMinimize: () => invoke<void>("window_minimize"),

  windowToggleMaximize: () => invoke<boolean>("window_toggle_maximize"),

  windowIsMaximized: () => invoke<boolean>("window_is_maximized"),

  windowClose: () => invoke<void>("window_close"),

  windowStartDrag: () => invoke<void>("window_start_drag"),

  windowStartResize: (direction: string) => invoke<void>("window_start_resize", { direction }),

  openSubWindow: (kind: "logs" | "tasks", serial: string, user: string) =>
    invoke<void>("open_sub_window", { kind, serial, user }),
};
