using System;
using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class DeviceLogs : Form
    {
        private string currentDevice = string.Empty;
        private string poolString = string.Empty;
        private System.Timers.Timer timer;

        public DeviceLogs(string CurrentDevice)
        {
            this.currentDevice = CurrentDevice;
            InitializeComponent();
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        private void buttonLogcat_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerLog.IsBusy)
                backgroundWorkerLog.RunWorkerAsync(" -s " + currentDevice + " shell logcat");
        }

        private void buttonGetEvent_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerLog.IsBusy)
                backgroundWorkerLog.RunWorkerAsync(" -s " + currentDevice + " shell getevent");
        }

        private void backgroundWorkerLog_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            poolString += e.UserState;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Invoke((Action)delegate
                {
                    richTextBoxLog.Text += poolString;
                    richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                    richTextBoxLog.ScrollToCaret();
                    poolString = string.Empty;
                });
            }
            catch
            {
                MainForm.MessageShowBox("Error during log event", 0);
            }

            poolString = string.Empty;
        }

        private void backgroundWorkerLog_DoWorkAsync(object sender, DoWorkEventArgs e)
        {
            timer.Start();

            StringBuilder ret = new StringBuilder();
            string line = string.Empty;
            int lineCounter = 0;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;

            startInfo.FileName = "adb.exe";
            process.StartInfo = startInfo;

            startInfo.Arguments = e.Argument as string;
            process.Start();
            while (!process.HasExited)
            {
                line = process.StandardOutput.ReadLine() + "\n";
                if (backgroundWorkerLog.CancellationPending) break;
                backgroundWorkerLog.ReportProgress(lineCounter++, line);
            }
            process.Close();
        }

        private void DeviceLogs_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkAndStop();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            checkAndStop();
        }

        private void checkAndStop()
        {
            if (!backgroundWorkerLog.IsBusy) return;
            backgroundWorkerLog.CancelAsync();
            timer.Stop();
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            poolString = string.Empty;
            richTextBoxLog.Clear();
        }
    }
}
