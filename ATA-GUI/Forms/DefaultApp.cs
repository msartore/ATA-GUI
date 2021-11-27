using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class DefaultApp : Form
    {

        readonly string apk = string.Empty;

        public DefaultApp(string apk)
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
            this.Close();
        }

        private void buttonSetDefault_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("-s " + MainForm.CurrentDeviceSelected + " shell cmd role add-role-holder android.app.role." + comboBoxType.Text + " " + apk);

            MainForm.systemCommand("adb -s " + MainForm.CurrentDeviceSelected + " shell cmd role add-role-holder android.app.role." + comboBoxType.Text + " " + apk);

            MainForm.MessageShowBox("Command injected", 1);
        }
    }
}
