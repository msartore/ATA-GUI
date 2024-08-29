using ATA_GUI.Utils;
using System;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class DefaultAppForm : Form
    {
        private readonly string apk = string.Empty;

        public DefaultAppForm(string apk)
        {
            this.apk = apk;
            InitializeComponent();
        }

        private void DefaultApp_Load(object sender, EventArgs e)
        {
            labelAPKName.Text = apk;
            comboBoxType.SelectedIndex = 0;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSetDefault_Click(object sender, EventArgs e)
        {
            _ = ConsoleProcess.AdbProcess(MainForm.commandAssemblerF("shell cmd role add-role-holder android.app.role." + comboBoxType.Text + " " + apk));

            MainForm.MessageShowBox("Command injected", 2);
        }
    }
}
