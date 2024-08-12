using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ATA_GUI.Classes
{
    internal class ATA
    {
        public static readonly string CURRENTVERSION = "v3.10.0";
        public static readonly string IPFileName = "IPList.txt";

        public HashSet<string> IPList { get; } = new HashSet<string>();
        public List<DeviceData> Devices { get; } = new List<DeviceData>();
        public string FILEADB { get; }
        public string FILEFastboot { get; }
        public bool TextboxClear { get; set; }
        public bool IsConnected { get; set; }
        public bool IsMaximize { get; set; }
        public Tab CurrentTab { get; private set; }
        public static DeviceData CurrentDeviceSelected { get; set; }
        public static string FASTBOOTPath { get; set; }
        public static string ADBPath { get; set; }
        public Size windowSize;
        public List<DataGridViewRow> selectedRows { get; set; }


        public ATA()
        {
            FILEADB = "adb.exe";
            FILEFastboot = "fastboot.exe";
            IsConnected = true;
            IsMaximize = false;
            CurrentTab = Tab.SYSTEM;
            selectedRows = new List<DataGridViewRow>();
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

        public static string FindExecutable(string executable)
        {
            string customPath = Directory.GetCurrentDirectory();
            List<string> paths = new List<string> { customPath };

            var envPaths = Environment.GetEnvironmentVariable("Path");
            if (envPaths != null)
            {
                paths.AddRange(envPaths.Split(Path.PathSeparator));
            }

            foreach (var path in paths)
            {
                string fullPath = Path.Combine(path, executable);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            return null;
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
