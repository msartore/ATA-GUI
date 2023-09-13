using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ATA_GUI.Utils;
using Ionic.Zip;

namespace ATA_GUI
{
    public partial class UpdateForm : Form
    {
        private readonly string link;
        private const string ataUFileName = "ATAUpdater.bat";
        private const string ataZipFileName = "ATAUpdate.zip";

        public UpdateForm(string linkTmp)
        {
            link = linkTmp;
            InitializeComponent();
        }

        public new void Update()
        {
            progressBar1.Maximum = 2;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            using (WebClient client = new WebClient())
            {
                try
                {
                    if (!File.Exists(ataUFileName))
                    {
                        using (StreamWriter sw = File.CreateText(ataUFileName))
                        {
                            sw.WriteLine("@echo off\nTITLE ATA-GUI Updater\ntaskkill /f /im \"ATA-GUI.exe\"\ndel \"ATA-GUI.exe\"\ndel ATAUpdate.zip\ncd ATAUpdate\ndel ATAUpdater.bat\nmove /y * ../\ncd ..\nrmdir ATAUpdate\nstart \"\" \"ATA-GUI.exe\"\nexit");
                        }
                    }
                    labelLog.Text = "Downloading update...";
                    _ = ConsoleProcess.systemCommandAsync("rmdir /s /q ATAUpdate");
                    Refresh();
                    if (!File.Exists(ataUFileName))
                    {
                        labelLog.Text = "Failed to update, missing " + ataUFileName;
                        labelLog.BackColor = System.Drawing.Color.Red;
                        Close();
                        Refresh();
                        Thread.Sleep(500);
                        return;
                    }
                    if (File.Exists(ataZipFileName)) { File.Delete(ataZipFileName); }
                    labelWarning.Text = "Don't close this program!";
                    labelWarning.BackColor = System.Drawing.Color.Red;
                    Refresh();
                    client.DownloadFile(link, ataZipFileName);
                    reportProgress();
                    labelLog.Text = "Unzipping update...";
                    Refresh();
                    using (ZipFile zip = ZipFile.Read(ataZipFileName))
                    {
                        _ = ConsoleProcess.systemCommandAsync("mkdir ATAUpdate");
                        zip.ExtractAll(Application.ExecutablePath.Replace("\\ATA-GUI.exe", "") + "\\ATAUpdate");
                    }
                    reportProgress();
                    labelLog.Text = "Starting to update...";
                    Refresh();
                    labelLog.Text = "Closing App...";
                    Refresh();
                    _ = ConsoleProcess.systemCommandAsync("start \"\" " + ataUFileName);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MainForm.MessageShowBox(ex.ToString(), 0);
                    labelLog.Text = "Failed to update";
                    labelLog.BackColor = System.Drawing.Color.Red;
                }
            }
            Close();
        }

        private void UpdateForm_Shown(object sender, EventArgs e)
        {
            Update();
        }

        private void reportProgress()
        {
            _ = Invoke((MethodInvoker)delegate
            {
                progressBar1.PerformStep();
                progressBar1.Refresh();
                Application.DoEvents();
            });
            Thread.Sleep(500);
        }
    }
}
