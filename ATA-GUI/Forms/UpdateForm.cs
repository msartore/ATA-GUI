using ATA_GUI.Utils;
using Ionic.Zip;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task UpdateAsync()
        {
            progressBar1.Maximum = 2;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            using (HttpClient client = new())
            {
                try
                {
                    if (!File.Exists(ataUFileName))
                    {
                        using StreamWriter sw = File.CreateText(ataUFileName);
                        sw.WriteLine("@echo off\nTITLE ATA-GUI Updater\ntaskkill /f /im \"ATA-GUI.exe\"\ndel \"ATA-GUI.exe\"\ndel ATAUpdate.zip\ncd ATAUpdate\nmove /y * \"" + Path.GetDirectoryName(Application.ExecutablePath) + "\"\ncd ..\nrmdir ATAUpdate\nstart \"\" \"ATA-GUI.exe\"\nexit");
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
                    using (HttpResponseMessage response = await client.GetAsync(link))
                    {
                        _ = response.EnsureSuccessStatusCode();
                        using FileStream fs = new(ataZipFileName, FileMode.Create);
                        await response.Content.CopyToAsync(fs);
                    }
                    reportProgress();
                    labelLog.Text = "Unzipping update...";
                    Refresh();
                    using (ZipFile zip = ZipFile.Read(ataZipFileName))
                    {
                        _ = ConsoleProcess.systemCommand("mkdir ATAUpdate");
                        zip.ExtractAll(Path.GetDirectoryName(Application.ExecutablePath) + "\\ATAUpdate");
                    }
                    reportProgress();
                    labelLog.Text = "Starting to update...";
                    Refresh();
                    labelLog.Text = "Closing App...";
                    Refresh();
                    _ = ConsoleProcess.systemCommandAsync("start " + ataUFileName);
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
            _ = UpdateAsync();
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
