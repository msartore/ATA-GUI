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
                    labelLog.Text = "Downloading update...";
                    MainForm.systemCommand("rmdir /s /q ATAUpdate");
                    this.Refresh();
                    if (!File.Exists("ATAUpdater.bat"))
                    {
                        labelLog.Text = "Failed to update, missing ATAUpdater.bat";
                        return;
                    }
                    if (File.Exists("ATAUpdate.zip")) { File.Delete("ATAUpdate.zip"); }
                    labelWarning.Text = "Don't close this program!";
                    labelWarning.BackColor = System.Drawing.Color.Red;
                    this.Refresh();
                    client.DownloadFile(link, "ATAUpdate.zip");
                    UpdateProgressBar(1);
                    labelLog.Text = "Unzipping update...";
                    this.Refresh();
                    using (ZipFile zip = ZipFile.Read("ATAUpdate.zip"))
                    {
                        MainForm.systemCommand("mkdir ATAUpdate");
                        zip.ExtractAll(Application.ExecutablePath.Replace("\\ATA GUI.exe", "") + "\\ATAUpdate");
                    }
                    UpdateProgressBar(2);
                    labelLog.Text = "Starting to update...";
                    this.Refresh();
                    labelLog.Text = "Closing App...";
                    this.Refresh();
                    MainForm.systemCommand("start \"\" ATAUpdater.bat");
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MainForm.MessageShowBox(ex.ToString(), 0);
                    labelLog.Text = "Failed to update";
                    this.Refresh();
                    this.Close();
                }
            }
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
