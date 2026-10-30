// Never attach a console window, in release *and* debug builds: the UI is the
// only surface, and child processes are already spawned with CREATE_NO_WINDOW.
#![windows_subsystem = "windows"]

fn main() {
    ata_gui_lib::run()
}
