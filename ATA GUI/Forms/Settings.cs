using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace ATA_GUI
{
    public partial class Settings : Form
    {
        private string changelog = string.Empty;
        private static readonly string CURRENTVERSION = MainForm.CURRENTVERSION;
        private bool runningCheck;
        private bool starting = true;

        public Settings()
        {
            InitializeComponent();
        }

        private async void buttonCheckLastVersion_ClickAsync(object sender, EventArgs e)
        {
            if(!runningCheck)
            {
                runningCheck = true;
                if (MainForm.pingCheck())
                {
                    Release currentRelease = new Release();
                    Release latestRelease = new Release();
                    string json;
                    try
                    {
                        labelLog.Text = "Checking for new ATA version...";
                        HttpClient _client = new HttpClient();
                        _client.Timeout = TimeSpan.FromSeconds(60);
                        _client.DefaultRequestHeaders.Add("User-Agent", "ATA");
                        json = await _client.GetStringAsync("https://ata.msartore.dev/api/links.json");
                        dynamic jsonMirror = JsonConvert.DeserializeObject(json);
                        json = await _client.GetStringAsync(jsonMirror[0]["url"].ToString());
                        dynamic jsonReal = JsonConvert.DeserializeObject(json);
                        string latestReleaseName = jsonReal[0]["tag_name"];
                        latestRelease.Number = int.Parse(Regex.Replace(latestReleaseName, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                        if (latestReleaseName.Contains("Pre")) { latestRelease.Pre = true; }
                        currentRelease.Number = int.Parse(Regex.Replace(CURRENTVERSION, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                        if (CURRENTVERSION.Contains("Pre")) { currentRelease.Pre = true; }
                        string linkString = jsonReal[0]["assets"][0]["browser_download_url"];
                        changelog = jsonReal[0]["body"];
                        if ((latestRelease.Number > currentRelease.Number) || ((latestRelease.Number == currentRelease.Number) && (currentRelease.Pre && !latestRelease.Pre)))
                        {
                            if (MessageBox.Show("New version found: " + latestReleaseName + "\nCurrent Version: " + CURRENTVERSION + "\n\nDo you want to update it?", "Update found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start((string)jsonReal[0]["html_url"]);
                                UpdateForm update = new UpdateForm(linkString);
                                update.ShowDialog();
                            }
                            else
                            {
                                labelLog.Text = "ATA is not up to date, update\nit as soon as you can!";
                            }
                        }
                        else
                        {
                            labelLog.Text = "ATA is up to date!";
                        }
                        labelLatestRelease.Text = latestReleaseName;
                    }
                    catch
                    {
                        labelLog.Text = string.Empty;
                        MainForm.MessageShowBox("Timeout Error occurred while connecting to the Server", 0);
                    }
                }
                else
                {
                    labelLog.Text = "You are offline";
                }
                runningCheck = false;
            }
            else
            {
                MainForm.MessageShowBox("Wait, check is still running", 2);
            }
        }

        private void buttonCredits_Click(object sender, EventArgs e)
        {
            (new About()).ShowDialog();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            labelCurrentRelease.Text = CURRENTVERSION;

            if (!Feedback.checkFeedbackFile())
            { 
                checkBoxInitPopUp.Checked = true;
            }
            starting = false;
        }

        private void linkLabelChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ScrollableMessageBox.show(changelog, "Changelog");
        }

        private void checkBoxInitPopUp_CheckedChanged(object sender, EventArgs e)
        {
            if(starting)
            {
                return;
            }
            if(checkBoxInitPopUp.Checked)
            {
                Feedback.changeFeedbackFile(false);
                return;
            }
            Feedback.changeFeedbackFile(true);
        }
    }
}
