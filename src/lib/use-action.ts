import { useCallback, useState } from "react";
import { useUi } from "../components/ui";
import { errorText } from "./util";

export interface RunOptions {
  /** Called before the alert, e.g. to reset a parent's state on failure. */
  onError?: (error: unknown) => void;
  /** Suppress the alert when the caller wants to handle the failure itself. */
  silent?: boolean;
}

export interface ActionRunner {
  busy: boolean;
  /** Run an async task with a busy flag and a single error → alert funnel. */
  run: <T>(task: () => Promise<T>, options?: RunOptions) => Promise<T | undefined>;
}

/**
 * Replaces the `setBusy(true) / try / catch / alert / finally setBusy(false)`
 * block that every dialog used to repeat.
 */
export function useAction(): ActionRunner {
  const ui = useUi();
  const [busy, setBusy] = useState(false);

  const run = useCallback(
    async <T,>(task: () => Promise<T>, options?: RunOptions): Promise<T | undefined> => {
      setBusy(true);
      try {
        return await task();
      } catch (error) {
        options?.onError?.(error);
        if (!options?.silent) await ui.alert(errorText(error));
        return undefined;
      } finally {
        setBusy(false);
      }
    },
    [ui],
  );

  return { busy, run };
}
