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

export function PairingDialog({
  ip,
  port,
  onDone,
  onClose,
}: {
  ip: string;
  port: string;
  onDone: (ok: boolean) => void;
  onClose: () => void;
}) {
  const { t } = useI18n();
  const ui = useUi();
  const [code, setCode] = useState("");
  const { busy, run } = useAction();

  const submit = () => {
    if (!/^\d{6}$/.test(code)) {
      void ui.alert(t("error.codeDigits"), "warning");
      return;
    }
    void run(
      async () => {
        const result = await api.adbPair(ip, port, code);
        onDone(result.ok);
        if (!result.ok) {
          const hint = result.code === "auth" ? `\n\n${t("hint.pairAuth")}` : "";
          await ui.alert(`${t("log.pairFailed", { ip })}: ${result.message}${hint}`);
        }
      },
      { onError: () => onDone(false) },
    );
  };

  return (
    <Modal
      title={t("dialog.pairingTitle")}
      subtitle={`${ip}:${port}`}
      onClose={onClose}
      footer={
        <>
          <Btn onClick={onClose}>{t("button.cancel")}</Btn>
          <Btn variant="primary" onClick={() => void submit()} disabled={busy}>
            {t("button.pair")}
          </Btn>
        </>
      }
    >
      <p className="hint">{t("dialog.pairingHint")}</p>
      <input
        className="input is-mono pairing-code"
        value={code}
        maxLength={6}
        inputMode="numeric"
        placeholder="000000"
        onChange={(event) => setCode(event.target.value.replace(/\D/g, ""))}
        onKeyDown={(event) => event.key === "Enter" && void submit()}
      />
    </Modal>
  );
}
