using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ATA_GUI
{
    public partial class LoadingForm : Form
    {
        private readonly List<string> arrayApk;
        private readonly string command;
        private readonly string[] fileArray;
        private readonly string deviceSerial;
        private readonly bool fileTransfer;


        public LoadingForm(List<string> arrayApkTemp, string commandTemp, string labelTemp, string deviceSerialTmp)
        {
            InitializeComponent();
            arrayApk = arrayApkTemp;
            command = commandTemp;
            labelText.Text = labelTemp;
            fileTransfer = false;
            deviceSerial = deviceSerialTmp;
        }

        public LoadingForm(string [] array, string deviceSerialTmp)
        {
            InitializeComponent();
            fileArray = array;
            fileTransfer = true;
            labelText.Text = "Transfering file:";
            deviceSerial = deviceSerialTmp;
        }

        private void LoadingForm_Shown(Object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                MainForm.MessageShowBox("Background worker is busy", 0);
            }
        }


        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = progressBar1.Maximum;
            progressBar1.Update();
            progressBar1.Refresh();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke((Action)delegate
            {
                if (fileTransfer)
                {
                    MainForm.systemCommand("adb -s "+ deviceSerial + " shell mkdir sdcard/ATA");
                    progressBar1.Maximum = fileArray.Length;
                    int i = 0;
                    foreach (string file in fileArray)
                    {
                        if (File.Exists(file))
                        {
                            labelFileName.Text = file.Substring(file.LastIndexOf('\\') + 1);
                            this.Refresh();
                            if (MainForm.adbFastbootCommandR(new[] { "-s " + deviceSerial + " push " + file + " sdcard/ATA " }, 0) == null)
                            {
                                MainForm.MessageShowBox(labelFileName.Text + " not transfered", 0);
                            }
                            backgroundWorker.ReportProgress(++i);
                        }
                        else
                        {
                            backgroundWorker.ReportProgress(++i);
                        }
                    }
                }
                else
                {
                    progressBar1.Maximum = arrayApk.Count;
                    int i = 0;
                    foreach (string apk in arrayApk)
                    {
                        labelFileName.Text = apk;
                        this.Refresh();
                        MainForm.systemCommand("adb " + command + apk);
                        backgroundWorker.ReportProgress(++i);
                    }
                }
            });
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.Value = e.ProgressPercentage - 1;
            progressBar1.Value = e.ProgressPercentage;
            Thread.Sleep(1000);
            progressBar1.Update();
            progressBar1.Refresh();
        }
    }
}
