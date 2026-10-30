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

export function ScrcpyDialog({ serial, onClose }: { serial: string; onClose: () => void }) {
  const { t } = useI18n();
  const ui = useUi();
  const [video, setVideo] = useState<EncoderData[]>([]);
  const [audio, setAudio] = useState<EncoderData[]>([]);
  const [videoIndex, setVideoIndex] = useState(0);
  const [audioIndex, setAudioIndex] = useState(0);
  const [turnScreenOff, setTurnScreenOff] = useState(false);
  const [powerOffOnClose, setPowerOffOnClose] = useState(false);
  const [showTouches, setShowTouches] = useState(false);
  const [powerOn, setPowerOn] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    (async () => {
      try {
        const media = await api.scrcpyEncoders(serial);
        setVideo(media.video);
        setAudio(media.audio);
      } catch (error) {
        await ui.alert(errorText(error));
      } finally {
        setLoading(false);
      }
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [serial]);

  const start = async () => {
    const args: string[] = [];
    if (turnScreenOff) args.push("-Sw");
    if (powerOffOnClose) args.push("--power-off-on-close");
    if (showTouches) args.push("-t");
    if (!powerOn) args.push("--no-power-on");

    const videoEncoder = video[videoIndex];
    const audioEncoder = audio[audioIndex];
    if (videoEncoder) {
      args.push(`--video-codec=${videoEncoder.codec}`, `--video-encoder=${videoEncoder.encoder}`);
    }
    if (audioEncoder) {
      args.push(`--audio-codec=${audioEncoder.codec}`, `--audio-encoder=${audioEncoder.encoder}`);
    }

    try {
      await api.scrcpyRun(serial, args);
      onClose();
    } catch (error) {
      await ui.alert(errorText(error));
    }
  };

  return (
    <Modal
      title={t("dialog.scrcpyTitle")}
      onClose={onClose}
      footer={
        <>
          <Btn onClick={onClose}>{t("button.close")}</Btn>
          <Btn variant="primary" icon="play" onClick={() => void start()} disabled={loading}>
            {t("scrcpy.start")}
          </Btn>
        </>
      }
    >
      {loading ? (
        <Empty>
          <Spinner /> {t("value.loading")}
        </Empty>
      ) : (
        <>
          <label className="control">
            {t("label.videoCodec")}
            <select
              className="select"
              value={videoIndex}
              onChange={(event) => setVideoIndex(Number(event.target.value))}
            >
              {video.map((encoder, index) => (
                <option key={`${encoder.codec}-${encoder.encoder}`} value={index}>
                  {encoder.codec} — {encoder.encoder}
                </option>
              ))}
            </select>
          </label>

          <label className="control">
            {t("label.audioCodec")}
            <select
              className="select"
              value={audioIndex}
              onChange={(event) => setAudioIndex(Number(event.target.value))}
            >
              {audio.map((encoder, index) => (
                <option key={`${encoder.codec}-${encoder.encoder}`} value={index}>
                  {encoder.codec} — {encoder.encoder}
                </option>
              ))}
            </select>
          </label>

          <div className="row">
            <label className="check">
              <input type="checkbox" checked={turnScreenOff} onChange={(event) => setTurnScreenOff(event.target.checked)} />
              {t("scrcpy.turnScreenOff")}
            </label>
            <label className="check">
              <input type="checkbox" checked={powerOffOnClose} onChange={(event) => setPowerOffOnClose(event.target.checked)} />
              {t("scrcpy.powerOffOnClose")}
            </label>
            <label className="check">
              <input type="checkbox" checked={showTouches} onChange={(event) => setShowTouches(event.target.checked)} />
              {t("scrcpy.showTouches")}
            </label>
            <label className="check">
              <input type="checkbox" checked={powerOn} onChange={(event) => setPowerOn(event.target.checked)} />
              {t("scrcpy.powerOnOnStart")}
            </label>
          </div>
        </>
      )}
    </Modal>
  );
}
