using ATA_GUI.Utils;
using System;
using System.Windows.Forms;

namespace ATA_GUI.Forms
{
    public partial class WirelessPairingForm : Form
    {
        static string IP;
        static string PORT;

        public WirelessPairingForm(string ip, string port)
        {
            InitializeComponent();

            IP = ip;
            PORT = port;
        }

        private void buttonPair_Click(object sender, EventArgs e)
        {
            buttonPair.Enabled = false;

            string code = textBoxCode.Text;
            string output = string.Empty;

            if (code.Length == 6)
            {
                foreach (char c in code)
                {
                    if (!char.IsDigit(c))
                    {
                        MainForm.MessageShowBox("Wrong code format, the code must be a number without letters or special characters allowed!", 1);
                        buttonPair.Enabled = true;
                        return;
                    }
                }
            }
            else
            {
                MainForm.MessageShowBox("Wrong code format, the code must consist of exactly six digits!", 1);
                buttonPair.Enabled = true;
                return;
            }

            output = ConsoleProcess.adbProcess(string.Format("pair {0}:{1} {2}", IP, PORT, code));

            DialogResult = output.Contains("Successfully paired") ? DialogResult.OK : DialogResult.No;

            Close();
        }
    }
}
