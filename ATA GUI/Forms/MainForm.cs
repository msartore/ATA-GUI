using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Net;
using Ionic.Zip;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ATA_GUI
{
    public partial class MainForm : Form
    {

        private List<string> arrayApks = new List<string>();
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private bool textboxClear = false;
        private readonly string FILEADB = "adb.exe";

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
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

        private void buttonCloseWindows_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
                    if (adbFastbootCommandR(new string[] { "devices" }, 1).Contains("fastboot"))
                    {
                        string[] log = adbFastbootCommandR(new string[] { "getvar all" }, 1).Split(' ', '\n');
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
                                if(log[a].Contains("yes"))
                                    labelBootloaderStatus.Text = "Yes";
                                else
                                    labelBootloaderStatus.Text = "No";
                            }
                        }
                        panelFastboot.Enabled = true;
                        pictureBoxLoading2.Visible = false;
                        LogWriteLine("Device found!");
                    }
                    else
                    {
                        MessageShowBox("Device not found!", 0);
                        panelFastboot.Enabled = false;
                        pictureBoxLoading2.Visible = false;
                    }
                }
                else
                {
                    MessageShowBox("Error fastboot not found", 0);
                    pictureBoxLoading2.Visible = false;
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
            string ret = "";
            string line;
            Cursor.Current = Cursors.WaitCursor;
            if (!checkAdbFastboot(1)) return null;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
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
                        if (line.Length > 0)
                            ret += line;
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
                        if (line.Length > 0)
                            ret += line + "\n";

                        line = process.StandardOutput.ReadToEnd();
                        if (line.Length > 0)
                            ret += line + "\n";
                        process.Close();
                    }
                    break;
            }
            return ret;
        }

        /* Type 0 Error, Type 1 Warning, Type 2 Info */
        private void MessageShowBox(string message, int type)
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

        private void backgroundWorkerSync_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Object paramObj = e.Argument as Object;
            String paramObjTmp = "0";
            if(paramObj.ToString()=="1")
            {
                paramObjTmp = "1";
                paramObj = "0";
            }
            if (checkAdbFastboot(0))
            {
                LogWriteLine("Checking device...");
                if(adbFastbootCommandR(new string[] { "shell getprop ro.build.version.release"}, 0).Any(char.IsDigit))
                { 
                    switch (paramObj.ToString())
                    {
                        case "0":
                            string[] arrayDeviceInfoCommands = { "shell getprop ro.build.version.release", "shell getprop ro.build.user",  "shell getprop ro.product.cpu.abilist", "shell getprop ro.product.manufacturer" , "shell getprop ro.product.model",
                                   "shell getprop ro.product.board", "shell getprop ro.product.device", "shell ip route", "devices" };
                            string deviceinfo = adbFastbootCommandR(arrayDeviceInfoCommands, 0);
                            string[] arrayDeviceInfo = deviceinfo.Split('\n');
                            if (arrayDeviceInfo.Length > 6)
                            {
                                Invoke((Action)delegate
                                {
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
                                        textBoxIP.Text = labelIP.Text = arrayDeviceInfo[7].Substring(arrayDeviceInfo[7].IndexOf("src") + 4);
                                        if (deviceinfo.Contains(textBoxIP.Text.Substring(0, textBoxIP.Text.Length-2) +":5555"))
                                            labelStatus.Text = "Wireless";
                                        else
                                            labelStatus.Text = "Cable";
                                    }
                                    LogWriteLine("Device info extracted");
                                    radioButtoNonSystemApp.Checked = true;
                                    panelSystem.Enabled = true;
                                    groupBoxADBNet.Enabled = true;
                                    groupBoxRebootMenu.Enabled = true;
                                });
                            }
                            else
                            {
                                panelSystem.Enabled = false;
                                groupBoxADBNet.Enabled = false;
                                groupBoxRebootMenu.Enabled = false;
                                LogWriteLine("Error: failed to extract device info!");
                            }
                            break;
                        case "2":
                            arrayApks.Clear();
                            Invoke((Action)delegate
                            {
                                checkedListBoxApp.Items.Clear();
                            });
                            string[] command = { };
                            var arrayApksUni = new List<int>();
                            if (radioButtoNonSystemApp.Checked)
                            {
                                command = new string[] { "shell pm list packages -3" };
                            }
                            if (radioButtonSystemApp.Checked)
                            {
                                command = new string[] { "shell pm list packages -s" };
                            }

                            string stringApk;
                            if ((stringApk = adbFastbootCommandR(command,0))!=null)
                            {
                                LogWriteLine("Loading apps...");
                                string[] arrayApkTmp = stringApk.Split('\n');
                                foreach (string line in arrayApkTmp)
                                {
                                    if (line.Contains("package:"))
                                    {
                                        arrayApks.Add(line.Substring(8));
                                    }
                                }
                                foreach (string str in arrayApks)
                                {
                                    Invoke((Action)delegate
                                    {
                                        checkedListBoxApp.Items.Add(str);
                                        checkedListBoxApp.CheckOnClick = true;
                                    });
                                }
                                LogWriteLine("Apps loaded!");
                            }
                            else
                            {
                                MessageShowBox("Error during apk loading", 0);
                            }
                            break;
                    }
                }
                else
                {
                    Invoke((Action)delegate
                    {
                        panelSystem.Enabled = false;
                        groupBoxADBNet.Enabled = false;
                        groupBoxRebootMenu.Enabled = false;
                        LogWriteLine("DEVICE NOT FOUND/MULTIPLE DEVICES FOUND!");
                        if (paramObjTmp == "0")
                            MessageShowBox("Error device not found/ multiple devices found", 0);
                    });
                }
            }
            else
            {
                Invoke((Action)delegate
                {
                    panelSystem.Enabled = false;
                    groupBoxADBNet.Enabled = false;
                    groupBoxRebootMenu.Enabled = false;
                });
                adbMissingForm adbError = new adbMissingForm();
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
                            LogWriteLine("sdk platform tool downloaded!");
                            LogWriteLine("Getting things ready...");
                            systemCommand("move platform-tools\\adb.exe \"%cd%\"");
                            systemCommand("move platform-tools\\AdbWinUsbApi.dll \"%cd%\"");
                            systemCommand("move platform-tools\\AdbWinApi.dll \"%cd%\"");
                            systemCommand("move platform-tools\\fastboot.exe \"%cd%\"");
                            File.Delete("sdkplatformtool.zip");
                            systemCommand("rmdir /Q /S platform-tools");
                            LogWriteLine("ATA ready!");
                            syncFun(1);
                            }
                            catch(Exception ex)
                            {
                                MessageShowBox(ex.ToString(), 0);
                            }
                        }
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.OK:
                        System.Diagnostics.Process.Start("https://developer.android.com/license?authuser=2");
                        break;
                    default:
                        break;
                }
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
            if (labelStatus.Text == "Cable")
                rebootSmartphone();
            else
                MessageShowBox("Can't reboot smartphone while is connected via wireless", 0);
        }

        private void buttonRR_Click(object sender, EventArgs e)
        {
            if (labelStatus.Text == "Cable")
            {
                systemCommand("adb reboot recovery");
                LogWriteLine("Rebooted!");
            }
            else
                MessageShowBox("Can't reboot smartphone while is connected via wireless", 0);
        }

        private void buttonRF_Click(object sender, EventArgs e)
        {
            if (labelStatus.Text == "Cable")
            {
                systemCommand("adb reboot-bootloader");
                LogWriteLine("Rebooted!");
            }
            else
                MessageShowBox("Can't reboot smartphone while is connected via wireless", 0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if(!File.Exists("DotNetZip.dll"))
            {
                MessageShowBox("DotNetZip.dll not found!", 0);
                Application.Exit();
            }
            Disclaimer disclaimer = new Disclaimer();
            if (!disclaimer.checkDiscalimer())
            {
                disclaimer.ShowDialog();
            }
            pictureBoxLoading2.Visible = false;
            panelFastboot.Enabled = false;
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Normal;
            this.Focus(); 
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            syncFun(1);
        }

        private void buttonApk_Click(object sender, EventArgs e)
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
                        LogWriteLine("Installing " + file);
                        systemCommand("adb install -r \"" + file + "\"");
                        LogWriteLine(file + " installed");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }



        private void buttonReloadApps_Click(object sender, EventArgs e)
        {
            try
            {
                syncFun(2);
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.ToString(), 1);
            }
        }

        private void buttonUninstallApk_Click(object sender, EventArgs e)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                string command = "";
                if (radioButtoNonSystemApp.Checked)
                {
                    command = "adb uninstall ";
                }
                if (radioButtonSystemApp.Checked)
                {
                    command = "adb shell pm uninstall -k --user 0 ";
                }
                List<string> arrayApk = new List<string>();
                foreach (Object list in checkedListBoxApp.CheckedItems)
                {
                    arrayApk.Add(list.ToString());
                }
                LoadingForm load = new LoadingForm(arrayApk, command);
                load.ShowDialog();
                if (load.DialogResult == DialogResult.OK)
                    syncFun(2);
                else
                    MessageShowBox("Error during uninstallation process", 0);
            }
            else
            {
                MessageShowBox("No app selected", 1);
            }
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
        }

        private void radioButtonSystemApp_CheckedChanged(object sender, EventArgs e)
        {
            buttonSyncApp.Enabled = true;
            if (radioButtonSystemApp.Checked)
                LogWriteLine("System App selected");
        }

        private void radioButtoNonSystemApp_CheckedChanged(object sender, EventArgs e)
        {
            buttonSyncApp.Enabled = true;
            if (!radioButtonSystemApp.Checked)
                LogWriteLine("Non System App selected");
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
                    if((log = adbFastbootCommandR(new string[] { command1 + list.ToString() + command2 }, 0))!=null)
                    {
                        if (type == 1)
                            ScrollableMessageBox.show(log, "Granted permissions");
                        else
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

        private void buttonCheckPermissions_Click(object sender, EventArgs e)
        {
            appFunc("shell dumpsys package ", null, 1);
        }

        private void buttonGrantDump_Click(object sender, EventArgs e)
        {
            appFunc("shell pm grant ", " android.permission.DUMP", 0);
        }


        private void buttonGrantPermission_Click(object sender, EventArgs e)
        {
            appFunc("shell pm grant ", " android.permission.WRITE_SECURE_SETTINGS", 0);
        }

        private void buttonSearchFile_Click(object sender, EventArgs e)
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
                LogWriteLine("Installing " + textBoxDirFile.Text);
                string log = adbFastbootCommandR(new string[] { "sideload \"" + textBoxDirFile.Text + "\"" }, 0);
                if(log.ToLower().Contains("error") || log.ToLower().Contains("failed") || log=="")
                {
                    LogWriteLine(textBoxDirFile.Text + " failed to flashed");
                }
                else
                {
                    LogWriteLine(textBoxDirFile.Text + " flashed");
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
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDirImg.Text = this.openFileDialogZip.FileName;
            }
        }

        private void buttonFlashImg_Click(object sender, EventArgs e)
        {
            if (textBoxDirImg.Text.Length > 0)
            {
                if (radioButtonBoot.Checked)
                    flash(0);
                if (radioButtonBootloader.Checked)
                    flash(1);
                if (radioButtonCache.Checked)
                    flash(2);
                if (radioButtonRadio.Checked)
                    flash(3);
                if (radioButtonRecovery.Checked)
                    flash(4);
                if (radioButtonRom.Checked)
                    flash(5);
                if (radioButtonSystem.Checked)
                    flash(6);
                if (radioButtonVendor.Checked)
                    flash(7);
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
                pictureBoxLoading2.Visible = true;
                backgroundWorkerFlashImg.RunWorkerAsync(index);
                pictureBoxLoading2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageShowBox(ex.ToString(), 0);
                pictureBoxLoading2.Visible = false;
            }
        }

        private void backgroundWorkerFlashImg_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string log = "";
            string command = "";
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
            if ((log = adbFastbootCommandR(new string[] { command + textBoxDirImg.Text }, 1))!=null)
            {
                LogWriteLine(log);
            }
            else
            {
                MessageShowBox("Something went wrong!", 0);
            }
        }

        static private bool checkAdbFastboot(int exeN)
        {
            string exeTmp = "adb.exe";
            if (exeN==1)
            {
                exeTmp = "fastboot.exe";
            }
            if (File.Exists(exeTmp) && File.Exists("AdbWinUsbApi.dll") && File.Exists("AdbWinApi.dll"))
                return true;
            else
                return false;
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
            systemCommand("adb reboot");
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

        private void buttonCredits_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
    }
}
