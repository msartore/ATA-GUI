import { useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, Empty, Panel, useUi } from "../components/ui";
import { Icon } from "../components/Icon";
import { FlashRun, type FlashStepSpec } from "../components/FlashRun";
import { IMAGE_PARTITIONS, basename, errorKey } from "../lib/util";

export function RecoveryTab() {
  const { t } = useI18n();
  const m = useMain();
  const ui = useUi();
  const [zipPath, setZipPath] = useState("");
  const [zipSize, setZipSize] = useState(0);
  const [partition, setPartition] = useState(IMAGE_PARTITIONS[0]);
  const [run, setRun] = useState<FlashStepSpec[] | null>(null);
  const connected = Boolean(m.device);

  const pickZip = async () => {
    const files = await api.pickFiles("zip", false);
    if (!files[0]) return;
    setZipPath(files[0]);
    setZipSize(await api.fileSize(files[0]).catch(() => 0));
  };

  const flashZip = async () => {
    const target = m.device;
    if (!target) return;
    if (!zipPath.toLowerCase().endsWith(".zip")) {
      await ui.alert(t("error.noZip"), "warning");
      return;
    }
    setRun([
      {
        key: "sideload",
        label: basename(zipPath),
        kind: "sideload",
        image: zipPath,
        size: zipSize,
      },
    ]);
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
        ui.notify(t("toast.imageExtracted", { name: basename(result.path) }), "ok", {
          label: t("toast.showInFolder"),
          run: () => void api.revealPath(result.path).catch(() => undefined),
        });
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
      m.log("ERROR", String(error));
    }
  };

  return (
    <div className="recovery-grid">
      <Panel
        title={t("flash.recoveryTitle")}
        className="is-wide"
        actions={
          run ? null : (
            <Btn small variant="primary" icon="install" disabled={!connected || !zipPath} onClick={() => void flashZip()}>
              {t("button.flash")}
            </Btn>
          )
        }
      >
        {run ? (
          <FlashRun
            target="main"
            serial={m.device?.id ?? ""}
            steps={run}
            onClose={() => setRun(null)}
            onLog={(ok, steps) => {
              const name = steps[0]?.label ?? "";
              if (ok) m.log("OK", t("log.flashed", { name }));
              else m.log("ERROR", t("log.flashFailed", { name }));
            }}
          />
        ) : (
          <div className="flash-setup">
            <button type="button" className="flash-pick" onClick={() => void pickZip()}>
              <span className="flash-pick-icon">
                <Icon name="box" size={18} />
              </span>
              <span className="flash-pick-body">
                <span className="flash-pick-title mono">{zipPath ? basename(zipPath) : "*.zip"}</span>
                <span className="flash-pick-hint">
                  {zipPath
                    ? `${(zipSize / 1_048_576).toFixed(1)} MB`
                    : t("flash.sideloadHint")}
                </span>
              </span>
              <span className="flash-pick-action">{t("button.browse")}</span>
            </button>
            {!connected ? <p className="hint">{t("value.noDevice")}</p> : null}
          </div>
        )}
      </Panel>

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

      {!connected ? (
        <Panel title={t("label.status")} className="is-full">
          <Empty>{t("value.noDevice")}</Empty>
        </Panel>
      ) : null}
    </div>
  );
}
