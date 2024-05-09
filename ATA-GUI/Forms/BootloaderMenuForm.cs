using System;
using System.Windows.Forms;
using ATA_GUI.Utils;

namespace ATA_GUI
{
    public partial class BootloaderMenuForm : Form
    {
        public BootloaderMenuForm()
        {
            InitializeComponent();
        }

        private void buttonUB2014_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("oem device-info"), MainForm.commandAssemblerF("oem unlock"), MainForm.commandAssemblerF("getvar unlocked") }, 1) + "\n";
        }

        private void buttonLB2014_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("oem device-info"), MainForm.commandAssemblerF("oem lock"), MainForm.commandAssemblerF("getvar unlocked") }, 1) + "\n";
        }

        private void buttonUB2015_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("oem device-info"), MainForm.commandAssemblerF("flashing unlock"), MainForm.commandAssemblerF("getvar unlocked") }, 1) + "\n";
        }

        private void buttonLB2015_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("oem device-info"), MainForm.commandAssemblerF("flashing lock"), MainForm.commandAssemblerF("getvar unlocked") }, 1) + "\n";
        }

        private void buttonVivoUnlock_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("bbk unlock_vivo") }, 1) + "\n";
        }

        private void buttonVivoLock_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("bbk lock_vivo") }, 1) + "\n";
        }

        private void buttonDI_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("getvar all") }, 1) + "\n";
        }

        private void buttonDID_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text += ConsoleProcess.adbFastbootCommandR(new[] { MainForm.commandAssemblerF("oem device-id") }, 1) + "\n";
        }
    }
}
