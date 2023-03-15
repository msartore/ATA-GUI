using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class PackageMenu : Form
    {
        private bool closedByMe;

        public int DialogResult1 { get; private set; }

        public PackageMenu(List<string> apklist)
        {
            InitializeComponent();
            richTextBoxAPKList.Text = string.Join("", apklist);
        }

        private void ButtonDisable_Click(object sender, EventArgs e)
        {
            SetResults(0);
        }

        private void ButtonEnable_Click(object sender, EventArgs e)
        {
            SetResults(1);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            SetResults(-1);
        }

        private void ButtonClearData_Click(object sender, EventArgs e)
        {
            SetResults(2);
        }

        private void SetResults(int d)
        {
            DialogResult1 = d;
            closedByMe = true;
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!closedByMe)
            {
                DialogResult1 = -1;
            }
        }
    }
}
