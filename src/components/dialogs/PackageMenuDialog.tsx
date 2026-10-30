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

export function PackageMenuDialog({
  serial,
  user,
  packages,
  onClose,
  onDone,
}: {
  serial: string;
  user: string;
  packages: string[];
  onClose: () => void;
  onDone: () => void;
}) {
  const { t } = useI18n();
  const ui = useUi();
  const { busy, run } = useAction();

  const apply = (action: "disable" | "enable" | "clear") =>
    void run(async () => {
      const results = await api.appsApply(serial, user, action, packages);
      const failed = results.filter((result) => !result.ok);
      if (failed.length) {
        await ui.alert(
          `${t("log.operationFailed")}\n\n${failed.map((r) => r.target).join("\n")}`,
          "warning",
        );
      }
      onDone();
      onClose();
    });

  return (
    <Modal
      title={t("dialog.packageMenuTitle")}
      onClose={onClose}
      footer={
        <>
          <Btn onClick={onClose} disabled={busy}>
            {t("button.cancel")}
          </Btn>
          <Btn icon="stop" onClick={() => apply("disable")} disabled={busy}>
            {t("value.disabled")}
          </Btn>
          <Btn icon="check" onClick={() => apply("enable")} disabled={busy}>
            {t("button.start")}
          </Btn>
          <Btn icon="trash" variant="danger" onClick={() => apply("clear")} disabled={busy}>
            {t("button.clear")}
          </Btn>
        </>
      }
    >
      <pre className="pre-block">{packages.join("\n")}</pre>
    </Modal>
  );
}
