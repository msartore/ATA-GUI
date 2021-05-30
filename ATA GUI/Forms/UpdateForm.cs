using Ionic.Zip;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

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
            using (var client = new WebClient())
            {
                try
                {
                    if (!File.Exists(ataUFileName))
                    {
                        using (StreamWriter sw = File.CreateText(ataUFileName))
                        {
                            sw.WriteLine("@echo off\nTITLE ATA GUI Updater\ntaskkill /f /im \"ATA GUI.exe\"\ndel \"ATA GUI.exe\"\ndel ATAUpdate.zip\ncd ATAUpdate\ndel ATAUpdater.bat\nmove /y * ../\ncd ..\nrmdir ATAUpdate\nstart \"\" \"ATA GUI.exe\"\nexit");
                        }
                    }
                    labelLog.Text = "Downloading update...";
                    MainForm.systemCommand("rmdir /s /q ATAUpdate");
                    this.Refresh();
                    if (!File.Exists(ataUFileName))
                    {
                        labelLog.Text = "Failed to update, missing " + ataUFileName;
                        labelLog.BackColor = System.Drawing.Color.Red;
                        this.Close();
                        this.Refresh();
                        Thread.Sleep(2000);
                        return;
                    }
                    if (File.Exists(ataZipFileName)) { File.Delete(ataZipFileName); }
                    labelWarning.Text = "Don't close this program!";
                    labelWarning.BackColor = System.Drawing.Color.Red;
                    this.Refresh();
                    client.DownloadFile(link, ataZipFileName);
                    UpdateProgressBar(1);
                    labelLog.Text = "Unzipping update...";
                    this.Refresh();
                    using (ZipFile zip = ZipFile.Read(ataZipFileName))
                    {
                        MainForm.systemCommand("mkdir ATAUpdate");
                        zip.ExtractAll(Application.ExecutablePath.Replace("\\ATA GUI.exe", "") + "\\ATAUpdate");
                    }
                    UpdateProgressBar(2);
                    labelLog.Text = "Starting to update...";
                    this.Refresh();
                    labelLog.Text = "Closing App...";
                    this.Refresh();
                    MainForm.systemCommand("start \"\" " + ataUFileName);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MainForm.MessageShowBox(ex.ToString(), 0);
                    labelLog.Text = "Failed to update";
                    labelLog.BackColor = System.Drawing.Color.Red;
                }
            }
            this.Close();
        }

        private void UpdateForm_Shown(object sender, EventArgs e)
        {
            Update();
        }

        private void UpdateProgressBar(int value)
        {
            progressBar1.Value = value;
            progressBar1.Value = value - 1;
            progressBar1.Value = value;
            Thread.Sleep(1000);
            progressBar1.Update();
            progressBar1.Refresh();
        }
    }
}
