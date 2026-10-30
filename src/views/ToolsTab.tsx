import { useEffect, useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { useMain } from "../lib/main-context";
import { Btn, DropZone, Panel, useUi } from "../components/ui";
import { errorText } from "../lib/util";

export function ToolsTab() {
  const { t } = useI18n();
  const m = useMain();
  const ui = useUi();
  const [display, setDisplay] = useState(0);
  const [text, setText] = useState("");
  const [cameraMode, setCameraMode] = useState("back");
  const connected = Boolean(m.device);

  useEffect(() => {
    if (m.maxDisplay >= 0 && display > m.maxDisplay) setDisplay(0);
  }, [m.maxDisplay, display]);

  const toggleRotation = async () => {
    const enabled = !m.rotationFree;
    if (enabled && !(await ui.confirm(t("confirm.setDisplayBody", { display }), t("confirm.setDisplayTitle")))) {
      return;
    }
    await m.setRotation(enabled, display);
  };

  const inject = async () => {
    if (!m.device) return;
    if (!text.trim()) {
      await ui.alert(t("error.enterText"), "warning");
      return;
    }
    try {
      await api.injectText(m.device.id, text);
      m.log("OK", t("log.textInjected"));
    } catch (error) {
      m.log("ERROR", errorText(error));
    }
  };

  const startCamera = async () => {
    if (!m.device) return;
    if (!m.info || m.info.sdk < 31) {
      await ui.alert(t("error.cameraUnsupported"));
      return;
    }
    if (!m.tools?.scrcpy) {
      await m.shareScreen();
      return;
    }
    const version = await api.scrcpyVersion().catch(() => "");
    const major = Number(version.match(/(\d+)\./)?.[1] ?? 0);
    if (version && major < 22) {
      await ui.alert(t("error.scrcpyOutdated"), "warning");
      return;
    }
    try {
      await api.scrcpyRun(m.device.id, [
        "--video-source=camera",
        "--camera-size=1920x1080",
        `--camera-facing=${cameraMode}`,
      ]);
    } catch (error) {
      await ui.alert(errorText(error));
    }
  };

  return (
    <div className="tools-grid">
      <Panel title={t("section.freeRotation")}>
        <label className="control">
          {t("label.type")}
          <select className="select" value={display} onChange={(event) => setDisplay(Number(event.target.value))}>
            {Array.from({ length: Math.max(1, m.maxDisplay + 1) }, (_, index) => index).map((value) => (
              <option key={value} value={value}>
                {value}
              </option>
            ))}
          </select>
        </label>
        <Btn icon="rotate" disabled={!connected} onClick={() => void toggleRotation()}>
          {m.rotationFree ? t("button.unset") : t("button.set")}
        </Btn>
        <p className="hint">{m.rotationFree ? t("log.freeRotationSet") : t("log.freeRotationUnset")}</p>
      </Panel>

      <Panel title={t("section.textInjection")}>
        <textarea
          className="textarea"
          rows={3}
          value={text}
          onChange={(event) => setText(event.target.value)}
        />
        <div className="row">
          <Btn icon="terminal" disabled={!connected} onClick={() => void inject()}>
            {t("button.inject")}
          </Btn>
          <Btn variant="ghost" icon="x" onClick={() => setText("")}>
            {t("button.clear")}
          </Btn>
        </div>
      </Panel>

      <Panel title={t("section.fileTransfer")}>
        <DropZone
          icon="upload"
          label={t("section.fileTransfer")}
          hint="sdcard/ATA"
          onFiles={(paths) => void m.transferFiles(paths)}
        />
      </Panel>

      <Panel title={t("section.liveCamera")}>
        <label className="control">
          {t("label.type")}
          <select className="select" value={cameraMode} onChange={(event) => setCameraMode(event.target.value)}>
            {["front", "back", "external"].map((mode) => (
              <option key={mode} value={mode}>
                {mode}
              </option>
            ))}
          </select>
        </label>
        <Btn icon="camera" disabled={!connected} onClick={() => void startCamera()}>
          {t("button.start")}
        </Btn>
      </Panel>
    </div>
  );
}
