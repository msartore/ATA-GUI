@echo off
TITLE ATA-GUI Updater
taskkill /f /im "ATA-GUI.exe"
del "ATA-GUI.exe"
del ATAUpdate.zip
cd ATAUpdate
del ATAUpdater.bat
move /y * ../
cd ..
rmdir ATAUpdate
start "" "ATA-GUI.exe"
exit