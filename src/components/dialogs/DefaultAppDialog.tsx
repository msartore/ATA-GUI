import { useEffect, useMemo, useRef, useState } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "../../lib/api";
import { useI18n } from "../../i18n";
import { Btn, Empty, Field, Modal, Spinner, useUi } from "../ui";
import { LICENSE_TEXT } from "../../lib/license";
import { OEM_DRIVERS } from "../../lib/oem-drivers";
import { ANDROID_PERMISSIONS } from "../../lib/permissions";
import type { BloatEntry, Config, EncoderData, ToolStatus } from "../../lib/types";
import { ATA_BRIDGE, ATA_REPO, ATA_SITE, UAD_SITE, Link, ScrollableDialog, MissingDialog, LoadingDialog } from "./shared";
import { useAction } from "../../lib/use-action";
import { errorText } from "../../lib/util";

const ROLES = [
  "SYSTEM_GALLERY",
  "CALL_SCREENING",
  "SMS",
  "DIALER",
  "HOME",
  "ASSISTANT",
  "EMERGENCY",
  "CALL_REDIRECTION",
  "BROWSER",
];

export function DefaultAppDialog({
  serial,
  pkg,
  onClose,
}: {
  serial: string;
  pkg: string;
  onClose: () => void;
}) {
  const { t } = useI18n();
  const ui = useUi();
  const [role, setRole] = useState(ROLES[0]);
  const { busy, run } = useAction();

  const apply = () =>
    void run(async () => {
      await api.runTool("adb", serial, ["shell", "cmd", "role", "add-role-holder", `android.app.role.${role}`, pkg]);
      ui.notify(t("log.commandInjected"), "ok");
      onClose();
    });

  return (
    <Modal
      title={t("dialog.defaultAppTitle")}
      onClose={onClose}
      footer={
        <>
          <Btn onClick={onClose}>{t("button.close")}</Btn>
          <Btn variant="primary" onClick={apply} disabled={busy}>
            {t("button.setDefault")}
          </Btn>
        </>
      }
    >
      <Field label={t("label.apkName")} value={pkg} mono />
      <label className="control">
        {t("label.type")}
        <select className="select" value={role} onChange={(event) => setRole(event.target.value)}>
          {ROLES.map((item) => (
            <option key={item} value={item}>
              {item}
            </option>
          ))}
        </select>
      </label>
    </Modal>
  );
}
