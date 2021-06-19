using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATA_GUI
{
    public partial class Feedback : Form
    {
        public Feedback()
        {
            InitializeComponent();
        }

        private void buttonSendFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI/issues/new/choose");
        }

        private void buttonShareTwitter_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/intent/tweet?text=Check+out+this+cool+uninstaller+that+I+found!+https://github.com/MassimilianoSartore/ATA-GUI");
        }

        private void buttonSGF_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI/issues/new?assignees=&labels=enhancement&template=feature_request.md&title=%5BFEATURE%5D");
        }

        private void buttonSGI_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI/issues/new?assignees=&labels=bug&template=bug_report.md&title=%5BBUG%5D");
        }

        private void buttonDonate_Click(object sender, EventArgs e)
        {
            Process.Start("https://msartore.dev/Donation/");
        }
    }
}
