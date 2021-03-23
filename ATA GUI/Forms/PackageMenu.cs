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
    public partial class PackageMenu : Form
    {
        public int dialogResult;
        private bool closedByMe;

        public PackageMenu(List<string> apklist)
        {
            InitializeComponent();
            richTextBoxAPKList.Text = string.Join("", apklist);
        }

        private void buttonDisable_Click(object sender, EventArgs e)
        {
            dialogResult = 0;
            closedByMe = true;
            this.Close();
        }

        private void buttonEnable_Click(object sender, EventArgs e)
        {
            dialogResult = 1;
            closedByMe = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            dialogResult = -1;
            closedByMe = true;
            this.Close();
        }

        private void buttonClearData_Click(object sender, EventArgs e)
        {
            dialogResult = 2;
            closedByMe = true;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!closedByMe)
            {
                dialogResult = -1;
            }
        }
    }
}
