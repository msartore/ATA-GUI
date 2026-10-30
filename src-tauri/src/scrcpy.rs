use crate::state::AppPaths;
use crate::tools;
use serde::Serialize;
use std::path::PathBuf;
use std::process::Command;

#[derive(Serialize, Clone, Debug)]
#[serde(rename_all = "camelCase")]
pub struct EncoderData {
    pub codec: String,
    pub encoder: String,
}

#[derive(Serialize, Clone, Debug, Default)]
#[serde(rename_all = "camelCase")]
pub struct ScrcpyMedia {
    pub video: Vec<EncoderData>,
    pub audio: Vec<EncoderData>,
}

pub fn executable(paths: &AppPaths) -> Option<PathBuf> {
    tools::find_tool(paths, "scrcpy")
}

pub fn version(paths: &AppPaths) -> Result<String, String> {
    let exe = executable(paths).ok_or("errors.scrcpyMissing")?;
    let out = tools::run(&exe, &["--version".to_string()])?;
    Ok(out.merged())
}

pub fn encoders(paths: &AppPaths, serial: &str) -> Result<ScrcpyMedia, String> {
    let exe = executable(paths).ok_or("errors.scrcpyMissing")?;
    let out = tools::run(
        &exe,
        &[
            "-s".to_string(),
            serial.to_string(),
            "--list-encoders".to_string(),
        ],
    )?;
    let text = out.merged();
    Ok(ScrcpyMedia {
        video: parse_encoders(&text, true),
        audio: parse_encoders(&text, false),
    })
}

/// Port of `EncoderData.ExtractEncoders`.
pub fn parse_encoders(text: &str, video: bool) -> Vec<EncoderData> {
    let section = if video {
        "List of video encoders:"
    } else {
        "List of audio encoders:"
    };
    let prefix = if video {
        "--video-codec"
    } else {
        "--audio-codec"
    };

    let mut in_section = false;
    let mut encoders = Vec::new();

    for line in text.lines() {
        if line.contains(section) {
            in_section = true;
            continue;
        }
        if line.contains("[server] INFO: List of") && !line.contains(section) {
            in_section = false;
            continue;
        }
        if in_section && line.trim().starts_with(prefix) {
            let parts: Vec<&str> = line.split_whitespace().collect();
            if parts.len() < 2 {
                continue;
            }
            let codec = parts[0].split('=').nth(1).unwrap_or("").to_string();
            let encoder = parts[1]
                .split('=')
                .nth(1)
                .unwrap_or("")
                .trim_matches('\'')
                .trim_matches('"')
                .to_string();
            if !codec.is_empty() && !encoder.is_empty() {
                encoders.push(EncoderData { codec, encoder });
            }
        }
    }

    encoders
}

pub fn run(paths: &AppPaths, serial: &str, args: &[String]) -> Result<(), String> {
    let exe = executable(paths).ok_or("errors.scrcpyMissing")?;
    let mut cmd = Command::new(exe);
    if !serial.is_empty() {
        cmd.arg("-s").arg(serial);
    }
    cmd.args(args);
    tools::spawn_detached(cmd)
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = "\
[server] INFO: List of video encoders:
    --video-codec=h264 --video-encoder='OMX.google.h264.encoder'
    --video-codec=h265 --video-encoder='c2.qti.hevc.encoder'
[server] INFO: List of audio encoders:
    --audio-codec=opus --audio-encoder='c2.android.opus.encoder'
";

    #[test]
    fn extracts_video_encoders() {
        let encoders = parse_encoders(SAMPLE, true);
        assert_eq!(encoders.len(), 2);
        assert_eq!(encoders[0].codec, "h264");
        assert_eq!(encoders[0].encoder, "OMX.google.h264.encoder");
        assert_eq!(encoders[1].codec, "h265");
    }

    #[test]
    fn extracts_audio_encoders() {
        let encoders = parse_encoders(SAMPLE, false);
        assert_eq!(encoders.len(), 1);
        assert_eq!(encoders[0].codec, "opus");
        assert_eq!(encoders[0].encoder, "c2.android.opus.encoder");
    }
}
