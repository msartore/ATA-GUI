import { useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { Btn, Modal, useUi } from "./ui";
import { errorText } from "../lib/util";
import type { LogType } from "../lib/types";

const OPERATIONS: Array<{ key: string; op: string }> = [
  { key: "Unlock (2014)", op: "oem unlock" },
  { key: "Lock (2014)", op: "oem lock" },
  { key: "Unlock (2015)", op: "flashing unlock" },
  { key: "Lock (2015)", op: "flashing lock" },
  { key: "Vivo unlock", op: "bbk unlock_vivo" },
  { key: "Vivo lock", op: "bbk lock_vivo" },
  { key: "Device info", op: "getvar all" },
  { key: "Device ID", op: "oem device-id" },
];

export function BootloaderDialog({
  serial,
  onClose,
  onLog,
}: {
  serial: string;
  onClose: () => void;
  onLog: (type: LogType, message: string) => void;
}) {
  const { t } = useI18n();
  const ui = useUi();
  const [output, setOutput] = useState("");
  const [busy, setBusy] = useState(false);

  const run = async (op: string) => {
    setBusy(true);
    try {
      const steps = await api.bootloaderOps(serial, op);
      const text = steps
        .map((step) => `$ fastboot ${step.command}\n${step.output}`.trim())
        .join("\n\n");
      setOutput((current) => `${current}${text}\n\n`);
      onLog(steps.every((step) => step.ok) ? "OK" : "ERROR", `fastboot ${op}`);
    } catch (error) {
      await ui.alert(errorText(error));
    } finally {
      setBusy(false);
    }
  };

  return (
    <Modal
      title={t("button.bootloaderMenu")}
      onClose={onClose}
      xl
      footer={
        <>
          <Btn onClick={() => setOutput("")}>{t("button.clear")}</Btn>
          <Btn variant="primary" onClick={onClose}>
            {t("button.close")}
          </Btn>
        </>
      }
    >
      <div className="stack">
        {OPERATIONS.map((operation) => (
          <Btn
            key={operation.op}
            className="btn-block"
            icon={operation.op.includes("unlock") ? "unlock" : operation.op.includes("lock") ? "lock" : "info"}
            disabled={busy}
            onClick={() => void run(operation.op)}
          >
            {operation.key}
          </Btn>
        ))}
      </div>
      <pre className="pre-block log">{output || "—"}</pre>
    </Modal>
  );
}
