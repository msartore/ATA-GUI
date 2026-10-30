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

export function AboutDialog({ version, onClose }: { version: string; onClose: () => void }) {
  const { t } = useI18n();

  return (
    <Modal
      title={t("dialog.aboutTitle")}
      onClose={onClose}
      footer={
        <Btn variant="primary" onClick={onClose}>
          {t("button.close")}
        </Btn>
      }
    >
      <div className="about-head">
        <img src="/resources/logo.png" alt="ATA-GUI" className="about-logo" />
        <div>
          <strong>{t("app.name")}</strong>
          <p className="hint">{t("app.tagline")}</p>
        </div>
      </div>

      <Field label={t("label.currentRelease")} value={version} mono />
      <p className="hint">{t("about.credits")}</p>

      <hr className="rule" />

      <div className="link-grid">
        <Link href={ATA_REPO}>{t("about.repo")}</Link>
        <Link href="https://raw.githubusercontent.com/msartore/ATA-GUI/main/LICENSE">{t("about.license")}</Link>
        <Link href="https://developer.android.com/studio/releases/platform-tools">{t("about.sdk")}</Link>
        <Link href="https://github.com/Genymobile/scrcpy">{t("about.scrcpy")}</Link>
        <Link href="https://icons8.com">{t("about.icons")}</Link>
        <Link href={UAD_SITE}>{t("about.debloater")}</Link>
        <Link href={ATA_SITE}>{t("app.name")}</Link>
        <Link href="https://msartore.dev">msartore.dev</Link>
      </div>
    </Modal>
  );
}
