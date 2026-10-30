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

export function ShellCommandsDialog({
  serial,
  user,
  pkg,
  onClose,
}: {
  serial: string;
  user: string;
  pkg: string;
  onClose: () => void;
}) {
  const { t } = useI18n();
  const ui = useUi();
  const [method, setMethod] = useState("grant");
  const [permission, setPermission] = useState("WRITE_SECURE_SETTINGS");
  const { busy, run } = useAction();

  const submit = () => {
    if (!permission.trim()) {
      void ui.alert(t("error.noAppSelected"), "warning");
      return;
    }
    void run(async () => {
      const result = await api.permissionGrant(serial, user, pkg, permission.trim(), method);
      const output = (result.stdout + result.stderr).trim();
      if (output) await ui.alert(`${t("log.commandInjectionFailed")}\n\n${output}`, "warning");
      else ui.notify(t("log.commandInjected"), "ok");
    });
  };

  return (
    <Modal
      title={t("dialog.shellCommandsTitle")}
      onClose={onClose}
      wide
      footer={
        <>
          <Btn onClick={onClose}>{t("button.close")}</Btn>
          <Btn variant="primary" icon="play" onClick={submit} disabled={busy}>
            {t("button.run")}
          </Btn>
        </>
      }
    >
      <Field label={t("label.package")} value={pkg} mono />
      <label className="control">
        {t("label.method")}
        <select className="select" value={method} onChange={(event) => setMethod(event.target.value)}>
          <option value="grant">grant</option>
          <option value="revoke">revoke</option>
        </select>
      </label>
      <label className="control">
        {t("label.permission")}
        <input
          className="input is-mono"
          list="android-permissions"
          value={permission}
          onChange={(event) => setPermission(event.target.value.toUpperCase())}
        />
      </label>
      <datalist id="android-permissions">
        {ANDROID_PERMISSIONS.map((item) => (
          <option key={item} value={item} />
        ))}
      </datalist>
    </Modal>
  );
}
