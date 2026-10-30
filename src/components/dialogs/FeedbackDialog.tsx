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

export function FeedbackDialog({ onClose }: { onClose: () => void }) {
  const { t } = useI18n();

  return (
    <Modal
      title={t("dialog.feedbackTitle")}
      onClose={onClose}
      footer={
        <>
          <Btn variant="ghost" onClick={() => void api.openUrl("https://msartore.dev/donate")}>
            {t("button.donate")}
          </Btn>
          <Btn variant="primary" onClick={onClose}>
            {t("button.close")}
          </Btn>
        </>
      }
    >
      <p className="message">{t("dialog.feedbackBody")}</p>
      <p className="hint">{t("feedback.body")}</p>

      <div className="stack">
        <Btn icon="star" className="btn-block" onClick={() => void api.openUrl(`${ATA_REPO}/issues/new/choose`)}>
          {t("button.sendFeedback")}
        </Btn>
        <Btn
          icon="download"
          className="btn-block"
          onClick={() =>
            void api.openUrl(
              `${ATA_REPO}/issues/new?assignees=&labels=enhancement&template=feature_request.md&title=%5BFEATURE%5D`,
            )
          }
        >
          {t("button.featureRequest")}
        </Btn>
        <Btn
          icon="error"
          className="btn-block"
          onClick={() =>
            void api.openUrl(
              `${ATA_REPO}/issues/new?assignees=&labels=bug&template=bug_report.md&title=%5BBUG%5D`,
            )
          }
        >
          {t("button.bugReport")}
        </Btn>
        <Btn icon="download" className="btn-block" onClick={() => void api.openUrl(ATA_BRIDGE)}>
          {t("feedback.bridge")}
        </Btn>
      </div>
    </Modal>
  );
}
