using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATA_GUI.Classes
{
    internal class ATA
    {
        public static readonly string CURRENTVERSION = "v2.5.2";
        public static readonly string IPFileName = "IPList.txt";

        public ATA()
        {
            FILEADB = "adb.exe";
            IsConnected = true;
            IsMaximize = false;
        }

        public HashSet<string> IPList { get; } = new HashSet<string>();
        public List<Tuple<string, string>> Devices { get; } = new List<Tuple<string, string>>();
        public string FILEADB { get; }

        public bool TextboxClear { get; set; }
        public bool IsConnected { get; set; }
        public bool IsMaximize { get; set; }
        public static string CurrentDeviceSelected { get; set; } = string.Empty;

        public static async Task<bool> CheckVersion(Func<Release, Release, dynamic, bool> command)
        {
            Release currentRelease = new Release();
            Release latestRelease = new Release();

            using (HttpClient _client = new HttpClient())
            {
                _client.Timeout = TimeSpan.FromSeconds(60);
                _client.DefaultRequestHeaders.Add("User-Agent", "ATA");
                string json = await _client.GetStringAsync("https://ata.msartore.dev/api/links.json");
                dynamic jsonMirror = JsonConvert.DeserializeObject(json);
                json = await _client.GetStringAsync(jsonMirror[0]["url"].ToString());
                dynamic jsonReal = JsonConvert.DeserializeObject(json);
                latestRelease.Name = jsonReal[0]["tag_name"];
                latestRelease.Number = int.Parse(Regex.Replace(latestRelease.Name, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                if (latestRelease.Name.Contains("Pre")) { latestRelease.Pre = true; }
                currentRelease.Number = int.Parse(Regex.Replace(CURRENTVERSION, @"[^\d]+(\d*:abc$)|[^\d]+", ""));
                if (CURRENTVERSION.Contains("Pre")) { currentRelease.Pre = true; }

                command(currentRelease, latestRelease, jsonReal);
            }

            return true;
        }
    }
}
