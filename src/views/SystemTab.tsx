import { useCallback, useEffect, useMemo, useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, DropZone, Dropdown, Empty, Field, IconBtn, MenuItem, Panel, useUi } from "../components/ui";
import { errorKey, errorText } from "../lib/util";
import { VirtualList } from "../components/VirtualList";
import type { AppMode, MdnsService } from "../lib/types";

export function SystemTab() {
  const { t } = useI18n();
  const m = useMain();
  const ui = useUi();
  const [ip, setIp] = useState("");
  const [port, setPort] = useState("5555");
  const [wireless, setWireless] = useState(false);
  const [found, setFound] = useState<MdnsService[]>([]);

  // Android 11+ advertises the real IP:port over mDNS; the fixed 5555 port is
  // only valid after `adb tcpip 5555`, which is why manual connects often fail.
  const discover = useCallback(async () => {
    try {
      const services = await api.mdnsServices();
      setFound(services.filter((service) => service.service.includes("_adb-tls-connect")));
    } catch {
      setFound([]);
    }
  }, []);

  useEffect(() => {
    if (wireless) void discover();
    else setFound([]);
  }, [wireless, discover]);
  const [networkLocked, setNetworkLocked] = useState(true);
  const [filterOpen, setFilterOpen] = useState(false);

  useEffect(() => {
    if (m.info?.ip) setIp(m.info.ip);
    if (m.device?.connection === "WIRELESS") setWireless(true);
  }, [m.info?.ip, m.device?.connection]);

  const info = m.info;
  const connected = Boolean(m.device);

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

  const connect = async () => {
    if (!ip.trim()) {
      await ui.alert(t("error.enterIp"), "warning");
      return;
    }
    m.log("INFO", t("log.connecting"));
    try {
      const result = await api.adbConnect(ip.trim(), port.trim() || "5555", wireless ? m.device?.id ?? null : null);
      if (result.ok) {
        m.log("OK", t("log.connected", { ip: ip.trim() }));
        await m.addIp(ip.trim());
        await m.refreshDevices();
      } else {
        m.log("ERROR", `${t("log.connectFailed", { ip: ip.trim() })}\n${result.message}`);
        const hint =
          result.code === "refused"
            ? t("hint.connectionRefused")
            : result.code === "timeout" || result.code === "unreachable"
              ? t("hint.connectionTimeout")
              : "";
        if (hint) {
          m.log("WARNING", hint);
          await ui.alert(hint, "warning");
        }
      }
    } catch (error) {
      m.log("ERROR", errorText(error));
    }
  };

  const disconnect = async () => {
    if (!ip.trim()) {
      await ui.alert(t("error.enterIp"), "warning");
      return;
    }
    try {
      const result = await api.adbDisconnect(ip.trim(), port.trim() || "5555");
      if (result.ok) {
        m.log("OK", t("log.disconnected", { ip: ip.trim() }));
        await m.refreshDevices();
      } else {
        m.log("ERROR", t("log.disconnectFailed", { ip: ip.trim() }));
      }
    } catch (error) {
      m.log("ERROR", errorText(error));
    }
  };

  const pair = async () => {
    if (!ip.trim()) {
      await ui.alert(t("error.enterIp"), "warning");
      return;
    }
    m.openDialog("pairing", { ip: ip.trim(), port: port.trim() || "5555" });
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

        <Panel title={t("section.adbOverNetwork")}>
          <div className="row">
            <label className="control grow">
              {t("label.ip")}
              <input
                className="input is-mono"
                value={ip}
                list="ip-history"
                disabled={!networkLocked}
                onChange={(event) => setIp(event.target.value)}
                onKeyDown={(event) => event.key === "Enter" && void connect()}
              />
              <datalist id="ip-history">
                {m.ips.map((known) => (
                  <option key={known} value={known} />
                ))}
              </datalist>
            </label>
            <label className="control w-port">
              {t("label.port")}
              <input
                className="input is-mono"
                value={port}
                disabled={!networkLocked}
                onChange={(event) => setPort(event.target.value)}
              />
            </label>
          </div>

          <div className="row">
            <div className="radio-row grow">
              <label className="check">
                <input type="radio" checked={!wireless} onChange={() => setWireless(false)} />
                {t("label.wired")}
              </label>
              <label className="check">
                <input type="radio" checked={wireless} onChange={() => setWireless(true)} />
                {t("label.wireless")}
              </label>
            </div>
            <IconBtn
              icon={networkLocked ? "lock" : "unlock"}
              label={t("button.connect")}
              onClick={() => setNetworkLocked((value) => !value)}
            />
          </div>

          <div className="row">
            <Btn small icon="wifi" onClick={() => void connect()}>
              {t("button.connect")}
            </Btn>
            <Btn small icon="x" onClick={() => void disconnect()}>
              {t("button.disconnect")}
            </Btn>
            {wireless ? (
              <>
                <Btn small icon="key" onClick={() => void pair()}>
                  {t("button.pair")}
                </Btn>
                <Btn small icon="qr" onClick={() => m.openDialog("qr")}>
                  QR
                </Btn>
                <IconBtn icon="refresh" label={t("button.searchDevices")} onClick={() => void discover()} />
              </>
            ) : null}
          </div>

          {wireless && found.length ? (
            <div className="row">
              <span className="hint">{t("label.discovered")}</span>
              {found.map((service) => (
                <Btn
                  key={service.address}
                  small
                  icon="wifi"
                  onClick={() => {
                    setIp(service.ip);
                    setPort(service.port);
                  }}
                >
                  {service.address}
                </Btn>
              ))}
            </div>
          ) : null}
        </Panel>
      </div>

      <div className="system-right">
        <Panel title={t("section.apkMenu")} flush>
          <div className="toolbar">
            <IconBtn icon="refresh" label={t("apk.reloadApps")} disabled={!connected} onClick={() => void m.loadApps(m.appsMode)} />

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

            <input
              className="input input-sm grow"
              placeholder={t("label.search")}
              value={m.search}
              onChange={(event) => m.setSearch(event.target.value)}
            />

            <IconBtn
              icon="trash"
              label={t("button.uninstall")}
              disabled={!connected || m.selectedApps.size === 0}
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

            <IconBtn
              icon="box"
              label={t("button.packageManager")}
              disabled={!connected || m.selectedApps.size === 0}
              onClick={() => m.openDialog("packageMenu", { packages: [...m.selectedApps] })}
            />

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

            <IconBtn
              icon="star"
              label={t("button.setDefault")}
              disabled={!connected || m.selectedApps.size !== 1}
              onClick={() => m.openDialog("defaultApp", { pkg: [...m.selectedApps][0] })}
            />

            <IconBtn
              icon="check"
              label={t("button.restoreApp")}
              disabled={!connected || m.selectedApps.size === 0}
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

            <IconBtn
              icon="extract"
              label={t("button.extract")}
              disabled={!connected || m.selectedApps.size === 0}
              onClick={() => void m.extractApks([...m.selectedApps])}
            />

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
          </div>

          {connected && !m.bridge ? (
            <p className="hint apps-bridge-note">{t("apk.bridgeMissing")}</p>
          ) : null}

          <div className="table apps-table">
            <div className="table-head apps-grid">
              <span>{t("label.name")}</span>
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
                    className={`table-row apps-grid ${m.selectedApps.has(app.package) ? "is-selected" : ""}`}
                    onClick={() => m.toggleApp(app.package)}
                  >
                    <span className="app-name">
                      {app.name ?? "—"}
                    </span>
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
              {m.bridge ? <span className="badge badge-info">ATA-Bridge</span> : null}
              <span className="right">
                <Btn small variant="ghost" icon="shield" onClick={() => m.openDialog("bloatware")} disabled={!connected}>
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
