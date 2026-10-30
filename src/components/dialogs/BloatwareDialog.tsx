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

function severity(removal: string): string {
  switch (removal.toLowerCase()) {
    case "recommended":
      return "sev-recommended";
    case "advanced":
      return "sev-advanced";
    case "unsafe":
      return "sev-unsafe";
    case "expert":
      return "sev-expert";
    default:
      return "sev-none";
  }
}

export function BloatwareDialog({
  serial,
  user,
  onClose,
  onApplied,
}: {
  serial: string;
  user: string;
  onClose: () => void;
  onApplied: () => void;
}) {
  const { t } = useI18n();
  const ui = useUi();
  const [level, setLevel] = useState("basic");
  const [actionMode, setActionMode] = useState("0");
  const [entries, setEntries] = useState<BloatEntry[]>([]);
  const [selected, setSelected] = useState<Set<string>>(new Set());
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(false);
  const { busy, run } = useAction();
  const [error, setError] = useState("");
  const askedForList = useRef(false);

  const scan = async (scanLevel: string) => {
    setLoading(true);
    setError("");
    try {
      const available = await api.bloatwareAvailable();
      if (!available) {
        if (!askedForList.current) {
          askedForList.current = true;
          const download = await ui.confirm(t("bloatware.downloadQuestion"), t("bloatware.downloadTitle"));
          if (!download) {
            setError(t("bloatware.missing"));
            return;
          }
          await api.downloadBloatwareList();
          ui.notify(t("bloatware.downloaded"), "ok");
        }
      }
      const list = await api.bloatwareScan(serial, user, scanLevel);
      setEntries(list);
      setSelected(new Set());
    } catch (err) {
      setError(String(err));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    void scan(level);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [level]);

  const visible = useMemo(() => {
    const needle = search.trim().toLowerCase();
    if (!needle) return entries;
    return entries.filter(
      (entry) =>
        entry.package.toLowerCase().includes(needle) ||
        entry.description.toLowerCase().includes(needle),
    );
  }, [entries, search]);

  const toggle = (pkg: string) => {
    setSelected((current) => {
      const next = new Set(current);
      if (next.has(pkg)) next.delete(pkg);
      else next.add(pkg);
      return next;
    });
  };

  const apply = () => {
    if (!selected.size) {
      void ui.alert(t("error.noAppSelected"), "warning");
      return;
    }
    void run(async () => {
      const targets = entries.filter((entry) => selected.has(entry.package));
      const remove = actionMode === "1";
      const toUninstall = targets.filter((entry) => remove || !entry.system).map((entry) => entry.package);
      const toDisable = targets.filter((entry) => !remove && entry.system).map((entry) => entry.package);

      const failures: string[] = [];
      if (toUninstall.length) {
        const results = await api.appsApply(serial, user, "uninstall", toUninstall);
        failures.push(...results.filter((result) => !result.ok).map((result) => result.target));
      }
      if (toDisable.length) {
        const results = await api.appsApply(serial, user, "disable", toDisable);
        failures.push(...results.filter((result) => !result.ok).map((result) => result.target));
      }

      if (failures.length) {
        await ui.alert(`${t("bloatware.notApplied")}\n\n${failures.join("\n")}\n\n${t("bloatware.hint")}`, "warning");
      } else {
        ui.notify(t("bloatware.applied"), "ok");
      }
      onApplied();
      onClose();
    });
  };

  return (
    <Modal
      title={t("dialog.bloatwareTitle")}
      onClose={onClose}
      xl
      footer={
        <>
          <span className="hint grow">{t("bloatware.note")}</span>
          <Btn onClick={onClose} disabled={busy}>
            {t("button.cancel")}
          </Btn>
          <Btn variant="primary" icon="play" onClick={apply} disabled={busy || loading}>
            {t("button.run")}
          </Btn>
        </>
      }
    >
      <div className="row">
        <label className="control">
          {t("label.scanLevel")}
          <select className="select" value={level} onChange={(event) => setLevel(event.target.value)}>
            <option value="basic">{t("bloatware.scanLevelBasic")}</option>
            <option value="medium">{t("bloatware.scanLevelMedium")}</option>
            <option value="advance">{t("bloatware.scanLevelAdvance")}</option>
            <option value="expert">{t("bloatware.scanLevelExpert")}</option>
          </select>
        </label>

        <label className="control">
          {t("label.actionMode")}
          <select className="select" value={actionMode} onChange={(event) => setActionMode(event.target.value)}>
            <option value="0">{t("bloatware.actionDisableOrRemove")}</option>
            <option value="1">{t("bloatware.actionRemove")}</option>
          </select>
        </label>

        <label className="control grow">
          {t("label.search")}
          <input className="input" value={search} onChange={(event) => setSearch(event.target.value)} />
        </label>

        <label className="check check-top">
          <input
            type="checkbox"
            checked={selected.size > 0 && selected.size === visible.length}
            onChange={(event) => {
              if (event.target.checked) setSelected(new Set(visible.map((entry) => entry.package)));
              else setSelected(new Set());
            }}
          />
          {t("bloatware.selectAll")}
        </label>

        <div className="right">
          <span className="badge badge-info">{t("bloatware.found", { count: visible.length })}</span>
        </div>
      </div>

      {error ? <p className="message is-error">{error}</p> : null}

      <div className="table bloatware-table">
        <div className="table-head bloatware-grid">
          <span />
          <span>{t("label.package")}</span>
          <span>{t("label.description")}</span>
          <span>{t("label.removal")}</span>
        </div>
        <div className="table-scroll">
          {loading ? (
            <Empty>
              <Spinner /> {t("state.checking")}
            </Empty>
          ) : visible.length === 0 ? (
            <Empty>{t("apk.noApps")}</Empty>
          ) : (
            visible.map((entry) => (
              <div
                key={entry.package}
                className={`table-row bloatware-grid ${selected.has(entry.package) ? "is-selected" : ""}`}
                onClick={() => toggle(entry.package)}
              >
                <input
                  type="checkbox"
                  checked={selected.has(entry.package)}
                  onChange={() => toggle(entry.package)}
                  onClick={(event) => event.stopPropagation()}
                />
                <span className="pkg" title={entry.package}>
                  {entry.package}
                </span>
                <span className="cell" title={entry.description}>
                  {entry.description}
                </span>
                <span>
                  <span className={`sev ${severity(entry.removal)}`}>{entry.removal || "—"}</span>
                </span>
              </div>
            ))
          )}
        </div>
      </div>
    </Modal>
  );
}
