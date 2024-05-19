# ATA GUI

<img src="docs/resources/banner.png">

![Codacy grade](https://img.shields.io/codacy/grade/27d499575ce84ce683866adf5645e9b5?cacheSeconds=3600) ![GitHub](https://img.shields.io/github/license/msartore/ATA-GUI?cacheSeconds=3600) ![GitHub all releases](https://img.shields.io/github/downloads/msartore/ATA-GUI/total?cacheSeconds=3600) ![GitHub release (latest by date)](https://img.shields.io/github/v/release/msartore/ATA-GUI?cacheSeconds=3600) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/msartore/ATA-GUI/dotnet-desktop.yml?cacheSeconds=3600) ![Latest release downloads](https://img.shields.io/github/downloads/msartore/ATA-GUI/latest/total?label=downloads%20on%20latest&cacheSeconds=3600)



## 💡Introduction
ATA-GUI is an app that lets you perform advanced tasks on your Android™ device with ease. You can use ATA-GUI to access and modify your device's system settings, files, and features. You can also use ATA-GUI to install, uninstall, and restore apps on your device. ATA-GUI is powered by SDK Platform Tools, which are a set of command-line tools that allow you to communicate with your device and execute various operations. ATA-GUI is the app for advanced Android™ users who want to have more control over their device.

## ✨Features
* Uninstall, data cleaning and disable/enable system/user applications without root
* Install applications, Uploading files
* Check for granted permissions to an application
* ADB over network with easy and fast pairing system
* Bloatware Detector
* Reboot into Recovery, Fastboot, and System
* Image Flash and Extraction
* Erase user data and cache
* Screen and Camera mirroring
* Images extraction
* UNLOCK BOOTLOADER (Not all device supported)
* LOCK BOOTLOADER (Not all device supported)
* Device info extration in system and fastboot
* Sideload a zip
* Command injection
* Text injection
* Screen Share
* Logcat
* Task manager
* Grant WRITE_SECURE_SETTINGS, DUMP permissions
* OEM device-id
* Support to Multiple user device

## ATA License

    Copyright (C) 2021-2024 Massimiliano Sartore

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
    
## ❓What is Scrcpy?

[Scrcpy](https://github.com/Genymobile/scrcpy) was created by the team behind the popular Android emulator Genymotion, but it is not an Android emulator itself, it displays and controls Android devices connected via USB or TCP/IP, it does not require any root access.

Scrcpy works by running a server on your Android device, and the desktop application communicates using USB (or using ADB tunneling wireless). The server streams the H.264 video of the device screen. The client decodes the video frames and displays them. The client captures input (keyboard and mouse) events, sends them to the server, and the server injects them into the device. [The documentation](https://github.com/Genymobile/scrcpy/blob/master/DEVELOP.md) provides more details.

If you want to see your Android screen interact with the app or content on your desktop, record your phone screen or perform other basic tasks, then Scrcpy is a good choice.

In short, Scrcpy is an excellent way to easily view your Android screen on your computer and interact with it in real time.

## Scrcpy License

    Copyright (C) 2018 Genymobile
    Copyright (C) 2018-2024 Romain Vimont

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.


## ❓What is SDK Platform Tools?

[Android™ SDK Platform-Tools](https://developer.android.com/studio/releases/platform-tools.html) is a component for the Android™ SDK. It includes tools that interface with the Android platform, such as adb, fastboot, and systrace. These tools are required for Android app development. They're also needed if you want to unlock your device bootloader and flash it with a new system image.

Although some new features in these tools are available only for recent versions of Android, the tools are backward compatible, so you need only one version of the SDK Platform-Tools.

## SDK Platform Tools License

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
    
    Android is a trademark of Google LLC.

    Copyright (C) 2016-2019 Google LLC	https://developer.android.com/license

