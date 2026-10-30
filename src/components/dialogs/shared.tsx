import { useEffect, useMemo, useRef, useState } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "../../lib/api";
import { useI18n } from "../../i18n";
import { Btn, Empty, Field, Modal, Spinner, useUi } from "../ui";
import { LICENSE_TEXT } from "../../lib/license";
import { OEM_DRIVERS } from "../../lib/oem-drivers";
import { ANDROID_PERMISSIONS } from "../../lib/permissions";
import type { BloatEntry, Config, EncoderData, ToolStatus } from "../../lib/types";

export const ATA_REPO = "https://github.com/msartore/ATA-GUI";
export const ATA_SITE = "https://msartore.dev/projects/ata-gui";
export const ATA_BRIDGE = "https://play.google.com/store/apps/details?id=dev.msartore.atabridge";
export const UAD_SITE =
  "https://github.com/Universal-Debloater-Alliance/universal-android-debloater-next-generation";

export function Link({ href, children }: { href: string; children: React.ReactNode }) {
  return (
    <button type="button" className="link" onClick={() => void api.openUrl(href)}>
      {children}
    </button>
  );
}

export function ScrollableDialog({
  title,
  children,
  onClose,
  wide,
}: {
  title: string;
  children: React.ReactNode;
  onClose: () => void;
  wide?: boolean;
}) {
  const { t } = useI18n();

  return (
    <Modal
      title={title}
      onClose={onClose}
      wide={wide}
      footer={
        <Btn variant="primary" onClick={onClose}>
          {t("button.ok")}
        </Btn>
      }
    >
      {children}
    </Modal>
  );
}

export function MissingDialog({
  title,
  body,
  onYes,
  onNo,
  onInfo,
}: {
  title: string;
  body: string;
  onYes: () => void;
  onNo: () => void;
  onInfo: () => void;
}) {
  const { t } = useI18n();

  return (
    <Modal
      title={title}
      onClose={onNo}
      footer={
        <>
          <Btn icon="info" variant="ghost" onClick={onInfo}>
            {t("button.info")}
          </Btn>
          <Btn onClick={onNo}>{t("button.no")}</Btn>
          <Btn variant="primary" onClick={onYes}>
            {t("button.yes")}
          </Btn>
        </>
      }
    >
      <p className="message">{body}</p>
    </Modal>
  );
}

export function LoadingDialog({
  label,
  detail,
  progress,
}: {
  label: string;
  detail?: string;
  progress?: number;
}) {
  return (
    <Modal title={label}>
      <div className="row">
        <Spinner size={16} />
        <span className="mono">{detail ?? ""}</span>
      </div>
      <div className="progress">
        <i style={{ width: `${progress ?? 35}%` }} />
      </div>
    </Modal>
  );
}
