using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ATA_GUI
{
    public partial class Disclaimer : Form
    {
        private bool closedByMe;
        private const string fileName = "InfoCache.xml";

        public Disclaimer()
        {
            InitializeComponent();
        }

        public bool checkDiscalimer()
        {
            return readFromFile();
        }

        private bool readFromFile()
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    return false;
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode node = doc.DocumentElement.SelectSingleNode("/ATA/license");
                if (node.InnerText == "yes")
                {
                    return true;
                }
            }catch
            {
                File.Delete(fileName);
            }
            return false;
        }

        private void createDiscalimer(string agreed)
        {
            using (XmlWriter writer = XmlWriter.Create(fileName))
            {
                writer.WriteStartElement("ATA");
                writer.WriteElementString("license", agreed);
                writer.WriteElementString("initialPopUpFeedback", "yes");
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            createDiscalimer("yes");
            closedByMe = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!closedByMe)
            { 
                Application.Exit();
            }
        }
    }
}
