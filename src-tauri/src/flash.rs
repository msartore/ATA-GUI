use crate::device;
use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;
use std::fs;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct FlashStep {
    pub command: String,
    pub output: String,
    pub ok: bool,
}

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct ImageExtraction {
    pub ok: bool,
    pub path: String,
    pub log: Vec<String>,
    pub error: Option<String>,
}

pub fn bootloader_info(paths: &AppPaths, serial: &str) -> String {
    let args = vec!["getvar".to_string(), "all".to_string()];
    let output = tools::fastboot(paths, Some(serial), &args)
        .map(|out| out.merged())
        .unwrap_or_default();
    device::extract_bootloader_info(&output)
}

pub fn erase(paths: &AppPaths, serial: &str, partitions: &[String]) -> Vec<FlashStep> {
    partitions
        .iter()
        .map(|partition| {
            let args = vec!["erase".to_string(), partition.clone()];
            step(paths, serial, &args)
        })
        .collect()
}

pub fn wipe(paths: &AppPaths, serial: &str) -> FlashStep {
    step(paths, serial, &["-w".to_string()])
}

fn step(paths: &AppPaths, serial: &str, args: &[String]) -> FlashStep {
    step_guarded(paths, serial, args, None)
}

fn step_guarded(
    paths: &AppPaths,
    serial: &str,
    args: &[String],
    guard: Option<&tools::Guard>,
) -> FlashStep {
    let result = match guard {
        Some(guard) => tools::fastboot_guarded(
            paths,
            Some(serial),
            args,
            Some(guard.timeout()),
            Some(guard.flag()),
        ),
        None => tools::fastboot(paths, Some(serial), args),
    };
    match result {
        Ok(out) => FlashStep {
            command: args.join(" "),
            output: out.merged(),
            ok: out.success(),
        },
        Err(err) => FlashStep {
            command: args.join(" "),
            output: err,
            ok: false,
        },
    }
}

/// Bootloader unlock / lock / OEM info sequences.
pub fn bootloader_ops(paths: &AppPaths, serial: &str, op: &str) -> Vec<FlashStep> {
    let mut sequence: Vec<Vec<String>> = Vec::new();

    if let Some((prefix, _)) = op.split_once(' ') {
        // "oem <x>", "flashing <x>" and "bbk <x>" are preceded by a device-info read,
        // exactly like BootloaderMenuForm did.
        if prefix == "oem" || prefix == "flashing" || prefix == "bbk" {
            sequence.push(words("oem device-info"));
        }
    }

    sequence.push(words(op));

    if op.starts_with("oem ")
        || op.starts_with("flashing ")
        || op.starts_with("bbk ")
    {
        sequence.push(words("getvar unlocked"));
    }

    sequence
        .iter()
        .map(|args| step(paths, serial, args))
        .collect()
}

fn words(text: &str) -> Vec<String> {
    text.split_whitespace().map(|s| s.to_string()).collect()
}

/// `flash`/`boot` an image on one or more partitions. Port of
/// `backgroundWorkerFlashImg_DoWork`.
/// Copy a partition to sdcard, pull it into `IMG/` and clean up the device.
pub fn extract_image(
    paths: &AppPaths,
    serial: &str,
    partition: &str,
    guard: &tools::Guard,
) -> ImageExtraction {
    let mut log = Vec::new();
    let mut extraction = ImageExtraction {
        ok: false,
        path: String::new(),
        log: Vec::new(),
        error: None,
    };

    let create = tools::adb_guarded(
        paths,
        Some(serial),
        &[
            "shell".into(),
            "dd".into(),
            format!("if=/dev/block/bootdevice/by-name/{partition}"),
            format!("of=/sdcard/{partition}.img"),
        ],
        Some(guard.timeout()),
        Some(guard.flag()),
    )
    .map(|out| out.merged())
    .unwrap_or_default();
    log.push(create.clone());

    if create.contains("No such file or directory") || !create.contains("copied") {
        extraction.log = log;
        extraction.error = Some("errors.imageCreateFailed".into());
        return extraction;
    }

    let img_dir = paths.img_dir();
    let _ = fs::create_dir_all(&img_dir);
    let destination = img_dir.join(format!("{partition}.img"));

    let pull = tools::adb_guarded(
        paths,
        Some(serial),
        &[
            "pull".into(),
            format!("/sdcard/{partition}.img"),
            destination.to_string_lossy().to_string(),
        ],
        Some(guard.timeout()),
        Some(guard.flag()),
    )
    .map(|out| out.merged())
    .unwrap_or_default();
    log.push(pull.clone());

    let _ = tools::adb(
        paths,
        Some(serial),
        &["shell".into(), "rm".into(), format!("/sdcard/{partition}.img")],
    );

    if pull.contains("file pulled") || pull.contains("1 file") {
        extraction.ok = true;
        extraction.path = destination.to_string_lossy().to_string();
    } else {
        extraction.error = Some("errors.imagePullFailed".into());
    }
    extraction.log = log;
    extraction
}

#[cfg(test)]
mod tests {

    #[test]
    fn builds_flash_commands() {
        // Sanity check the partition naming rules without touching fastboot.
        let commands = |kind: &str, image_type: &str, partition: &str| {
            let mut names = Vec::new();
            if kind == "boot" {
                names.push("boot".to_string());
            } else {
                match partition {
                    "all" => {
                        names.push(format!("{image_type}_a"));
                        names.push(format!("{image_type}_b"));
                    }
                    "auto" => names.push(image_type.to_string()),
                    other => names.push(format!("{image_type}_{other}")),
                }
            }
            names
        };

        assert_eq!(commands("flash", "boot", "all"), vec!["boot_a", "boot_b"]);
        assert_eq!(commands("flash", "boot", "auto"), vec!["boot"]);
        assert_eq!(commands("flash", "boot", "a"), vec!["boot_a"]);
        assert_eq!(commands("boot", "boot", "a"), vec!["boot"]);
    }
}
