using ATA_GUI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class LoadingForm : Form
    {
        private List<string> array;
        private readonly string command;
        private readonly string deviceSerial;
        private readonly OperationType operation;

        public LoadingForm(List<string> arrayApkTemp, string commandTemp, string label)
        {
            InitializeComponent();
            array = arrayApkTemp;
            command = commandTemp;
            labelText.Text = label;
            operation = OperationType.Install;
            Text = label.Replace(":", "");
            Update();
        }

        public LoadingForm(List<string> arrayFile, string deviceSerialTmp)
        {
            InitializeComponent();
            array = arrayFile;
            labelText.Text = "Transfering file:";
            deviceSerial = deviceSerialTmp;
            operation = OperationType.Transfer;
            Text = "Transfering";
            Update();
        }

        public LoadingForm(string deviceSerialTmp, List<string> arrayApp)
        {
            InitializeComponent();
            array = arrayApp;
            labelText.Text = "Extracting file:";
            deviceSerial = deviceSerialTmp;
            operation = OperationType.Extraction;
            Text = "Extracting";
            Update();
        }

        private void LoadingForm_Shown(object sender, EventArgs e)
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
            Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> failedApps = new();
            array = array.Select(it => it.Trim()).ToList();

            Invoke(delegate
            {
                progressBar1.Minimum = 1;
                progressBar1.Step = 1;
                progressBar1.Maximum = array.Count;

                switch (operation)
                {
                    case OperationType.Install:

                        array.ForEach(x =>
                        {
                            labelFileName.Text = x;
                            Refresh();
                            string result = ConsoleProcess.adbProcess(command + x).ToLowerInvariant();
                            if (result.Contains("not found") || result.Contains("fail") || result.Trim().Length == 0)
                            {
                                failedApps.Add(x);
                            }
                            ReportProgress();
                        });

                        break;
                    case OperationType.Transfer:
                        _ = ConsoleProcess.adbProcess("-s " + deviceSerial + " shell mkdir sdcard/ATA");

                        array.ForEach(file =>
                        {
                            if (File.Exists(file))
                            {
                                labelFileName.Text = file[(file.LastIndexOf('\\') + 1)..];
                                Refresh();
                                if (ConsoleProcess.adbProcess("-s " + deviceSerial + " push " + file + " sdcard/ATA ") == null)
                                {
                                    failedApps.Add(file);
                                }
                            }
                            ReportProgress();
                        });

                        break;
                    case OperationType.Extraction:

                        if (!Directory.Exists("APKS"))
                        {
                            _ = ConsoleProcess.systemCommand("mkdir APKS");
                        }

                        array.ForEach(x =>
                        {
                            labelFileName.Text = x;
                            Refresh();
                            string[] pathList = ConsoleProcess.adbProcess("-s " + deviceSerial + " shell pm path " + x).Split('\n').Where(it => it.Contains("package")).ToArray();
                            foreach (string path in pathList)
                            {
                                string result = ConsoleProcess.adbProcess(("-s " + deviceSerial + " pull " + path.Replace("package:", "").Trim() + " " + Application.StartupPath + "APKS\\" + x + "_" + path[(path.LastIndexOf('/') + 1)..]).Trim());
                                if (!result.Contains("file pulled"))
                                {
                                    failedApps.Add(result);
                                }
                            }
                            ReportProgress();
                        });

                        break;
                    default:
                        break;
                }

                if (failedApps.Count > 0)
                {
                    MainForm.MessageShowBox(string.Format("ATA failed to run the action on the following items:\n{0}", string.Join("\n", failedApps)), 1);
                }
                else
                {
                    MainForm.MessageShowBox("ATA successfully performed the action on all the items selected", 2);
                }
            });
        }

        private void ReportProgress()
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

    public enum OperationType
    {
        Transfer,
        Extraction,
        Install
    }
}
