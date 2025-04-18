﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ATA_GUI.Utils;

namespace ATA_GUI
{
    public partial class FeedbackForm : Form
    {
        private const string fileName = "InfoCache.xml";

        public FeedbackForm()
        {
            InitializeComponent();
        }

        public static bool checkFeedbackFile()
        {
            if (!File.Exists(fileName))
            {
                return false;
            }
            XmlDocument doc = new();
            doc.Load(fileName);
            XmlNode node = doc.DocumentElement.SelectSingleNode("/ATA/initialPopUpFeedback");
            return node.InnerText == "yes";
        }

        public static bool changeFeedbackFile(bool popUp)
        {
            if (!File.Exists(fileName))
            {
                MainForm.MessageShowBox("Error: InfoCache.xml has been deleted", 0);
                return false;
            }
            XmlDocument doc = new();
            doc.Load(fileName);
            XmlNode root = doc.DocumentElement;
            XmlNode myNode = root.SelectSingleNode("/ATA/initialPopUpFeedback");

            myNode.FirstChild.Value = popUp ? "yes" : "no";
            doc.Save(fileName);
            return false;
        }

        private void buttonSendFeedback_Click(object sender, EventArgs e)
        {
            ConsoleProcess.OpenLink("https://github.com/msartore/ATA-GUI/issues/new/choose");
        }

        private void buttonShareTwitter_Click(object sender, EventArgs e)
        {
            ConsoleProcess.OpenLink("https://twitter.com/intent/tweet?text=Check+out+this+cool+Android™+tool+that+I+found!+https://github.com/msartore/ATA-GUI");
        }

        private void buttonSGF_Click(object sender, EventArgs e)
        {
            ConsoleProcess.OpenLink("https://github.com/msartore/ATA-GUI/issues/new?assignees=&labels=enhancement&template=feature_request.md&title=%5BFEATURE%5D");
        }

        private void buttonSGI_Click(object sender, EventArgs e)
        {
            ConsoleProcess.OpenLink("https://github.com/msartore/ATA-GUI/issues/new?assignees=&labels=bug&template=bug_report.md&title=%5BBUG%5D");
        }

        private void buttonDonate_Click(object sender, EventArgs e)
        {
            ConsoleProcess.OpenLink("https://msartore.dev/donate");
        }

        private void buttonDownloadATAB_Click(object sender, EventArgs e)
        {
            ConsoleProcess.OpenLink("https://play.google.com/store/apps/details?id=dev.msartore.atabridge");
        }
    }
}
