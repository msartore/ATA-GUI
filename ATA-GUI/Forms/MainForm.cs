using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATA_GUI.Classes;
using ATA_GUI.Utils;
using Ionic.Zip;
using Newtonsoft.Json.Linq;

namespace ATA_GUI
{
    public partial class MainForm : Form
    {
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private readonly ATA ata = new();
        private readonly Device device = new();

        public static readonly List<string> arrayApks = new();
        private static readonly int WM_NCLBUTTONDOWN = 0xA1;
        private static readonly int HT_CAPTION = 0x2;
        private static readonly Regex regex = new(@"\s+");

        public static string RemoveWhiteSpaces(string str)
        {
            return regex.Replace(str, string.Empty);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void panelTopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _ = ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

                if (ata.IsMaximize)
                {
                    ata.IsMaximize = false;
                    pictureBoxMaximize.Image = Properties.Resources.icons8_maximize_button_16;
                    Size = new Size(1126, 457);
                }
            }
        }

        private void buttonSyncApp_Click(object sender, EventArgs e)
        {
            switch (ata.CurrentTab)
            {
                case Tab.SYSTEM:
                    syncFun(3);
                    break;
                case Tab.FASTBOOT:
                    if (checkAdbFastboot(false))
                    {
                        string[] log = ConsoleProcess.adbFastbootCommandR(commandAssemblerF("getvar all"), 1).Split(' ', '\n');
                        for (int a = 0; a < log.Count(); a++)
                        {
                            if (log[a].Contains("partition-type:userdata:"))
                            {
                                labelUDT.Text = log[a]["partition-type:userdata:".Length..];
                            }
                            else if (log[a].Contains("partition-type:cache:"))
                            {
                                labelCDT.Text = log[a]["partition-type:cache:".Length..];
                            }
                            else if (log[a].Contains("unlocked:"))
                            {
                                labelBootloaderStatus.Text = log[a].Contains("yes") ? "Yes" : "No";
                            }
                        }
                        panelFastboot.Enabled = true;
                        LogWriteLine("Info extracted!");
                    }
                    else
                    {
                        panelFastboot.Enabled = false;
                    }
                    break;
                case Tab.RECOVERY:
                    if (ATA.CurrentDeviceSelected.sameMode(Tab.RECOVERY))
                    {
                        panelRecovery.Enabled = true;
                        groupBoxFlash.Enabled = ATA.CurrentDeviceSelected.Mode == DeviceMode.SIDELOAD;
                        groupBoxRecoveryRM.Enabled = ATA.CurrentDeviceSelected.Mode != DeviceMode.SIDELOAD;
                    }
                    else
                    {
                        panelRecovery.Enabled = false;
                    }
                    break;
                default:
                    MessageShowBox("Can not sync your device in this page", 1);
                    break;
            }
        }

        public static string commandAssembler(bool isAdb, string command)
        {
            return string.Format("{0} -s {1} {2}", isAdb ? "adb" : "fastboot", ATA.CurrentDeviceSelected.ID, command);
        }

        public static string commandAssemblerF(string command)
        {
            return string.Format("-s {0} {1}", ATA.CurrentDeviceSelected.ID, command);
        }

        private void syncFun(object paramObj)
        {
            try
            {
                textBoxSearch.Text = "Search";
                backgroundWorkerSync.RunWorkerAsync(paramObj);
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.Message, 0);
            }
        }

        private void buttonLogClear_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Clear();
        }

        private void LogWriteLine(string str)
        {
            Invoke(delegate
            {
                richTextBoxLog.Text += str + "\n";
                richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                richTextBoxLog.ScrollToCaret();
            });
        }

        /* Type 0 Error, Type 1 Warning, Type 2 Info */
        public static void MessageShowBox(string message, int type)
        {
            switch (type)
            {
                case 0:
                    _ = MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case 1:
                    _ = MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 2:
                    _ = MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    MessageShowBox("MessageShowBox generic error", 0);
                    break;
            }
        }

        private async Task updateCheckAsync()
        {
            try
            {
                _ = await ATA.CheckVersion((currentRelease, latestRelease, jsonReal) =>
                    {
                        Invoke(delegate
                        {
                            if (Utils.Version.CompareVersions(latestRelease, currentRelease) == Utils.Version.VersionComparisonResult.GreaterThan)
                            {
                                if (MessageBox.Show("New version found: " + latestRelease + "\nCurrent Version: " + ATA.CURRENTVERSION + "\n\nDo you want to update it?", "Update found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    ConsoleProcess.openLink((string)jsonReal[0]["html_url"]);
                                    jsonReal[0]["assets"][0].TryGetValue("browser_download_url", out JToken urlDownload);
                                    UpdateForm update = new(urlDownload.ToString());
                                    _ = update.ShowDialog();
                                }
                                else
                                {
                                    LogWriteLine("[WARNING] Your ATA is not up to date. It is recommended that you update it as soon as possible to ensure optimal performance and security");
                                }
                            }
                            else
                            {
                                LogWriteLine("ATA is up to date!");
                            }
                        });
                        return true;
                    }
                );
            }
            catch
            {
                LogWriteLine("[ERROR] A timeout error occurred while attempting to connect to the server. Please check your network connection and try again");
                LogWriteLine("Please open the settings menu to check if a new version of the software is available for download");
            }
        }

        private void backgroundWorkerSync_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object paramObj = e.Argument;
            string paramObjTmp = "0";

            Invoke(delegate
            {
                textBoxPort.Text = "5555";
            });

            if (paramObj.ToString() == "3")
            {
                paramObjTmp = "1";
            }
            if (checkAdbFastboot(true))
            {
                LogWriteLine("Checking device...");
                try
                {
                    if (ATA.CurrentDeviceSelected == null)
                    {
                        throw new NullReferenceException();
                    }

                    string version = ConsoleProcess.adbFastbootCommandR("-s " + Regex.Replace(ATA.CurrentDeviceSelected.ID, @"\s", "") + " shell getprop ro.build.version.release", 0);

                    if (version.Length == 0)
                    {
                        reloadList();
                        throw new Exception();
                    }

                    if (version.Any(char.IsDigit))
                    {
                        int count = 1;
                        if (paramObj.ToString() == "3")
                        {
                            count = 2;
                            paramObj = "0";
                        }
                        for (int a = 0; a < count; a++)
                        {
                            if (a == 1)
                            {
                                paramObj = "2";
                            }
                            switch (paramObj.ToString())
                            {
                                case "0":
                                    string[] arrayDeviceInfoCommands = { "-s "+ ATA.CurrentDeviceSelected.ID +" shell getprop ro.build.version.release", "-s "+ ATA.CurrentDeviceSelected.ID +" shell getprop ro.build.user",
                                        "-s "+ ATA.CurrentDeviceSelected.ID +" shell getprop ro.product.cpu.abilist", "-s "+ ATA.CurrentDeviceSelected.ID +" shell getprop ro.product.manufacturer" , "-s "+ ATA.CurrentDeviceSelected.ID +" shell getprop ro.product.model",
                                        "-s "+ ATA.CurrentDeviceSelected.ID +" shell getprop ro.product.board", "-s "+ ATA.CurrentDeviceSelected.ID +" shell getprop ro.product.device"};
                                    string deviceinfo = ConsoleProcess.adbFastbootCommandR(arrayDeviceInfoCommands, 0);
                                    string[] arrayDeviceInfo = deviceinfo.Split('\n');
                                    if (arrayDeviceInfo.Length > 6)
                                    {
                                        string localIp = ConsoleProcess.systemCommand("adb.exe -s " + ATA.CurrentDeviceSelected.ID + " shell ip route").Split(' ').Where(it => Regex.Match(it, "(\\b25[0-5]|\\b2[0-4][0-9]|\\b[01]?[0-9][0-9]?)(\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}").Success).Last();

                                        disableSystem(false);
                                        Invoke(delegate
                                        {
                                            toolStripLabelTotalApps.Text = "Total: 0";
                                            checkedListBoxApp.Items.Clear();
                                            LogWriteLine("device found!");
                                            labelAV.Text = arrayDeviceInfo[0];
                                            labelBU.Text = arrayDeviceInfo[1];
                                            labelCA.Text = arrayDeviceInfo[2];
                                            labelManu.Text = arrayDeviceInfo[3];
                                            labelModel.Text = arrayDeviceInfo[4];
                                            labelB.Text = arrayDeviceInfo[5];
                                            labelD.Text = arrayDeviceInfo[6];
                                            if (arrayDeviceInfo.Length > 6)
                                            {
                                                if (localIp.Length > 4)
                                                {
                                                    comboBoxIP.Text = labelIP.Text = localIp;
                                                }
                                                if (labelIP.Text.Contains("t of devices attached") || localIp.Length == 0)
                                                {
                                                    labelIP.Text = "Not connected to a network";
                                                    comboBoxIP.ResetText();
                                                    buttonConnectToIP.Enabled = false;
                                                    buttonDisconnectIP.Enabled = false;
                                                }
                                            }
                                            if (ATA.CurrentDeviceSelected.Connection == DeviceConnection.WIRELESS)
                                            {
                                                labelStatus.Text = "Wireless";
                                                buttonConnectToIP.Enabled = false;
                                                buttonDisconnectIP.Enabled = true;
                                                device.DeviceWireless = true;
                                            }
                                            else
                                            {
                                                labelStatus.Text = "Cable";
                                                buttonConnectToIP.Enabled = true;
                                                buttonDisconnectIP.Enabled = false;
                                                device.DeviceWireless = false;
                                            }
                                            if (int.Parse(labelAV.Text) > 7)
                                            {
                                                labelUser.Text = device.User = Regex.Replace(ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell am get-current-user", 0), @"\t|\n|\r", "");
                                            }
                                            else
                                            {
                                                MessageShowBox("Feature currently not avaiable for device under android 8", 2);
                                            }

                                            _ = int.TryParse(ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell cmd display get - displays", 0), out int maxDisplay);

                                            for (int i = maxDisplay; i > -1; i--)
                                            {
                                                _ = domainUpDownFreeRotation.Items.Add(i);
                                            }

                                            device.IsRotationFreeEnabled = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell wm get-ignore-orientation-request", 0).Contains("true");
                                            buttonSetRotation.Text = device.IsRotationFreeEnabled ? "Unset" : "Set";

                                            LogWriteLine("Device info extracted");
                                            disableEnableSystem(true);
                                        });
                                    }
                                    else
                                    {
                                        Invoke(delegate
                                        {
                                            disableEnableSystem(false);
                                            buttonDisconnectIP.Enabled = false;
                                        });
                                        LogWriteLine("[ERROR] failed to extract device info!");
                                    }
                                    break;
                                case "2":
                                    arrayApks.Clear();
                                    Invoke(delegate
                                    {
                                        labelSelectedAppCount.Text = "Selected App: 0";
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    string[] command;
                                    command = !device.SystemApp
                                        ? (new[] { "-s " + ATA.CurrentDeviceSelected.ID + " shell pm list packages -3 --user " + device.User })
                                        : (new[] { "-s " + ATA.CurrentDeviceSelected.ID + " shell pm list packages -s --user " + device.User });
                                    if ((device.StringApk = ConsoleProcess.adbFastbootCommandR(command, 0)) != null)
                                    {
                                        LogWriteLine("Loading apps...");
                                        sortApks(device.StringApk.Split('\n'));
                                        toolStripLabelTotalApps.Text = "Total: " + checkedListBoxApp.Items.Count;
                                        LogWriteLine("Apps loaded!");
                                    }
                                    else
                                    {
                                        MessageShowBox("Error during apk loading", 0);
                                    }
                                    break;
                                case "4":
                                    List<string> arrayApkTmp = new();
                                    arrayApks.Clear();
                                    Invoke(delegate
                                    {
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    LogWriteLine("Loading apps...");
                                    if ((device.StringApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell pm list packages -3 --user " + device.User, 0)) != null)
                                    {
                                        arrayApkTmp.AddRange(device.StringApk.Split('\n'));
                                    }
                                    else
                                    {
                                        MessageShowBox("Error during apk loading", 0);
                                        break;
                                    }
                                    if ((device.StringApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell pm list packages -s --user " + device.User, 0)) != null)
                                    {
                                        arrayApkTmp.AddRange(device.StringApk.Split('\n'));
                                        sortApks(arrayApkTmp.ToArray());
                                    }
                                    else
                                    {
                                        MessageShowBox("Error during apk loading", 0);
                                        break;
                                    }
                                    LogWriteLine("Apps loaded!");
                                    toolStripLabelTotalApps.Text = "Total: " + checkedListBoxApp.Items.Count;
                                    break;
                                case "5":
                                    string stringInstalledApk;

                                    arrayApks.Clear();
                                    Invoke(delegate
                                    {
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    LogWriteLine("Loading apps...");
                                    if ((device.StringApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell pm list packages -u --user " + device.User, 0)) != null)
                                    {
                                        if ((stringInstalledApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell pm list packages --user " + device.User, 0)) != null)
                                        {
                                            List<string> diff;
                                            IEnumerable<string> set1 = device.StringApk.Split('\n').Distinct();
                                            IEnumerable<string> set2 = stringInstalledApk.Split('\n').Distinct();

                                            diff = set2.Count() > set1.Count() ? set2.Except(set1).ToList() : set1.Except(set2).ToList();

                                            sortApks(diff.ToArray());
                                        }
                                        else
                                        {
                                            MessageShowBox("Error during apk loading", 0);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        MessageShowBox("Error during apk loading", 0);
                                        break;
                                    }
                                    LogWriteLine("Apps loaded!");
                                    toolStripLabelTotalApps.Text = "Total: " + checkedListBoxApp.Items.Count;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Invoke(delegate
                    {
                        disableEnableSystem(false);
                        buttonDisconnectIP.Enabled = false;
                        LogWriteLine("[ERROR] Device not found. If the issue persists, please check if the USB debugging option is enabled. For more information on how to solve this issue, please watch this video tutorial: https://www.youtube.com/watch?v=W7nkxS9LMXs");
                        if (paramObjTmp == "0")
                        {
                            MessageShowBox("Device not found!", 0);
                        }
                    });
                }
            }
            else
            {
                adbDownload();
            }
        }

        private void sortApks(string[] arrayApkTmp)
        {
            foreach (string line in arrayApkTmp)
            {
                if (line.Contains("package:"))
                {
                    arrayApks.Add(line[8..]);
                }
            }
            arrayApks.Sort();
            Invoke(delegate
            {
                checkedListBoxApp.Items.AddRange(arrayApks.ToArray());
                checkedListBoxApp.CheckOnClick = true;
            });
        }

        private void disableEnableSystem(bool enable)
        {
            textBoxPort.Enabled = !enable;
            buttonMobileScreenShare.Enabled = enable;
            groupBoxDeviceInfo.Enabled = enable;
            groupBoxRebootMenu.Enabled = enable;
            groupBoxAPKMenu.Enabled = enable;
            buttonDeviceLogs.Enabled = enable;
            buttonTaskManager.Enabled = enable;
            tabPageTools.Enabled = enable;
            groupBoxTerminal.Enabled = enable;
        }

        private void adbDownload()
        {
            try
            {
                backgroundWorkerAdbDownloader.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.Message, 0);
            }
        }

        private void textboxClick(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == "Search" && !ata.TextboxClear)
            {
                textBoxSearch.Clear();
                ata.TextboxClear = true;
            }
        }

        private void buttonRS_Click(object sender, EventArgs e)
        {
            if (!device.DeviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                rebootSmartphone();
            }
        }

        private void buttonRR_Click(object sender, EventArgs e)
        {
            if (!device.DeviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _ = ConsoleProcess.systemCommand(commandAssembler(true, "reboot recovery"));
                LogWriteLine("Rebooted!");
                reloadList();
                syncFun(3);
            }
        }

        private void buttonRF_Click(object sender, EventArgs e)
        {
            if (!device.DeviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _ = ConsoleProcess.systemCommand(commandAssembler(true, "reboot-bootloader"));
                LogWriteLine("Rebooted!");
                reloadList();
                syncFun(3);
            }
        }

        public void ToolTipGenerator(Control button, string title, string message)
        {
            ToolTip buttonToolTip = new()
            {
                ToolTipTitle = title,
                UseFading = true,
                UseAnimation = true,
                IsBalloon = true,
                ShowAlways = true,
                AutoPopDelay = 0,
                InitialDelay = 0,
                ReshowDelay = 0
            };
            buttonToolTip.SetToolTip(button, message);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            radioButtonBoot.Checked = true;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ToolTipGenerator(buttonConnectToIP, "Connect device", "Connect to your device with this IP");
            ToolTipGenerator(buttonDisconnectIP, "Disconnect device", "Disconnect from your device with this IP");
            ToolTipGenerator(buttonMobileScreenShare, "Share Device Screen", "Your device screen will be shared on your PC");
            ToolTipGenerator(buttonLogClear, "Clear log", "Log will be clean from the box");
            ToolTipGenerator(buttonRR, "Reboot to recovery", "Your device will be rebooted to recovery");
            ToolTipGenerator(buttonRS, "Reboot to system", "Your device will be rebooted to system");
            ToolTipGenerator(buttonRF, "Reboot to fastboot", "Your device will be rebooted to fastboot");
            ToolTipGenerator(buttonReloadDevicesList, "Reload devices", "The devices list will be updated");
            ToolTipGenerator(buttonSyncApp, "Sync smartphone", "All the info needed will be extracted from your smartphone");
            ToolTipGenerator(buttonRebootRecovery, "Reboot to recovery", "Your device will be rebooted to recovery");
            ToolTipGenerator(groupBox2, "File Transfer", "Drop a file in this box and it will be trasfered in a new folder called ATA inside your device");
            ToolTipGenerator(groupBox6, "Apk Installer", "Drop an apk file and it will be installed inside your device");

            if (File.Exists(ATA.IPFileName))
            {
                foreach (string ip in File.ReadAllLines(ATA.IPFileName))
                {
                    _ = ata.IPList.Add(ip.Trim());
                }
                comboBoxIP.Items.AddRange(ata.IPList.ToArray());
            }

            comboBoxDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCameraModes.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCameraModes.SelectedIndex = 0;
            groupBox6.AllowDrop = true;
            groupBox2.AllowDrop = true;
            Disclaimer disclaimer = new();
            if (!disclaimer.checkDiscalimer())
            {
                _ = disclaimer.ShowDialog();
            }
            panelFastboot.Enabled = false;
            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Normal;
            _ = Focus();
        }

        private void DevicesListUpdate()
        {
            deviceListExtractor(ata.CurrentTab != Tab.FASTBOOT);
        }

        private void deviceListExtractor(bool isAdb)
        {
            List<string> devicesRaw;

            if (checkAdbFastboot(isAdb))
            {
                LogWriteLine("Searching for devices...");

                string dev = ConsoleProcess.adbFastbootCommandR("devices", isAdb ? 0 : 1);

                if (dev != null)
                {
                    ata.Devices.Clear();
                    List<DeviceData> devices = new();
                    devicesRaw = dev.Split('\n').Where(it => it.Contains('\t')).ToList();

                    devicesRaw.ForEach(it =>
                    {
                        string id = it.Split('\t')[0];
                        string name = "";
                        DeviceConnection deviceConnection = DeviceConnection.CABLE;
                        DeviceData deviceData;

                        if (isAdb)
                        {
                            name = ConsoleProcess.adbFastbootCommandR("-s " + Regex.Replace(id, @"\s", "") + " shell getprop ro.product.model", 0);

                            if (Regex.Match(id, "(\\b25[0-5]|\\b2[0-4][0-9]|\\b[01]?[0-9][0-9]?)(\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}").Success)
                            {
                                deviceConnection = DeviceConnection.WIRELESS;
                            }
                        }

                        deviceData = new DeviceData(name, id, DeviceMode.UNKNOWN, deviceConnection);

                        if (it.Contains("recovery"))
                        {
                            deviceData.Mode = DeviceMode.RECOVERY;
                        }
                        else if (it.Contains("device"))
                        {
                            deviceData.Mode = DeviceMode.SYSTEM;
                        }
                        else if (it.Contains("unauthorized"))
                        {
                            deviceData.Mode = DeviceMode.UNAUTHORIZED;
                        }
                        else if (it.Contains("fastboot"))
                        {
                            deviceData.Mode = DeviceMode.FASTBOOT;
                        }
                        else if (it.Contains("sideload"))
                        {
                            deviceData.Mode = DeviceMode.SIDELOAD;
                        }

                        devices.Add(deviceData);
                    });

                    foreach (DeviceData device in devices.Where(iterator => iterator.sameMode(ata.CurrentTab)))
                    {
                        _ = comboBoxDevices.Items.Add(string.Format("{0} [{1}] [{2}]", device.Mode is DeviceMode.RECOVERY or DeviceMode.SYSTEM ? device.Name : device.ID, device.Mode.ToString(), device.getConnectionSymbol()));
                        ata.Devices.Add(device);
                    }

                    if (comboBoxDevices.Items.Count > 0)
                    {
                        comboBoxDevices.SelectedIndex = 0;
                        ATA.CurrentDeviceSelected = ata.Devices[0];
                    }

                    buttonSyncApp.Enabled = comboBoxDevices.Items.Count > 0;

                    switch (ata.Devices.Count)
                    {
                        case 0:
                            disableEnableSystem(false);
                            panelFastboot.Enabled = false;
                            panelRecovery.Enabled = false;
                            LogWriteLine("No device found");
                            break;
                        case 1:
                            LogWriteLine("One device has been found");
                            break;
                        default:
                            LogWriteLine("More than one device has been found");
                            break;
                    }
                }
                else
                {
                    MessageShowBox("Something went wrong with adb", 1);
                }
            }
        }

        public static bool pingCheck()
        {
            Ping myPing = new();
            PingReply reply;
            try
            {
                reply = myPing.Send(IPAddress.Parse("1.1.1.1"), 1000, new byte[32], new PingOptions());
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch
            {
                MessageShowBox("Error during connection check", 0);
            }
            return false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Feedback.checkFeedbackFile())
            {
                _ = new Feedback().ShowDialog();
            }
            LogWriteLine("Checking connection...");
            toolStripButtonRestoreApp.Enabled = false;

            if (!pingCheck())
            {
                LogWriteLine("[WARNING] You are offline");
                disableSystem(true);
            }
            else
            {
                ata.IsConnected = true;
            }

            LogWriteLine("Connection checked!");

            LogWriteLine("Starting adb...");

            DevicesListUpdate();
            syncFun(3);
            _ = updateCheckAsync();
        }

        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSelectAll.Checked)
            {
                for (int i = 0; i < checkedListBoxApp.Items.Count; i++)
                {
                    checkedListBoxApp.SetItemCheckState(i, CheckState.Checked);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBoxApp.Items.Count; i++)
                {
                    checkedListBoxApp.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
            updateAppCount();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxSearch.Text.ToLowerInvariant();

            checkedListBoxApp.Items.Clear();
            updateAppCount();
            checkBoxSelectAll.Checked = false;

            foreach (string str in arrayApks)
            {
                if (str.Contains(text))
                {
                    _ = checkedListBoxApp.Items.Add(str);
                }
            }
        }

        private void updateAppCount()
        {
            labelSelectedAppCount.Text = "Selected App: " + checkedListBoxApp.CheckedItems.Count;
        }

        private void buttonConnectToIP_Click(object sender, EventArgs e)
        {
            connectToIp();
        }

        private void connectToIp()
        {
            if (comboBoxIP.Text.Length > 0)
            {
                if (!backgroundWorkerADBConnect.IsBusy)
                {
                    LogWriteLine("Trying to connect to your device...");
                    backgroundWorkerADBConnect.RunWorkerAsync();
                }
                else
                {
                    MessageShowBox("Wait, process still running", 1);
                }
            }
            else
            {
                MessageShowBox("You have to enter an IP", 1);
            }
        }

        private void buttonDisconnectIP_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerADBDisconnect.IsBusy)
            {
                backgroundWorkerADBDisconnect.RunWorkerAsync();
            }
            else
            {
                MessageShowBox("Wait, process still running", 1);
            }
        }

        private void appFunc(string command1, string command2, int type)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                foreach (object current in checkedListBoxApp.CheckedItems)
                {
                    string log;
                    if ((log = ConsoleProcess.adbFastbootCommandR(" -s " + ATA.CurrentDeviceSelected.ID + " " + command1 + "--user " + device.User + " " + current + command2, 0)) != null)
                    {
                        if (type == 1)
                        {
                            ScrollableMessageBox.show(log, "Granted permissions");
                            continue;
                        }
                        LogWriteLine("The command has been injected");
                    }
                    else
                    {
                        LogWriteLine("[ERROR] The command injection has failed");
                    }
                }
            }
            else
            {
                MessageShowBox("No app selected", 1);
            }
        }

        private void buttonFlashZip_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerZip.IsBusy)
            {
                if (textBoxDirFile.Text.Contains(".zip"))
                {
                    backgroundWorkerZip.RunWorkerAsync();
                }
                else
                {
                    MessageShowBox("No zip selected", 1);
                }
            }
            else
            {
                MessageShowBox("Wait, process still running", 1);
            }
        }

        private void backgroundWorkerZip_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string fileName = textBoxDirFile.Text[(textBoxDirFile.Text.LastIndexOf('\\') + 1)..];
            LogWriteLine("Flashing " + fileName + "...");
            string log = ConsoleProcess.adbFastbootCommandR(commandAssemblerF(string.Format("sideload \"{0}\"", textBoxDirFile.Text)), 0);
            if (log.ToLower().Contains("error") || log.ToLower().Contains("failed") || log.Trim() == "")
            {
                LogWriteLine("[ERROR] " + fileName + " failed to flash, try restarting the sideload process or unplugging and replugging the device to resolve the issue.");
            }
            else
            {
                LogWriteLine(fileName + " flashed");
            }
        }

        private void buttonSearchFileFastboot_Click(object sender, EventArgs e)
        {
            openFileDialogZip.Filter = "Img files *.img|*.img;";
            openFileDialogZip.FileName = "";
            openFileDialogZip.Multiselect = false;
            openFileDialogZip.Title = "Select Img";

            DialogResult dr = openFileDialogZip.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxDirImg.Text = openFileDialogZip.FileName;
            }
        }

        private void flash(int index)
        {
            try
            {
                backgroundWorkerFlashImg.RunWorkerAsync(index);
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.ToString(), 0);
            }
        }

        private void backgroundWorkerFlashImg_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string command;
            string log;

            switch (sender.ToString())
            {
                case "0":
                    command = "flash boot ";
                    break;
                case "1":
                    command = "flash bootloader ";
                    break;
                case "2":
                    command = "flash cache ";
                    break;
                case "3":
                    command = "flash radio ";
                    break;
                case "4":
                    command = "flash recovery ";
                    break;
                case "5":
                    command = "-w && fastboot update ";
                    break;
                case "6":
                    command = "flash system ";
                    break;
                case "7":
                    command = "flash vendor ";
                    break;
                default:
                    return;
            }

            if ((log = ConsoleProcess.adbFastbootCommandR(commandAssemblerF(command + textBoxDirImg.Text), 1)) != null)
            {
                LogWriteLine(log);
            }
            else
            {
                MessageShowBox("Something went wrong!", 0);
            }
        }

        private bool checkAdbFastboot(bool isAdb)
        {
            string exeTmp = isAdb ? "adb.exe" : "fastboot.exe";
            bool exist = File.Exists(exeTmp) && File.Exists("AdbWinUsbApi.dll") && File.Exists("AdbWinApi.dll");
            return exist;
        }

        private void buttonRebootToSystem_Click(object sender, EventArgs e)
        {
            LogWriteLine(ConsoleProcess.adbFastbootCommandR(commandAssemblerF("reboot"), 1));
            Thread.Sleep(1000);
            reloadList();
        }

        private void buttonHardReset_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to erase your phone?", "Erase Phone", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (checkAdbFastboot(false))
                {
                    LogWriteLine("Erasing process started...");
                    _ = ConsoleProcess.systemCommand(commandAssembler(false, "fastboot erase userdata && fastboot erase cache")); ;
                    LogWriteLine("Erasing process finished!!");
                }
            }
        }

        private void rebootSmartphone()
        {
            LogWriteLine("Rebooting smartphone...");
            _ = ConsoleProcess.systemCommand("adb -s " + ATA.CurrentDeviceSelected.ID + " reboot");
            LogWriteLine("Smartphone rebooted");
        }

        private void buttonRebootRecovery_Click(object sender, EventArgs e)
        {
            LogWriteLine(ConsoleProcess.adbFastbootCommandR(commandAssemblerF("reboot recovery"), 1));
            Thread.Sleep(1000);
            reloadList();
        }

        private void buttonBootloaderMenu_Click(object sender, EventArgs e)
        {
            BootloaderMenu bootloaderMenu = new();
            _ = bootloaderMenu.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                syncFun(4);
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.Message, 1);
            }
        }

        public void uninstaller(CheckedListBox.CheckedItemCollection foundPackageList)
        {
            string command = "adb -s " + ATA.CurrentDeviceSelected.ID + " shell pm uninstall -k --user " + device.User + " ";
            LoadingForm load;
            List<string> arrayApkSelect = new();
            foreach (object list in foundPackageList)
            {
                arrayApkSelect.Add(list.ToString());
            }
            load = new LoadingForm(arrayApkSelect, command, "Uninstalled:");
            _ = load.ShowDialog();
            if (load.DialogResult != DialogResult.OK)
            {
                MessageShowBox("Error during uninstallation process", 0);
            }
            syncFun(4);
            checkBoxSelectAll.Checked = false;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                uninstaller(checkedListBoxApp.CheckedItems);
            }
            else
            {
                MessageShowBox("No app selected", 1);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                List<string> arrayApkSelect = new();
                foreach (object list in checkedListBoxApp.CheckedItems)
                {
                    arrayApkSelect.Add(list.ToString());
                }

                PackageMenu packageMenu = new(arrayApkSelect);
                _ = packageMenu.ShowDialog();
                string command;
                string commandName;
                switch (packageMenu.DialogResult1)
                {
                    case 1:
                        command = "adb -s " + ATA.CurrentDeviceSelected.ID + " shell pm enable --user " + device.User + " ";
                        commandName = "Enabled:";
                        break;
                    case 0:
                        command = "adb -s " + ATA.CurrentDeviceSelected.ID + " shell pm disable-user --user " + device.User + " ";
                        commandName = "Disabled:";
                        break;
                    case 2:
                        command = "adb -s " + ATA.CurrentDeviceSelected.ID + " shell pm clear --user " + device.User + " ";
                        commandName = "Cleared:";
                        break;
                    case -1:
                        LogWriteLine("Operations canceled");
                        return;
                    default:
                        MessageShowBox("Generic error", 0);
                        return;
                }
                loadMethod(arrayApkSelect, command, commandName);
            }
            else
            {
                MessageShowBox("No app selected", 1);
            }
        }

        public void loadMethod(List<string> arrayApkSelect, string command, string commandName)
        {
            LoadingForm load = new(arrayApkSelect, command, commandName);
            _ = load.ShowDialog();
            if (load.DialogResult == DialogResult.OK)
            {
                syncFun(2);
            }
            else
            {
                MessageShowBox("Error during this process", 0);
            }
        }

        private void nonSystemAppToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripButtonUninstallApp.Enabled = true;
            toolStripButtonRestoreApp.Enabled = false;
            toolStripButtonPermissionMenu.Enabled = true;
            toolStripButtonExtract.Enabled = true;
            toolStripButtonPackageManager.Enabled = true;
            toolStripButtonBloatwareDetecter.Enabled = true;
            device.SystemApp = false;
            syncFun(2);
        }

        private void systemAppToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripButtonUninstallApp.Enabled = true;
            toolStripButtonExtract.Enabled = true;
            toolStripButtonRestoreApp.Enabled = false;
            toolStripButtonPermissionMenu.Enabled = true;
            toolStripButtonPackageManager.Enabled = true;
            toolStripButtonBloatwareDetecter.Enabled = true;
            device.SystemApp = true;
            syncFun(2);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonUninstallApp.Enabled = true;
            toolStripButtonRestoreApp.Enabled = false;
            toolStripButtonExtract.Enabled = true;
            toolStripButtonPermissionMenu.Enabled = true;
            toolStripButtonPackageManager.Enabled = true;
            toolStripButtonBloatwareDetecter.Enabled = true;
            syncFun(4);
        }

        private void toolStripButtonFilter_Click(object sender, EventArgs e)
        {
            contextMenuStripFilterBy.Show(Cursor.Position);
        }

        private void textBoxSearch_Click(object sender, EventArgs e)
        {
            textBoxSearch.SelectAll();
        }

        private void toolStripButtonPermissionMenu_Click(object sender, EventArgs e)
        {
            contextMenuStripPermissionMenu.Show(Cursor.Position);
        }

        private void groupBox6_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                backgroundWorkerAPKinstall.RunWorkerAsync((string[])e.Data.GetData(DataFormats.FileDrop));
            }
        }

        private void groupBox6_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void checkedListBoxApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateAppCount();
        }

        private void checkGrantedPermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell dumpsys package --user " + device.User, null, 1);
        }

        private void groupBox2_DragDrop(object sender, DragEventArgs e)
        {
            backgroundWorkerFileTransfer.RunWorkerAsync(e);
        }

        private void groupBox2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void comboBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ATA.CurrentDeviceSelected = ata.Devices[comboBoxDevices.SelectedIndex];

            disableEnableSystem(false);
            panelFastboot.Enabled = false;
            panelRecovery.Enabled = false;
        }

        private void buttonReloadDevicesList_Click(object sender, EventArgs e)
        {
            reloadList();
        }

        private void reloadList()
        {
            Invoke(delegate
            {
                if (!checkAdbFastboot(true))
                {
                    adbDownload();
                }
                comboBoxDevices.Items.Clear();
                ata.Devices.Clear();
                DevicesListUpdate();
            });
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            Tab oldTabTmp = ata.CurrentTab;
            ata.setCurrentTab(tabControls.SelectedTab.Text);

            if (ata.CurrentTab != oldTabTmp)
            {
                ata.Devices.Clear();
                comboBoxDevices.Items.Clear();
                buttonDeviceLogs.Enabled = false;
                buttonTaskManager.Enabled = false;
                buttonMobileScreenShare.Enabled = false;
                disableEnableSystem(false);
            }

            if (ata.CurrentTab != Tab.SYSTEM)
            {
                buttonSyncApp.Enabled = false;
            }

            panelRecovery.Enabled = false;
            panelFastboot.Enabled = false;
        }

        private async void backgroundWorkerAdbDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!ata.IsConnected)
            {
                MessageShowBox("You are offline, ATA can not download ADB", 0);
                return;
            }
            Invoke(delegate
            {
                disableEnableSystem(false);
                buttonDisconnectIP.Enabled = false;
            });

            ExeMissingForm adbError = new("adb.exe not found\n\nDo you want to download sdk platform tool?\n\n[By pressing YES you agree sdk platform tool terms and conditions]\nfor more info press info button", "Error, ADB Missing!");
            _ = adbError.ShowDialog();

            switch (adbError.DialogResult)
            {
                case DialogResult.Yes:
                    LogWriteLine("Downloading sdk platform tool...");
                    using (HttpClient client = new())
                    {
                        try
                        {
                            string url = "https://dl.google.com/android/repository/platform-tools-latest-windows.zip";
                            string filePath = "sdkplatformtool.zip";
                            using (HttpResponseMessage response = await client.GetAsync(url))
                            {
                                _ = response.EnsureSuccessStatusCode();
                                using FileStream fs = new(filePath, FileMode.Create);
                                await response.Content.CopyToAsync(fs);
                            }
                            LogWriteLine("sdk platform tool downloaded!");
                            LogWriteLine("unzipping sdk platform tool...");
                            if (Directory.Exists("platform-tools"))
                            {
                                Directory.Delete("platform-tools", true);
                            }
                            using (ZipFile zip = ZipFile.Read("sdkplatformtool.zip"))
                            {
                                zip.ExtractAll(Path.GetDirectoryName(Application.ExecutablePath));
                            }
                            LogWriteLine("sdk platform tool extraced!");
                            LogWriteLine("Getting things ready...");
                            _ = ConsoleProcess.systemCommand("taskkill /f /im adb.exe");
                            _ = ConsoleProcess.systemCommand("taskkill /f /im fastboot.exe");
                            _ = ConsoleProcess.systemCommand("del adb.exe");
                            _ = ConsoleProcess.systemCommand("del AdbWinUsbApi.dll");
                            _ = ConsoleProcess.systemCommand("del AdbWinApi.dll");
                            _ = ConsoleProcess.systemCommand("del fastboot.exe");
                            _ = ConsoleProcess.systemCommand("move platform-tools\\adb.exe \"%cd%\"");
                            _ = ConsoleProcess.systemCommand("move platform-tools\\AdbWinUsbApi.dll \"%cd%\"");
                            _ = ConsoleProcess.systemCommand("move platform-tools\\AdbWinApi.dll \"%cd%\"");
                            _ = ConsoleProcess.systemCommand("move platform-tools\\fastboot.exe \"%cd%\"");
                            Directory.Delete("platform-tools", true);
                            File.Delete("sdkplatformtool.zip");
                            if (!checkAdbFastboot(true))
                            {
                                throw new Exception();
                            }

                            LogWriteLine("ATA ready!");
                        }
                        catch (Exception ex)
                        {
                            LogWriteLine("[ERROR] An error occurred while attempting to download the SDK Platform Tools");
                            MessageShowBox("An error occurred while attempting to download the SDK Platform Tools\nError message:" + ex, 0);
                            disableSystem(true);
                        }
                    }
                    break;
                case DialogResult.No:
                    disableSystem(true);
                    break;
                case DialogResult.OK:
                    _ = Process.Start("https://developer.android.com/license");
                    disableSystem(true);
                    break;
                default:
                    disableSystem(true);
                    break;
            }
        }

        private void disableSystem(bool a)
        {
            Invoke(delegate
            {
                tabPageSystem.Enabled = !a;
                buttonMobileScreenShare.Enabled = !a;
            });
        }

        private void toolStripButtonBloatwareDetecter_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.Items.Count > 0)
            {
                List<string> listOfApps = new();
                foreach (object list in checkedListBoxApp.Items)
                {
                    listOfApps.Add(list.ToString());
                }
                BloatwareDetecter bloatwareDetecter = new(listOfApps, this)
                {
                    CurrentDevice = ATA.CurrentDeviceSelected.ID
                };
                _ = bloatwareDetecter.ShowDialog();
            }
            else
            {
                MessageShowBox("Apps not loaded!", 1);
            }
        }

        private void toolStripButtonRestoreApp_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                List<string> apps = new();
                foreach (object app in checkedListBoxApp.CheckedItems)
                {
                    apps.Add(app.ToString());
                }
                LoadingForm load = new(apps, "adb -s " + ATA.CurrentDeviceSelected.ID + " shell cmd package install-existing --user " + device.User + " ", "Restored:");
                _ = load.ShowDialog();
                if (load.DialogResult != DialogResult.OK)
                {
                    MessageShowBox("Error during restoring process", 0);
                }
                syncFun(5);
            }
            else
            {
                MessageShowBox("No app selected", 1);
            }
        }

        private void uninstalledAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonUninstallApp.Enabled = false;
            toolStripButtonRestoreApp.Enabled = true;
            toolStripButtonPermissionMenu.Enabled = false;
            toolStripButtonPackageManager.Enabled = false;
            toolStripButtonExtract.Enabled = false;
            toolStripButtonBloatwareDetecter.Enabled = false;
            device.SystemApp = false;
            syncFun(5);
        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            _ = Process.Start("https://github.com/MassimilianoSartore/ATA-GUI");
        }

        private void buttonMobileScreenShare_Click(object sender, EventArgs e)
        {
            if (File.Exists("scrcpy.exe"))
            {
                _ = ConsoleProcess.scrcpyProcess("-s " + ATA.CurrentDeviceSelected.ID);
            }
            else
            {
                try
                {
                    backgroundWorkerExeDownloader.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageShowBox(ex.Message, 0);
                }
            }
        }

        private async void backgroundWorkerExeDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!ata.IsConnected)
            {
                MessageShowBox("You are offline, ATA can not download scrcpy", 0);
                return;
            }
            ExeMissingForm scrcpyError = new("scrcpy.exe not found\n\nDo you want to download scrcpy?\n\n[By pressing YES you agree scrcpy terms and conditions]\nfor more info press info button", "Error, scrcpy Missing!");
            _ = scrcpyError.ShowDialog();
            switch (scrcpyError.DialogResult)
            {
                case DialogResult.Yes:
                    LogWriteLine("downloading scrcpy...");
                    using (HttpClient client = new())
                    {
                        try
                        {
                            string url = "https://ata.msartore.dev/scrcpy/download";
                            string filePath = "scrcpy.zip";
                            using (HttpResponseMessage response = await client.GetAsync(url))
                            {
                                _ = response.EnsureSuccessStatusCode();
                                using FileStream fs = new(filePath, FileMode.Create);
                                await response.Content.CopyToAsync(fs);
                            }
                            LogWriteLine("scrcpy downloaded!");
                            LogWriteLine("unzipping scrcpy...");
                            using (ZipFile zip = ZipFile.Read("scrcpy.zip"))
                            {
                                zip.ExtractAll(Path.GetDirectoryName(Application.ExecutablePath), ExtractExistingFileAction.DoNotOverwrite);
                            }
                            string directory = Directory.GetDirectories(Path.GetDirectoryName(Application.ExecutablePath)).Where(it => it.Contains("scrcpy-win64")).FirstOrDefault();

                            if (directory != null)
                            {
                                foreach (string item in Directory.EnumerateFiles(directory))
                                {
                                    string name = item[(item.LastIndexOf('\\') + 1)..];
                                    if (!File.Exists(name))
                                    {
                                        File.Copy(item, Path.GetDirectoryName(Application.ExecutablePath) + "\\" + name, false);
                                    }
                                }
                                LogWriteLine("scrcpy extracted!");
                                LogWriteLine("Getting things ready...");
                                Directory.Delete(directory, true);
                                File.Delete("scrcpy.zip");
                                LogWriteLine("scrcpy ready!");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageShowBox(ex.Message, 0);
                            disableSystem(true);
                        }
                    }
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.OK:
                    _ = Process.Start("https://raw.githubusercontent.com/Genymobile/scrcpy/master/LICENSE");
                    break;
                default:
                    break;
            }
        }

        private void labelHelp_MouseLeave(object sender, EventArgs e)
        {
            labelHelp.BackColor = Color.Black;
        }

        private void labelSettings_MouseLeave(object sender, EventArgs e)
        {
            labelSettings.BackColor = Color.Black;
        }

        private void labelSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new();
            _ = settings.ShowDialog();
        }

        private void labelHelp_Click(object sender, EventArgs e)
        {
            contextMenuStripHelp.Show(Cursor.Position);
        }

        private void videoTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = Process.Start("https://github.com/MassimilianoSartore/ATA-GUI/wiki#coming-soon");
        }

        private void pictureBoxMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("adb").Length != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to kill ADB?", "Kill ADB", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    _ = ConsoleProcess.systemCommand("adb.exe kill-server");
                }
            }
            Application.Exit();
        }

        private void pictureBoxMinimize_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxMinimize.BackColor = System.Drawing.ColorTranslator.FromHtml("#1f2121");
        }

        private void pictureBoxMinimize_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxMinimize.BackColor = Color.Black;
        }

        private void pictureBoxClose_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxClose.BackColor = Color.Red;
        }

        private void pictureBoxClose_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxClose.BackColor = Color.Black;
        }

        private void buttonDeviceLogs_Click(object sender, EventArgs e)
        {
            DeviceLogs deviceLog = new(ATA.CurrentDeviceSelected.ID);
            deviceLog.Show();
        }

        private void submitFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Feedback feedback = new();
            _ = feedback.ShowDialog();
        }

        private void grantWriteSecureSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm grant ", " android.permission.WRITE_SECURE_SETTINGS", 0);
        }

        private void revokeWriteSecureSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm revoke ", " android.permission.WRITE_SECURE_SETTINGS", 0);
        }

        private void grantDUMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm grant ", " android.permission.DUMP", 0);
        }

        private void revokeDUMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm revoke ", " android.permission.DUMP", 0);
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.CheckedItems.Count == 0)
            {
                MessageShowBox("No app selected", 1);
                return;
            }
            contextMenuStripSearch.Show(Cursor.Position);
        }

        private void duckduckgoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (string file in checkedListBoxApp.CheckedItems)
            {
                _ = Process.Start("https://duckduckgo.com/?q=" + file);
            }
        }

        private void googleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (string file in checkedListBoxApp.CheckedItems)
            {
                _ = Process.Start("https://www.google.com/search?q=" + file);
            }
        }

        private void playMarketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (string file in checkedListBoxApp.CheckedItems)
            {
                _ = Process.Start("https://play.google.com/store/apps/details?id=" + file);
            }
        }

        private void APKMirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (string file in checkedListBoxApp.CheckedItems)
            {
                _ = Process.Start("https://www.apkmirror.com/?post_type=app_release&searchtype=apk&s=" + file);
            }
        }

        private void fDroidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (string file in checkedListBoxApp.CheckedItems)
            {
                _ = Process.Start("https://f-droid.org/en/packages/" + file);
            }
        }

        private void disabledAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonUninstallApp.Enabled = true;
            toolStripButtonRestoreApp.Enabled = false;
            toolStripButtonPermissionMenu.Enabled = true;
            toolStripButtonPackageManager.Enabled = true;
            toolStripButtonExtract.Enabled = false;
            toolStripButtonBloatwareDetecter.Enabled = true;
            device.SystemApp = true;
        }

        private void toolStripMenuItemADBKill_Click(object sender, EventArgs e)
        {
            _ = ConsoleProcess.systemCommand("taskkill /f /im " + ata.FILEADB);
            MessageShowBox("Adb.exe killed!", 2);
        }

        private void labelTools_MouseEnter(object sender, EventArgs e)
        {
            labelTools.BackColor = ColorTranslator.FromHtml("#1f2121");
        }

        private void labelTools_MouseLeave(object sender, EventArgs e)
        {
            labelTools.BackColor = Color.Black;
        }

        private void labelHelp_MouseEnter(object sender, EventArgs e)
        {
            labelHelp.BackColor = ColorTranslator.FromHtml("#1f2121");
        }

        private void labelSettings_MouseEnter(object sender, EventArgs e)
        {
            labelSettings.BackColor = ColorTranslator.FromHtml("#1f2121");
        }

        private void labelTools_Click(object sender, EventArgs e)
        {
            contextMenuStripTools.Show(Cursor.Position);
        }

        private void buttonTaskManager_Click(object sender, EventArgs e)
        {
            _ = new TaskManager().ShowDialog();
        }

        private void backgroundWorkerADBConnect_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int deviceCountTmp = ata.Devices.Count;
            string ip = string.Empty;
            string port = textBoxPort.Text.Trim();

            Invoke(delegate
            {
                ip = comboBoxIP.Text.Trim();
            });

            if (ip.Length > 1)
            {
                if (ATA.CurrentDeviceSelected != null)
                {
                    if (ATA.CurrentDeviceSelected.ID.Length > 0)
                    {
                        _ = ConsoleProcess.adbFastbootCommandR(" -s " + ATA.CurrentDeviceSelected.ID + " tcpip " + port, 0);
                    }
                }

                string result = ConsoleProcess.adbFastbootCommandR("connect " + ip + ":" + port, 0);

                if (!result.Contains("cannot connect"))
                {
                    if (result.Contains("already"))
                    {
                        MessageShowBox("Already connected to " + ip, 2);
                    }
                    else
                    {
                        Invoke(delegate
                        {
                            if (!ata.IPList.Contains(ip))
                            {
                                _ = ata.IPList.Add(ip);
                                _ = comboBoxIP.Items.Add(ip);

                                using StreamWriter writer = new(ATA.IPFileName, true);
                                writer.WriteLine(ip);
                            }
                            buttonConnectToIP.Enabled = false;
                            buttonDisconnectIP.Enabled = true;
                        });

                        MessageShowBox("Connected to " + ip, 2);

                        Thread.Sleep(1000);

                        reloadList();
                    }
                }
                else
                {
                    MessageShowBox(string.Format("Failed to connect to {0}\nError message: {1}", ip, result), 0);
                }
            }
        }

        private void backgroundWorkerADBDisconnect_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int deviceCountTmp = ata.Devices.Count;
            string ip = string.Empty;

            Invoke(delegate
            {
                ip = comboBoxIP.Text.Trim();
            });

            if (ip.Length > 1)
            {
                bool result = ConsoleProcess.systemCommand("adb disconnect " + ip).Contains("disconnected");

                if (result)
                {
                    MessageShowBox(ip + " disconnected", 2);
                    Invoke(delegate
                    {
                        buttonConnectToIP.Enabled = true;
                        buttonDisconnectIP.Enabled = false;
                    });

                    reloadList();

                    syncFun(3);
                }
                else
                {
                    MessageShowBox(ip + " failed to disconnect", 0);
                }
            }
        }

        private void installAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            installApp("");
        }

        private void downgradeAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            installApp("-d");
        }

        private void installApp(string command)
        {
            openFileDialogAPK.Filter = "Apk files *.Apk|*.apk;";
            openFileDialogAPK.FileName = "";
            openFileDialogAPK.Multiselect = true;
            openFileDialogAPK.Title = "Select Apk";

            DialogResult dr = openFileDialogAPK.ShowDialog();
            if (dr == DialogResult.OK)
            {
                backgroundWorkerAPKinstall.RunWorkerAsync(openFileDialogAPK.FileNames);
            }
        }

        private void textBoxIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                connectToIp();
            }
        }

        private void grantSYSTEMALERTWINDOWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm grant ", " android.permission.SYSTEM_ALERT_WINDOW", 0);
        }

        private void revokeSYSTEMALERTWINDOWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm revoke ", " android.permission.SYSTEM_ALERT_WINDOW", 0);
        }

        private void toolStripButtonSetDefault_Click(object sender, EventArgs e)
        {
            CheckedListBox.CheckedItemCollection checkedItems = checkedListBoxApp.CheckedItems;

            switch (checkedItems.Count)
            {
                case 0:
                    MessageShowBox("No app seleted", 1);
                    break;
                case 1:
                    foreach (object current in checkedItems)
                    {
                        _ = new DefaultApp(current.ToString()).ShowDialog();
                    }
                    break;
                default:
                    MessageShowBox("Too much app seleted", 1);
                    break;
            }
        }

        private void comboBoxIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                connectToIp();
            }
        }

        private void comboBoxIP_TextUpdate(object sender, EventArgs e)
        {
            buttonConnectToIP.Enabled = buttonDisconnectIP.Enabled = true;
        }

        private void toolStripButtonExtract_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                List<string> arrayApkSelect = new();
                foreach (object list in checkedListBoxApp.CheckedItems)
                {
                    arrayApkSelect.Add(list.ToString());
                }
                LoadingForm loadingForm = new(ATA.CurrentDeviceSelected.ID, arrayApkSelect);
                _ = loadingForm.ShowDialog();
                for (int i = 0; i < checkedListBoxApp.Items.Count; i++)
                {
                    checkedListBoxApp.SetItemChecked(i, false);
                }
            }
            else
            {
                MessageShowBox("No app selected!", 1);
            }
        }

        private void buttonTurnOffAdb_Click(object sender, EventArgs e)
        {
            _ = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell settings put global adb_enabled 0", 0);
            LogWriteLine("The command has been ejected");
            syncFun(3);
        }

        private void richTextBoxLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            _ = Process.Start(e.LinkText);
        }

        private void buttonUnlockButtons_Click(object sender, EventArgs e)
        {
            textBoxPort.Enabled = true;
            buttonConnectToIP.Enabled = true;
            buttonDisconnectIP.Enabled = true;
        }

        private void buttonSetRotation_Click(object sender, EventArgs e)
        {
            string commandRun = " shell wm set-ignore-orientation-request ";

            if (domainUpDownFreeRotation.Items.Count > 1)
            {
                commandRun += " -d " + domainUpDownFreeRotation.Text;
            }

            commandRun += (!device.IsRotationFreeEnabled).ToString().ToLowerInvariant();

            _ = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + commandRun, 0);
            device.IsRotationFreeEnabled = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell wm get-ignore-orientation-request", 0).Contains("true");
            LogWriteLine("Free rotation " + buttonSetRotation.Text.ToLowerInvariant() + "ed");
            buttonSetRotation.Text = device.IsRotationFreeEnabled ? "Unset" : "Set";
        }

        private void buttonCommandInject_Click(object sender, EventArgs e)
        {
            if (richTextBoxTerminal.Text.Trim().Length == 0)
            {
                MessageShowBox("You have to enter a command!", 1);
            }
            else
            {
                LogWriteLine(ConsoleProcess.adbFastbootCommandR(richTextBoxTerminal.Text.Trim(), radioButtonADB.Checked ? 0 : 1));
            }
        }

        private void buttonInjectText_Click(object sender, EventArgs e)
        {
            _ = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " shell input text \"" + richTextBoxSend.Text + "\"", 0);
            if (richTextBoxSend.Text.Length == 0)
            {
                MessageShowBox("You have to enter a text to inject!", 1);
            }
            else
            {
                LogWriteLine("Text injected");
            }
        }

        private void buttonClearTextSend_Click(object sender, EventArgs e)
        {
            richTextBoxSend.Clear();
        }

        private void pictureBoxMaximize_Click(object sender, EventArgs e)
        {
            maximizeWindow();
        }

        private void pictureBoxMaximize_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxMaximize.BackColor = ColorTranslator.FromHtml("#1f2121");
        }

        private void pictureBoxMaximize_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxMaximize.BackColor = Color.Black;
        }

        private void pictureBoxMaximize_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBoxMaximize.Image = ata.IsMaximize ? Properties.Resources.icons8_restore_down_16 : Properties.Resources.icons8_maximize_button_16;
        }

        private void panelTopBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            maximizeWindow();
        }

        private void maximizeWindow()
        {
            if (ata.IsMaximize)
            {
                Size = new Size(1314, 527);
                CenterToScreen();
            }
            else
            {
                Size = new Size(Screen.GetWorkingArea(this).Width, Screen.GetWorkingArea(this).Height);
                Location = Screen.GetWorkingArea(this).Location;
            }
            ata.IsMaximize = !ata.IsMaximize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxDirImg.Text.Length > 0)
            {
                if (radioButtonBoot.Checked)
                {
                    flash(0);
                }
                if (radioButtonBootloader.Checked)
                {
                    flash(1);
                }
                if (radioButtonCache.Checked)
                {
                    flash(2);
                }
                if (radioButtonRadio.Checked)
                {
                    flash(3);
                }
                if (radioButtonRecovery.Checked)
                {
                    flash(4);
                }
                if (radioButtonRom.Checked)
                {
                    flash(5);
                }
                if (radioButtonSystem.Checked)
                {
                    flash(6);
                }
                if (radioButtonVendor.Checked)
                {
                    flash(7);
                }
            }
            else
            {
                MessageShowBox("Please select a img file", 1);
            }
        }

        private void buttonBrowseFile_Click(object sender, EventArgs e)
        {
            openFileDialogZip.Filter = "Zip files *.zip|*.zip;";
            openFileDialogZip.FileName = "";
            openFileDialogZip.Multiselect = false;
            openFileDialogZip.Title = "Select Zip";

            DialogResult dr = openFileDialogZip.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxDirFile.Text = openFileDialogZip.FileName;
            }
        }

        private void backgroundWorkerAPKinstall_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            APK.installApk((string[])e.Argument, device.User, (name) =>
            {
                Invoke(delegate
                {
                    LogWriteLine("Installing " + name + " ...");
                });
            });
            syncFun(3);
        }

        private void backgroundWorkerFileTransfer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (((DragEventArgs)e.Argument).Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])((DragEventArgs)e.Argument).Data.GetData(DataFormats.FileDrop);
                LoadingForm loading = new(filePaths.ToList(), ATA.CurrentDeviceSelected.ID);
                _ = loading.ShowDialog();
            }
        }

        private void buttonrr__Click(object sender, EventArgs e)
        {
            _ = ConsoleProcess.systemCommand(commandAssembler(true, "reboot recovery"));
            LogWriteLine("Rebooted!");
            reloadList();
        }

        private void buttonrs__Click(object sender, EventArgs e)
        {
            rebootSmartphone();
            LogWriteLine("Rebooted!");
            reloadList();
        }

        private void buttonrf__Click(object sender, EventArgs e)
        {
            _ = ConsoleProcess.systemCommand(commandAssembler(true, "reboot-bootloader"));
            LogWriteLine("Rebooted!");
            reloadList();
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("scrcpy.exe"))
                {
                    string[] result = ConsoleProcess.scrcpyProcess("--version").Split(' ');
                    foreach (string item in result)
                    {
                        if (item.Contains("."))
                        {
                            string nTmp = "";

                            for (int i = 0; i < item.Length; i++)
                            {
                                if (char.IsDigit(item[i]))
                                {
                                    nTmp += item[i];
                                    if (nTmp.Length == 2)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (nTmp.Length < 0)
                            {
                                continue;
                            }

                            if (int.Parse(nTmp) < 22)
                            {
                                MessageShowBox("scrcpy is not up-to-date, update it to use this feature", 1);
                                return;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    _ = ConsoleProcess.systemCommandAsync(string.Format("scrcpy -s {0} --video-source=camera --camera-size=1920x1080 --camera-facing={1}", ATA.CurrentDeviceSelected.ID, comboBoxCameraModes.Text));
                }
                else
                {
                    backgroundWorkerExeDownloader.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.Message, 0);
            }
        }
    }
}
