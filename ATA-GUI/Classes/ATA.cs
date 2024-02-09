using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ATA_GUI.Classes
{
    internal class ATA
    {
        public static readonly string CURRENTVERSION = "v3.4.6";
        public static readonly string IPFileName = "IPList.txt";

        public HashSet<string> IPList { get; } = new HashSet<string>();
        public List<DeviceData> Devices { get; } = new List<DeviceData>();
        public string FILEADB { get; }
        public bool TextboxClear { get; set; }
        public bool IsConnected { get; set; }
        public bool IsMaximize { get; set; }
        public Tab CurrentTab { get; private set; }
        public static DeviceData CurrentDeviceSelected { get; set; }
        public Size windowSize;


        public ATA()
        {
            FILEADB = "adb.exe";
            IsConnected = true;
            IsMaximize = false;
            CurrentTab = Tab.SYSTEM;
        }

        public static async Task<bool> CheckVersion(Func<string, string, dynamic, bool> command)
        {
            string currentRelease;
            string latestRelease;

            using HttpClient _client = new();
            _client.Timeout = TimeSpan.FromSeconds(60);
            _client.DefaultRequestHeaders.Add("User-Agent", "ATA");
            string json = await _client.GetStringAsync("https://ata.msartore.dev/api/links.json");
            dynamic jsonMirror = JsonConvert.DeserializeObject(json);
            json = await _client.GetStringAsync(jsonMirror[0]["url"].ToString());
            dynamic jsonReal = JsonConvert.DeserializeObject(json);
            latestRelease = jsonReal[0]["tag_name"];
            currentRelease = CURRENTVERSION;

            command(currentRelease, latestRelease, jsonReal);

            return true;
        }

        public void setCurrentTab(string currentTab)
        {
            CurrentTab = currentTab.ToLowerInvariant() switch
            {
                "system" or "tools" => Tab.SYSTEM,
                "recovery" => Tab.RECOVERY,
                "fastboot" => Tab.FASTBOOT,
                _ => Tab.UNKNOWN,
            };
        }
    }

    internal enum Tab
    {
        SYSTEM,
        FASTBOOT,
        RECOVERY,
        UNKNOWN
    }

    internal enum LogType
    {
        ERROR,
        OK,
        WARNING,
        INFO,
        UNKNOWN
    }
}
