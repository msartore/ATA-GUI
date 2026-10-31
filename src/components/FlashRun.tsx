import { useEffect, useRef, useState } from "react";
import { listen } from "@tauri-apps/api/event";
import { api } from "../lib/api";
import type { FlashLine, FlashPhase } from "../lib/types";
import { useI18n } from "../i18n";
import { Btn } from "./ui";
import { Icon } from "./Icon";
import { errorText } from "../lib/util";

export interface FlashStepSpec {
  key: string;
  label: string;
  kind: "flash" | "boot" | "erase" | "sideload";
  image?: string;
  partition?: string;
  size?: number;
}

type StepState = "pending" | "running" | "done" | "failed";
type RunState = "running" | "done" | "failed" | "cancelled";

const MAX_LINES = 300;

function megabytes(bytes?: number | null): string {
  if (!bytes || bytes <= 0) return "";
  if (bytes < 1024 * 1024) return `${Math.round(bytes / 1024)} KB`;
  return `${(bytes / 1_048_576).toFixed(1)} MB`;
}

/**
 * Runs a list of flash steps one after another and renders what is actually
 * happening: the queue, the phase the tool reports, real bytes for sideload
 * (the archive is piped through stdin, so we know how much was sent) and the
 * tool output.
 */
export function FlashRun({
  target,
  serial,
  steps,
  onClose,
  onLog,
}: {
  target: string;
  serial: string;
  steps: FlashStepSpec[];
  onClose: () => void;
  onLog?: (ok: boolean, steps: FlashStepSpec[]) => void;
}) {
  const { t } = useI18n();
  const [state, setState] = useState<RunState>("running");
  const [index, setIndex] = useState(0);
  const [states, setStates] = useState<StepState[]>(steps.map(() => "pending"));
  const [phase, setPhase] = useState<FlashPhase | null>(null);
  const [lines, setLines] = useState<string[]>([]);
  const [startedAt] = useState(() => Date.now());
  const [elapsed, setElapsed] = useState(0);
  const outRef = useRef<HTMLDivElement | null>(null);
  const live = useRef(true);

  useEffect(() => {
    const timer = window.setInterval(() => setElapsed(Date.now() - startedAt), 250);
    return () => window.clearInterval(timer);
  }, [startedAt]);

  useEffect(() => {
    let disposed = false;
    const stops: Array<() => void> = [];

    (async () => {
      const stopLine = await listen<FlashLine>("flash:output", ({ payload }) => {
        setLines((current) => {
          const next = [...current, payload.line];
          return next.length > MAX_LINES ? next.slice(next.length - MAX_LINES) : next;
        });
      });
      const stopPhase = await listen<FlashPhase>("flash:phase", ({ payload }) => setPhase(payload));
      if (disposed) {
        stopLine();
        stopPhase();
        return;
      }
      stops.push(stopLine, stopPhase);
    })();

    return () => {
      disposed = true;
      live.current = false;
      for (const stop of stops) stop();
    };
  }, []);

  useEffect(() => {
    const node = outRef.current;
    if (node) node.scrollTop = node.scrollHeight;
  }, [lines]);

  useEffect(() => {
    let cancelled = false;
    const run = async () => {
      let ok = true;

      for (let i = 0; i < steps.length; i += 1) {
        if (cancelled) break;
        const step = steps[i];
        setIndex(i);
        setPhase(null);
        setStates((current) => current.map((value, at) => (at === i ? "running" : value)));

        let code = -1;
        try {
          code =
            step.kind === "sideload"
              ? await api.flashSideload(target, serial, step.image ?? "")
              : await api.flashStep(target, serial, step.kind, step.image ?? "", step.partition ?? "");
        } catch (error) {
          setLines((current) => [...current, errorText(error)]);
          code = -1;
        }

        const good = code === 0;
        setStates((current) => current.map((value, at) => (at === i ? (good ? "done" : "failed") : value)));
        if (!good) {
          ok = false;
          break;
        }
      }

      if (!cancelled) {
        setState(ok ? "done" : "failed");
        onLog?.(ok, steps);
      }
    };

    void run();
    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const done = states.filter((value) => value === "done").length;
  // Milestones reported by the tools themselves — not a timer. Sending, then
  // writing (fastboot) or installing (sideload), then done.
  const PHASE_WEIGHT: Record<string, number> = {
    sending: 0.35,
    writing: 0.8,
    installing: 0.85,
    done: 1,
  };
  const fraction = state === "running" && phase ? (PHASE_WEIGHT[phase.phase] ?? 0.05) : 0;
  const percent = Math.round(((done + fraction) / Math.max(1, steps.length)) * 100);

  const phaseLabel = (withSize = true) => {
    if (state !== "running") return "";
    if (!phase) return t("flash.connecting");
    if (phase.phase === "sending") {
      const size = withSize ? megabytes(phase.total) || megabytes(steps[index]?.size) : "";
      return `${t("flash.sending")} ${size}`.trim();
    }
    if (phase.phase === "writing") return t("flash.writing");
    if (phase.phase === "installing") return t("flash.installing");
    return t("flash.finishing");
  };

  const cancel = async () => {
    setState("cancelled");
    await api.flashCancel().catch(() => false);
  };

  const headline =
    state === "running"
      ? phaseLabel()
      : state === "done"
        ? t("flash.done")
        : state === "cancelled"
          ? t("flash.cancelled")
          : t("flash.failed");

  return (
    <div className={`flash-run is-${state}`}>
      <div className="flash-head">
        <div className="flash-headline">
          <span className={`flash-dot is-${state}`} />
          <span>{headline}</span>
        </div>
        <span className="flash-meta">
          {t("flash.steps", { done, total: steps.length })} · {(elapsed / 1000).toFixed(1)}s
        </span>
      </div>

      <div className={`flash-bar is-${state}`}>
        <span style={{ width: `${percent}%` }} />
      </div>

      <ol className="flash-steps">
        {steps.map((step, at) => {
          const shape = states[at];
          return (
            <li key={step.key} className={`flash-step is-${shape}`}>
              <span className="flash-mark">
                {shape === "done" ? (
                  <Icon name="check" size={13} />
                ) : shape === "failed" ? (
                  <Icon name="close" size={13} />
                ) : shape === "running" ? (
                  <span className="flash-spin" />
                ) : (
                  <span className="flash-idle" />
                )}
              </span>
              <span className="flash-label mono">{step.label}</span>
              <span className="flash-size mono">{megabytes(step.size)}</span>
              <span className="flash-state">
                {shape === "running"
                  ? phaseLabel(false)
                  : shape === "done"
                    ? t("flash.ok")
                    : shape === "failed"
                      ? t("flash.failed")
                      : t("flash.waiting")}
              </span>
            </li>
          );
        })}
      </ol>

      <div className="flash-out" ref={outRef}>
        {lines.length === 0 ? <span className="flash-out-empty">{t("flash.noOutput")}</span> : null}
        {lines.map((line, at) => (
          <div key={at}>{line}</div>
        ))}
      </div>

      <div className="flash-actions">
        {state === "running" ? (
          <Btn small variant="danger" icon="stop" onClick={() => void cancel()}>
            {t("button.cancel")}
          </Btn>
        ) : null}
        {state !== "running" ? (
          <Btn small variant="primary" icon="check" onClick={onClose}>
            {t("button.close")}
          </Btn>
        ) : null}
      </div>
    </div>
  );
}
