using System;
using System.Diagnostics;
using System.IO;
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

            Process cmd = new();
            cmd.StartInfo.FileName = "adb.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = "pair " + IP + ":" + PORT;
            _ = cmd.Start();
            StreamReader sr = cmd.StandardOutput;

            while (!sr.EndOfStream)
            {
                output += (char)sr.Read();

                if (output.Contains(":"))
                {
                    cmd.StandardInput.WriteLine(code);
                }
            }

            cmd.WaitForExit();
            cmd.Close();

            DialogResult = output.Contains("Successfully paired") ? DialogResult.OK : DialogResult.No;

            Close();
        }
    }
}
