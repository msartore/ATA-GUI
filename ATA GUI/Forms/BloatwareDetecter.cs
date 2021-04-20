﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ATA_GUI
{
    public partial class BloatwareDetecter : Form
    {
        private readonly List<string> packageList = new List<string>();
        private readonly List<string> installedPackageList;
        private readonly List<string> foundPackageList = new List<string>();
        private readonly MainForm mainForm;

        public BloatwareDetecter(List<string> listOfApps, MainForm main)
        {
            InitializeComponent();
            mainForm = main;
            installedPackageList = listOfApps;
        }

        private void AppFinder()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Properties.Resources.bloatwareList);
            packageList.Clear();
            foundPackageList.Clear();
            checkedListBoxBloatwareList.Items.Clear();

            switch (trackBarBloatwareInt.Value)
            {
                case 0:
                    labelWaring.Text = "Normal bloatware remove";
                    labelWaring.BackColor = Color.Green;
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/basic").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    break;
                case 1:
                    labelWaring.Text = "Medium bloatware remove";
                    labelWaring.BackColor = Color.Orange;
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/basic").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/medium").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    break;
                case 2:
                    labelWaring.Text = "Advance bloatware remove";
                    labelWaring.BackColor = Color.Red;
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/basic").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/medium").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    packageList.AddRange(doc.DocumentElement.SelectSingleNode("/bloatware/advance").InnerText.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
                    break;
                default:
                    MainForm.MessageShowBox("Error, this value can't be set", 0);
                    break;
            }
            string temp;
            foreach (string app in packageList)
            {
                temp = RemoveWhitespace(app);
                foreach (string appInstalled in installedPackageList)
                {
                    if (appInstalled.Contains(temp))
                    {
                        foundPackageList.Add(temp);
                    }
                }
            }
            if (foundPackageList.Count > 0)
            {
                checkedListBoxBloatwareList.Items.AddRange(foundPackageList.ToArray());
            }
            labelAppCount.Text = "App installed: " + installedPackageList.Count + "\nApp Found: " + foundPackageList.Count;
        }

        private void trackBarBloatwareInt_Scroll(object sender, EventArgs e)
        {
            AppFinder();
        }

         public string RemoveWhitespace(string input)
         {
            return new string(input.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
         }

        private void BloatwareDetecter_Shown(object sender, EventArgs e)
        {
            AppFinder();
        }

        private void buttonUninstall_Click(object sender, EventArgs e)
        {
            if(checkedListBoxBloatwareList.CheckedItems.Count> 0)
            { 
                mainForm.uninstaller(checkedListBoxBloatwareList.CheckedItems);
                this.Close();
            }
            else
            {
                MainForm.MessageShowBox("No app selected", 1);
            }
        }
    }
}