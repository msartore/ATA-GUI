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
            _ = Process.Start("https://github.com/MassimilianoSartore/ATA-GUI");
        }

        private void labelLicense_Click(object sender, EventArgs e)
        {
            _ = Process.Start("https://raw.githubusercontent.com/MassimilianoSartore/ATA-GUI/main/LICENSE");
        }

        private void linkLabelDNZ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://github.com/haf/DotNetZip.Semverd");
        }

        private void linkLabelSDK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://developer.android.com/studio/releases/platform-tools");
        }

        private void linkLabelRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://github.com/MassimilianoSartore/ATA-GUI");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabelNewtonsoft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://www.newtonsoft.com/json");
        }

        private void linkLabelScrcpy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://github.com/Genymobile/scrcpy");
        }

        private void linkLabelIcons_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://icons8.com");
        }

        private void About_Load(object sender, EventArgs e)
        {
            labelVersion.Text = "Version: " + MainForm.CURRENTVERSION;
        }

        private void linkLabelGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://github.com/msartore");
        }

        private void linkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ = Process.Start("https://msartore.dev");
        }
    }
}
