using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class DeviceLogs : Form
    {
        private readonly string currentDevice = string.Empty;
        private string line = string.Empty;
        private readonly System.Diagnostics.Process process = new System.Diagnostics.Process();
        private readonly SynchronizationContext synchronizationContext;
        private readonly System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        private bool keepScrolling = true;
        private bool processBusy = false;

        public DeviceLogs(string CurrentDevice)
        {
            this.currentDevice = CurrentDevice;
            InitializeComponent();

            synchronizationContext = SynchronizationContext.Current;   

            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;

            startInfo.FileName = "adb.exe";
            process.StartInfo = startInfo;
        }

        private void buttonLogcat_Click(object sender, EventArgs e)
        {
            if (!processBusy)
            {
                backgroundWorkerLog.RunWorkerAsync(" -s " + currentDevice + " shell logcat");
            }
        }

        private void buttonGetEvent_Click(object sender, EventArgs e)
        {
            if (!processBusy)
            {
                backgroundWorkerLog.RunWorkerAsync(" -s " + currentDevice + " shell getevent");
            }
        }

        private async void backgroundWorkerLog_DoWorkAsync(object sender, DoWorkEventArgs e)
        {
            startInfo.Arguments = e.Argument as string;
            process.Start();
            processBusy = true;
            while (!process.HasExited)
            {
                line = process.StandardOutput.ReadLine() + "\n";

                try 
                {
                    Invoke((Action)delegate
                    {
                        richTextBoxLog.Text += line;
                        richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                        if (keepScrolling) richTextBoxLog.ScrollToCaret();
                    });

                    await Task.Delay(50);
                }
                catch { }

                if (backgroundWorkerLog.CancellationPending)
                {
                    break;
                }
            }
            processBusy = false;
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
            if (!processBusy)
            {
                return;
            }
            backgroundWorkerLog.CancelAsync();
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Clear();
        }

        private void buttonCopyText_Click(object sender, EventArgs e)
        {
            if (richTextBoxLog.Text.Length <= 0)
            {
                return;
            }
            Clipboard.SetText(richTextBoxLog.Text);
            MainForm.MessageShowBox("Text copied!", 2);
        }

        private void buttonKeepScrolling_Click(object sender, EventArgs e)
        {
            keepScrolling = !keepScrolling;
            if (keepScrolling)
            {
                buttonKeepScrolling.Text = "Stop scrolling";
            }
            else
            {
                buttonKeepScrolling.Text = "Keep scrolling";
            }
        }
    }
}
