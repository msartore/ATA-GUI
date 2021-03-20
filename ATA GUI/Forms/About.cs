using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void pictureRepo_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI");
        }

        private void labelLicense_Click(object sender, EventArgs e)
        {
            Process.Start("https://raw.githubusercontent.com/MassimilianoSartore/ATA-GUI/main/LICENSE");
        }

        private void linkLabelDNZ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/haf/DotNetZip.Semverd");
        }

        private void linkLabelSDK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://developer.android.com/studio/releases/platform-tools");
        }

        private void linkLabelRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
