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

export function UpdateDialog({ url, onClose }: { url: string; onClose: () => void }) {
  const { t } = useI18n();
  const tRef = useRef(t);
  tRef.current = t;
  const started = useRef(false);
  const [message, setMessage] = useState(t("update.downloading"));
  const [progress, setProgress] = useState(0);
  const [failed, setFailed] = useState(false);

  useEffect(() => {
    // The update replaces the running binary: it must never be issued twice,
    // and a language/theme change must not restart it.
    if (started.current) return;
    started.current = true;
    let mounted = true;

    const unlisten = listen<{ phase: string; received: number; total: number | null }>(
      "download:progress",
      (event) => {
        const { phase, received, total } = event.payload;
        if (!mounted) return;
        if (phase === "update") {
          setMessage(tRef.current("update.downloading"));
          if (total) setProgress(Math.min(95, Math.round((received / total) * 90)));
        } else {
          setMessage(phase === "unpacking" ? tRef.current("update.unzipping") : tRef.current("update.starting"));
          setProgress(96);
        }
      },
    );

    (async () => {
      try {
        await api.applyUpdate(url);
      } catch (error) {
        if (!mounted) return;
        setFailed(true);
        setMessage(`${tRef.current("update.failed")}: ${error}`);
      }
    })();

    return () => {
      mounted = false;
      void unlisten.then((stop) => stop());
    };
  }, [url]);

  return (
    <Modal
      title={t("dialog.updateTitle")}
      onClose={failed ? onClose : undefined}
      footer={
        failed ? (
          <Btn onClick={onClose}>{t("button.close")}</Btn>
        ) : (
          <span className="hint">{t("update.dontClose")}</span>
        )
      }
    >
      <p className="message">{message}</p>
      <div className="progress">
        <i style={{ width: `${progress}%` }} />
      </div>
    </Modal>
  );
}
