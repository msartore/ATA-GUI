import { useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, Empty, Panel, useUi } from "../components/ui";
import { IMAGE_PARTITIONS, basename, errorKey, errorText } from "../lib/util";

export function RecoveryTab() {
  const { t } = useI18n();
  const m = useMain();
  const ui = useUi();
  const [zipPath, setZipPath] = useState("");
  const [partition, setPartition] = useState(IMAGE_PARTITIONS[0]);
  const connected = Boolean(m.device);

  const flashZip = async () => {
    const target = m.device;
    if (!target) return;
    if (!zipPath.toLowerCase().endsWith(".zip")) {
      await ui.alert(t("error.noZip"), "warning");
      return;
    }
    await m.runBusy(t("log.flashing", { name: basename(zipPath) }), async () => {
      const step = await api.sideloadZip(target.id, zipPath);
      const failed = /error|failed/i.test(step.output) || !step.output.trim();
      if (failed) m.log("ERROR", t("log.flashFailed", { name: basename(zipPath) }));
      else m.log("OK", t("log.flashed", { name: basename(zipPath) }));
      if (step.output.trim()) m.log("INFO", step.output);
    });
  };

  const extract = async () => {
    const target = m.device;
    if (!target) return;
    await m.runBusy(t("log.extractingImage", { partition }), async () => {
      m.log("INFO", t("log.extractingImage", { partition }));
      const result = await api.imageExtract(target.id, partition);
      for (const line of result.log) if (line.trim()) m.log("INFO", line);
      if (result.ok) {
        m.log("OK", t("log.imageExtracted", { path: result.path }));
      } else {
        m.log("ERROR", t(errorKey(result.error ?? "") || "error.generic"));
      }
    });
  };

  const reboot = async (target: "system" | "recovery" | "bootloader") => {
    if (!m.device) return;
    if (m.device.connection === "WIRELESS" && !(await ui.confirm(t("confirm.wirelessReboot"), t("confirm.wirelessRebootTitle")))) {
      return;
    }
    try {
      const args = target === "system" ? ["reboot"] : target === "recovery" ? ["reboot", "recovery"] : ["reboot-bootloader"];
      await api.runTool("adb", m.device.id, args);
      m.log("OK", t("log.rebooting"));
    } catch (error) {
      m.log("ERROR", errorText(error));
    }
  };

  return (
    <div className="recovery-grid">
      <Panel title={t("section.flashMenu")}>
        <div className="row">
          <input
            className="input is-mono grow"
            value={zipPath}
            placeholder="*.zip"
            onChange={(event) => setZipPath(event.target.value)}
          />
          <Btn
            small
            icon="folder"
            onClick={async () => {
              const files = await api.pickFiles("zip", false);
              if (files[0]) setZipPath(files[0]);
            }}
          >
            {t("button.browse")}
          </Btn>
        </div>
        <Btn variant="primary" icon="install" disabled={!connected} onClick={() => void flashZip()}>
          {t("button.flash")}
        </Btn>
        <p className="hint">{t("section.flashMenu")} — sideload</p>
      </Panel>

      <div className="stack">
        <Panel title={t("section.imageExtraction")}>
          <label className="control">
            {t("label.partition")}
            <select className="select" value={partition} onChange={(event) => setPartition(event.target.value)}>
              {IMAGE_PARTITIONS.map((item) => (
                <option key={item} value={item}>
                  {item}
                </option>
              ))}
            </select>
          </label>
          <Btn icon="extract" disabled={!connected} onClick={() => void extract()}>
            {t("button.extract")}
          </Btn>
          <p className="hint">dd → /sdcard → IMG/</p>
        </Panel>

        <Panel title={t("section.rebootMenu")}>
          <Btn className="btn-block" icon="boot" disabled={!connected} onClick={() => void reboot("recovery")}>
            {t("button.rebootRecovery")}
          </Btn>
          <Btn className="btn-block" icon="refresh" disabled={!connected} onClick={() => void reboot("system")}>
            {t("button.rebootSystem")}
          </Btn>
          <Btn className="btn-block" icon="chip" disabled={!connected} onClick={() => void reboot("bootloader")}>
            {t("button.rebootFastboot")}
          </Btn>
        </Panel>
      </div>

      {!connected ? (
        <Panel title={t("label.status")}>
          <Empty>{t("value.noDevice")}</Empty>
        </Panel>
      ) : null}
    </div>
  );
}
