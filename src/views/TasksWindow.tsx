import { useCallback, useEffect, useMemo, useState } from "react";
import { api } from "../lib/api";
import { useI18n } from "../i18n";
import { Btn, Empty, IconBtn, Panel, Spinner, useUi } from "../components/ui";
import { TitleBar, ResizeHandles } from "../components/TitleBar";
import { errorText } from "../lib/util";

export function TasksWindow({ serial, user }: { serial: string; user: string }) {
  const { t } = useI18n();
  const ui = useUi();
  const [rows, setRows] = useState<string[][]>([]);
  const [packages, setPackages] = useState<string[]>([]);
  const [search, setSearch] = useState("");
  const [appsOnly, setAppsOnly] = useState(false);
  const [selected, setSelected] = useState<Set<number>>(new Set());
  const [loading, setLoading] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const list = await api.processes(serial);
      setRows(list);
      setSelected(new Set());
    } catch (error) {
      await ui.alert(errorText(error));
    } finally {
      setLoading(false);
    }
  }, [serial, ui]);

  useEffect(() => {
    void load();
    void api
      .appsLoad(serial, user, "all")
      .then((result) => setPackages(result.apps.map((app) => app.package)))
      .catch(() => {});
  }, [load, serial, user]);

  const nameOf = (row: string[]) => row[row.length - 1] ?? "";

  const visible = useMemo(() => {
    const needle = search.trim().toLowerCase();
    return rows.filter((row) => {
      const name = nameOf(row).toLowerCase();
      if (needle && !name.includes(needle)) return false;
      if (appsOnly && packages.length) {
        return packages.some((pkg) => name.includes(pkg));
      }
      return true;
    });
  }, [rows, search, appsOnly, packages]);

  const kill = async () => {
    const names = [...selected]
      .map((index) => nameOf(visible[index]))
      .filter(Boolean);
    if (!names.length) {
      await ui.alert(t("error.noAppSelected"), "warning");
      return;
    }
    await api.forceStop(serial, names).catch(async (error) => ui.alert(errorText(error)));
    await load();
  };

  return (
    <div className="app">
      <TitleBar tools={null} compact title={t("tasks.title")} />
      <div className="window-head">
        <h1>{t("tasks.title")}</h1>
        <span className="badge badge-info mono" title={serial}>{serial}</span>
      </div>

      <div className="workspace is-window">
        <Panel
          flush
          actions={
            <div className="row is-tight">
              <IconBtn icon="refresh" label={t("tasks.reload")} onClick={() => void load()} />
              <Btn small variant={appsOnly ? "primary" : "subtle"} icon="filter" onClick={() => setAppsOnly((v) => !v)}>
                {t("tasks.filterApps")}
              </Btn>
              <Btn small variant="danger" icon="stop" onClick={() => void kill()} disabled={selected.size === 0}>
                {t("tasks.kill")}
              </Btn>
            </div>
          }
        >
          <div className="log-toolbar">
            <input
              className="input input-sm w-filter-lg"
              placeholder={t("tasks.searchPlaceholder")}
              value={search}
              onChange={(event) => setSearch(event.target.value)}
            />
            <span className="hint">
              {t("tasks.total", { count: visible.length })}
            </span>
            <span className="right hint">
              {t("label.selectedApps")}: {selected.size}
            </span>
          </div>

          <div className="table tasks-table">
            <div className="table-head tasks-grid-full">
              <span>{t("tasks.user")}</span>
              <span>{t("tasks.pid")}</span>
              <span>{t("tasks.ppid")}</span>
              <span>{t("tasks.vsz")}</span>
              <span>{t("tasks.rss")}</span>
              <span>{t("tasks.state")}</span>
              <span>{t("tasks.name")}</span>
            </div>
            <div className="table-scroll">
              {loading ? (
                <Empty>
                  <Spinner /> {t("value.loading")}
                </Empty>
              ) : visible.length === 0 ? (
                <Empty>{t("apk.noApps")}</Empty>
              ) : (
                visible.map((row, index) => (
                  <div
                    key={`${nameOf(row)}-${index}`}
                    className={`table-row tasks-grid-full ${selected.has(index) ? "is-selected" : ""}`}
                    onClick={() =>
                      setSelected((current) => {
                        const next = new Set(current);
                        if (next.has(index)) next.delete(index);
                        else next.add(index);
                        return next;
                      })
                    }
                  >
                    <span className="cell">{row[0]}</span>
                    <span className="cell">{row[1]}</span>
                    <span className="cell">{row[2]}</span>
                    <span className="cell">{row[3]}</span>
                    <span className="cell">{row[4]}</span>
                    <span className="cell">{row[row.length - 2]}</span>
                    <span className="cell mono">{nameOf(row)}</span>
                  </div>
                ))
              )}
            </div>
          </div>
        </Panel>
      </div>

      <ResizeHandles />
    </div>
  );
}
