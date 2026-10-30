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
import { errorText } from "../../lib/util";
import { AboutDialog } from "./AboutDialog";

export function SettingsDialog({
  config,
  tools,
  onChange,
  onClose,
}: {
  config: Config;
  tools: ToolStatus | null;
  onChange: (config: Config) => void;
  onClose: () => void;
}) {
  const { t, languages, reload } = useI18n();
  const ui = useUi();
  const [checking, setChecking] = useState(false);
  const [updatingTools, setUpdatingTools] = useState(false);
  const [statusKey, setStatusKey] = useState("");
  const [latest, setLatest] = useState("");
  const [changelog, setChangelog] = useState("");
  const [dialog, setDialog] = useState<"about" | "changelog" | null>(null);

  const status = statusKey.startsWith("state.") ? t(statusKey) : statusKey;

  const check = async () => {
    setChecking(true);
    setStatusKey("state.checking");
    try {
      const info = await api.checkUpdate();
      setLatest(info.latest);
      setChangelog(info.body);
      setStatusKey(info.newVersion ? "state.notUpToDate" : "state.upToDate");
    } catch (error) {
      setStatusKey(String(error));
      await ui.alert(errorText(error));
    } finally {
      setChecking(false);
    }
  };

  return (
    <>
      <Modal
        title={t("dialog.settingsTitle")}
        onClose={onClose}
        footer={
          <Btn variant="primary" onClick={onClose}>
            {t("button.close")}
          </Btn>
        }
      >
        <div className="settings-grid">
          <Field label={t("label.currentRelease")} value={tools?.version ?? "-"} mono />
          <Field
            label={t("label.latestRelease")}
            value={latest || "—"}
            mono
          />
        </div>

        <div className="row">
          <Btn icon="refresh" onClick={() => void check()} disabled={checking}>
            {checking ? <Spinner size={13} /> : null}
            {t("button.checkVersion")}
          </Btn>
          {changelog ? (
            <Btn variant="ghost" icon="list" onClick={() => setDialog("changelog")}>
              {t("button.changelog")}
            </Btn>
          ) : null}
          {status ? <span className="hint">{status}</span> : null}
        </div>

        <hr className="rule" />

        <label className="control">
          {t("settings.appearance")}
          <select
            className="select"
            value={config.theme ?? "dark"}
            onChange={(event) =>
              onChange({ ...config, theme: event.target.value as Config["theme"] })
            }
          >
            <option value="dark">{t("settings.themeDark")}</option>
            <option value="light">{t("settings.themeLight")}</option>
            <option value="system">{t("settings.themeSystem")}</option>
          </select>
        </label>

        <label className="control">
          {t("settings.language")}
          <select
            className="select"
            value={config.language}
            onChange={(event) => onChange({ ...config, language: event.target.value })}
          >
            <option value="system">{t("settings.languageSystem")}</option>
            {languages.map((language) => (
              <option key={language.code} value={language.code}>
                {language.label}
                {language.source === "external" ? " *" : ""}
              </option>
            ))}
          </select>
        </label>

        <label className="check">
          <input
            type="checkbox"
            checked={!config.feedbackPopup}
            onChange={(event) => onChange({ ...config, feedbackPopup: !event.target.checked })}
          />
          {t("settings.feedbackPopup")}
        </label>

        <div className="row">
          <Btn
            icon="folder"
            onClick={async () => {
              try {
                const dir = await api.localesOpenDir();
                ui.notify(dir, "ok");
              } catch (error) {
                await ui.alert(errorText(error));
              }
            }}
          >
            {t("settings.openLocales")}
          </Btn>
          <Btn
            icon="refresh"
            onClick={async () => {
              await reload();
              ui.notify(t("settings.reloadLocales"), "ok");
            }}
          >
            {t("settings.reloadLocales")}
          </Btn>
          <Btn
            icon="download"
            onClick={async () => {
              try {
                const path = await api.localesExportTemplate();
                ui.notify(path, "ok");
              } catch (error) {
                await ui.alert(errorText(error));
              }
            }}
          >
            {t("settings.exportTemplate")}
          </Btn>
        </div>
        <p className="hint">{t("settings.translationsHint")}</p>

        <hr className="rule" />

        <div className="row">
          <Btn
            icon="download"
            disabled={updatingTools}
            onClick={async () => {
              setUpdatingTools(true);
              try {
                await api.downloadPlatformTools();
                ui.notify(t("log.toolsReady"), "ok");
              } catch (error) {
                await ui.alert(errorText(error));
              } finally {
                setUpdatingTools(false);
              }
            }}
          >
            {updatingTools ? <Spinner size={13} /> : null}
            {t("button.updateLocalSdk")}
          </Btn>
          <Btn
            icon="trash"
            onClick={() => {
              void api.ipClear().then(() => {
                ui.notify(t("settings.ipHistoryDeleted"), "ok");
                onChange({ ...config, ips: [] });
              });
            }}
          >
            {t("button.deleteIpHistory")}
          </Btn>
        </div>

        <div className="row">
          <Btn icon="download" onClick={() => void api.openUrl(ATA_BRIDGE)}>
            {t("button.downloadBridge")}
          </Btn>
          <Btn icon="info" onClick={() => setDialog("about")}>
            {t("button.about")}
          </Btn>
        </div>

        <p className="hint">{t("settings.bridgeHint")}</p>
      </Modal>

      {dialog === "about" ? (
        <AboutDialog version={tools?.version ?? ""} onClose={() => setDialog(null)} />
      ) : null}

      {dialog === "changelog" ? (
        <Modal title={t("dialog.changelogTitle")} onClose={() => setDialog(null)} wide>
          <pre className="pre-block">{changelog}</pre>
        </Modal>
      ) : null}
    </>
  );
}
