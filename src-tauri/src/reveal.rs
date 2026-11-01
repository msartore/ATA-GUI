//! Reveal a file or folder in the OS file manager.
//!
//! Used by the extraction snackbar: after an APK set or a partition image is
//! written, the user gets one button that opens the containing folder with the
//! file already selected where the platform allows it.

use std::path::{Path, PathBuf};

/// Program and arguments that show `target` in the native file manager.
///
/// Kept separate from spawning so the argument shapes can be unit-tested
/// without opening windows during the test run.
pub fn plan(target: &Path) -> (&'static str, Vec<String>) {
    let shown = target.display().to_string();
    let is_dir = target.is_dir();

    #[cfg(target_os = "windows")]
    {
        // explorer takes /select,<path> as one argument; a bare directory is
        // opened directly instead of highlighting it in its parent.
        if is_dir {
            ("explorer", vec![shown])
        } else {
            ("explorer", vec![format!("/select,{shown}")])
        }
    }

    #[cfg(target_os = "macos")]
    {
        if is_dir {
            ("open", vec![shown])
        } else {
            ("open", vec!["-R".to_string(), shown])
        }
    }

    #[cfg(all(unix, not(target_os = "macos")))]
    {
        // xdg-open has no "select" verb: open the folder that holds the file.
        let folder = if is_dir {
            shown
        } else {
            target
                .parent()
                .map(|parent| parent.display().to_string())
                .unwrap_or(shown)
        };
        ("xdg-open", vec![folder])
    }
}

pub fn show(path: &str) -> Result<(), String> {
    let target = PathBuf::from(path);
    if !target.exists() {
        return Err(format!("path does not exist: {}", target.display()));
    }

    let (program, args) = plan(&target);
    let mut command = std::process::Command::new(program);
    command.args(args);

    #[cfg(target_os = "windows")]
    {
        use std::os::windows::process::CommandExt;
        // No console window flashes for a child of a GUI process.
        const CREATE_NO_WINDOW: u32 = 0x0800_0000;
        command.creation_flags(CREATE_NO_WINDOW);
    }

    command
        .spawn()
        .map(|_| ())
        .map_err(|error| format!("{program}: {error}"))
}

#[cfg(test)]
mod tests {
    use super::plan;

    #[test]
    fn plans_a_file_and_a_folder() {
        let dir = std::env::temp_dir();
        let file = dir.join("ata-gui-reveal-test.txt");
        std::fs::write(&file, b"x").unwrap();

        let (program, args) = plan(&file);
        assert!(!program.is_empty());
        assert_eq!(args.len(), if cfg!(target_os = "macos") { 2 } else { 1 });

        #[cfg(target_os = "windows")]
        {
            assert_eq!(program, "explorer");
            assert!(args[0].starts_with("/select,"));
            assert!(args[0].ends_with("ata-gui-reveal-test.txt"));

            let (_, folder_args) = plan(&dir);
            assert_eq!(folder_args.len(), 1);
            assert!(!folder_args[0].starts_with("/select,"));
        }

        std::fs::remove_file(&file).ok();
    }

    #[test]
    fn rejects_a_missing_path() {
        assert!(super::show("C:/definitely/not/here/ata-gui.img").is_err());
    }
}
