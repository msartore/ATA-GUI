using ATA_GUI.Classes;
using ATA_GUI.Utils;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class MainForm : Form
    {
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private readonly ATA ata = new ATA();
        private readonly Device device = new Device();

        public static readonly List<string> arrayApks = new List<string>();
        private static readonly int WM_NCLBUTTONDOWN = 0xA1;
        private static readonly int HT_CAPTION = 0x2;
        private static readonly Regex regex = new Regex(@"\s+");

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
            if (tabControls.SelectedTab.Name.Contains("System") || tabControls.SelectedTab.Name.Contains("Tools"))
            {
                syncFun(3);
                return;
            }
            if (tabControls.SelectedTab.Name.Contains("Fastboot"))
            {
                if (checkAdbFastboot(false))
                {
                    if (ConsoleProcess.adbFastbootCommandR("devices", 1).Contains("fastboot"))
                    {
                        string[] log = ConsoleProcess.adbFastbootCommandR("getvar all", 1).Split(' ', '\n');
                        for (int a = 0; a < log.Count(); a++)
                        {
                            if (log[a].Contains("partition-type:userdata:"))
                            {
                                labelUDT.Text = log[a].Substring("partition-type:userdata:".Length);
                            }
                            else if (log[a].Contains("partition-type:cache:"))
                            {
                                labelCDT.Text = log[a].Substring("partition-type:cache:".Length);
                            }
                            else if (log[a].Contains("unlocked:"))
                            {
                                labelBootloaderStatus.Text = log[a].Contains("yes") ? "Yes" : "No";
                            }
                        }
                        panelFastboot.Enabled = true;
                        LogWriteLine("Device found!");
                    }
                    else
                    {
                        MessageShowBox("Device not found!", 0);
                        panelFastboot.Enabled = false;
                    }
                }
                else
                {
                    panelFastboot.Enabled = false;
                }
                return;
            }
            MessageShowBox("Can not sync your device in this page", 1);
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
                if (ex.ToString().Contains("DotNetZip"))
                {
                    MessageShowBox("DotNetZip.dll not found!", 0);
                }
            }
        }

        private void buttonLogClear_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Clear();
        }

        private void LogWriteLine(string str)
        {
            _ = Invoke((Action)delegate
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
                    _ = MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case 1:
                    _ = MessageBox.Show(message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 2:
                    _ = MessageBox.Show(message, "Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        _ = Invoke((Action)delegate
                        {
                            if ((latestRelease.Number > currentRelease.Number) || ((latestRelease.Number == currentRelease.Number) && currentRelease.Pre && !latestRelease.Pre))
                            {
                                if (MessageBox.Show("New version found: " + latestRelease.Name + "\nCurrent Version: " + ATA.CURRENTVERSION + "\n\nDo you want to update it?", "Update found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    _ = Process.Start((string)jsonReal[0]["html_url"]);
                                    UpdateForm update = new UpdateForm(jsonReal[0]["assets"][0]["browser_download_url"]);
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

            _ = Invoke((Action)delegate
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
                string version = ConsoleProcess.adbFastbootCommandR("-s " + Regex.Replace(ATA.CurrentDeviceSelected, @"\s", "") + " shell getprop ro.build.version.release", 0);
                if (version != null)
                {
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
                                    string[] arrayDeviceInfoCommands = { "-s "+ ATA.CurrentDeviceSelected +" shell getprop ro.build.version.release", "-s "+ ATA.CurrentDeviceSelected +" shell getprop ro.build.user",  "-s "+ ATA.CurrentDeviceSelected +" shell getprop ro.product.cpu.abilist", "-s "+ ATA.CurrentDeviceSelected +" shell getprop ro.product.manufacturer" , "-s "+ ATA.CurrentDeviceSelected +" shell getprop ro.product.model",
                                           "-s "+ ATA.CurrentDeviceSelected +" shell getprop ro.product.board", "-s "+ ATA.CurrentDeviceSelected +" shell getprop ro.product.device", "-s "+ ATA.CurrentDeviceSelected +" shell ip route"};
                                    string deviceinfo = ConsoleProcess.adbFastbootCommandR(arrayDeviceInfoCommands, 0);
                                    string[] arrayDeviceInfo = deviceinfo.Split('\n');
                                    if (arrayDeviceInfo.Length > 6)
                                    {
                                        disableSystem(false);
                                        _ = Invoke((Action)delegate
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
                                            if (arrayDeviceInfo.Length > 7)
                                            {
                                                if (arrayDeviceInfo[7].Length > 4)
                                                {
                                                    comboBoxIP.Text = labelIP.Text = arrayDeviceInfo[7].Substring(arrayDeviceInfo[7].IndexOf("src") + 4);
                                                }
                                                if (labelIP.Text.Contains("t of devices attached") || arrayDeviceInfo[7].Length == 0)
                                                {
                                                    labelIP.Text = "Not connected to a network";
                                                    comboBoxIP.ResetText();
                                                    buttonConnectToIP.Enabled = false;
                                                    buttonDisconnectIP.Enabled = false;
                                                }
                                            }
                                            if (ATA.CurrentDeviceSelected.Contains("192"))
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
                                                labelUser.Text = device.User = Regex.Replace(ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell am get-current-user", 0), @"\t|\n|\r", "");
                                            }
                                            else
                                            {
                                                MessageShowBox("Feature currently not avaiable for device under android 8", 2);
                                            }

                                            _ = int.TryParse(ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell cmd display get - displays", 0), out int maxDisplay);

                                            groupBoxFreeRotation.Enabled = true;

                                            for (int i = maxDisplay; i > -1; i--)
                                            {
                                                _ = domainUpDownFreeRotation.Items.Add(i);
                                            }

                                            device.IsRotationFreeEnabled = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell wm get-ignore-orientation-request", 0).Contains("true");
                                            buttonSetRotation.Text = device.IsRotationFreeEnabled ? "Unset" : "Set";

                                            LogWriteLine("Device info extracted");
                                            disableEnableSystem(true);
                                        });
                                    }
                                    else
                                    {
                                        _ = Invoke((Action)delegate
                                        {
                                            disableEnableSystem(false);
                                            buttonDisconnectIP.Enabled = false;
                                        });
                                        LogWriteLine("[ERROR] failed to extract device info!");
                                    }
                                    break;
                                case "2":
                                    arrayApks.Clear();
                                    _ = Invoke((Action)delegate
                                    {
                                        labelSelectedAppCount.Text = "Selected App: 0";
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    string[] command;
                                    command = !device.SystemApp
                                        ? (new[] { "-s " + ATA.CurrentDeviceSelected + " shell pm list packages -3 --user " + device.User })
                                        : (new[] { "-s " + ATA.CurrentDeviceSelected + " shell pm list packages -s --user " + device.User });
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
                                    List<string> arrayApkTmp = new List<string>();
                                    arrayApks.Clear();
                                    _ = Invoke((Action)delegate
                                    {
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    LogWriteLine("Loading apps...");
                                    if ((device.StringApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell pm list packages -3 --user " + device.User, 0)) != null)
                                    {
                                        arrayApkTmp.AddRange(device.StringApk.Split('\n'));
                                    }
                                    else
                                    {
                                        MessageShowBox("Error during apk loading", 0);
                                        break;
                                    }
                                    if ((device.StringApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell pm list packages -s --user " + device.User, 0)) != null)
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
                                    _ = Invoke((Action)delegate
                                    {
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    LogWriteLine("Loading apps...");
                                    if ((device.StringApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell pm list packages -u --user " + device.User, 0)) != null)
                                    {
                                        if ((stringInstalledApk = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell pm list packages --user " + device.User, 0)) != null)
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
                    else
                    {
                        _ = Invoke((Action)delegate
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
                    _ = Invoke((Action)delegate
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
                    arrayApks.Add(line.Substring(8));
                }
            }
            arrayApks.Sort();
            _ = Invoke((Action)delegate
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
            groupBoxFreeRotation.Enabled = enable;
            groupBoxTextInject.Enabled = enable;
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
                _ = ConsoleProcess.systemCommandAsync("adb -s " + ATA.CurrentDeviceSelected + " reboot recovery");
                LogWriteLine("Rebooted!");
            }
        }

        private void buttonRF_Click(object sender, EventArgs e)
        {
            if (!device.DeviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _ = ConsoleProcess.systemCommandAsync("adb -s " + ATA.CurrentDeviceSelected + " reboot-bootloader");
                LogWriteLine("Rebooted!");
            }
        }

        public void ToolTipGenerator(Control button, string title, string message)
        {
            ToolTip buttonToolTip = new ToolTip
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
            groupBox6.AllowDrop = true;
            groupBox2.AllowDrop = true;
            if (!File.Exists("DotNetZip.dll"))
            {
                MessageShowBox("DotNetZip.dll not found!", 0);
                Application.Exit();
            }
            Disclaimer disclaimer = new Disclaimer();
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
            if (checkAdbFastboot(true))
            {
                List<string> devicesTmp;
                int deviceCounter = 0;
                Tuple<string, string> devices;

                string dev = ConsoleProcess.adbFastbootCommandR("devices", 0);
                if (dev != null)
                {
                    ata.Devices.Clear();
                    devicesTmp = dev.Substring("List of devices attached".Length).Split().ToList();

                    for (int i = 0; i < devicesTmp.Count; i++)
                    {

                        if (Regex.Match(devicesTmp[i], "^[a-zA-Z0-9.:]*$").Success && devicesTmp[i].Length > 2 && devicesTmp[i] != "recovery" && devicesTmp[i] != "device")
                        {
                            devices = new Tuple<string, string>(
                                devicesTmp[i],
                                ConsoleProcess.adbFastbootCommandR("-s " + Regex.Replace(devicesTmp[i], @"\s", "") + " shell getprop ro.product.model", 0)
                            );

                            deviceCounter++;

                            ata.Devices.Add(devices);
                        }
                    }
                    if (ata.Devices.Count == 0)
                    {
                        disableEnableSystem(false);
                        return;
                    }
                    foreach ((_, string name) in ata.Devices)
                    {
                        _ = comboBoxDevices.Items.Add(name);
                    }
                    comboBoxDevices.SelectedIndex = 0;
                    ATA.CurrentDeviceSelected = Regex.Replace(ata.Devices[0].Item1, @"\s", "");
                }
            }
        }

        public static bool pingCheck()
        {
            Ping myPing = new Ping();
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
                    if ((log = ConsoleProcess.adbFastbootCommandR(" -s " + ATA.CurrentDeviceSelected + " " + command1 + "--user " + device.User + " " + current + command2, 0)) != null)
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
            string fileName = textBoxDirFile.Text.Substring(textBoxDirFile.Text.LastIndexOf('\\') + 1);
            LogWriteLine("Installing " + fileName);
            string log = ConsoleProcess.adbFastbootCommandR("sideload \"" + textBoxDirFile.Text + "\"", 0);
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

            if ((log = ConsoleProcess.adbFastbootCommandR(command + textBoxDirImg.Text, 1)) != null)
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
            if (!exist)
            {
                LogWriteLine(exeTmp + " not found!");
            }
            return exist;
        }

        private void buttonRebootToSystem_Click(object sender, EventArgs e)
        {
            LogWriteLine(ConsoleProcess.adbFastbootCommandR("reboot", 1));
        }

        private void buttonHardReset_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to erase your phone?", "Erase Phone", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (checkAdbFastboot(false))
                {
                    LogWriteLine("Erasing process started...");
                    _ = ConsoleProcess.systemCommandAsync("fastboot erase userdata && fastboot erase cache");
                    LogWriteLine("Erasing process finished!!");
                }
            }
        }

        private void rebootSmartphone()
        {
            LogWriteLine("Rebooting smartphone...");
            _ = ConsoleProcess.systemCommandAsync("adb -s " + ATA.CurrentDeviceSelected + " reboot");
            LogWriteLine("Smartphone rebooted");
        }

        private void buttonRebootRecovery_Click(object sender, EventArgs e)
        {
            LogWriteLine("Rebooting smartphone...");
            _ = ConsoleProcess.systemCommandAsync("fastboot reboot recovery");
            LogWriteLine("Smartphone rebooted");
        }

        private void buttonBootloaderMenu_Click(object sender, EventArgs e)
        {
            BootloaderMenu bootloaderMenu = new BootloaderMenu();
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
            string command = "adb -s " + ATA.CurrentDeviceSelected + " shell pm uninstall -k --user " + device.User + " ";
            LoadingForm load;
            List<string> arrayApkSelect = new List<string>();
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
                List<string> arrayApkSelect = new List<string>();
                foreach (object list in checkedListBoxApp.CheckedItems)
                {
                    arrayApkSelect.Add(list.ToString());
                }

                PackageMenu packageMenu = new PackageMenu(arrayApkSelect);
                _ = packageMenu.ShowDialog();
                string command;
                string commandName;
                switch (packageMenu.DialogResult1)
                {
                    case 1:
                        command = "adb -s " + ATA.CurrentDeviceSelected + " shell pm enable --user " + device.User + " ";
                        commandName = "Enabled:";
                        break;
                    case 0:
                        command = "adb -s " + ATA.CurrentDeviceSelected + " shell pm disable-user --user " + device.User + " ";
                        commandName = "Disabled:";
                        break;
                    case 2:
                        command = "adb -s " + ATA.CurrentDeviceSelected + " shell pm clear --user " + device.User + " ";
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
            LoadingForm load = new LoadingForm(arrayApkSelect, command, commandName);
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
            ATA.CurrentDeviceSelected = Regex.Replace(ata.Devices[comboBoxDevices.SelectedIndex].Item1, @"\s", "");
        }

        private void buttonReloadDevicesList_Click(object sender, EventArgs e)
        {
            reloadList();
        }

        private void reloadList()
        {
            if (!checkAdbFastboot(true))
            {
                adbDownload();
            }
            comboBoxDevices.Items.Clear();
            ata.Devices.Clear();
            DevicesListUpdate();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            comboBoxDevices.Enabled = true;
            buttonReloadDevicesList.Enabled = true;

            if (tabControls.SelectedTab.Text != "System" && tabControls.SelectedTab.Text != "Tools")
            {
                comboBoxDevices.Enabled = false;
                buttonReloadDevicesList.Enabled = false;
                buttonDeviceLogs.Enabled = false;
                buttonTaskManager.Enabled = false;
                buttonMobileScreenShare.Enabled = false;
                disableEnableSystem(false);
            }
        }

        private void backgroundWorkerAdbDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!ata.IsConnected)
            {
                MessageShowBox("You are offline, ATA can not download ADB", 0);
                return;
            }
            _ = Invoke((Action)delegate
            {
                disableEnableSystem(false);
                buttonDisconnectIP.Enabled = false;
            });
            ExeMissingForm adbError = new ExeMissingForm("adb.exe not found\n\nDo you want to download sdk platform tool?\n\n[By pressing YES you agree sdk platform tool terms and conditions]\nfor more info press info button", "Error, ADB Missing!");
            _ = adbError.ShowDialog();
            switch (adbError.DialogResult)
            {
                case DialogResult.Yes:
                    LogWriteLine("Downloading sdk platform tool...");
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile("https://dl.google.com/android/repository/platform-tools-latest-windows.zip", "sdkplatformtool.zip");
                            LogWriteLine("sdk platform tool downloaded!");
                            LogWriteLine("unzipping sdk platform tool...");
                            using (ZipFile zip = ZipFile.Read("sdkplatformtool.zip"))
                            {
                                zip.ExtractAll(Path.GetDirectoryName(Application.ExecutablePath));
                            }
                            LogWriteLine("sdk platform tool extraced!");
                            LogWriteLine("Getting things ready...");
                            _ = ConsoleProcess.systemCommandAsync("taskkill /f /im adb.exe");
                            _ = ConsoleProcess.systemCommandAsync("taskkill /f /im fastboot.exe");
                            _ = ConsoleProcess.systemCommandAsync("del adb.exe");
                            _ = ConsoleProcess.systemCommandAsync("del AdbWinUsbApi.dll");
                            _ = ConsoleProcess.systemCommandAsync("del AdbWinApi.dll");
                            _ = ConsoleProcess.systemCommandAsync("del fastboot.exe");
                            _ = ConsoleProcess.systemCommandAsync("move platform-tools\\adb.exe \"%cd%\"");
                            _ = ConsoleProcess.systemCommandAsync("move platform-tools\\AdbWinUsbApi.dll \"%cd%\"");
                            _ = ConsoleProcess.systemCommandAsync("move platform-tools\\AdbWinApi.dll \"%cd%\"");
                            _ = ConsoleProcess.systemCommandAsync("move platform-tools\\fastboot.exe \"%cd%\"");
                            File.Delete("sdkplatformtool.zip");
                            _ = ConsoleProcess.systemCommandAsync("rmdir /Q /S platform-tools");
                            LogWriteLine("ATA ready!");
                        }
                        catch
                        {
                            LogWriteLine("[ERROR] An error occurred while attempting to download the SDK Platform Tools");
                            MessageShowBox("An error occurred while attempting to download the SDK Platform Tools", 0);
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
            _ = Invoke((Action)delegate
            {
                tabPageSystem.Enabled = !a;
                buttonMobileScreenShare.Enabled = !a;
            });
        }

        private void toolStripButtonBloatwareDetecter_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.Items.Count > 0)
            {
                List<string> listOfApps = new List<string>();
                foreach (object list in checkedListBoxApp.Items)
                {
                    listOfApps.Add(list.ToString());
                }
                BloatwareDetecter bloatwareDetecter = new BloatwareDetecter(listOfApps, this)
                {
                    CurrentDevice = ATA.CurrentDeviceSelected
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
                List<string> apps = new List<string>();
                foreach (object app in checkedListBoxApp.CheckedItems)
                {
                    apps.Add(app.ToString());
                }
                LoadingForm load = new LoadingForm(apps, "adb -s " + ATA.CurrentDeviceSelected + " shell cmd package install-existing --user " + device.User + " ", "Restored:");
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
                _ = ConsoleProcess.systemCommandAsync("start scrcpy");
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
            ExeMissingForm scrcpyError = new ExeMissingForm("scrcpy.exe not found\n\nDo you want to download scrcpy?\n\n[By pressing YES you agree scrcpy terms and conditions]\nfor more info press info button", "Error, scrcpy Missing!");
            _ = scrcpyError.ShowDialog();
            switch (scrcpyError.DialogResult)
            {
                case DialogResult.Yes:
                    LogWriteLine("Downloading scrcpy...");
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile("https://ata.msartore.dev/scrcpy/download", "scrcpy.zip");
                            LogWriteLine("scrcpy downloaded!");
                            LogWriteLine("unzipping scrcpy...");
                            string[] directories = Directory.GetDirectories(Path.GetDirectoryName(Application.ExecutablePath));
                            using (ZipFile zip = ZipFile.Read("scrcpy.zip"))
                            {
                                zip.ExtractAll(Path.GetDirectoryName(Application.ExecutablePath), ExtractExistingFileAction.DoNotOverwrite);
                            }
                            string[] newDirectories = Directory.GetDirectories(Path.GetDirectoryName(Application.ExecutablePath));

                            for (int i = 0; i < newDirectories.Length; i++)
                            {
                                if (i == directories.Length || newDirectories.Length == directories.Length || newDirectories[i] != directories[i])
                                {
                                    _ = await ConsoleProcess.systemCommandAsync("robocopy " + newDirectories[i] + " " + Path.GetDirectoryName(Application.ExecutablePath) + " /E");
                                    LogWriteLine("scrcpy Extracted!");
                                    LogWriteLine("Getting things ready...");
                                    _ = ConsoleProcess.systemCommandAsync("rmdir /s /q " + newDirectories[i]);
                                    _ = ConsoleProcess.systemCommandAsync("del scrcpy.zip");
                                    LogWriteLine("scrcpy ready!");
                                    break;
                                }
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
            Settings settings = new Settings();
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
                    _ = ConsoleProcess.systemCommandAsync("taskkill /f /im " + ata.FILEADB);
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
            DeviceLogs deviceLog = new DeviceLogs(ATA.CurrentDeviceSelected);
            deviceLog.Show();
        }

        private void submitFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Feedback feedback = new Feedback();
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
            _ = ConsoleProcess.systemCommandAsync("taskkill /f /im " + ata.FILEADB);
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

            _ = Invoke((Action)delegate
            {
                ip = comboBoxIP.Text.Trim();
            });

            if (ip.Length > 1)
            {
                if (ATA.CurrentDeviceSelected.Length > 0)
                {
                    _ = ConsoleProcess.adbFastbootCommandR(" -s " + ATA.CurrentDeviceSelected + " tcpip " + port, 0);
                }

                _ = ConsoleProcess.systemCommandAsync("adb connect " + ip + ":" + port);

                Thread.Sleep(1000);

                _ = Invoke((Action)delegate
                {
                    reloadList();
                });

                if (deviceCountTmp < ata.Devices.Count)
                {
                    _ = Invoke((Action)delegate
                    {
                        if (!ata.IPList.Contains(ip))
                        {
                            _ = ata.IPList.Add(ip);
                            _ = comboBoxIP.Items.Add(ip);

                            using (StreamWriter writer = new StreamWriter(ATA.IPFileName, true))
                            {
                                writer.WriteLine(ip);
                            }
                        }
                        buttonConnectToIP.Enabled = false;
                        buttonDisconnectIP.Enabled = true;
                    });
                    MessageShowBox("Connected to " + ip, 2);
                }
                else
                {
                    MessageShowBox("Failed to connect to " + ip, 0);
                }

                syncFun(3);
            }
        }

        private void backgroundWorkerADBDisconnect_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int deviceCountTmp = ata.Devices.Count;
            string ip = string.Empty;

            _ = Invoke((Action)delegate
            {
                ip = comboBoxIP.Text.Trim();
            });

            if (ip.Length > 1)
            {
                _ = ConsoleProcess.systemCommandAsync("adb disconnect " + ip);

                Thread.Sleep(1000);

                _ = Invoke((Action)delegate
                {
                    reloadList();
                });

                if (deviceCountTmp > ata.Devices.Count)
                {
                    MessageShowBox(ip + " disconnected", 2);
                    _ = Invoke((Action)delegate
                    {
                        buttonConnectToIP.Enabled = true;
                        buttonDisconnectIP.Enabled = false;
                    });
                }
                else
                {
                    MessageShowBox(ip + " failed to disconnect", 0);
                }

                syncFun(3);
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
                List<string> arrayApkSelect = new List<string>();
                foreach (object list in checkedListBoxApp.CheckedItems)
                {
                    arrayApkSelect.Add(list.ToString());
                }
                LoadingForm loadingForm = new LoadingForm(ATA.CurrentDeviceSelected, arrayApkSelect);
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
            _ = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell settings put global adb_enabled 0", 0);
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

            _ = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + commandRun, 0);
            device.IsRotationFreeEnabled = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell wm get-ignore-orientation-request", 0).Contains("true");
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
            _ = ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected + " shell input text \"" + richTextBoxSend.Text + "\"", 0);
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
                Size = new Size(1126, 457);
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
            APK.installApk((string[])e.Argument, device.User);
        }

        private void backgroundWorkerFileTransfer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (((DragEventArgs)e.Argument).Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])((DragEventArgs)e.Argument).Data.GetData(DataFormats.FileDrop);
                LoadingForm loading = new LoadingForm(filePaths.ToList(), ATA.CurrentDeviceSelected);
                _ = loading.ShowDialog();
            }
        }
    }
}
