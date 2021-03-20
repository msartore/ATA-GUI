using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ATA_GUI
{
    public partial class Disclaimer : Form
    {
        private bool closedByMe = false;
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
            if (!File.Exists(fileName))
            {
                return false;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode node = doc.DocumentElement.SelectSingleNode("/disclaimer/agreed");
            string a = node.InnerText;
            if (node.InnerText == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void createDiscalimer(string agreed)
        {
            using (XmlWriter writer = XmlWriter.Create(fileName))
            {
                writer.WriteStartElement("disclaimer");
                writer.WriteElementString("agreed", agreed);
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
            if (!closedByMe) Application.Exit(); 
        }
    }
}
