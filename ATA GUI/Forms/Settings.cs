using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace ATA_GUI
{
    public partial class Settings : Form
    {
        private string changelog = "";

        private const string CURRENTVERSION = "v1.6.7";
        private bool runningCheck = false;

        public Settings()
        {
            InitializeComponent();
        }

        private async void buttonCheckLastVersion_ClickAsync(object sender, EventArgs e)
        {
            if(!runningCheck)
            {
                runningCheck = true;
                Ping myPing = new Ping();
                String host = "1.1.1.1";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
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
                        json = await _client.GetStringAsync("https://api.github.com/repos/MassimilianoSartore/ATA-GUI/releases");
                        dynamic jsonReal = JsonConvert.DeserializeObject(json);
                        string latestReleaseName = jsonReal[0]["tag_name"];
                        latestRelease.Number = int.Parse(Regex.Replace(latestReleaseName, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                        if (latestReleaseName.Contains("Pre")) { latestRelease.Pre = true; }
                        currentRelease.Number = int.Parse(Regex.Replace(CURRENTVERSION, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                        if (CURRENTVERSION.Contains("Pre")) { currentRelease.Pre = true; }
                        string linkString = jsonReal[0]["assets"][0]["browser_download_url"];
                        string linkRepository = jsonReal[0]["html_url"];
                        changelog = jsonReal[0]["body"];
                        if ((latestRelease.Number > currentRelease.Number) || ((latestRelease.Number == currentRelease.Number) && (currentRelease.Pre && !latestRelease.Pre)))
                        {
                            if (MessageBox.Show("Update found, do you want to update it?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start((string)jsonReal[0]["html_url"]);
                                UpdateForm update = new UpdateForm(linkString);
                                update.ShowDialog();
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
                    runningCheck = false;
                }
                else
                {
                    labelLog.Text = "You are offline";
                }
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
        }

        private void linkLabelChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ScrollableMessageBox.show(changelog, "Changelog");
        }
    }
}
