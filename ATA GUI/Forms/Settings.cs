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

        private const string currentVersion = "v1.5.0";

        public Settings()
        {
            InitializeComponent();
        }

        private async void buttonCheckLastVersion_ClickAsync(object sender, EventArgs e)
        {
            Ping myPing = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
            {
                Release currentRelease = new Release();
                Release latestRelease = new Release();
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                string json = await _client.GetStringAsync("https://api.github.com/repos/MassimilianoSartore/ATA-GUI/releases");
                dynamic jsonReal = JsonConvert.DeserializeObject(json);
                labelLatestRelease.Text = jsonReal[0]["tag_name"];
                latestRelease.Number = int.Parse(Regex.Replace(labelLatestRelease.Text, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                if (labelLatestRelease.Text.Contains("Pre")) { latestRelease.Pre = true; }
                currentRelease.Number = int.Parse(Regex.Replace(currentVersion, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                if (currentVersion.Contains("Pre")) { currentRelease.Pre = true; }
                string linkString = jsonReal[0]["assets"][0]["browser_download_url"];
                changelog = jsonReal[0]["body"];
                if ((latestRelease.Number > currentRelease.Number) || ((latestRelease.Number == currentRelease.Number) && (currentRelease.Pre && !latestRelease.Pre)))
                {
                    if (MessageBox.Show("Update found, do you want to update it?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UpdateForm update = new UpdateForm(linkString);
                        update.ShowDialog();
                    }
                }
                else
                {
                    labelLog.Text = "ATA GUI is up to date";
                    this.Refresh();
                }
            }
            else
            {
                labelLog.Text = "You are offline";
            }
        }

        private void buttonCredits_Click(object sender, EventArgs e)
        {
            (new About()).ShowDialog();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            labelCurrentRelease.Text = currentVersion;
        }

        private void linkLabelChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ScrollableMessageBox.show(changelog, "Changelog");
        }
    }
}
