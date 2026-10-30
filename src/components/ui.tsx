import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
  type ButtonHTMLAttributes,
  type ReactNode,
} from "react";
import { Icon, type IconName } from "./Icon";
import type { MessageKind } from "../lib/types";

/* ------------------------------------------------------------------ buttons */

type BtnVariant = "default" | "primary" | "danger" | "ghost" | "subtle";

export function Btn({
  children,
  icon,
  variant = "default",
  small,
  active,
  ...rest
}: ButtonHTMLAttributes<HTMLButtonElement> & {
  icon?: IconName;
  variant?: BtnVariant;
  small?: boolean;
  active?: boolean;
}) {
  const classes = [
    "btn",
    `btn-${variant}`,
    small ? "btn-sm" : "",
    active ? "is-active" : "",
    rest.className ?? "",
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <button type="button" {...rest} className={classes}>
      {icon ? <Icon name={icon} size={small ? 13 : 15} /> : null}
      {children ? <span>{children}</span> : null}
    </button>
  );
}

export function IconBtn({
  icon,
  label,
  active,
  ...rest
}: ButtonHTMLAttributes<HTMLButtonElement> & {
  icon: IconName;
  label: string;
  active?: boolean;
}) {
  return (
    <button
      type="button"
      title={label}
      aria-label={label}
      {...rest}
      className={`icon-btn ${active ? "is-active" : ""} ${rest.className ?? ""}`}
    >
      <Icon name={icon} size={16} />
    </button>
  );
}

/* ------------------------------------------------------------------ layout */

export function Panel({
  title,
  actions,
  children,
  className,
  flush,
}: {
  title?: ReactNode;
  actions?: ReactNode;
  children: ReactNode;
  className?: string;
  flush?: boolean;
}) {
  return (
    <section className={`panel ${className ?? ""}`}>
      {title || actions ? (
        <header className="panel-head">
          <h2>{title}</h2>
          {actions ? <div className="panel-actions">{actions}</div> : null}
        </header>
      ) : null}
      <div className={flush ? "panel-body is-flush" : "panel-body"}>{children}</div>
    </section>
  );
}

export function Field({
  label,
  value,
  mono,
  title,
}: {
  label: string;
  value: ReactNode;
  mono?: boolean;
  title?: string;
}) {
  return (
    <div className="field" title={title}>
      <span className="field-label">{label}</span>
      <span className={mono ? "field-value is-mono" : "field-value"}>{value}</span>
    </div>
  );
}

export function Empty({ children }: { children: ReactNode }) {
  return <div className="empty">{children}</div>;
}

export function Badge({
  children,
  tone = "muted",
}: {
  children: ReactNode;
  tone?: "muted" | "ok" | "warn" | "danger" | "info";
}) {
  return <span className={`badge badge-${tone}`}>{children}</span>;
}

export function Spinner({ size = 14 }: { size?: number }) {
  return <span className="spinner" style={{ width: size, height: size }} />;
}

/* ------------------------------------------------------------------ modal */

export function Modal({
  title,
  subtitle,
  onClose,
  children,
  footer,
  wide,
  xl,
}: {
  title: ReactNode;
  subtitle?: ReactNode;
  onClose?: () => void;
  children: ReactNode;
  footer?: ReactNode;
  wide?: boolean;
  xl?: boolean;
}) {
  useEffect(() => {
    if (!onClose) return;
    const handler = (event: KeyboardEvent) => {
      if (event.key === "Escape") onClose();
    };
    window.addEventListener("keydown", handler);
    return () => window.removeEventListener("keydown", handler);
  }, [onClose]);

  return (
    <div className="modal-overlay" onMouseDown={(event) => event.target === event.currentTarget && onClose?.()}>
      <div className={`modal ${xl ? "modal-xl" : wide ? "modal-wide" : ""}`} role="dialog" aria-modal="true">
        <header className="modal-head">
          <div>
            <h3>{title}</h3>
            {subtitle ? <p className="modal-subtitle">{subtitle}</p> : null}
          </div>
          {onClose ? (
            <button type="button" className="modal-close" onClick={onClose} aria-label="Close">
              <Icon name="close" size={15} />
            </button>
          ) : null}
        </header>
        <div className="modal-body">{children}</div>
        {footer ? <footer className="modal-foot">{footer}</footer> : null}
      </div>
    </div>
  );
}

/* ------------------------------------------------------------------ dropzone */

export function DropZone({
  icon = "upload",
  label,
  hint,
  onFiles,
}: {
  icon?: IconName;
  label: string;
  hint?: string;
  onFiles: (paths: string[]) => void;
}) {
  const [over, setOver] = useState(false);

  useEffect(() => {
    let unlisten: (() => void) | undefined;
    let cancelled = false;

    (async () => {
      try {
        const { getCurrentWebview } = await import("@tauri-apps/api/webview");
        const stop = await getCurrentWebview().onDragDropEvent((event) => {
          if (event.payload.type === "over") setOver(true);
          if (event.payload.type === "drop") {
            setOver(false);
            const paths = event.payload.paths ?? [];
            if (paths.length) onFiles(paths);
          }
          if (event.payload.type === "leave") setOver(false);
        });
        if (cancelled) stop();
        else unlisten = stop;
      } catch {
        /* drag & drop unavailable — the browse button still works */
      }
    })();

    return () => {
      cancelled = true;
      unlisten?.();
    };
  }, [onFiles]);

  return (
    <div className={`dropzone ${over ? "is-over" : ""}`}>
      <Icon name={icon} size={22} />
      <span>{label}</span>
      {hint ? <small>{hint}</small> : null}
    </div>
  );
}

/* ------------------------------------------------------------------ ui host */

interface Toast {
  id: number;
  message: string;
  kind: MessageKind | "ok";
}

interface AlertState {
  message: string;
  kind: MessageKind;
  resolve: () => void;
}

interface ConfirmState {
  message: string;
  title?: string;
  resolve: (value: boolean) => void;
}

export interface UiApi {
  alert: (message: string, kind?: MessageKind) => Promise<void>;
  confirm: (message: string, title?: string) => Promise<boolean>;
  notify: (message: string, kind?: MessageKind | "ok") => void;
}

const UiContext = createContext<UiApi>({
  alert: async () => {},
  confirm: async () => false,
  notify: () => {},
});

let toastId = 0;

export function UiProvider({ children }: { children: ReactNode }) {
  const [alertState, setAlertState] = useState<AlertState | null>(null);
  const [confirmState, setConfirmState] = useState<ConfirmState | null>(null);
  const [toasts, setToasts] = useState<Toast[]>([]);
  // Kept in refs so a second alert/confirm can settle the one it replaces:
  // otherwise the first caller's `await` never resolves and its flow hangs.
  const alertRef = useRef<AlertState | null>(null);
  const confirmRef = useRef<ConfirmState | null>(null);

  const dismissAlert = useCallback(() => {
    alertRef.current = null;
    setAlertState(null);
  }, []);

  const settleConfirm = useCallback((value: boolean) => {
    confirmRef.current?.resolve(value);
    confirmRef.current = null;
    setConfirmState(null);
  }, []);

  const alert = useCallback((message: string, kind: MessageKind = "error") => {
    return new Promise<void>((resolve) => {
      alertRef.current?.resolve();
      const next = { message, kind, resolve };
      alertRef.current = next;
      setAlertState(next);
    });
  }, []);

  const confirm = useCallback((message: string, title?: string) => {
    return new Promise<boolean>((resolve) => {
      confirmRef.current?.resolve(false);
      const next = { message, title, resolve };
      confirmRef.current = next;
      setConfirmState(next);
    });
  }, []);

  const notify = useCallback((message: string, kind: MessageKind | "ok" = "info") => {
    const id = ++toastId;
    setToasts((current) => [...current, { id, message, kind }]);
    window.setTimeout(() => {
      setToasts((current) => current.filter((toast) => toast.id !== id));
    }, 4500);
  }, []);

  const api = useMemo(() => ({ alert, confirm, notify }), [alert, confirm, notify]);

  return (
    <UiContext.Provider value={api}>
      {children}

      {alertState ? (
        <Modal
          title={alertState.kind === "info" ? "Info" : alertState.kind === "warning" ? "Warning" : "Error"}
          onClose={() => {
            alertState.resolve();
            dismissAlert();
          }}
          footer={
            <Btn
              variant="primary"
              onClick={() => {
                alertState.resolve();
                dismissAlert();
              }}
            >
              OK
            </Btn>
          }
        >
          <p className={`message is-${alertState.kind}`}>{alertState.message}</p>
        </Modal>
      ) : null}

      {confirmState ? (
        <Modal
          title={confirmState.title ?? "Confirm"}
          onClose={() => settleConfirm(false)}
          footer={
            <>
              <Btn onClick={() => settleConfirm(false)}>
                Cancel
              </Btn>
              <Btn variant="primary" onClick={() => settleConfirm(true)}>
                OK
              </Btn>
            </>
          }
        >
          <p className="message">{confirmState.message}</p>
        </Modal>
      ) : null}

      <div className="toast-stack">
        {toasts.map((toast) => (
          <div key={toast.id} className={`toast toast-${toast.kind}`}>
            {toast.message}
          </div>
        ))}
      </div>
    </UiContext.Provider>
  );
}

export function useUi(): UiApi {
  return useContext(UiContext);
}

/* ------------------------------------------------------------------ misc */

export function useOutsideClick<T extends HTMLElement>(onOutside: () => void) {
  const ref = useRef<T | null>(null);

  useEffect(() => {
    const handler = (event: MouseEvent) => {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        onOutside();
      }
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, [onOutside]);

  return ref;
}

export function Dropdown({
  label,
  icon,
  children,
  align = "left",
}: {
  label: string;
  icon?: IconName;
  children: ReactNode;
  align?: "left" | "right";
}) {
  const [open, setOpen] = useState(false);
  const ref = useOutsideClick<HTMLDivElement>(() => setOpen(false));

  return (
    <div className="dropdown" ref={ref}>
      <button type="button" className={`dropdown-trigger ${open ? "is-open" : ""}`} onClick={() => setOpen((v) => !v)}>
        {icon ? <Icon name={icon} size={14} /> : null}
        <span>{label}</span>
        <Icon name="chevron" size={12} />
      </button>
      {open ? (
        <div className={`dropdown-menu ${align === "right" ? "is-right" : ""}`} onClick={() => setOpen(false)}>
          {children}
        </div>
      ) : null}
    </div>
  );
}

export function MenuItem({
  children,
  onClick,
  icon,
  disabled,
}: {
  children: ReactNode;
  onClick: () => void;
  icon?: IconName;
  disabled?: boolean;
}) {
  return (
    <button type="button" className="menu-item" onClick={onClick} disabled={disabled}>
      {icon ? <Icon name={icon} size={14} /> : null}
      <span>{children}</span>
    </button>
  );
}
