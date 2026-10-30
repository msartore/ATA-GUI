import { useCallback, useEffect, useState } from "react";
import { api } from "./lib/api";
import { listen } from "@tauri-apps/api/event";
import { I18nProvider } from "./i18n";
import { UiProvider } from "./components/ui";
import { MainWindow } from "./views/MainWindow";
import { LogsWindow } from "./views/LogsWindow";
import { TasksWindow } from "./views/TasksWindow";
import type { Config, LocaleBundle } from "./lib/types";
import { applyTheme, watchTheme } from "./lib/theme";

const DEFAULT_CONFIG: Config = {
  language: "system",
  feedbackPopup: true,
  disclaimerAccepted: false,
  ips: [],
  theme: "dark",
};

export default function App() {
  const params = new URLSearchParams(window.location.search);
  const windowKind = params.get("window");
  const serial = params.get("serial") ?? "";
  const user = params.get("user") ?? "0";

  const [config, setConfig] = useState<Config | null>(null);
  const [bundles, setBundles] = useState<LocaleBundle[] | null>(null);
  const [error, setError] = useState("");

  const loadLocales = useCallback(async () => {
    try {
      const list = await api.localesBundles();
      setBundles(list);
      setError(list.length === 0 ? "No translation catalogs available" : "");
    } catch (cause) {
      setError(String(cause));
    }
  }, []);

  useEffect(() => {
    void (async () => {
      const loaded = await api.configGet().catch(() => DEFAULT_CONFIG);
      setConfig(loaded);
      await loadLocales();
    })();
  }, [loadLocales]);

  useEffect(() => {
    if (!config) return;
    void applyTheme(config.theme);
    const stopSystem = watchTheme(config.theme, () => void applyTheme(config.theme));
    let unlisten: (() => void) | undefined;
    void listen<string>("theme:changed", (event) => {
      void applyTheme((event.payload as Config["theme"]) ?? "dark");
    })
      .then((stop) => {
        unlisten = stop;
      })
      .catch(() => {});
    return () => {
      stopSystem();
      unlisten?.();
    };
  }, [config?.theme]);

  if (!config || bundles === null) {
    return (
      <div className="splash">
        <span className="spinner" />
        <span>ATA-GUI</span>
      </div>
    );
  }

  if (bundles.length === 0) {
    return (
      <div className="splash is-error">
        <span>{error || "Translations unavailable"}</span>
        <button
          type="button"
          className="btn btn-primary"
          onClick={() => {
            void loadLocales();
          }}
        >
          Retry
        </button>
      </div>
    );
  }

  return (
    <I18nProvider preference={config.language} bundles={bundles} onReload={() => void loadLocales()}>
      <UiProvider>
        {windowKind === "logs" ? (
          <LogsWindow serial={serial} />
        ) : windowKind === "tasks" ? (
          <TasksWindow serial={serial} user={user} />
        ) : (
          <MainWindow config={config} setConfig={setConfig} />
        )}
      </UiProvider>
    </I18nProvider>
  );
}
