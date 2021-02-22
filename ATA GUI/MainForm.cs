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
        private static string FILEDEVICEINFO = "deviceinfo.tmp";
        private static string FILEADB = "adb.exe";
        private static string FILEFASTBOOT = "fastboot.exe";

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
                    systemCommand("fastboot devices > "+ FILEDEVICEINFO);
                    if (File.Exists(FILEDEVICEINFO))
                    {
                        if (File.ReadAllText(FILEDEVICEINFO).Contains("fastboot"))
                        {
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
                        File.Delete(FILEDEVICEINFO);
                    }
                    else
                    {
                        MessageShowBox("Something went wrong!", 0);
                        panelFastboot.Enabled = false;
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
                if (ex.ToString().Contains("Ionic"))
                {
                    MessageShowBox("Ionic.Zip.dll not found!", 0);
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
            int i = 0;
            if (checkAdbFastboot(0))
            {
                LogWriteLine("Checking device...");
                systemCommand("adb shell getprop ro.build.version.release > "+ FILEDEVICEINFO);
                if (File.Exists("deviceInfo.tmp"))
                {
                    if (File.ReadAllText("deviceInfo.tmp").Any(char.IsDigit))
                    {
                        switch (paramObj.ToString())
                        {
                            case "0":
                                string[] arrayDeviceInfo;
                                systemCommand("adb shell getprop ro.build.user  >> "+ FILEDEVICEINFO);
                                systemCommand("adb shell getprop ro.product.cpu.abilist  >> "+ FILEDEVICEINFO);
                                systemCommand("adb shell getprop ro.product.manufacturer  >> " + FILEDEVICEINFO);
                                systemCommand("adb shell getprop ro.product.model  >> " + FILEDEVICEINFO);
                                systemCommand("adb shell getprop ro.product.board  >> " + FILEDEVICEINFO);
                                systemCommand("adb shell getprop ro.product.device  >> " + FILEDEVICEINFO);
                                systemCommand("adb shell ip route >> " + FILEDEVICEINFO);
                                LogWriteLine("device found!");
                                arrayDeviceInfo = File.ReadAllLines(FILEDEVICEINFO);
                                Invoke((Action)delegate
                                {
                                    if (arrayDeviceInfo.Length > 6)
                                    {
                                        labelAV.Text = arrayDeviceInfo[0];
                                        labelBU.Text = arrayDeviceInfo[1];
                                        labelCA.Text = arrayDeviceInfo[2];
                                        labelManu.Text = arrayDeviceInfo[3];
                                        labelModel.Text = arrayDeviceInfo[4];
                                        labelB.Text = arrayDeviceInfo[5];
                                        labelD.Text = arrayDeviceInfo[6];
                                        if(arrayDeviceInfo.Length > 7)
                                            textBoxIP.Text = labelIP.Text = arrayDeviceInfo[7].Substring(arrayDeviceInfo[7].IndexOf("src") + 4);
                                        LogWriteLine("Device info extracted");
                                        radioButtoNonSystemApp.Checked = true;
                                        panelSystem.Enabled = true;
                                        groupBoxADBNet.Enabled = true;
                                        groupBoxRebootMenu.Enabled = true;
                                    }
                                    else
                                    {
                                        panelSystem.Enabled = false;
                                        groupBoxADBNet.Enabled = false;
                                        groupBoxRebootMenu.Enabled = false;
                                        LogWriteLine("Error: failed to extract device info!");
                                    }
                                });
                                break;
                            case "2":
                                arrayApks.Clear();
                                Invoke((Action)delegate
                                {
                                    checkedListBoxApp.Items.Clear();
                                });
                                string filename ="";
                                var arrayApksUni = new List<int>();
                                if (radioButtoNonSystemApp.Checked)
                                {
                                    systemCommand("adb shell pm list packages -3 > APKNS.tmp");
                                    filename = "APKNS.tmp";
                                }
                                if (radioButtonSystemApp.Checked)
                                {
                                    systemCommand("adb shell pm list packages -s > APKS.tmp");
                                    filename = "APKS.tmp";
                                }

                                if (File.Exists(filename))
                                {
                                    LogWriteLine("Loading apps...");
                                    foreach (string line in File.ReadLines(filename))
                                    {
                                        if (line.Contains("package:"))
                                        {
                                            arrayApks.Add(line.Substring(8));
                                        }
                                    }
                                    LogWriteLine("Apps loaded!");
                                    File.Delete(filename);
                                    foreach (string str in arrayApks)
                                    {
                                        Invoke((Action)delegate
                                        {
                                            checkedListBoxApp.Items.Add(str);
                                            checkedListBoxApp.CheckOnClick = true;
                                        });
                                    }
                                }
                                else
                                {
                                    File.Delete(filename);
                                    MessageShowBox(filename + " not found!", 0);
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
                        LogWriteLine("Error: file not found!");
                    });
                }
                File.Delete(FILEDEVICEINFO);
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
                            LogWriteLine("sdk platform tool downloaded");
                            LogWriteLine("unzipping sdk platform tool");
                            using (ZipFile zip = ZipFile.Read("sdkplatformtool.zip"))
                            {
                                zip.ExtractAll(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
                            }
                            LogWriteLine("sdk platform tool downloaded Downloaded!");
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
            rebootSmartphone();
        }

        private void buttonRR_Click(object sender, EventArgs e)
        {
            systemCommand("adb reboot recovery");
            LogWriteLine("Rebooted!");
        }

        private void buttonRF_Click(object sender, EventArgs e)
        {
            systemCommand("adb reboot-bootloader");
            LogWriteLine("Rebooted!");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            pictureBoxLoading2.Visible = false;
            pictureBoxLoading.Visible = false;
            panelFastboot.Enabled = false;
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Normal;
            this.Focus(); this.Show();
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
                systemCommand("adb tcpip 5555");
                systemCommand("adb connect " + textBoxIP.Text + " | findstr \"" + textBoxIP.Text + ":5555\" && if %ERRORLEVEL%==0 0 > "+ FILEDEVICEINFO);
                if (File.Exists(FILEDEVICEINFO))
                {
                    MessageShowBox("connected to " + textBoxIP.Text, 2);
                }
                else
                {
                    MessageShowBox("Failed to connect to " + textBoxIP.Text, 0);
                }
                File.Delete(FILEDEVICEINFO);
                syncFun(0);
            }
        }

        private void buttonDisconnectIP_Click(object sender, EventArgs e)
        {
            if (textBoxIP.TextLength > 1)
            {
                systemCommand("adb disconnect " + textBoxIP.Text + " | findstr \"disconnect\" && if %ERRORLEVEL%==0 0 > "+ FILEDEVICEINFO);
                if (File.Exists(FILEDEVICEINFO))
                {
                    MessageShowBox(textBoxIP.Text + " disconnected", 2);
                }
                else
                {
                    MessageShowBox(textBoxIP.Text + "Failed to disconnect", 0);
                }
                File.Delete(FILEDEVICEINFO);
                syncFun(0);
            }
        }

        private void appFunc(string command1, string command2, int type)
        {
            if (checkedListBoxApp.CheckedItems.Count > 0)
            {
                foreach (Object list in checkedListBoxApp.CheckedItems)
                {
                    systemCommand(command1 + list.ToString() + command2 + " > "+ FILEDEVICEINFO);
                    if (File.Exists(FILEDEVICEINFO))
                    {
                        if (type == 1)
                            ScrollableMessageBox.show(File.ReadAllText(FILEDEVICEINFO), "Granted permissions");
                        else
                            LogWriteLine("Command injected!");
                    }
                    else
                    {
                        LogWriteLine("Command failed!");
                    }
                }
                File.Delete(FILEDEVICEINFO);
            }
            else
            {
                MessageShowBox("No app selected", 1);
            }
        }

        private void buttonCheckPermissions_Click(object sender, EventArgs e)
        {
            appFunc("adb shell dumpsys package ", null, 1);
        }

        private void buttonGrantDump_Click(object sender, EventArgs e)
        {
            appFunc("adb shell pm grant ", " android.permission.DUMP", 0);
        }


        private void buttonGrantPermission_Click(object sender, EventArgs e)
        {
            appFunc("adb shell pm grant ", " android.permission.WRITE_SECURE_SETTINGS", 0);
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
                pictureBoxLoading.Visible = true;
                systemCommand("adb sideload \"" + textBoxDirFile.Text + "\" | findstr \"error\" && if %ERRORLEVEL%==0 0 > "+ FILEDEVICEINFO);
                pictureBoxLoading.Visible = false;
                if (File.Exists(FILEDEVICEINFO))
                {
                    LogWriteLine(textBoxDirFile.Text + " installed");
                    File.Delete(FILEDEVICEINFO);
                }
                else
                {
                    LogWriteLine(textBoxDirFile.Text + " not installed");
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
            string filenameLog = "flashlog.log";
            string command = "";
            switch (sender.ToString())
            {
                case "0":
                    command = "fastboot flash boot ";
                    break;
                case "1":
                    command = "fastboot flash bootloader ";
                    break;
                case "2":
                    command = "fastboot flash cache ";
                    break;
                case "3":
                    command = "fastboot flash radio ";
                    break;
                case "4":
                    command = "fastboot flash recovery ";
                    break;
                case "5":
                    command = "fastboot -w && fastboot update ";
                    return;
                case "6":
                    command = "fastboot flash system ";
                    break;
                case "7":
                    command = "fastboot flash vendor ";
                    break;
            }
            systemCommand(command + textBoxDirImg.Text + " > " + filenameLog);
            if (File.Exists(filenameLog))
            {
                LogWriteLine(File.ReadAllLines(filenameLog).ToString());
            }
            else
            {
                MessageShowBox("Something went wrong!", 0);
            }
        }

        private bool checkAdbFastboot(int exeN)
        {
            string exeTmp = FILEADB;
            if(exeN==1)
            {
                exeTmp = FILEFASTBOOT;
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
    }
}
