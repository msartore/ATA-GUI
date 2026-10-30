import type { Config } from "./types";

/** Fallback used before the backend answers and when no config file exists. */
export const DEFAULT_CONFIG: Config = {
  language: "system",
  feedbackPopup: true,
  disclaimerAccepted: false,
  ips: [],
  theme: "dark",
};
