using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class LoadingForm : Form
    {
        private readonly List<string> arrayApk;
        private readonly string command;
        private readonly string[] fileArray;
        private readonly string deviceSerial;
        private readonly bool fileTransfer;


        public LoadingForm(List<string> arrayApkTemp, string commandTemp, string labelTemp)
        {
            InitializeComponent();
            arrayApk = arrayApkTemp;
            command = commandTemp;
            labelText.Text = labelTemp;
            fileTransfer = false;
        }

        public LoadingForm(string[] array, string deviceSerialTmp)
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
                    MainForm.systemCommand("adb -s " + deviceSerial + " shell mkdir sdcard/ATA");
                    progressBar1.Maximum = fileArray.Length;
                    progressBar1.Minimum = 1;
                    progressBar1.Step = 1;
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
                            reportProgress();
                            Thread.Sleep(500);
                        }
                        else
                        {
                            reportProgress();
                            Thread.Sleep(500);
                        }
                    }
                }
                else
                {
                    progressBar1.Maximum = arrayApk.Count;
                    progressBar1.Minimum = 1;
                    progressBar1.Step = 1;
                    int i = 0;
                    foreach (string apk in arrayApk)
                    {
                        labelFileName.Text = apk;
                        this.Refresh();
                        MainForm.systemCommand(command + apk);
                        reportProgress();
                        Thread.Sleep(500);
                    }
                }
            });
        }

        private void reportProgress()
        {
            Invoke((MethodInvoker)delegate
            {
                progressBar1.PerformStep();
                progressBar1.Refresh();
                Application.DoEvents();
            });
        }
    }
}
