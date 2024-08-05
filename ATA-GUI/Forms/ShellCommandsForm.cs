using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ATA_GUI.Classes;
using ATA_GUI.Utils;

namespace ATA_GUI.Forms
{
    public partial class ShellCommandsForm : Form
    {
        private readonly string Package;
        private readonly List<string> Permissions;

        public ShellCommandsForm(string package)
        {
            InitializeComponent();
            Package = package;
            comboBoxMethod.SelectedIndex = 0;
        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
            if (comboBoxPermission.SelectedIndex > -1)
            {
                string result = ConsoleProcess.AdbProcess(" -s " + ATA.CurrentDeviceSelected.ID + " shell pm grant " + Package + " android.permission." + comboBoxPermission.Text + " --user " + ATA.CurrentDeviceSelected.User);
                if (result.Length < 1)
                {
                    MainForm.MessageShowBox("Command injected successfully!", 2);
                }
                else
                {
                    MainForm.MessageShowBox("Command failed to be injected!\nError:" + result, 2);
                }

            }
            else
            {
                MainForm.MessageShowBox("No permission selected!", 1);
            }
        }
    }
}
