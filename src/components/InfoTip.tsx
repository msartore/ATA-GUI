import { useRef, useState } from "react";
import { createPortal } from "react-dom";

/**
 * A "?" badge that explains something on hover/focus.
 *
 * The popover is rendered into <body> and positioned from the badge's rect so
 * panel overflow, transforms or scroll containers cannot clip it.
 */
export function InfoTip({ label, children }: { label: string; children: React.ReactNode }) {
  const anchor = useRef<HTMLSpanElement | null>(null);
  const [position, setPosition] = useState<{ top: number; left: number } | null>(null);

  const show = () => {
    const rect = anchor.current?.getBoundingClientRect();
    if (!rect) return;
    const width = 244;
    const left = Math.max(8, Math.min(rect.left, window.innerWidth - width - 12));
    setPosition({ top: rect.bottom + 8, left });
  };

  const hide = () => setPosition(null);

  return (
    <>
      <span
        ref={anchor}
        className="info-tip"
        role="button"
        tabIndex={0}
        aria-label={label}
        onMouseEnter={show}
        onMouseLeave={hide}
        onFocus={show}
        onBlur={hide}
      >
        ?
      </span>
      {position
        ? createPortal(
            <div
              className="info-pop"
              role="tooltip"
              style={{ top: position.top, left: position.left }}
            >
              {children}
            </div>,
            document.body,
          )
        : null}
    </>
  );
}
