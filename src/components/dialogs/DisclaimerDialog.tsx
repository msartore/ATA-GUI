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

export function DisclaimerDialog({
  onAccept,
  onDecline,
}: {
  onAccept: () => void;
  onDecline: () => void;
}) {
  const { t } = useI18n();

  return (
    <Modal
      title={t("dialog.disclaimerTitle")}
      subtitle={t("dialog.disclaimerImportant")}
      footer={
        <>
          <Btn variant="danger" onClick={onDecline}>
            {t("button.cancel")}
          </Btn>
          <Btn variant="primary" onClick={onAccept}>
            {t("button.accept")}
          </Btn>
        </>
      }
    >
      <p className="hint">{t("dialog.disclaimerIntro")}</p>
      <pre className="pre-block is-license">{LICENSE_TEXT}</pre>
    </Modal>
  );
}
