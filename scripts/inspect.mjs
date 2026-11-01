#!/usr/bin/env node
/**
 * Minimal Chrome DevTools Protocol driver for the running ATA-GUI WebView2.
 *
 * Usage (the app must be started with
 *   WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS="--remote-debugging-port=9222"
 * on Windows or
 *   WEBKIT_INSPECTOR_SERVER=127.0.0.1:9222
 * on Linux, then open the inspector endpoint):
 *
 *   node scripts/inspect.mjs text            # document.body.innerText
 *   node scripts/inspect.mjs html            # document.body.innerHTML
 *   node scripts/inspect.mjs eval "1+1"      # evaluate an expression
 *   node scripts/inspect.mjs shot out.png    # screenshot
 *   node scripts/inspect.mjs errors          # console errors for 2 seconds
 */
import fs from "node:fs";

const ENDPOINT = process.env.CDP_ENDPOINT ?? "http://127.0.0.1:9222";

async function targetUrl() {
  const list = await fetch(`${ENDPOINT}/json/list`).then((response) => response.json());
  const pages = list.filter((entry) => entry.type === "page");
  const match = process.env.CDP_MATCH;
  const pick = match?.startsWith("re:")
    ? (entry) => new RegExp(match.slice(3)).test(entry.url)
    : (entry) => entry.url.includes(match ?? "");
  const page = match ? pages.find(pick) : pages[0];
  if (!page) throw new Error(`no page target found (${pages.length} pages, match=${match ?? "-"})`);
  return page.webSocketDebuggerUrl;
}

async function connect() {
  const url = await targetUrl();
  const socket = new WebSocket(url);
  const pending = new Map();
  const events = [];
  let counter = 0;

  socket.addEventListener("message", (event) => {
    const message = JSON.parse(event.data);
    if (message.id && pending.has(message.id)) {
      pending.get(message.id)(message);
      pending.delete(message.id);
    } else if (message.method) {
      events.push(message);
    }
  });

  await new Promise((resolve, reject) => {
    socket.addEventListener("open", resolve, { once: true });
    socket.addEventListener("error", reject, { once: true });
  });

  const send = (method, params = {}) =>
    new Promise((resolve) => {
      const id = ++counter;
      pending.set(id, resolve);
      socket.send(JSON.stringify({ id, method, params }));
    });

  return { socket, send, events };
}

async function evaluate(send, expression) {
  const response = await send("Runtime.evaluate", {
    expression,
    returnByValue: true,
    awaitPromise: true,
    userGesture: true,
  });
  if (response.result?.exceptionDetails) {
    throw new Error(JSON.stringify(response.result.exceptionDetails));
  }
  return response.result?.result?.value;
}

const [mode, argument] = process.argv.slice(2);
const { socket, send, events } = await connect();
await send("Runtime.enable");
await send("Log.enable");
await send("Page.enable");

try {
  switch (mode) {
    case "text":
      console.log(await evaluate(send, "document.body.innerText"));
      break;
    case "html":
      console.log(await evaluate(send, "document.body.innerHTML"));
      break;
    case "eval":
      console.log(JSON.stringify(await evaluate(send, argument), null, 2));
      break;
    case "shot": {
      // shot <path> [x,y,w,h] [scale] — clip a region and/or capture at 2x.
      const [file, clipArg, scaleArg] = (argument ?? "").split(" ");
      const options = { format: "png", captureBeyondViewport: false };
      if (clipArg) {
        const [x, y, width, height] = clipArg.split(",").map(Number);
        options.clip = { x, y, width, height, scale: Number(scaleArg) || 1 };
      }
      const result = await send("Page.captureScreenshot", options);
      fs.writeFileSync(file || "shot.png", Buffer.from(result.result.data, "base64"));
      console.log(`saved ${file || "shot.png"}`);
      break;
    }
    case "shotfull": {
      // shotfull <path> — capture the whole scrollable page.
      const metrics = await send("Page.getLayoutMetrics");
      const size = metrics.result.cssContentSize || metrics.result.contentSize;
      const result = await send("Page.captureScreenshot", {
        format: "png",
        captureBeyondViewport: true,
        clip: { x: 0, y: 0, width: size.width, height: size.height, scale: 1 },
      });
      fs.writeFileSync(argument ?? "shot-full.png", Buffer.from(result.result.data, "base64"));
      console.log(`saved ${argument ?? "shot-full.png"} (${Math.round(size.width)}x${Math.round(size.height)})`);
      break;
    }
    case "size": {
      if (argument === "reset") {
        await send("Emulation.clearDeviceMetricsOverride");
        console.log("viewport reset");
        break;
      }
      const [w, h] = (argument ?? "").split("x").map(Number);
      if (!w || !h) {
        console.log("usage: inspect.mjs size <width>x<height> | size reset");
        break;
      }
      await send("Emulation.setDeviceMetricsOverride", {
        width: w,
        height: h,
        deviceScaleFactor: 1,
        mobile: false,
      });
      console.log(`viewport ${w}x${h}`);
      break;
    }
    case "errors": {
      await new Promise((resolve) => setTimeout(resolve, 2000));
      const problems = events.filter(
        (event) =>
          event.method === "Runtime.exceptionThrown" ||
          (event.method === "Runtime.consoleAPICalled" && event.params.type === "error") ||
          (event.method === "Log.entryAdded" && event.params.entry.level === "error"),
      );
      console.log(problems.length ? JSON.stringify(problems, null, 2) : "no console errors");
      break;
    }
    default:
      console.log("usage: inspect.mjs text|html|eval <expr>|shot <file>|size <w>x<h>|errors");
  }
} finally {
  socket.close();
}
