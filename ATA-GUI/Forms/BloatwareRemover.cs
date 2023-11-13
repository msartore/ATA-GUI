using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ATA_GUI.Classes;
using ATA_GUI.Utils;

namespace ATA_GUI
{
    public partial class BloatwareRemover : Form
    {
        private readonly List<string> packageList = new();
        private readonly List<string> nonSystemApp;
        private readonly List<string> systemApp;
        private readonly HashSet<string> foundPackageList = new();

        public string CurrentDevice { get; set; }

        public BloatwareRemover(List<string> nonSystemApp, List<string> systemApp)
        {
            InitializeComponent();
            this.nonSystemApp = nonSystemApp.Select(it => it.Trim()).ToList();
            this.systemApp = systemApp.Select(it => it.Trim()).ToList();
        }

        public string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        private void BloatwareRemover_Shown(object sender, EventArgs e)
        {
            comboBoxActionMode.SelectedIndex = 0;
            comboBoxScanLevel.SelectedIndex = 0;
            LoadApp();
            MainForm.MessageShowBox("Warning: Be careful before disabling/removing any system app or service. You must ensure that the package is not used by system " +
                "to function. Disabling a critical system app may result in bricking your phone. So always double check before disabling any system app.", 1);
        }

        private void LoadApp()
        {
            XmlDocument doc = new();
            doc.LoadXml(Properties.Resources.bloatwareList);
            foundPackageList.Clear();
            packageList.Clear();
            checkedListBoxBloatwareList.Items.Clear();
            int counter = 0;

            switch (comboBoxScanLevel.Text)
            {
                case "basic":
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/basic").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    break;
                case "medium":
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/basic").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/medium").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    break;
                case "advance":
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/basic").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/medium").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/advance").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    break;
                default:
                    MainForm.MessageShowBox("Error, this value can't be set", 0);
                    break;
            }

            packageList.Sort();

            foreach (string item in packageList)
            {
                string tmp = item.Trim();
                if (nonSystemApp.Any(it => it.Equals(tmp)) || systemApp.Any(it => it.Equals(tmp)))
                {
                    _ = checkedListBoxBloatwareList.Items.Add(tmp);
                    counter++;
                }
            }

            labelBC.Text = "Bloatware found: " + counter;
        }

        private void buttonAction_Click(object sender, EventArgs e)
        {
            if (checkedListBoxBloatwareList.CheckedItems.Count > 0)
            {
                List<string> listFailed = new();
                List<string> listSuccess = new();

                foreach (object item in checkedListBoxBloatwareList.CheckedItems)
                {
                    string app = item.ToString();

                    if (nonSystemApp.Any(it => it.Equals(app)) && comboBoxActionMode.SelectedIndex == 1)
                    {
                        if (ConsoleProcess.adbFastbootCommandR(MainForm.commandAssemblerF("shell pm uninstall -k --user " + ATA.CurrentDeviceSelected.User + " " + item), 0) != "Success")
                        {
                            listFailed.Add(app);
                        }
                        else
                        {
                            listSuccess.Add(app);
                        }
                    }
                    else
                    {
                        if (!ConsoleProcess.adbFastbootCommandR(MainForm.commandAssemblerF("shell pm disable-user --user " + ATA.CurrentDeviceSelected.User + " " + item), 0).Contains(item + " new state: disabled-user"))
                        {
                            listFailed.Add(app);
                        }
                        else
                        {
                            listSuccess.Add(app);
                        }
                    }
                }

                MainForm.MessageShowBox(string.Format("Following apps {0}:\n{1}\n{2}", comboBoxActionMode.Text, string.Join("\n", listSuccess), listFailed.Count > 0 ? $"ATA was not able to disable/remove the following apps:\n{string.Join("\n", listFailed)}\n{1}" : ""), 2);

                Close();
            }
            else
            {
                MainForm.MessageShowBox("No app selected!", 1);
            }
        }

        private void comboBoxScanLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadApp();
        }

        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxBloatwareList.Items.Count; i++)
            {
                checkedListBoxBloatwareList.SetItemChecked(i, checkBoxSelectAll.Checked);
            };
        }
    }
}