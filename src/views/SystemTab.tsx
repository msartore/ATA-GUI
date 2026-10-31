import { useCallback, useEffect, useMemo, useState } from "react";
import type { ComponentProps } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, DropZone, Dropdown, Empty, Field, IconBtn, MenuItem, Panel, useUi } from "../components/ui";
import { errorKey, errorText } from "../lib/util";
import { VirtualList } from "../components/VirtualList";
import { InfoTip } from "../components/InfoTip";
import type { AppMode, MdnsService } from "../lib/types";

const ATA_BRIDGE_URL =
  "https://play.google.com/store/apps/details?id=dev.msartore.atabridge";

/// Toolbar actions appear once they can act, instead of sitting greyed out.
function ToolButton({ enabled, ...rest }: ComponentProps<typeof IconBtn> & { enabled: boolean }) {
  return enabled ? <IconBtn {...rest} /> : null;
}

export function SystemTab() {
  const { t } = useI18n();
  const m = useMain();
  const ui = useUi();
  const [ip, setIp] = useState("");
  const [port, setPort] = useState("5555");
  const [connectServices, setConnectServices] = useState<MdnsService[]>([]);
  const [pairServices, setPairServices] = useState<MdnsService[]>([]);
  const [scanning, setScanning] = useState(false);
  const [busy, setBusy] = useState<string | null>(null);
  const [filterOpen, setFilterOpen] = useState(false);

  // Android 11+ picks a random port for wireless debugging, so the advertised
  // `_adb-tls-connect` endpoint is the only reliable address; 5555 exists only
  // after an explicit `adb tcpip 5555`.
  const discover = useCallback(async () => {
    setScanning(true);
    try {
      const services = await api.mdnsServices();
      setConnectServices(services.filter((service) => service.service.includes("_adb-tls-connect")));
      setPairServices(services.filter((service) => service.service.includes("_adb-tls-pairing")));
    } catch {
      setConnectServices([]);
      setPairServices([]);
    } finally {
      setScanning(false);
    }
  }, []);

  useEffect(() => {
    void discover();
  }, [discover]);

  // Follow the selected device: its endpoint is the truth, not the last typed IP.
  useEffect(() => {
    if (m.device?.connection === "WIRELESS") {
      const [host, endpointPort] = m.device.id.split(":");
      if (host) setIp(host);
      if (endpointPort) setPort(endpointPort);
      return;
    }
    if (m.info?.ip) setIp(m.info.ip);
  }, [m.device?.id, m.device?.connection, m.info?.ip]);

  const info = m.info;
  const connected = Boolean(m.device);
  const selected = m.selectedApps.size;
  const hasSelection = selected > 0;

  const wifiReboot = async (target: "system" | "recovery" | "fastboot") => {
    if (!m.device) return;
    if (m.device.connection === "WIRELESS" && !(await ui.confirm(t("confirm.wirelessReboot"), t("confirm.wirelessRebootTitle")))) {
      return;
    }
    m.log("INFO", t("log.rebooting"));
    try {
      const args = target === "system" ? ["reboot"] : target === "recovery" ? ["reboot", "recovery"] : ["reboot-bootloader"];
      await api.runTool("adb", m.device.id, args);
      m.log("OK", t("log.rebooted"));
    } catch (error) {
      m.log("ERROR", errorText(error));
    }
  };

  const wirelessDevice = m.device?.connection === "WIRELESS" ? m.device : null;
  // The endpoint we are already on is shown in the status line, not listed again.
  const discovered = connectServices.filter((service) => service.address !== wirelessDevice?.id);

  const connectTo = async (host: string, rawPort: string) => {
    const address = host.trim();
    const target = (rawPort ?? "").trim() || "5555";
    if (!address) {
      await ui.alert(t("error.enterIp"), "warning");
      return;
    }
    if (!/^\d{1,3}(\.\d{1,3}){3}$/.test(address)) {
      await ui.alert(t("error.invalidIp"), "warning");
      return;
    }
    setBusy(`connect:${address}:${target}`);
    m.log("INFO", t("log.connecting"));
    try {
      const result = await api.adbConnect(address, target, null);
      if (result.ok) {
        m.log("OK", t("log.connected", { ip: address }));
        await m.addIp(address);
        await m.refreshDevices();
        return;
      }
      m.log("ERROR", `${t("log.connectFailed", { ip: address })}\n${result.message}`);
      const hint =
        result.code === "refused"
          ? t("hint.connectionRefused")
          : result.code === "timeout" || result.code === "unreachable"
            ? t("hint.connectionTimeout")
            : result.code === "auth"
              ? t("hint.pairAuth")
              : "";
      if (hint) {
        m.log("WARNING", hint);
        await ui.alert(hint, "warning");
      }
    } catch (error) {
      m.log("ERROR", errorText(error));
    } finally {
      setBusy(null);
    }
  };

  const disconnectFrom = async (endpoint: string) => {
    const [host, endpointPort] = endpoint.split(":");
    setBusy(`disconnect:${endpoint}`);
    try {
      const result = await api.adbDisconnect(host, endpointPort || "5555");
      if (result.ok) {
        m.log("OK", t("log.disconnected", { ip: host }));
        await m.refreshDevices();
      } else {
        m.log("ERROR", t("log.disconnectFailed", { ip: host }));
      }
    } catch (error) {
      m.log("ERROR", errorText(error));
    } finally {
      setBusy(null);
    }
  };

  const pair = () => {
    // Prefer the address the phone is advertising right now: its pairing port
    // differs from the connect port and from 5555.
    const service = pairServices[0];
    m.openDialog("pairing", {
      ip: ip.trim() || service?.ip || "",
      port: service?.port || port.trim() || "",
    });
  };

  const filteredApps = useMemo(() => {
    const needle = m.search.trim().toLowerCase();
    if (!needle) return m.apps;
    return m.apps.filter(
      (app) =>
        app.package.toLowerCase().includes(needle) ||
        (app.name ?? "").toLowerCase().includes(needle),
    );
  }, [m.apps, m.search]);

  return (
    <div className="system-grid">
      <div className="system-left">
        <Panel title={t("section.deviceInfo")}>
          <div className="kv-grid">
            <Field label={t("label.buildUser")} value={info?.buildUser || "—"} />
            <Field label={t("label.cpuAbilities")} value={info?.cpuAbilist || "—"} />
            <Field label={t("label.manufacturer")} value={info?.manufacturer || "—"} />
            <Field label={t("label.model")} value={info?.model || "—"} />
            <Field label={t("label.board")} value={info?.board || "—"} />
            <Field label={t("label.device")} value={info?.device || "—"} />
            <Field label={t("label.androidVersion")} value={info?.androidVersion || "—"} />
            <Field label={t("label.ip")} value={info?.ip || "—"} mono />
            <Field
              label={t("label.connectedVia")}
              value={
                m.device
                  ? m.device.connection === "WIRELESS"
                    ? t("value.wireless")
                    : t("value.cable")
                  : t("value.noDevice")
              }
            />
            <Field label={t("label.user")} value={m.user || "—"} mono />
          </div>

          <div className="row">
            <Btn
              small
              icon="power"
              variant="danger"
              disabled={!connected}
              onClick={async () => {
                if (!m.device) return;
                try {
                  await api.runTool("adb", m.device.id, ["shell", "settings", "put", "global", "adb_enabled", "0"]);
                  m.log("INFO", t("log.commandInjected"));
                  await m.syncDevice();
                } catch (error) {
                  m.log("ERROR", errorText(error));
                  await ui.alert(errorText(error));
                }
              }}
            >
              {t("button.turnOffAdb")}
            </Btn>
            {info?.error ? <span className="badge badge-danger">{t(errorKey(info.error) || info.error)}</span> : null}
          </div>
        </Panel>
      </div>

      <div className="system-mid">
        <Panel title={t("section.rebootMenu")}>
          <Btn className="btn-block" icon="refresh" disabled={!connected} onClick={() => void wifiReboot("system")}>
            {t("button.rebootSystem")}
          </Btn>
          <Btn className="btn-block" icon="boot" disabled={!connected} onClick={() => void wifiReboot("recovery")}>
            {t("button.rebootRecovery")}
          </Btn>
          <Btn className="btn-block" icon="chip" disabled={!connected} onClick={() => void wifiReboot("fastboot")}>
            {t("button.rebootFastboot")}
          </Btn>
        </Panel>

        <Panel
          title={t("section.adbOverNetwork")}
          actions={
            <div className="row is-tight">
              <InfoTip label={t("section.adbOverNetwork")}>{t("wireless.hint")}</InfoTip>
              <IconBtn
                icon="refresh"
                label={t("button.searchDevices")}
                onClick={() => void discover()}
              />
            </div>
          }
        >
          <div className="wireless-status">
            <span className={`dot ${wirelessDevice ? "is-on" : ""}`} />
            <span className="mono grow">
              {wirelessDevice
                ? `${t("wireless.connectedTo")} ${wirelessDevice.id}`
                : t("wireless.notConnected")}
            </span>
            {wirelessDevice ? (
              <Btn
                small
                variant="ghost"
                icon="x"
                className="is-collapsible"
                title={t("button.disconnect")}
                disabled={busy !== null}
                onClick={() => void disconnectFrom(wirelessDevice.id)}
              >
                {t("button.disconnect")}
              </Btn>
            ) : null}
          </div>

          {!scanning && connectServices.length > 0 && discovered.length === 0 ? null : (
            <>
              <p className="wireless-label">{t("label.discovered")}</p>
              <div className="wireless-list">
                {scanning ? (
                  <p className="hint">{t("wireless.scanning")}</p>
                ) : connectServices.length === 0 ? (
                  <p className="hint">{t("wireless.noneFound")}</p>
                ) : (
                  discovered.map((service) => (
                    <div className="wireless-row" key={service.address}>
                      <span className="mono grow">{service.address}</span>
                      <Btn
                        small
                        icon="wifi"
                        disabled={busy !== null}
                        onClick={() => void connectTo(service.ip, service.port)}
                      >
                        {t("button.connect")}
                      </Btn>
                    </div>
                  ))
                )}
              </div>
            </>
          )}

          <p className="wireless-label">{t("wireless.manual")}</p>
          <div className="wireless-manual">
            <input
              className="input is-mono grow"
              value={ip}
              list="ip-history"
              placeholder="192.168.1.20"
              onChange={(event) => setIp(event.target.value)}
              onKeyDown={(event) => event.key === "Enter" && void connectTo(ip, port)}
            />
            <datalist id="ip-history">
              {m.ips.map((known) => (
                <option key={known} value={known} />
              ))}
            </datalist>
            <input
              className="input is-mono w-port"
              value={port}
              placeholder="5555"
              onChange={(event) => setPort(event.target.value)}
              onKeyDown={(event) => event.key === "Enter" && void connectTo(ip, port)}
            />
          </div>
          <Btn
            small
            variant="primary"
            icon="wifi"
            className="is-block"
            disabled={busy !== null || !ip.trim()}
            onClick={() => void connectTo(ip, port)}
          >
            {t("button.connect")}
          </Btn>

          <p className="wireless-label">{t("wireless.pairing")}</p>
          <div className="row is-wrap">
            <Btn
              small
              icon="key"
              disabled={busy !== null || (!ip.trim() && pairServices.length === 0)}
              onClick={pair}
            >
              {t("button.pair")}
            </Btn>
            <Btn small icon="qr" title={t("dialog.qrTitle")} onClick={() => m.openDialog("qr")}>
              QR
            </Btn>
          </div>

        </Panel>
      </div>

      <div className="system-right">
        <Panel title={t("section.apkMenu")} flush>
          <div className="toolbar">
            <ToolButton enabled={connected} icon="refresh" label={t("apk.reloadApps")} onClick={() => void m.loadApps(m.appsMode)} />

            {connected ? (
            <div className="dropdown">
              <IconBtn icon="filter" label={t("apk.filter")} active={filterOpen} onClick={() => setFilterOpen((v) => !v)} />
              {filterOpen ? (
                <div className="dropdown-menu" onClick={() => setFilterOpen(false)}>
                  {(
                    [
                      ["all", "apk.filterAll"],
                      ["system", "apk.filterSystem"],
                      ["nonSystem", "apk.filterNonSystem"],
                      ["uninstalled", "apk.filterUninstalled"],
                      ["disabled", "apk.filterDisabled"],
                    ] as Array<[AppMode, string]>
                  ).map(([mode, key]) => (
                    <MenuItem key={mode} onClick={() => void m.loadApps(mode)}>
                      {t(key)}
                    </MenuItem>
                  ))}
                </div>
              ) : null}
            </div>
            ) : null}

            <input
              className="input input-sm grow"
              placeholder={t("label.search")}
              value={m.search}
              onChange={(event) => m.setSearch(event.target.value)}
            />

            <ToolButton
              enabled={connected && hasSelection}
              icon="trash"
              label={t("button.uninstall")}
              onClick={async () => {
                if (!m.device) return;
                try {
                  const packages = [...m.selectedApps];
                  m.log("INFO", t("log.uninstalling", { count: packages.length }));
                  const results = await api.appsApply(m.device.id, m.user, "uninstall", packages);
                  const failed = results.filter((result) => !result.ok);
                  if (failed.length) m.log("ERROR", `${t("log.operationFailed")}: ${failed.map((f) => f.target).join(", ")}`);
                  else m.log("OK", t("log.uninstalled"));
                  await m.loadApps(m.appsMode);
                } catch (error) {
                  m.log("ERROR", errorText(error));
                  await ui.alert(errorText(error));
                }
              }}
            />

            <ToolButton
              enabled={connected && hasSelection}
              icon="box"
              label={t("button.packageManager")}
              onClick={() => m.openDialog("packageMenu", { packages: [...m.selectedApps] })}
            />

            {hasSelection ? (
            <Dropdown label="" icon="key">
              <MenuItem
                icon="list"
                onClick={async () => {
                  if (!m.device) return;
                  try {
                    const outputs: string[] = [];
                    for (const pkg of m.selectedApps) {
                      const out = await api.runTool("adb", m.device.id, ["shell", "appops", "get", pkg]);
                      outputs.push(`${pkg}\n${out.stdout}${out.stderr}`);
                    }
                    m.openDialog("scrollable", {
                      title: t("apk.checkGrantedPermissions"),
                      text: outputs.join("\n\n") || t("error.noAppSelected"),
                    });
                  } catch (error) {
                    m.log("ERROR", errorText(error));
                    await ui.alert(errorText(error));
                  }
                }}
              >
                {t("apk.checkGrantedPermissions")}
              </MenuItem>
              <MenuItem icon="key" onClick={() => m.openDialog("shellCommands", { pkg: [...m.selectedApps][0] })}>
                {t("apk.grantRevoke")}
              </MenuItem>
            </Dropdown>
            ) : null}

            <ToolButton
              enabled={connected && selected === 1}
              icon="star"
              label={t("button.setDefault")}
              onClick={() => m.openDialog("defaultApp", { pkg: [...m.selectedApps][0] })}
            />

            <ToolButton
              enabled={connected && hasSelection}
              icon="check"
              label={t("button.restoreApp")}
              onClick={async () => {
                if (!m.device) return;
                try {
                  m.log("INFO", t("log.restoring", { count: m.selectedApps.size }));
                  await api.appsApply(m.device.id, m.user, "restore", [...m.selectedApps]);
                  m.log("OK", t("log.restored"));
                  await m.loadApps(m.appsMode);
                } catch (error) {
                  m.log("ERROR", errorText(error));
                  await ui.alert(errorText(error));
                }
              }}
            />

            {hasSelection ? (
            <Dropdown label="" icon="search">
              {[
                ["DuckDuckGo", "https://duckduckgo.com/?q="],
                ["Google", "https://www.google.com/search?q="],
                ["Play Store", "https://play.google.com/store/apps/details?id="],
                ["APK Mirror", "https://www.apkmirror.com/?post_type=app_release&searchtype=apk&s="],
                ["F-Droid", "https://f-droid.org/en/packages/"],
              ].map(([label, base]) => (
                <MenuItem
                  key={label}
                  icon="search"
                  onClick={() => {
                    for (const pkg of m.selectedApps) {
                      void api.openUrl(base + encodeURIComponent(pkg));
                    }
                  }}
                >
                  {label}
                </MenuItem>
              ))}
            </Dropdown>
            ) : null}

            <ToolButton
              enabled={connected && hasSelection}
              icon="extract"
              label={t("button.extract")}
              onClick={() => void m.extractApks([...m.selectedApps])}
            />

            {connected ? (
            <Dropdown label={t("button.install")} icon="install">
              <MenuItem
                icon="install"
                onClick={async () => {
                  try {
                    const files = await api.pickFiles("apk", true);
                    if (files.length) await m.installApks(files, false);
                  } catch (error) {
                    m.log("ERROR", errorText(error));
                    await ui.alert(errorText(error));
                  }
                }}
              >
                {t("button.install")}
              </MenuItem>
              <MenuItem
                icon="install"
                onClick={async () => {
                  try {
                    const files = await api.pickFiles("apk", true);
                    if (files.length) await m.installApks(files, true);
                  } catch (error) {
                    m.log("ERROR", errorText(error));
                    await ui.alert(errorText(error));
                  }
                }}
              >
                {t("button.downgrade")}
              </MenuItem>
            </Dropdown>
            ) : null}
          </div>

          <div className="table apps-table">
            <div className={`table-head apps-grid${m.bridge ? "" : " is-single"}`}>
              {m.bridge ? <span>{t("label.name")}</span> : null}
              <span>{t("label.package")}</span>
            </div>
            {!connected ? (
              <div className="table-scroll">
                <Empty>{t("value.noDevice")}</Empty>
              </div>
            ) : (
              <VirtualList
                items={filteredApps}
                rowHeight={30}
                className="table-scroll"
                empty={<Empty>{t("apk.noApps")}</Empty>}
                renderRow={(app) => (
                  <div
                    key={app.package}
                    className={`table-row apps-grid${m.bridge ? "" : " is-single"} ${
                      m.selectedApps.has(app.package) ? "is-selected" : ""
                    }`}
                    onClick={() => m.toggleApp(app.package)}
                  >
                    {m.bridge ? <span className="app-name">{app.name ?? "—"}</span> : null}
                    <span className="pkg">{app.package}</span>
                  </div>
                )}
              />
            )}
            <div className="panel-foot">
              <span>
                {t("label.total")}: {m.apps.length}
              </span>
              <span>
                {t("label.selectedApps")}: {m.selectedApps.size}
              </span>
              {m.bridge ? (
                <span className="badge badge-info">ATA-Bridge</span>
              ) : (
                <button
                  type="button"
                  className="badge badge-warn is-action"
                  onClick={() => void api.openUrl(ATA_BRIDGE_URL)}
                >
                  {t("apk.bridgeMissing")}
                </button>
              )}
              <span className="right">
                <Btn
                  small
                  variant="ghost"
                  icon="shield"
                  className="is-collapsible"
                  title={t("apk.bloatwareRemover")}
                  onClick={() => m.openDialog("bloatware")}
                  disabled={!connected}
                >
                  {t("apk.bloatwareRemover")}
                </Btn>
              </span>
            </div>
          </div>

          <div className="apk-drop">
            <DropZone
              label={t("apk.installerHint")}
              hint=".apk"
              onFiles={(paths) => void m.installApks(paths, false)}
            />
          </div>
        </Panel>
      </div>
    </div>
  );
}
