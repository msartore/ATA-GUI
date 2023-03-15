using Ionic.Zip;
using Newtonsoft.Json;
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

namespace ATA_GUI
{
    public partial class MainForm : Form
    {
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static readonly string CURRENTVERSION = "v2.0.1";
        public static readonly List<string> arrayApks = new List<string>();
        public static readonly string IPFileName = "IPList.txt";
        private static readonly int WM_NCLBUTTONDOWN = 0xA1;
        private static readonly int HT_CAPTION = 0x2;
        private static readonly Regex regex = new Regex(@"\s+");
        private bool textboxClear;
        private bool connected = true;
        private bool systemApp;
        private bool deviceWireless;
        private readonly string FILEADB = "adb.exe";
        private readonly List<Device> devices = new List<Device>();
        private readonly HashSet<string> IPList = new HashSet<string>();
        private string stringApk;
        private string user;

        public static string CurrentDeviceSelected { get; set; } = string.Empty;

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
            }
        }

        private void buttonSyncApp_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.Contains("System"))
            {
                syncFun(3);
                return;
            }
            if (tabControl1.SelectedTab.Name.Contains("Fastboot"))
            {
                if (checkAdbFastboot(1))
                {
                    if (adbFastbootCommandR(new[] { "devices" }, 1).Contains("fastboot"))
                    {
                        string[] log = adbFastbootCommandR(new[] { "getvar all" }, 1).Split(' ', '\n');
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
                    MessageShowBox("Fastboot not found", 0);
                    panelFastboot.Enabled = false;
                }
                return;
            }
            MessageShowBox("Can't sync your device in this page", 1);
        }

        private void syncFun(object paramObj)
        {
            try
            {
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

        public static string systemCommand(string command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            _ = cmd.Start();
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            return cmd.StandardOutput.ReadToEnd();
        }

        public static string adbFastbootCommandR(string[] args, int type)
        {
            StringBuilder ret = new StringBuilder();
            string line;
            Cursor.Current = Cursors.WaitCursor;
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            switch (type)
            {
                case 0:
                    startInfo.FileName = "adb.exe";
                    process.StartInfo = startInfo;

                    foreach (string s in args)
                    {
                        startInfo.Arguments = s;
                        _ = process.Start();
                        line = process.StandardOutput.ReadToEnd();
                        if (line.Length > 0)
                        {
                            _ = ret.Append(line);
                        }
                        process.Close();
                    }
                    break;
                case 1:
                    startInfo.FileName = "fastboot.exe";
                    process.StartInfo = startInfo;

                    foreach (string s in args)
                    {
                        startInfo.Arguments = s;
                        _ = process.Start();
                        line = process.StandardError.ReadToEnd();
                        if (line.Length > 0) { _ = ret.Append(line + "\n"); }

                        line = process.StandardOutput.ReadToEnd();
                        if (line.Length > 0) { _ = ret.Append(line + "\n"); }
                        process.Close();
                    }
                    break;
                default:
                    break;
            }
            return ret.ToString();
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
                    MessageShowBox("Error in MessageShowBox", 0);
                    break;
            }
        }

        private async Task updateCheckAsync()
        {
            Release currentRelease = new Release();
            Release latestRelease = new Release();
            string json;
            try
            {
                HttpClient _client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(5)
                };
                _client.DefaultRequestHeaders.Add("User-Agent", "ATA");
                json = await _client.GetStringAsync("https://ata.msartore.dev/api/links.json");
                dynamic jsonMirror = JsonConvert.DeserializeObject(json);
                json = await _client.GetStringAsync(jsonMirror[0]["url"].ToString());
                dynamic jsonReal = JsonConvert.DeserializeObject(json);
                string latestReleaseName = jsonReal[0]["tag_name"];
                latestRelease.Number = int.Parse(Regex.Replace(latestReleaseName, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                if (latestReleaseName.Contains("Pre")) { latestRelease.Pre = true; }
                currentRelease.Number = int.Parse(Regex.Replace(CURRENTVERSION, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                if (CURRENTVERSION.Contains("Pre")) { currentRelease.Pre = true; }
                string linkString = jsonReal[0]["assets"][0]["browser_download_url"];
                string linkRepository = jsonReal[0]["html_url"];
                if ((latestRelease.Number > currentRelease.Number) || ((latestRelease.Number == currentRelease.Number) && currentRelease.Pre && !latestRelease.Pre))
                {
                    if (MessageBox.Show("New version found: " + latestReleaseName + "\nCurrent Version: " + CURRENTVERSION + "\n\nDo you want to update it?", "Update found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _ = Process.Start((string)jsonReal[0]["html_url"]);
                        UpdateForm update = new UpdateForm(linkString);
                        _ = update.ShowDialog();
                    }
                    else
                    {
                        LogWriteLine("ATA is not up to date, update it as soon as you can!");
                    }
                }
                else
                {
                    LogWriteLine("ATA is up to date!");
                }
            }
            catch
            {
                LogWriteLine("Timeout Error occurred while connecting to the Server!");
                LogWriteLine("Open settings to check if a new version is avaiable!");
            }
        }

        private void backgroundWorkerSync_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object paramObj = e.Argument;
            string paramObjTmp = "0";

            if (paramObj.ToString() == "3")
            {
                paramObjTmp = "1";
            }
            if (checkAdbFastboot(0))
            {
                LogWriteLine("Checking device...");
                string version = adbFastbootCommandR(new[] { "-s " + Regex.Replace(CurrentDeviceSelected, @"\s", "") + " shell getprop ro.build.version.release" }, 0);
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
                                    string[] arrayDeviceInfoCommands = { "-s "+ CurrentDeviceSelected +" shell getprop ro.build.version.release", "-s "+ CurrentDeviceSelected +" shell getprop ro.build.user",  "-s "+ CurrentDeviceSelected +" shell getprop ro.product.cpu.abilist", "-s "+ CurrentDeviceSelected +" shell getprop ro.product.manufacturer" , "-s "+ CurrentDeviceSelected +" shell getprop ro.product.model",
                                           "-s "+ CurrentDeviceSelected +" shell getprop ro.product.board", "-s "+ CurrentDeviceSelected +" shell getprop ro.product.device", "-s "+ CurrentDeviceSelected +" shell ip route"};
                                    string deviceinfo = adbFastbootCommandR(arrayDeviceInfoCommands, 0);
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
                                            if (CurrentDeviceSelected.Contains("192"))
                                            {
                                                labelStatus.Text = "Wireless";
                                                buttonConnectToIP.Enabled = false;
                                                buttonDisconnectIP.Enabled = true;
                                                deviceWireless = true;
                                            }
                                            else
                                            {
                                                labelStatus.Text = "Cable";
                                                buttonConnectToIP.Enabled = true;
                                                buttonDisconnectIP.Enabled = false;
                                                deviceWireless = false;
                                            }
                                            if (int.Parse(labelAV.Text) > 7)
                                            {
                                                labelUser.Text = user = Regex.Replace(adbFastbootCommandR(new[] { "-s " + CurrentDeviceSelected + " shell am get-current-user" }, 0), @"\t|\n|\r", "");
                                            }
                                            else
                                            {
                                                MessageShowBox("Feature currently not avaiable for device under android 8", 2);
                                            }
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
                                        LogWriteLine("Error: failed to extract device info!");
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
                                    command = !systemApp
                                        ? (new[] { "-s " + CurrentDeviceSelected + " shell pm list packages -3 --user " + user })
                                        : (new[] { "-s " + CurrentDeviceSelected + " shell pm list packages -s --user " + user });
                                    if ((stringApk = adbFastbootCommandR(command, 0)) != null)
                                    {
                                        LogWriteLine("Loading apps...");
                                        sortApks(stringApk.Split('\n'));
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
                                    if ((stringApk = adbFastbootCommandR(new[] { "-s " + CurrentDeviceSelected + " shell pm list packages -3 --user " + user }, 0)) != null)
                                    {
                                        arrayApkTmp.AddRange(stringApk.Split('\n'));
                                    }
                                    else
                                    {
                                        MessageShowBox("Error during apk loading", 0);
                                        break;
                                    }
                                    if ((stringApk = adbFastbootCommandR(new[] { "-s " + CurrentDeviceSelected + " shell pm list packages -s --user " + user }, 0)) != null)
                                    {
                                        arrayApkTmp.AddRange(stringApk.Split('\n'));
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
                                    if ((stringApk = adbFastbootCommandR(new[] { "-s " + CurrentDeviceSelected + " shell pm list packages -u --user " + user }, 0)) != null)
                                    {
                                        if ((stringInstalledApk = adbFastbootCommandR(new[] { "-s " + CurrentDeviceSelected + " shell pm list packages --user " + user }, 0)) != null)
                                        {
                                            List<string> diff;
                                            IEnumerable<string> set1 = stringApk.Split('\n').Distinct();
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
                            LogWriteLine("Error device not found!");
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
                        LogWriteLine("Error device not found!");
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
            buttonMobileScreenShare.Enabled = enable;
            groupBoxDeviceInfo.Enabled = enable;
            groupBoxRebootMenu.Enabled = enable;
            groupBoxAPKMenu.Enabled = enable;
            buttonDeviceLogs.Enabled = enable;
            buttonTaskManager.Enabled = enable;
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
            if (textBoxSearch.Text == "Search" && !textboxClear)
            {
                textBoxSearch.Clear();
                textboxClear = true;
            }
        }

        private void buttonRS_Click(object sender, EventArgs e)
        {
            if (!deviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                rebootSmartphone();
            }
        }

        private void buttonRR_Click(object sender, EventArgs e)
        {
            if (!deviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _ = systemCommand("adb -s " + CurrentDeviceSelected + " reboot recovery");
                LogWriteLine("Rebooted!");
            }
        }

        private void buttonRF_Click(object sender, EventArgs e)
        {
            if (!deviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _ = systemCommand("adb -s " + CurrentDeviceSelected + " reboot-bootloader");
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

            if (File.Exists(IPFileName))
            {
                foreach (string ip in File.ReadAllLines(IPFileName))
                {
                    _ = IPList.Add(ip.Trim());
                }
                comboBoxIP.Items.AddRange(IPList.ToArray());
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
            if (checkAdbFastboot(0))
            {
                List<string> devicesTmp;
                int deviceCounter = 0;
                Device deviceTmp;

                string dev = adbFastbootCommandR(new[] { "devices" }, 0);
                if (dev != null)
                {
                    devices.Clear();
                    devicesTmp = dev.Substring("List of devices attached".Length).Split().ToList();

                    for (int i = 0; i < devicesTmp.Count; i++)
                    {
                        deviceTmp = new Device();

                        if (Regex.Match(devicesTmp[i], "^[a-zA-Z0-9.:]*$").Success && devicesTmp[i].Length > 2 && devicesTmp[i] != "recovery" && devicesTmp[i] != "device")
                        {
                            deviceTmp.Name = adbFastbootCommandR(new[] { "-s " + Regex.Replace(devicesTmp[i], @"\s", "") + " shell getprop ro.product.model" }, 0);
                            deviceTmp.Serial = devicesTmp[i];
                            deviceCounter++;

                            devices.Add(deviceTmp);
                        }
                    }
                    if (devices.Count == 0)
                    {
                        disableEnableSystem(false);
                        return;
                    }
                    foreach (Device device in devices)
                    {
                        _ = comboBoxDevices.Items.Add(device.Name);
                    }
                    comboBoxDevices.SelectedIndex = 0;
                    CurrentDeviceSelected = Regex.Replace(devices[0].Serial, @"\s", "");
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
                LogWriteLine("You are offline");
                disableSystem(true);
            }
            else
            {
                connected = true;
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
            labelSelectedAppCount.Text = "Selected App: " + checkedListBoxApp.CheckedItems.Count;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            checkedListBoxApp.Items.Clear();
            foreach (string str in arrayApks)
            {
                if (str.Contains(textBoxSearch.Text))
                {
                    _ = checkedListBoxApp.Items.Add(str);
                }
            }
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
                    if ((log = adbFastbootCommandR(new[] { " -s " + CurrentDeviceSelected + " " + command1 + "--user " + user + " " + current + command2 }, 0)) != null)
                    {
                        if (type == 1)
                        {
                            ScrollableMessageBox.show(log, "Granted permissions");
                            continue;
                        }
                        LogWriteLine("Command injected!");
                    }
                    else
                    {
                        LogWriteLine("Command failed!");
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
                if (textBoxDirFile.TextLength > 0)
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
            string log = adbFastbootCommandR(new[] { "sideload \"" + textBoxDirFile.Text + "\"" }, 0);
            if (log.ToLower().Contains("error") || log.ToLower().Contains("failed") || log == "")
            {
                LogWriteLine(fileName + " failed to flash");
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

        private void buttonFlashImg_Click(object sender, EventArgs e)
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

            if ((log = adbFastbootCommandR(new[] { command + textBoxDirImg.Text }, 1)) != null)
            {
                LogWriteLine(log);
            }
            else
            {
                MessageShowBox("Something went wrong!", 0);
            }
        }

        private bool checkAdbFastboot(int exeN)
        {
            string exeTmp = "adb.exe";
            if (exeN == 1)
            {
                exeTmp = "fastboot.exe";
            }
            return File.Exists(exeTmp) && File.Exists("AdbWinUsbApi.dll") && File.Exists("AdbWinApi.dll");
        }

        private void buttonRebootToSystem_Click(object sender, EventArgs e)
        {
            if (checkAdbFastboot(1))
            {
                _ = systemCommand("fastboot reboot");
                LogWriteLine("Rebooted!");
            }
            else
            {
                MessageShowBox("Fastboot not found!", 0);
            }
        }

        private void buttonHardReset_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to erase your phone?", "Erase Phone", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (checkAdbFastboot(1))
                {
                    LogWriteLine("Erasing process started...");
                    _ = systemCommand("fastboot erase userdata && fastboot erase cache");
                    LogWriteLine("Erasing process finished!!");
                }
                else
                {
                    MessageShowBox("Fastboot not found!", 0);
                }
            }
        }

        private void rebootSmartphone()
        {
            LogWriteLine("Rebooting smartphone...");
            _ = systemCommand("adb -s " + CurrentDeviceSelected + " reboot");
            LogWriteLine("Smartphone rebooted");
        }

        private void buttonRebootRecovery_Click(object sender, EventArgs e)
        {
            LogWriteLine("Rebooting smartphone...");
            _ = systemCommand("fastboot reboot recovery");
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
            string command = "adb -s " + CurrentDeviceSelected + " shell pm uninstall -k --user " + user + " ";
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
                        command = "adb -s " + CurrentDeviceSelected + " shell pm enable --user " + user + " ";
                        commandName = "Enabled:";
                        break;
                    case 0:
                        command = "adb -s " + CurrentDeviceSelected + " shell pm disable-user --user " + user + " ";
                        commandName = "Disabled:";
                        break;
                    case 2:
                        command = "adb -s " + CurrentDeviceSelected + " shell pm clear --user " + user + " ";
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
            systemApp = false;
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
            systemApp = true;
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
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                string fileName;
                foreach (string fileLoc in filePaths)
                {
                    fileName = fileLoc.Substring(fileLoc.LastIndexOf('\\') + 1);
                    if (fileLoc.ToLower().Contains(".apk"))
                    {
                        if (File.Exists(fileLoc))
                        {
                            if (adbFastbootCommandR(new[] { "-s " + CurrentDeviceSelected + " install -r --user " + user + " \"" + fileLoc + "\"" }, 0) != null)
                            {
                                LogWriteLine(fileName + " installed!");
                                MessageShowBox(fileName + " installed", 2);
                            }
                            else
                            {
                                LogWriteLine(fileName + " installation failed");
                                MessageShowBox(fileName + " installation failed", 0);
                            }
                        }
                    }
                    else
                    {
                        MessageShowBox(fileName + " is not an apk file", 0);
                    }
                }
            }
        }

        private void groupBox6_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void checkedListBoxApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelSelectedAppCount.Text = "Selected App: " + checkedListBoxApp.CheckedItems.Count;
        }

        private void checkGrantedPermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell dumpsys package --user " + user, null, 1);
        }

        private void groupBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadingForm loading = new LoadingForm(filePaths.ToList(), CurrentDeviceSelected);
                _ = loading.ShowDialog();
            }
        }

        private void groupBox2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void comboBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentDeviceSelected = Regex.Replace(devices[comboBoxDevices.SelectedIndex].Serial, @"\s", "");
        }

        private void buttonReloadDevicesList_Click(object sender, EventArgs e)
        {
            reloadList();
        }

        private void reloadList()
        {
            if (!checkAdbFastboot(0))
            {
                adbDownload();
            }
            comboBoxDevices.Items.Clear();
            devices.Clear();
            DevicesListUpdate();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            comboBoxDevices.Enabled = true;
            buttonReloadDevicesList.Enabled = true;

            if (tabControl1.SelectedTab.Text != "System")
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
            if (!connected)
            {
                MessageShowBox("You are offline, ATA can't download ADB", 0);
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
                            _ = systemCommand("taskkill /f /im adb.exe");
                            _ = systemCommand("taskkill /f /im fastboot.exe");
                            _ = systemCommand("del adb.exe");
                            _ = systemCommand("del AdbWinUsbApi.dll");
                            _ = systemCommand("del AdbWinApi.dll");
                            _ = systemCommand("del fastboot.exe");
                            _ = systemCommand("move platform-tools\\adb.exe \"%cd%\"");
                            _ = systemCommand("move platform-tools\\AdbWinUsbApi.dll \"%cd%\"");
                            _ = systemCommand("move platform-tools\\AdbWinApi.dll \"%cd%\"");
                            _ = systemCommand("move platform-tools\\fastboot.exe \"%cd%\"");
                            File.Delete("sdkplatformtool.zip");
                            _ = systemCommand("rmdir /Q /S platform-tools");
                            LogWriteLine("ATA ready!");
                        }
                        catch
                        {
                            LogWriteLine("Error during sdk platform tool download!");
                            MessageShowBox("Error during sdk platform tool download!", 0);
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
                    CurrentDevice = CurrentDeviceSelected
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
                LoadingForm load = new LoadingForm(apps, "adb -s " + CurrentDeviceSelected + " shell cmd package install-existing --user " + user + " ", "Restored:");
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
            systemApp = false;
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
                _ = systemCommand("start scrcpy");
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

        private void backgroundWorkerExeDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!connected)
            {
                MessageShowBox("You are offline, ATA can't download scrcpy", 0);
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
                                    _ = systemCommand("robocopy " + newDirectories[i] + " " + Path.GetDirectoryName(Application.ExecutablePath) + " /E");
                                    LogWriteLine("scrcpy Extracted!");
                                    LogWriteLine("Getting things ready...");
                                    _ = systemCommand("rmdir /s /q " + newDirectories[i]);
                                    _ = systemCommand("del scrcpy.zip");
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
                    _ = systemCommand("taskkill /f /im " + FILEADB);
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
            DeviceLogs deviceLog = new DeviceLogs(CurrentDeviceSelected);
            deviceLog.Show();
        }

        private void pictureBoxSearchFile_Click(object sender, EventArgs e)
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

        private void pictureBoxSearchFile_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxSearchFile.BackColor = Color.DarkGray;
        }

        private void pictureBoxSearchFile_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxSearchFile.BackColor = Color.Transparent;
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
            systemApp = true;
        }

        private void toolStripMenuItemADBKill_Click(object sender, EventArgs e)
        {
            _ = systemCommand("taskkill /f /im " + FILEADB);
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
            int deviceCountTmp = devices.Count;
            string ip = string.Empty;

            _ = Invoke((Action)delegate
            {
                ip = comboBoxIP.Text.Trim();
            });

            if (ip.Length > 1)
            {
                if (CurrentDeviceSelected.Length > 0)
                {
                    _ = adbFastbootCommandR(new[] { " -s " + CurrentDeviceSelected + " tcpip 5555 " }, 0);
                }

                _ = systemCommand("adb connect " + ip);

                Thread.Sleep(1000);

                _ = Invoke((Action)delegate
                {
                    reloadList();
                });

                if (deviceCountTmp < devices.Count)
                {
                    _ = Invoke((Action)delegate
                    {
                        if (!IPList.Contains(ip))
                        {
                            _ = IPList.Add(ip);
                            _ = comboBoxIP.Items.Add(ip);

                            using (StreamWriter writer = new StreamWriter(IPFileName, true))
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
            int deviceCountTmp = devices.Count;
            string ip = string.Empty;

            _ = Invoke((Action)delegate
            {
                ip = comboBoxIP.Text.Trim();
            });

            if (ip.Length > 1)
            {
                _ = systemCommand("adb disconnect " + ip);

                Thread.Sleep(1000);

                _ = Invoke((Action)delegate
                {
                    reloadList();
                });

                if (deviceCountTmp > devices.Count)
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
                    MessageShowBox(ip + "Failed to disconnect", 0);
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
                foreach (string file in openFileDialogAPK.FileNames)
                {
                    try
                    {
                        LogWriteLine("Installing " + file.Substring(file.LastIndexOf('\\') + 1));
                        if (adbFastbootCommandR(new[] { "install -r " + command + " --user " + user + " \"" + file + "\"" }, 0).Contains("Success"))
                        {
                            LogWriteLine(file.Substring(file.LastIndexOf('\\') + 1) + " installed");
                        }
                        else
                        {
                            LogWriteLine(file.Substring(file.LastIndexOf('\\') + 1) + " not installed");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageShowBox(ex.Message, 0);
                    }
                }
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
                LoadingForm loadingForm = new LoadingForm(CurrentDeviceSelected, arrayApkSelect);
                _ = loadingForm.ShowDialog();
                checkedListBoxApp.ClearSelected();
            }
            else
            {
                MessageShowBox("No app selected!", 1);
            }
        }
    }
}
