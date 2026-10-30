import { useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, Empty, Panel, useUi } from "../components/ui";
import { PARTITION_SLOTS, PARTITION_TYPES, basename, errorText } from "../lib/util";

export function FastbootTab() {
  const { t } = useI18n();
  const m = useMain();
  const ui = useUi();
  const [imagePath, setImagePath] = useState("");
  const [imageType, setImageType] = useState(PARTITION_TYPES[0]);
  const [slot, setSlot] = useState(PARTITION_SLOTS[2]);

  const connected = Boolean(m.device);

  const flash = async (kind: "flash" | "boot") => {
    const target = m.device;
    if (!target) return;
    if (!imagePath) {
      await ui.alert(t("error.noImg"), "warning");
      return;
    }

    await m.runBusy(t("log.flashing", { name: basename(imagePath) }), async () => {
      m.log("INFO", t("log.flashingImage", { kind, image: imageType, partition: slot }));
      const steps = await api.flashImage(target.id, kind, imagePath, imageType, slot);
      for (const step of steps) {
        m.log(step.ok ? "OK" : "ERROR", `${step.command}\n${step.output}`.trim());
      }
    });
  };

  const reboot = async (target: "system" | "recovery") => {
    if (!m.device) return;
    try {
      if (target === "system") {
        await api.runTool("fastboot", m.device.id, ["reboot"]);
      } else {
        await api.runTool("fastboot", m.device.id, ["reboot", "recovery"]);
      }
      m.log("OK", t("log.rebooted"));
    } catch (error) {
      m.log("ERROR", errorText(error));
    }
  };

  const erase = async () => {
    if (!m.device) return;
    if (!(await ui.confirm(t("confirm.eraseBody"), t("confirm.eraseTitle")))) return;
    m.log("INFO", t("log.erasing"));
    const steps = await api.fastbootErase(m.device.id, ["userdata", "cache"]);
    for (const step of steps) m.log(step.ok ? "OK" : "ERROR", `${step.command}\n${step.output}`.trim());
  };

  const wipe = async () => {
    if (!m.device) return;
    if (!(await ui.confirm(t("confirm.wipeBody"), t("confirm.wipeTitle")))) return;
    m.log("INFO", t("log.wiping"));
    const step = await api.fastbootWipe(m.device.id);
    m.log(step.ok ? "OK" : "ERROR", step.output || step.command);
  };

  return (
    <div className="fastboot-grid">
      <div className="stack">
        <Panel title={t("section.flashBootImg")}>
          <div className="row">
            <input
              className="input is-mono grow"
              value={imagePath}
              placeholder="*.img"
              onChange={(event) => {
                setImagePath(event.target.value);
                const match = PARTITION_TYPES.find((type) =>
                  event.target.value.toLowerCase().includes(type.toLowerCase()),
                );
                if (match) setImageType(match);
              }}
            />
            <Btn
              small
              icon="folder"
              onClick={async () => {
                const files = await api.pickFiles("img", false);
                if (files[0]) {
                  setImagePath(files[0]);
                  const match = PARTITION_TYPES.find((type) => files[0].toLowerCase().includes(type.toLowerCase()));
                  if (match) setImageType(match);
                }
              }}
            >
              {t("button.searchFile")}
            </Btn>
          </div>

          <div className="row">
            <label className="control grow">
              {t("label.type")}
              <select className="select" value={imageType} onChange={(event) => setImageType(event.target.value)}>
                {PARTITION_TYPES.map((type) => (
                  <option key={type} value={type}>
                    {type}
                  </option>
                ))}
              </select>
            </label>
            <label className="control grow">
              {t("label.partition")}
              <select className="select" value={slot} onChange={(event) => setSlot(event.target.value)}>
                {PARTITION_SLOTS.map((option) => (
                  <option key={option} value={option}>
                    {option}
                  </option>
                ))}
              </select>
            </label>
          </div>

          <div className="row">
            <Btn variant="primary" icon="install" disabled={!connected} onClick={() => void flash("flash")}>
              {t("button.flash")}
            </Btn>
            <Btn icon="boot" disabled={!connected} onClick={() => void flash("boot")}>
              {t("button.boot")}
            </Btn>
          </div>
        </Panel>

        <Panel title={t("section.rebootMenu")}>
          <div className="row">
            <Btn icon="refresh" disabled={!connected} onClick={() => void reboot("system")}>
              {t("button.rebootSystem")}
            </Btn>
            <Btn icon="boot" disabled={!connected} onClick={() => void reboot("recovery")}>
              {t("button.rebootRecovery")}
            </Btn>
            <Btn icon="chip" disabled={!connected} onClick={() => m.openDialog("bootloader")}>
              {t("button.bootloaderMenu")}
            </Btn>
          </div>
          <div className="row">
            <Btn variant="danger" icon="trash" disabled={!connected} onClick={() => void erase()}>
              {t("button.eraseUserdata")}
            </Btn>
            <Btn variant="danger" icon="wipe" disabled={!connected} onClick={() => void wipe()}>
              {t("button.wipeData")}
            </Btn>
          </div>
        </Panel>
      </div>

      <Panel
        title={t("section.deviceInfo")}
        actions={
          <Btn
            small
            icon="refresh"
            disabled={!connected}
            onClick={async () => {
              if (!m.device) return;
              const text = await api.bootloaderInfo(m.device.id);
              m.log("OK", t("log.fastbootInfoExtracted"));
              m.openDialog("scrollable", { title: t("section.deviceInfo"), text: text || "—" });
            }}
          >
            {t("button.refreshDevices")}
          </Btn>
        }
        flush
      >
        <pre className="pre-block is-flush">{m.fastbootInfo || t("value.noDevice")}</pre>
      </Panel>
    </div>
  );
}
