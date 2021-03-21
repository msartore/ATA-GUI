using System;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class BootloaderMenu : Form
    {
        public BootloaderMenu()
        {
            InitializeComponent();
        }

        private void buttonUB2014_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "oem device-info", "oem unlock", "getvar unlocked" }, 1);
        }

        private void buttonLB2014_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "oem device-info", "oem lock", "getvar unlocked" }, 1);
        }

        private void buttonUB2015_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "oem device-info", "flashing unlock", "getvar unlocked" }, 1);
        }

        private void buttonLB2015_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "oem device-info", "flashing lock", "getvar unlocked" }, 1);
        }

        private void buttonVivoUnlock_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "bbk unlock_vivo" }, 1);
        }

        private void buttonVivoLock_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "bbk lock_vivo" }, 1);
        }

        private void buttonDI_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "getvar all" }, 1);
        }

        private void buttonDID_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += MainForm.adbFastbootCommandR(new [] { "oem device-id" }, 1);
        }
    }
}
