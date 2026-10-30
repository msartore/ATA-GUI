import { createContext, useContext, useMemo, type ReactNode } from "react";
import type { LocaleBundle } from "../lib/types";

export type { LocaleBundle } from "../lib/types";

export interface LanguageInfo {
  code: string;
  label: string;
  source: string;
}

type Dict = Record<string, string>;

export type TFunction = (key: string, params?: Record<string, string | number>) => string;

/** Parse a flat `key -> text` JSON catalog, ignoring non-string values. */
export function parseDictionary(json: string): Dict {
  try {
    const parsed = JSON.parse(json);
    if (!parsed || typeof parsed !== "object" || Array.isArray(parsed)) return {};
    const dictionary: Dict = {};
    for (const [key, value] of Object.entries(parsed)) {
      if (typeof value === "string") dictionary[key] = value;
    }
    return dictionary;
  } catch {
    return {};
  }
}

/**
 * Resolve the language to render:
 * explicit preference -> exact OS locale -> OS base language -> English -> first.
 */
export function resolveLanguage(preference: string, available: string[]): string {
  if (available.length === 0) return "en";
  if (preference && preference !== "system" && available.includes(preference)) {
    return preference;
  }

  const requested =
    (typeof navigator !== "undefined" &&
      (navigator.languages?.[0] || navigator.language)) ||
    "en";

  const exact = available.find((code) => code.toLowerCase() === requested.toLowerCase());
  if (exact) return exact;

  const base = requested.split("-")[0].toLowerCase();
  const partial = available.find(
    (code) => code.toLowerCase() === base || code.toLowerCase().startsWith(base),
  );
  if (partial) return partial;

  return available.includes("en") ? "en" : available[0];
}

function interpolate(template: string, params?: Record<string, string | number>): string {
  if (!params) return template;
  return template.replace(/\{(\w+)\}/g, (whole, key: string) =>
    key in params ? String(params[key]) : whole,
  );
}

export function translate(
  dictionaries: Record<string, Dict>,
  language: string,
  key: string,
  params?: Record<string, string | number>,
): string {
  const template = dictionaries[language]?.[key] ?? dictionaries.en?.[key] ?? key;
  return interpolate(template, params);
}

interface I18nValue {
  language: string;
  t: TFunction;
  languages: LanguageInfo[];
  reload: () => void;
}

const I18nContext = createContext<I18nValue>({
  language: "en",
  t: (key) => key,
  languages: [],
  reload: () => {},
});

export function I18nProvider({
  preference,
  bundles,
  onReload,
  children,
}: {
  preference: string;
  bundles: LocaleBundle[];
  onReload?: () => void;
  children: ReactNode;
}) {
  const dictionaries = useMemo(() => {
    const map: Record<string, Dict> = {};
    for (const bundle of bundles) {
      map[bundle.code] = parseDictionary(bundle.json);
    }
    return map;
  }, [bundles]);

  const codes = useMemo(() => bundles.map((bundle) => bundle.code), [bundles]);
  const language = resolveLanguage(preference, codes);

  const languages = useMemo<LanguageInfo[]>(
    () =>
      bundles.map((bundle) => ({
        code: bundle.code,
        label: bundle.name?.trim() || bundle.code,
        source: bundle.source,
      })),
    [bundles],
  );

  const value = useMemo<I18nValue>(
    () => ({
      language,
      t: (key, params) => translate(dictionaries, language, key, params),
      languages,
      reload: onReload ?? (() => {}),
    }),
    [language, dictionaries, languages, onReload],
  );

  return <I18nContext.Provider value={value}>{children}</I18nContext.Provider>;
}

export function useI18n(): I18nValue {
  return useContext(I18nContext);
}
