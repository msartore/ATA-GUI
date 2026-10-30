import { useCallback, useEffect, useRef, useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, Field, Modal, Spinner, useUi } from "./ui";
import { errorText } from "../lib/util";
import type { MdnsService, QrPairing } from "../lib/types";

type Phase = "waiting" | "found" | "paired" | "connecting" | "connected" | "error";

const sleep = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms));

function findPairing(services: MdnsService[], service: string) {
  return services.find(
    (entry) =>
      entry.service.includes("_adb-tls-pairing") &&
      (entry.name === service || entry.name.startsWith(service)),
  );
}

function findConnect(services: MdnsService[], ip: string) {
  return services.find(
    (entry) => entry.service.includes("_adb-tls-connect") && entry.ip === ip,
  );
}

/**
 * Wireless debugging pairing with the QR flow introduced in Android 11:
 * the phone scans `WIFI:T:ADB;S:…;P:…;;`, advertises `_adb-tls-pairing._tcp`,
 * the desktop pairs, then connects through `_adb-tls-connect._tcp`.
 */
export function QrPairingDialog({ onClose }: { onClose: () => void }) {
  const { t } = useI18n();
  const ui = useUi();
  const main = useMain();
  const mainRef = useRef(main);
  mainRef.current = main;
  const closeRef = useRef(onClose);
  closeRef.current = onClose;

  const [code, setCode] = useState<QrPairing | null>(null);
  const [phase, setPhase] = useState<Phase>("waiting");
  const [message, setMessage] = useState("");

  const generate = useCallback(async () => {
    try {
      const next = await api.qrPairingCode();
      setCode(next);
      setPhase("waiting");
      setMessage("");
    } catch (error) {
      await ui.alert(errorText(error));
    }
  }, [ui]);

  useEffect(() => {
    void generate();
  }, [generate]);

  useEffect(() => {
    if (!code) return;
    let cancelled = false;

    (async () => {
      const deadline = Date.now() + 5 * 60 * 1000;
      let pairing: MdnsService | undefined;

      while (!cancelled && Date.now() < deadline) {
        const services = await api.mdnsServices().catch(() => [] as MdnsService[]);
        pairing = findPairing(services, code.service);
        if (pairing) break;
        await sleep(1200);
      }
      if (cancelled) return;
      if (!pairing) {
        setPhase("error");
        setMessage(t("qr.timeout"));
        return;
      }

      setPhase("found");
      mainRef.current.log("INFO", t("qr.found"));

      const paired = await api
        .adbPair(pairing.ip, pairing.port, code.password)
        .catch((error) => ({ ok: false, message: errorText(error) }));
      if (cancelled) return;
      if (!paired.ok) {
        setPhase("error");
        setMessage(paired.message);
        mainRef.current.log("ERROR", paired.message);
        return;
      }

      setPhase("paired");
      mainRef.current.log("OK", t("qr.paired"));
      await sleep(800);

      setPhase("connecting");
      while (!cancelled && Date.now() < deadline) {
        const services = await api.mdnsServices().catch(() => [] as MdnsService[]);
        const connect = findConnect(services, pairing.ip);
        if (connect) {
          const result = await api
            .adbConnect(connect.ip, connect.port, null)
            .catch((error) => ({ ok: false, message: errorText(error) }));
          if (result.ok) {
            setPhase("connected");
            mainRef.current.log("OK", t("log.connected", { ip: connect.ip }));
            await mainRef.current.addIp(connect.ip);
            await mainRef.current.refreshDevices();
            window.setTimeout(() => {
              if (!cancelled) closeRef.current();
            }, 1500);
            return;
          }
        }
        await sleep(1200);
      }
      if (!cancelled) {
        setPhase("error");
        setMessage(t("qr.timeout"));
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [code, t]);

  const phaseLabel: Record<Phase, string> = {
    waiting: t("qr.waiting"),
    found: t("qr.found"),
    paired: t("qr.paired"),
    connecting: t("qr.connecting"),
    connected: t("qr.connected"),
    error: t("qr.failed"),
  };

  const tone =
    phase === "connected" ? "badge-ok" : phase === "error" ? "badge-danger" : "badge-info";

  return (
    <Modal
      title={t("dialog.qrTitle")}
      onClose={onClose}
      footer={
        <>
          <Btn onClick={onClose}>{t("button.cancel")}</Btn>
          <Btn icon="refresh" onClick={() => void generate()} disabled={phase === "found" || phase === "paired" || phase === "connecting"}>
            {t("qr.newCode")}
          </Btn>
        </>
      }
    >
      {code ? (
        <>
          <ol className="qr-steps">
            <li>{t("qr.step1")}</li>
            <li>{t("qr.step2")}</li>
            <li>{t("qr.step3")}</li>
          </ol>

          <div className="qr-box">
            <img
              src={`data:image/svg+xml;charset=utf-8,${encodeURIComponent(code.svg)}`}
              alt="QR"
              width={260}
              height={260}
            />
          </div>

          <div className="row">
            <span className={`badge ${tone}`}>
              {phase === "waiting" || phase === "connecting" ? <Spinner size={11} /> : null}
              {phaseLabel[phase]}
            </span>
            {message ? <span className="hint mono qr-message">{message}</span> : null}
          </div>

          <div className="qr-meta">
            <Field label={t("qr.service")} value={code.service} mono />
            <Field label={t("qr.password")} value={code.password} mono />
          </div>

          <p className="hint">{t("qr.hint")}</p>
        </>
      ) : (
        <div className="empty">
          <Spinner /> {t("value.loading")}
        </div>
      )}
    </Modal>
  );
}
