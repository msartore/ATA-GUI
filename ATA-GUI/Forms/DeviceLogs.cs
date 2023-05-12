using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class DeviceLogs : Form
    {
        private readonly string currentDevice = string.Empty;
        private string line = string.Empty;
        private readonly System.Diagnostics.Process process = new System.Diagnostics.Process();
        private readonly System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        private bool keepScrolling = true;
        private bool processBusy = false;

        public DeviceLogs(string CurrentDevice)
        {
            currentDevice = CurrentDevice;
            InitializeComponent();

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
                buttonLogcat.Text = "Stop";
            }
            else
            {
                checkAndStop();
                buttonLogcat.Text = "Start";
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
            _ = process.Start();
            processBusy = true;
            while (!process.HasExited)
            {
                line = process.StandardOutput.ReadLine() + "\n";

                if (line.ToLowerInvariant().Contains(textBoxFilter.Text.ToLowerInvariant()) || string.IsNullOrEmpty(textBoxFilter.Text))
                {
                    try
                    {
                        _ = Invoke((Action)delegate
                        {
                            richTextBoxLog.Text += line;
                            richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                            if (keepScrolling)
                            {
                                richTextBoxLog.ScrollToCaret();
                            }
                        });

                        await Task.Delay(10);
                    }
                    catch { }

                    if (backgroundWorkerLog.CancellationPending)
                    {
                        break;
                    }
                }
            }
            processBusy = false;
            process.Close();
        }

        private void DeviceLogs_FormClosing(object sender, FormClosingEventArgs e)
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
            buttonKeepScrolling.Text = keepScrolling ? "Stop scrolling" : "Keep scrolling";
        }

        private void buttonLogcatClear_Click(object sender, EventArgs e)
        {
            checkAndStop();
            buttonLogcat.Text = "Start";
            _ = MainForm.adbFastbootCommandR(new[] { "-s " + MainForm.CurrentDeviceSelected + " logcat -c" }, 0);
        }

        private void richTextBoxLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            _ = Process.Start(e.LinkText);
        }
    }
}
