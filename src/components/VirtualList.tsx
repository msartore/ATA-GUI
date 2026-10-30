import { useEffect, useRef, useState, type ReactNode } from "react";

/**
 * Fixed-row-height windowing for long lists (an Android device easily exposes
 * 800+ packages). Only the visible slice plus a small overscan is rendered, so
 * typing in the search box stays responsive.
 */
export function VirtualList<T>({
  items,
  rowHeight,
  className,
  renderRow,
  empty,
}: {
  items: T[];
  rowHeight: number;
  className?: string;
  renderRow: (item: T, index: number) => ReactNode;
  empty?: ReactNode;
}) {
  const ref = useRef<HTMLDivElement>(null);
  const [scrollTop, setScrollTop] = useState(0);
  const [height, setHeight] = useState(360);

  useEffect(() => {
    const element = ref.current;
    if (!element) return;
    const observer = new ResizeObserver(() => setHeight(element.clientHeight));
    observer.observe(element);
    setHeight(element.clientHeight);
    return () => observer.disconnect();
  }, []);

  const overscan = 8;
  const start = Math.max(0, Math.floor(scrollTop / rowHeight) - overscan);
  const end = Math.min(items.length, Math.ceil((scrollTop + height) / rowHeight) + overscan);

  return (
    <div
      ref={ref}
      className={className}
      onScroll={(event) => setScrollTop(event.currentTarget.scrollTop)}
      style={{ overflowY: "auto" }}
    >
      {items.length === 0 ? empty : null}
      <div style={{ height: start * rowHeight }} />
      {items.slice(start, end).map((item, index) => renderRow(item, start + index))}
      <div style={{ height: Math.max(0, (items.length - end) * rowHeight) }} />
    </div>
  );
}
