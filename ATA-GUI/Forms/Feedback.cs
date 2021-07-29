using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ATA_GUI
{
    public partial class Feedback : Form
    {
        private const string fileName = "InfoCache.xml";

        public Feedback()
        {
            InitializeComponent();
        }

        public static bool checkFeedbackFile()
        {
            if (!File.Exists(fileName))
            {
                return false;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode node = doc.DocumentElement.SelectSingleNode("/ATA/initialPopUpFeedback");
            if (node.InnerText == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool changeFeedbackFile(bool popUp)
        {
            if (!File.Exists(fileName))
            {
                MainForm.MessageShowBox("Error: InfoCache.xml has been deleted", 0);
                return false;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode root = doc.DocumentElement;
            XmlNode myNode = root.SelectSingleNode("/ATA/initialPopUpFeedback");

            if (popUp)
            {
                myNode.FirstChild.Value = "yes";
            }
            else
            {
                myNode.FirstChild.Value = "no";
            }
            doc.Save(fileName);
            return false;
        }

        private void buttonSendFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MassimilianoSartore/ATA-GUI/issues/new/choose");
        }

        private void buttonShareTwitter_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/intent/tweet?text=Check+out+this+cool+Android™+tool+that+I+found!+https://github.com/MassimilianoSartore/ATA-GUI");
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
            Process.Start("https://msartore.dev/donation/");
        }

        private void Feedback_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox4.Visible = false;
            pictureBox3.Visible = false;
        }
    }
}
