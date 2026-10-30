import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./styles.css";

try {
  const stored = localStorage.getItem("ata.theme");
  if (stored === "light" || stored === "dark") document.documentElement.dataset.theme = stored;
} catch {
  /* storage unavailable */
}

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
);
