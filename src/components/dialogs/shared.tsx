import { useEffect, useMemo, useRef, useState } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "../../lib/api";
import { useI18n } from "../../i18n";
import { Btn, Empty, Field, Modal, Spinner, useUi } from "../ui";
import { Icon } from "../Icon";
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
  license,
  licenseUrl,
  onYes,
  onNo,
}: {
  title: string;
  body: string;
  license: string;
  licenseUrl: string;
  onYes: () => void;
  onNo: () => void;
}) {
  const { t } = useI18n();

  // Catalogues written before 4.0 still carry the old bracketed sentence
  // ("[By pressing YES you agree to ...]"), and external locales/ files
  // overlay the bundled ones. Strip it: the consent note below replaces it.
  const cleanBody = body.replace(/\s*\n\s*\[[^\]]*\]\s*$/, "").trim();

  return (
    <Modal
      title={title}
      onClose={onNo}
      footer={
        <>
          <Btn onClick={onNo}>{t("button.no")}</Btn>
          <Btn variant="primary" onClick={onYes}>
            {t("button.yes")}
          </Btn>
        </>
      }
    >
      <p className="message">{cleanBody}</p>
      <div className="consent">
        <Icon name="info" size={13} />
        <span>
          {t("dialog.consentPrefix")} <Link href={licenseUrl}>{license}</Link>.
        </span>
      </div>
    </Modal>
  );
}

export function LoadingDialog({
  label,
  detail,
  progress,
  onCancel,
}: {
  label: string;
  detail?: string;
  progress?: number;
  onCancel?: () => void;
}) {
  const { t } = useI18n();

  return (
    <Modal
      title={label}
      footer={
        onCancel ? (
          <Btn icon="stop" onClick={onCancel}>
            {t("button.cancel")}
          </Btn>
        ) : undefined
      }
    >
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
