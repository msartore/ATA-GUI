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
    public partial class DisableEnableMenu : Form
    {


        public DisableEnableMenu(List<string> apklist)
        {
            InitializeComponent();
            richTextBoxAPKList.Text = string.Join("", apklist);
        }

        private void buttonDisable_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void buttonEnable_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
