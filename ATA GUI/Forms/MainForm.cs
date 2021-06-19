using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Net;
using Ionic.Zip;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Drawing;

namespace ATA_GUI
{
    public partial class MainForm : Form
    {
        private readonly List<string> arrayApks = new List<string>();
        private static readonly int WM_NCLBUTTONDOWN = 0xA1;
        private static readonly int HT_CAPTION = 0x2;
        private bool textboxClear;
        private readonly string FILEADB = "adb.exe";
        private bool systemApp;
        private readonly List<Device> devices = new List<Device>();
        private string currentDeviceSelected = string.Empty;
        private bool deviceWireless;
        private bool allApk;
        private string stringApk; 
        private string stringApkS;
        private bool connected = true;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private const string CURRENTVERSION = "v.1.7.1";
        private static readonly Regex regex = new Regex(@"\s+");

        public static string RemoveWhiteSpaces(string str)
        {
            return regex.Replace(str, String.Empty);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void panelTopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void buttonSyncApp_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.Contains("System"))
            {
                syncFun(0);
                return;
            }
            if (tabControl1.SelectedTab.Name.Contains("Fastboot"))
            {
                if (checkAdbFastboot(1))
                {
                    if (adbFastbootCommandR(new [] { "devices" }, 1).Contains("fastboot"))
                    {
                        string[] log = adbFastbootCommandR(new [] { "getvar all" }, 1).Split(' ', '\n');
                        for (int a=0; a< log.Count(); a++)
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
                                labelBootloaderStatus.Text = log[a].Contains("yes") ?  "Yes" : "No";
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
                    MessageShowBox("Error fastboot not found", 0);
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
            Invoke((Action)delegate
            {
                richTextBoxLog.Text += str + "\n";
                richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                richTextBoxLog.ScrollToCaret();
            });
        }

        static public void systemCommand(string command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }

        static public string adbFastbootCommandR(string[] args, int type)
        {
            StringBuilder ret = new StringBuilder();
            string line;
            Cursor.Current = Cursors.WaitCursor;
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            switch(type)
            {
                case 0:
                    startInfo.FileName = "adb.exe";
                    process.StartInfo = startInfo;
                                        
                    foreach (string s in args)
                    {
                        startInfo.Arguments = s;
                        process.Start();
                        line = process.StandardOutput.ReadToEnd();
                        if (line.Length > 0) { ret.Append(line); }
                        process.Close();
                    }
                    break;
                case 1:
                    startInfo.FileName = "fastboot.exe";
                    process.StartInfo = startInfo;
                    
                    foreach (string s in args)
                    {
                        startInfo.Arguments = s;
                        process.Start();
                        line = process.StandardError.ReadToEnd();
                        if (line.Length > 0) { ret.Append(line + "\n"); }

                        line = process.StandardOutput.ReadToEnd();
                        if (line.Length > 0) { ret.Append(line + "\n"); }
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
                    MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case 1:
                    MessageBox.Show(message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 2:
                    MessageBox.Show(message, "Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    MessageShowBox("Error in MessageShowBox", 0);
                    break;
            }
        }

        private async void updateCheckAsync()
        {
            Release currentRelease = new Release();
            Release latestRelease = new Release();
            string json;
            try
            {
                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(5);
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
                if ((latestRelease.Number > currentRelease.Number) || ((latestRelease.Number == currentRelease.Number) && (currentRelease.Pre && !latestRelease.Pre)))
                {
                    if (MessageBox.Show("Update found, do you want to update it?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start((string)jsonReal[0]["html_url"]);
                        UpdateForm update = new UpdateForm(linkString);
                        update.ShowDialog();
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
            Object paramObj = e.Argument as Object;
            String paramObjTmp = "0";

            if (paramObj.ToString()=="3" )
            {
                paramObjTmp = "1";
            }
            if (checkAdbFastboot(0))
            {
                LogWriteLine("Checking device...");
                string version = adbFastbootCommandR(new[] { "-s " + Regex.Replace(currentDeviceSelected, @"\s", "") + " shell getprop ro.build.version.release" }, 0);
                if (version != null)
                {
                    if(version.Any(char.IsDigit))
                    { 
                        int count = 1;
                        if(paramObj.ToString()=="3")
                        {
                            count = 2;
                            paramObj = "0";
                        }
                        for(int a = 0; a<count; a++)
                        {
                            if (a==1)
                            {
                                paramObj = "2";
                            }
                            switch (paramObj.ToString())
                            {
                                case "0":
                                    string[] arrayDeviceInfoCommands = { "-s "+ currentDeviceSelected +" shell getprop ro.build.version.release", "-s "+ currentDeviceSelected +" shell getprop ro.build.user",  "-s "+ currentDeviceSelected +" shell getprop ro.product.cpu.abilist", "-s "+ currentDeviceSelected +" shell getprop ro.product.manufacturer" , "-s "+ currentDeviceSelected +" shell getprop ro.product.model",
                                           "-s "+ currentDeviceSelected +" shell getprop ro.product.board", "-s "+ currentDeviceSelected +" shell getprop ro.product.device", "-s "+ currentDeviceSelected +" shell ip route"};
                                    string deviceinfo = adbFastbootCommandR(arrayDeviceInfoCommands, 0);
                                    string[] arrayDeviceInfo = deviceinfo.Split('\n');
                                    if (arrayDeviceInfo.Length > 5)
                                    {
                                        disableSystem(false);
                                        Invoke((Action)delegate
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
                                                if(arrayDeviceInfo[7].Length > 4)
                                                {
                                                    textBoxIP.Text = labelIP.Text = arrayDeviceInfo[7].Substring(arrayDeviceInfo[7].IndexOf("src") + 4);
                                                }
                                                if (labelIP.Text.Contains("t of devices attached") || arrayDeviceInfo[7].Length == 0)
                                                {
                                                    labelIP.Text = "Not connected to a network";
                                                    textBoxIP.Text = "";
                                                    buttonConnectToIP.Enabled = false;
                                                    buttonDisconnectIP.Enabled = false;
                                                }
                                            }
                                            if(currentDeviceSelected.Contains("192"))
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
                                            LogWriteLine("Device info extracted");
                                            disableEnableSystem(true);
                                        });
                                    }
                                    else
                                    {
                                        Invoke((Action)delegate
                                        {
                                            disableEnableSystem(false);
                                            buttonDisconnectIP.Enabled = false;
                                        });
                                        LogWriteLine("Error: failed to extract device info!");
                                    }
                                    break;
                                case "2":
                                    arrayApks.Clear(); 
                                    Invoke((Action)delegate
                                    {
                                        labelSelectedAppCount.Text = "Selected App: 0";
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    string[] command;
                                    if (!systemApp)
                                    {
                                        command = new[] { "-s " + currentDeviceSelected + " shell pm list packages -3" };
                                    }
                                    else
                                    {
                                        command = new[] { "-s " + currentDeviceSelected + " shell pm list packages -s" };
                                    }
                                    if ((stringApk = adbFastbootCommandR(command,0))!=null)
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
                                    var arrayApkTmp = new List<string>();
                                    arrayApks.Clear();
                                    Invoke((Action)delegate
                                    {
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    LogWriteLine("Loading apps...");
                                    if ((stringApk = adbFastbootCommandR(new[] { "-s " + currentDeviceSelected + " shell pm list packages -3"}, 0)) != null)
                                    {
                                        arrayApkTmp.AddRange(stringApk.Split('\n'));
                                    }
                                    else
                                    {
                                        MessageShowBox("Error during apk loading", 0);
                                        break;
                                    }
                                    if ((stringApkS = adbFastbootCommandR(new[] { "-s " + currentDeviceSelected + " shell pm list packages -s" }, 0)) != null)
                                    {
                                        arrayApkTmp.AddRange(stringApkS.Split('\n'));
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
                                    Invoke((Action)delegate
                                    {
                                        checkedListBoxApp.Items.Clear();
                                    });
                                    LogWriteLine("Loading apps...");
                                    if ((stringApk = adbFastbootCommandR(new[] { "-s " + currentDeviceSelected + " shell pm list packages -u" }, 0)) != null)
                                    {
                                        if ((stringInstalledApk = adbFastbootCommandR(new[] { "-s " + currentDeviceSelected + " shell pm list packages" }, 0)) != null)
                                        {
                                            List<string> diff;
                                            IEnumerable<string> set1 = stringApk.Split('\n').Distinct();
                                            IEnumerable<string> set2 = stringInstalledApk.Split('\n').Distinct();

                                            if (set2.Count() > set1.Count())
                                            {
                                                diff = set2.Except(set1).ToList();
                                            }
                                            else
                                            {
                                                diff = set1.Except(set2).ToList();
                                            }
                                            
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
                        Invoke((Action)delegate
                        {
                            disableEnableSystem(false);
                            buttonDisconnectIP.Enabled = false;
                            LogWriteLine("Error device not found!");
                            if (paramObjTmp == "0")
                            {
                                MessageShowBox("Error device not found!", 0);
                            }
                        });
                    }
                }
                else
                {
                    Invoke((Action)delegate
                    {
                        disableEnableSystem(false);
                        buttonDisconnectIP.Enabled = false;
                        LogWriteLine("Error device not found!");
                        if (paramObjTmp == "0")
                        {
                            MessageShowBox("Error device not found!", 0);
                        }
                    });
                }
            }
            else
            {
                adbDownload();
            }            
        }

        private void sortApks(string [] arrayApkTmp)
        {
            foreach (string line in arrayApkTmp)
            {
                if (line.Contains("package:"))
                {
                    arrayApks.Add(line.Substring(8));
                }
            }
            arrayApks.Sort();
            Invoke((Action)delegate
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
        }

        private void adbDownload()
        {
            try
            {
                backgroundWorkerAdbDownloader.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                MessageShowBox("Error: " + ex, 0);
            }
        }

        private void textboxClick(object sender, EventArgs e)
        {
            if(textBoxSearch.Text == "Search" && !textboxClear)
            {
                textBoxSearch.Clear();
                textboxClear = true;
            }
        }

        private void buttonRS_Click(object sender, EventArgs e)
        {
            if(!deviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { 
                rebootSmartphone();
            }
        }

        private void buttonRR_Click(object sender, EventArgs e)
        {
            if (!deviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { 
                systemCommand("adb -s " + currentDeviceSelected + " reboot recovery");
                LogWriteLine("Rebooted!");
            }
        }

        private void buttonRF_Click(object sender, EventArgs e)
        {
            if (!deviceWireless || MessageBox.Show("Adb is not able to check if the device rebooted via wireless mode, do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { 
                systemCommand("adb -s " + currentDeviceSelected + " reboot-bootloader");
                LogWriteLine("Rebooted!");
            }
        }

        public void ToolTipGenerator(Control button, string title, string message)
        {
            ToolTip buttonToolTip = new ToolTip();
            buttonToolTip.ToolTipTitle = title;
            buttonToolTip.UseFading = true;
            buttonToolTip.UseAnimation = true;
            buttonToolTip.IsBalloon = true;
            buttonToolTip.ShowAlways = true;
            buttonToolTip.AutoPopDelay = 0;
            buttonToolTip.InitialDelay = 0;
            buttonToolTip.ReshowDelay = 0;
            buttonToolTip.SetToolTip(button, message);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ToolTipGenerator(buttonConnectToIP, "Connect device", "Connect to your device with this IP");
            ToolTipGenerator(buttonDisconnectIP, "Disconnect device", "Disconnect from your device with this IP");
            ToolTipGenerator(buttonMobileScreenShare, "Share Device Screen", "Your device screen will be shared on your PC");
            ToolTipGenerator(buttonKillAdb, "Kill Adb", "If adb is still running it will be killed");
            ToolTipGenerator(buttonLogClear, "Clear log", "Log will be clean from the box");
            ToolTipGenerator(buttonRR, "Reboot to recovery", "Your device will be rebooted to recovery");
            ToolTipGenerator(buttonRS, "Reboot to system", "Your device will be rebooted to system");
            ToolTipGenerator(buttonRF, "Reboot to fastboot", "Your device will be rebooted to fastboot");
            ToolTipGenerator(buttonReloadDevicesList, "Reload devices", "The devices list will be updated");
            ToolTipGenerator(buttonSyncApp, "Sync smartphone", "All the info needed will be extracted from your smartphone");
            ToolTipGenerator(buttonRebootRecovery, "Reboot to recovery", "Your device will be rebooted to recovery");
            ToolTipGenerator(groupBox2, "File Transfer", "Drop a file in this box and it will be trasfered in a new folder called ATA inside your device");
            ToolTipGenerator(groupBox6, "Apk Installer", "Drop an apk file and it will be installed inside your device");

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
                disclaimer.ShowDialog();
            }
            panelFastboot.Enabled = false;
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Normal;
            this.Focus();
        }

        private void DevicesListUpdate()
        {
            if (checkAdbFastboot(0))
            {
                List<string> devicesTmp;
                int deviceCounter = 0;

                string dev = adbFastbootCommandR(new[] { "devices" }, 0);
                if (dev != null)
                {
                    devices.Clear();
                    devicesTmp = dev.Substring("List of devices attached".Length).Split('	').ToList();
                    for (int i = 0; i < devicesTmp.Count; i++)
                    {
                        Device deviceTmp = new Device();
                        
                        if(Regex.Match(devicesTmp[i], "^(?:\\w*)").Success && !devicesTmp[i].Contains("recovery") && !devicesTmp[i].Contains("device") && devicesTmp[i] != "\r\n\r\n")
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
                    for (int i = 0; i < devices.Count; i++)
                    {
                        comboBoxDevices.Items.Add(devices[i].Name);
                    }
                    comboBoxDevices.SelectedIndex = 0;
                    currentDeviceSelected = Regex.Replace(devices[0].Serial, @"\s", "");
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
            catch {}
            return false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LogWriteLine("Checking connection...");
            toolStripButtonRestoreApp.Enabled = false;

            if(!pingCheck())
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
            updateCheckAsync();
        }

        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSelectAll.Checked)
            {
                for (int i = 0; i < checkedListBoxApp.Items.Count; i++)
                    checkedListBoxApp.SetItemCheckState(i, System.Windows.Forms.CheckState.Checked);
            }
            else
            {
                for (int i = 0; i < checkedListBoxApp.Items.Count; i++)
                    checkedListBoxApp.SetItemCheckState(i, System.Windows.Forms.CheckState.Unchecked);
            }
            labelSelectedAppCount.Text = "Selected App: " + checkedListBoxApp.CheckedItems.Count;
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            checkedListBoxApp.Items.Clear();
            foreach (string str in arrayApks)
            {
                if (str.Contains(textBoxSearch.Text))
                    checkedListBoxApp.Items.Add(str);
            }
        }

        private void buttonConnectToIP_Click(object sender, EventArgs e)
        {
            if (textBoxIP.TextLength > 1)
            {
                string FILEName = "status.tmp";
                systemCommand("adb tcpip 5555");
                systemCommand("adb connect " + textBoxIP.Text + " | findstr \"" + textBoxIP.Text + ":5555\" && if %ERRORLEVEL%==0 0 > " + FILEName);
                if (File.Exists(FILEName))
                {
                    MessageShowBox("connected to " + textBoxIP.Text, 2);
                    buttonConnectToIP.Enabled = false;
                    buttonDisconnectIP.Enabled = true;
                }
                else
                {
                    MessageShowBox("Failed to connect to " + textBoxIP.Text, 0);
                }
                File.Delete(FILEName);
                syncFun(0);
            }
        }

        private void buttonDisconnectIP_Click(object sender, EventArgs e)
        {
            if (textBoxIP.TextLength > 1)
            {
                string FILEName = "status.tmp";
                systemCommand("adb disconnect " + textBoxIP.Text + " | findstr \"disconnect\" && if %ERRORLEVEL%==0 0 > " + FILEName);
                if (File.Exists(FILEName))
                {
                    MessageShowBox(textBoxIP.Text + " disconnected", 2);
                    buttonConnectToIP.Enabled = true;
                    buttonDisconnectIP.Enabled = false;
                }
                else
                {
                    MessageShowBox(textBoxIP.Text + "Failed to disconnect", 0);
                }
                File.Delete(FILEName);
                syncFun(0);
            }
        }

        private void appFunc(string command1, string command2, int type)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                foreach (Object list in checkedListBoxApp.CheckedItems)
                {
                    string log;
                    if((log = adbFastbootCommandR(new [] { " -s " + currentDeviceSelected + " " + command1 + list.ToString() + command2 }, 0))!=null)
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

        private void buttonSearchFile_Click(object sender, EventArgs e)
        {

        }

        private void buttonInstallZip_Click(object sender, EventArgs e)
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

        private void backgroundWorkerZip_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Invoke((Action)delegate
            {
                string fileName = textBoxDirFile.Text.Substring(textBoxDirFile.Text.LastIndexOf('\\') + 1);
                LogWriteLine("Installing " + fileName);
                string log = adbFastbootCommandR(new string[] { "sideload \"" + textBoxDirFile.Text + "\"" }, 0);
                if (log.ToLower().Contains("error") || log.ToLower().Contains("failed") || log=="")
                {
                    LogWriteLine(fileName + " failed to flash");
                }
                else
                {
                    LogWriteLine(fileName + " flashed");
                }
            });
        }

        private void buttonSearchFileFastboot_Click(object sender, EventArgs e)
        {
            this.openFileDialogZip.Filter = "Img files *.img|*.img;";
            this.openFileDialogZip.FileName = "";
            this.openFileDialogZip.Multiselect = false;
            this.openFileDialogZip.Title = "Select Img";

            DialogResult dr = this.openFileDialogZip.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxDirImg.Text = this.openFileDialogZip.FileName;
            }
        }

        private void buttonFlashImg_Click(object sender, EventArgs e)
        {
            if (textBoxDirImg.Text.Length > 0)
            {
                if (radioButtonBoot.Checked)    flash(0);
                if (radioButtonBootloader.Checked)  flash(1);
                if (radioButtonCache.Checked)   flash(2);
                if (radioButtonRadio.Checked)   flash(3);
                if (radioButtonRecovery.Checked)flash(4);
                if (radioButtonRom.Checked)     flash(5);
                if (radioButtonSystem.Checked)  flash(6);
                if (radioButtonVendor.Checked)  flash(7);
                MessageShowBox("Please select where to flash the img", 1);
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
            string command = string.Empty;
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
            if (exeN==1)
            {
                exeTmp = "fastboot.exe";
            }
            return File.Exists(exeTmp) && File.Exists("AdbWinUsbApi.dll") && File.Exists("AdbWinApi.dll");
        }

        private void buttonKillAdb_Click(object sender, EventArgs e)
        {
            systemCommand("taskkill /f /im "+ FILEADB);
            MessageShowBox("Adb.exe killed!", 2);
        }

        private void buttonRebootToSystem_Click(object sender, EventArgs e)
        {
            if (checkAdbFastboot(1))
            {
                systemCommand("fastboot reboot");
                LogWriteLine("Rebooted!");
            }
            else
            {
                MessageShowBox("Error fastboot not found!", 0);
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
                    systemCommand("fastboot erase userdata && fastboot erase cache");
                    LogWriteLine("Erasing process finished!!");
                }
                else
                {
                    MessageShowBox("Error fastboot not found!", 0);
                }
            }
        }

        private void rebootSmartphone()
        {
            LogWriteLine("Rebooting smartphone...");
            systemCommand("adb -s " + currentDeviceSelected + " reboot");
            LogWriteLine("Smartphone rebooted");
        }

        private void buttonRebootRecovery_Click(object sender, EventArgs e)
        {
            LogWriteLine("Rebooting smartphone...");
            systemCommand("fastboot reboot recovery");
            LogWriteLine("Smartphone rebooted");
        }

        private void buttonBootloaderMenu_Click(object sender, EventArgs e)
        {
            BootloaderMenu bootloaderMenu = new BootloaderMenu();
            bootloaderMenu.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!allApk)
                {
                    syncFun(2);
                }
                else
                {
                    syncFun(4);
                }
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.ToString(), 1);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.openFileDialogAPK.Filter = "Apk files *.Apk|*.apk;";
            this.openFileDialogAPK.FileName = "";
            this.openFileDialogAPK.Multiselect = true;
            this.openFileDialogAPK.Title = "Select Apk";

            DialogResult dr = this.openFileDialogAPK.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String file in openFileDialogAPK.FileNames)
                {
                    try
                    {
                        LogWriteLine("Installing " + file.Substring(file.LastIndexOf('\\') + 1));
                        systemCommand("adb install -r \"" + file + "\"");
                        LogWriteLine(file.Substring(file.LastIndexOf('\\') + 1) + " installed");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        public void uninstaller(CheckedListBox.CheckedItemCollection foundPackageList)
        {
            if (allApk)
            {
                LoadingForm load;
                foreach (Object list in foundPackageList)
                {
                    if (stringApk.Contains(list.ToString()))
                    {
                        load = new LoadingForm(new List<string> { list.ToString() }, "adb -s " + currentDeviceSelected + " uninstall ", "Uninstalled:");
                        load.ShowDialog();
                        if (load.DialogResult != DialogResult.OK)
                        {
                            MessageShowBox("Error during uninstallation process", 0);
                        }
                    }
                    else if (stringApkS.Contains(list.ToString()))
                    {
                        load = new LoadingForm(new List<string> { list.ToString() }, "adb -s " + currentDeviceSelected + " shell pm uninstall -k --user 0 ", "Uninstalled:");
                        load.ShowDialog();
                        if (load.DialogResult != DialogResult.OK)
                        {
                            MessageShowBox("Error during uninstallation process", 0);
                        }
                    }
                    else
                    {
                        MessageShowBox("Error during uninstallation process", 0);
                    }
                }
                syncFun(4);
                checkBoxSelectAll.Checked = false;
            }
            else
            {
                string command;
                List<string> arrayApkSelect = new List<string>();
                if (!systemApp)
                {
                    command = "adb -s " + currentDeviceSelected + " uninstall ";
                }
                else
                {
                    command = "adb -s " + currentDeviceSelected + " shell pm uninstall -k --user 0 ";
                }
                foreach (Object list in foundPackageList)
                {
                    arrayApkSelect.Add(list.ToString());
                }
                LoadingForm load = new LoadingForm(arrayApkSelect, command, "Uninstalled:");
                load.ShowDialog();
                if (load.DialogResult != DialogResult.OK)
                {
                    MessageShowBox("Error during uninstallation process", 0);
                }
                syncFun(2);
                checkBoxSelectAll.Checked = false;
            }
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
                string command = string.Empty;
                string commandName = string.Empty;
                List<string> arrayApkSelect = new List<string>();
                foreach (Object list in checkedListBoxApp.CheckedItems)
                {
                    arrayApkSelect.Add(list.ToString());
                }

                PackageMenu packageMenu = new PackageMenu(arrayApkSelect);
                packageMenu.ShowDialog();
                switch (packageMenu.DialogResult1)
                {
                    case 1:
                        command = "adb -s " + currentDeviceSelected + " shell pm enable ";
                        commandName = "Enabled:";
                        break;
                    case 0:
                        command = "adb -s " + currentDeviceSelected + " shell pm disable-user --user 0 ";
                        commandName = "Disabled:";
                        break;
                    case 2:
                        command = "adb -s " + currentDeviceSelected + " shell pm clear ";
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
            load.ShowDialog();
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
            toolStripButtonPackageManager.Enabled = true;
            toolStripButtonBloatwareDetecter.Enabled = true;
            allApk = false;
            systemApp = false;
            syncFun(2);
        }

        private void systemAppToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripButtonUninstallApp.Enabled = true;
            toolStripButtonRestoreApp.Enabled = false;
            toolStripButtonPermissionMenu.Enabled = true;
            toolStripButtonPackageManager.Enabled = true;
            toolStripButtonBloatwareDetecter.Enabled = true;
            allApk = false;
            systemApp = true;
            syncFun(2);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonUninstallApp.Enabled = true;
            toolStripButtonRestoreApp.Enabled = false;
            toolStripButtonPermissionMenu.Enabled = true;
            toolStripButtonPackageManager.Enabled = true;
            toolStripButtonBloatwareDetecter.Enabled = true;
            allApk = true;
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
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                string fileName;
                foreach (string fileLoc in filePaths)
                {
                    fileName = fileLoc.Substring(fileLoc.LastIndexOf('\\')+1);
                    if (fileLoc.ToLower().Contains(".apk"))
                    { 
                        if (File.Exists(fileLoc))
                        {
                            if(adbFastbootCommandR(new[] { "-s " + currentDeviceSelected + " install -r \"" + fileLoc + "\"" },0) != null)
                            {
                                LogWriteLine(fileName+" installed!");
                                MessageShowBox(fileName +" installed", 2);
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
                        MessageShowBox("Error: " + fileName + " is not an apk file", 0);
                    }
                }
            }
        }

        private void groupBox6_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void checkedListBoxApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelSelectedAppCount.Text = "Selected App: " + checkedListBoxApp.CheckedItems.Count;
        }

        private void checkGrantedPermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell dumpsys package ", null, 1);
        }

        private void grantDUMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm grant ", " android.permission.DUMP", 0);
        }

        private void grantWriteSecureSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            appFunc("shell pm grant ", " android.permission.WRITE_SECURE_SETTINGS", 0);
        }

        private void groupBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                LoadingForm loading = new LoadingForm(filePaths, currentDeviceSelected);
                loading.ShowDialog();
            }
        }

        private void groupBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void comboBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDeviceSelected = Regex.Replace(devices[comboBoxDevices.SelectedIndex].Serial, @"\s", "");
        }

        private void buttonReloadDevicesList_Click(object sender, EventArgs e)
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

            if (tabControl1.SelectedTab.Text!="System")
            {
                comboBoxDevices.Enabled = false;
                buttonReloadDevicesList.Enabled = false;
                buttonDeviceLogs.Enabled = false;
                buttonMobileScreenShare.Enabled = false;
                disableEnableSystem(false);
            }
        }

        private void backgroundWorkerAdbDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if(!connected)
            {
                MessageShowBox("You are offline, ATA can't download ADB", 0);
                return; 
            }
            Invoke((Action)delegate
            {
                disableEnableSystem(false);
                buttonDisconnectIP.Enabled = false;
            });
            ExeMissingForm adbError = new ExeMissingForm("adb.exe not found\n\nDo you want to download sdk platform tool?\n\n[By pressing YES you agree sdk platform tool terms and conditions]\nfor more info press info button", "Error, ADB Missing!");
            adbError.ShowDialog();
            switch (adbError.DialogResult)
            {
                case DialogResult.Yes:
                    LogWriteLine("Downloading sdk platform tool...");
                    using (var client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile("https://dl.google.com/android/repository/platform-tools-latest-windows.zip?authuser=2", "sdkplatformtool.zip");
                            LogWriteLine("sdk platform tool downloaded!");
                            LogWriteLine("unzipping sdk platform tool...");
                            using (ZipFile zip = ZipFile.Read("sdkplatformtool.zip"))
                            {
                                zip.ExtractAll(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
                            }
                            LogWriteLine("sdk platform tool extraced!");
                            LogWriteLine("Getting things ready...");
                            systemCommand("taskkill /f /im adb.exe");
                            systemCommand("taskkill /f /im fastboot.exe");
                            systemCommand("del adb.exe");
                            systemCommand("del AdbWinUsbApi.dll");
                            systemCommand("del AdbWinApi.dll");
                            systemCommand("del fastboot.exe");
                            systemCommand("move platform-tools\\adb.exe \"%cd%\"");
                            systemCommand("move platform-tools\\AdbWinUsbApi.dll \"%cd%\"");
                            systemCommand("move platform-tools\\AdbWinApi.dll \"%cd%\"");
                            systemCommand("move platform-tools\\fastboot.exe \"%cd%\"");
                            File.Delete("sdkplatformtool.zip");
                            systemCommand("rmdir /Q /S platform-tools");
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
                    Process.Start("https://developer.android.com/license?authuser=2");
                    disableSystem(true);
                    break;
                default:
                    disableSystem(true);
                    break;
            }
        }

        private void disableSystem(bool a)
        {
            Invoke((Action)delegate
            {
                tabPageSystem.Enabled = !a;
                buttonMobileScreenShare.Enabled = !a;
            });
        }

        private void toolStripButtonBloatwareDetecter_Click(object sender, EventArgs e)
        {
            if(checkedListBoxApp.Items.Count>0)
            { 
                List<string> listOfApps = new List<string>();
                foreach(Object list in checkedListBoxApp.Items)
                {
                    listOfApps.Add(list.ToString());
                }
                BloatwareDetecter bloatwareDetecter = new BloatwareDetecter(listOfApps, this);
                bloatwareDetecter.CurrentDevice = currentDeviceSelected;
                bloatwareDetecter.ShowDialog();
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
                List<String> apps = new List<String>();
                foreach (Object app in checkedListBoxApp.CheckedItems)
                {
                    apps.Add(app.ToString());
                }
                LoadingForm load = new LoadingForm(apps, "adb -s " + currentDeviceSelected + " shell cmd package install-existing ", "Restored:");
                load.ShowDialog();
                if (load.DialogResult != DialogResult.OK)
                {
                    MessageShowBox("Error during uninstallation process", 0);
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
            toolStripButtonBloatwareDetecter.Enabled = false;
            allApk = false;
            systemApp = false;
            syncFun(5);
        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI");
        }

        private void buttonMobileScreenShare_Click(object sender, EventArgs e)
        {
            if (File.Exists("scrcpy.exe"))
            {
                systemCommand("start scrcpy");
            }
            else
            {
                try
                {
                    backgroundWorkerExeDownloader.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageShowBox(ex.ToString(), 0);
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
            scrcpyError.ShowDialog();
            switch (scrcpyError.DialogResult)
            {
                case DialogResult.Yes:
                    LogWriteLine("Downloading scrcpy...");
                    using (var client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile("https://github.com/Genymobile/scrcpy/releases/download/v1.17/scrcpy-win32-v1.17.zip", "scrcpy.zip");
                            LogWriteLine("scrcpy downloaded!");
                            LogWriteLine("unzipping scrcpy...");
                            using (ZipFile zip = ZipFile.Read("scrcpy.zip"))
                            {
                                zip.ExtractAll(System.IO.Path.GetDirectoryName(Application.ExecutablePath), ExtractExistingFileAction.DoNotOverwrite);
                            }
                            LogWriteLine("scrcpy Extracted!");
                            LogWriteLine("Getting things ready...");
                            systemCommand("del scrcpy.zip");
                            LogWriteLine("scrcpy ready!");
                        }
                        catch (Exception ex)
                        {
                            MessageShowBox(ex.ToString(), 0);
                            disableSystem(true);
                        }
                    }
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.OK:
                    Process.Start("https://raw.githubusercontent.com/Genymobile/scrcpy/master/LICENSE");
                    break;
                default:
                    break;
            }
        }

        private void labelHelp_MouseHover(object sender, EventArgs e)
        {
            labelHelp.BackColor = System.Drawing.ColorTranslator.FromHtml("#1f2121");
        }

        private void labelHelp_MouseLeave(object sender, EventArgs e)
        {
            labelHelp.BackColor = System.Drawing.Color.Black;
        }

        private void labelSettings_MouseLeave(object sender, EventArgs e)
        {
            labelSettings.BackColor = System.Drawing.Color.Black;
        }

        private void labelSettings_MouseHover(object sender, EventArgs e)
        {
            labelSettings.BackColor = System.Drawing.ColorTranslator.FromHtml("#1f2121");
        }

        private void labelSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void labelHelp_Click(object sender, EventArgs e)
        {
            contextMenuStripHelp.Show(Cursor.Position);
        }

        private void videoTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI/wiki#coming-soon");
        }

        private void pictureBoxMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("adb").Length != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to kill ADB?", "Kill ADB", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    systemCommand("taskkill /f /im " + FILEADB);
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
            DeviceLogs deviceLog = new DeviceLogs(currentDeviceSelected);
            deviceLog.Show();
        }

        private void pictureBoxSearchFile_Click(object sender, EventArgs e)
        {
            this.openFileDialogZip.Filter = "Zip files *.zip|*.zip;";
            this.openFileDialogZip.FileName = "";
            this.openFileDialogZip.Multiselect = false;
            this.openFileDialogZip.Title = "Select Zip";

            DialogResult dr = this.openFileDialogZip.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDirFile.Text = this.openFileDialogZip.FileName;
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
            feedback.ShowDialog();
        }
    }
}
