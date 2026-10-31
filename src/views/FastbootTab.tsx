import { useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, Empty, Panel, useUi } from "../components/ui";
import { Icon } from "../components/Icon";
import { FlashRun, type FlashStepSpec } from "../components/FlashRun";
import { PARTITION_SLOTS, PARTITION_TYPES, basename, errorText } from "../lib/util";

interface Row {
  id: number;
  path: string;
  type: string;
  slot: string;
  size: number;
}

let rowId = 0;

/** `boot` + slot `all` means both slots; `auto` keeps the unsuffixed name. */
function partitionsFor(type: string, slot: string): string[] {
  if (slot === "auto") return [type];
  if (slot === "all") return [`${type}_a`, `${type}_b`];
  return [`${type}_${slot}`];
}

export function FastbootTab() {
  const { t } = useI18n();
  const m = useMain();
  const ui = useUi();
  const [rows, setRows] = useState<Row[]>([]);
  const [run, setRun] = useState<FlashStepSpec[] | null>(null);

  const connected = Boolean(m.device);

  const addImages = async () => {
    const files = await api.pickFiles("img", true);
    if (files.length === 0) return;
    const added: Row[] = [];
    for (const file of files) {
      // Match the file name only: a path like .../cache/scratch/x.img must not
      // be read as a "cache" partition.
      const name = basename(file).toLowerCase();
      const type = PARTITION_TYPES.find((candidate) => name.includes(candidate.toLowerCase())) ?? PARTITION_TYPES[0];
      added.push({
        id: ++rowId,
        path: file,
        type,
        slot: PARTITION_SLOTS[2],
        size: await api.fileSize(file).catch(() => 0),
      });
    }
    setRows((current) => [...current, ...added]);
  };

  const update = (id: number, patch: Partial<Row>) =>
    setRows((current) => current.map((row) => (row.id === id ? { ...row, ...patch } : row)));

  const startFlash = () => {
    const steps: FlashStepSpec[] = rows.flatMap((row) =>
      partitionsFor(row.type, row.slot).map((partition) => ({
        key: `${row.id}-${partition}`,
        label: partition,
        kind: "flash" as const,
        image: row.path,
        partition,
        size: row.size,
      })),
    );
    if (steps.length === 0) return;
    setRun(steps);
  };

  const bootImage = (row: Row) => {
    setRun([
      {
        key: `boot-${row.id}`,
        label: `${basename(row.path)} (${t("button.boot")})`,
        kind: "boot",
        image: row.path,
        size: row.size,
      },
    ]);
  };

  const reboot = async (target: "system" | "recovery") => {
    if (!m.device) return;
    try {
      await api.runTool("fastboot", m.device.id, target === "system" ? ["reboot"] : ["reboot", "recovery"]);
      m.log("OK", t("log.rebooted"));
    } catch (error) {
      m.log("ERROR", errorText(error));
    }
  };

  const erase = async () => {
    if (!m.device) return;
    if (!(await ui.confirm(t("confirm.eraseBody"), t("confirm.eraseTitle")))) return;
    m.log("INFO", t("log.erasing"));
    try {
      const steps = await api.fastbootErase(m.device.id, ["userdata", "cache"]);
      for (const step of steps) m.log(step.ok ? "OK" : "ERROR", `${step.command}\n${step.output}`.trim());
    } catch (error) {
      m.log("ERROR", errorText(error));
      await ui.alert(errorText(error));
    }
  };

  const wipe = async () => {
    if (!m.device) return;
    if (!(await ui.confirm(t("confirm.wipeBody"), t("confirm.wipeTitle")))) return;
    m.log("INFO", t("log.wiping"));
    try {
      const step = await api.fastbootWipe(m.device.id);
      m.log(step.ok ? "OK" : "ERROR", step.output || step.command);
    } catch (error) {
      m.log("ERROR", errorText(error));
      await ui.alert(errorText(error));
    }
  };

  const stepCount = rows.reduce((count, row) => count + partitionsFor(row.type, row.slot).length, 0);

  return (
    <div className="fastboot-grid">
      <div className="stack">
        <Panel
          title={run ? t("flash.running") : t("flash.imagesTitle")}
          actions={
            run ? null : (
              <div className="row is-tight">
                <Btn small icon="upload" onClick={() => void addImages()}>
                  {t("flash.addImages")}
                </Btn>
                <Btn small variant="primary" icon="install" disabled={!connected || stepCount === 0} onClick={startFlash}>
                  {t("flash.flashCount", { count: stepCount })}
                </Btn>
              </div>
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
                if (ok) m.log("OK", t("log.flashed", { name: steps.map((step) => step.label).join(", ") }));
                else m.log("ERROR", t("log.flashFailed", { name: steps.find((step) => step.kind)?.label ?? "" }));
              }}
            />
          ) : rows.length === 0 ? (
            <button type="button" className="flash-pick is-empty" onClick={() => void addImages()}>
              <span className="flash-pick-icon">
                <Icon name="upload" size={18} />
              </span>
              <span className="flash-pick-body">
                <span className="flash-pick-title">{t("flash.empty")}</span>
                <span className="flash-pick-hint">{t("flash.emptyHint")}</span>
              </span>
            </button>
          ) : (
            <ul className="flash-queue">
              {rows.map((row) => (
                <li key={row.id} className="flash-row">
                  <select
                    className="select select-sm"
                    value={row.type}
                    onChange={(event) => update(row.id, { type: event.target.value })}
                  >
                    {PARTITION_TYPES.map((type) => (
                      <option key={type} value={type}>
                        {type}
                      </option>
                    ))}
                  </select>
                  <select
                    className="select select-sm"
                    value={row.slot}
                    onChange={(event) => update(row.id, { slot: event.target.value })}
                  >
                    {PARTITION_SLOTS.map((slot) => (
                      <option key={slot} value={slot}>
                        {slot}
                      </option>
                    ))}
                  </select>
                  <span className="flash-file mono" title={row.path}>
                    {basename(row.path)}
                  </span>
                  <span className="flash-size mono">
                    {row.size > 0 ? `${(row.size / 1_048_576).toFixed(1)} MB` : ""}
                  </span>
                  <button
                    type="button"
                    className="icon-btn"
                    title={t("button.boot")}
                    disabled={!connected}
                    onClick={() => bootImage(row)}
                  >
                    <Icon name="play" size={13} />
                  </button>
                  <button
                    type="button"
                    className="icon-btn is-danger"
                    title={t("button.remove")}
                    onClick={() => setRows((current) => current.filter((item) => item.id !== row.id))}
                  >
                    <Icon name="close" size={13} />
                  </button>
                </li>
              ))}
            </ul>
          )}
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
