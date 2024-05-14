using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ATA_GUI.Classes
{
    internal class DeviceData
    {
        public DeviceData(string name, string id, DeviceMode mode, DeviceConnection connection)
        {
            Name = name;
            ID = id;
            Mode = mode;
            Connection = connection;
            AppsString = new();
        }

        public string Name { get; }
        public DeviceMode Mode { get; set; }
        public DeviceConnection Connection { get; }
        public bool IsRotationFreeEnabled { get; set; }
        public string User { get; set; }
        public List<string> AppsString { get; set; }
        public AppMode AppMode { get; set; }
        public int Version { get; set; }
        public List<AppData> AppsExtracted { get; set; }
        public bool isATABridgeInstalled { get; set; }
        public string ID { get; }

        public char getConnectionSymbol()
        {
            return Connection.ToString()[0];
        }

        public bool sameMode(Tab tab)
        {
            return (Mode == DeviceMode.RECOVERY && tab == Tab.RECOVERY) || (Mode == DeviceMode.FASTBOOT && tab == Tab.FASTBOOT) || (Mode == DeviceMode.SYSTEM && tab == Tab.SYSTEM) || (Mode == DeviceMode.SIDELOAD && tab == Tab.RECOVERY);
        }

        public static string ExtractBootloaderInfo(string input)
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


            var usefulKeys = new HashSet<string>
            {
                "DP",
                "token",
                "crc",
                "parallel-download-flash",
                "anti",
                "cpuid",
                "board_version",
                "hw-revision",
                "unlocked",
                "off-mode-charge",
                "charger-screen-enabled",
                "battery-soc-ok",
                "battery-voltage",
                "erase-block-size",
                "logical-block-size",
                "variant",
                "secure",
                "serialno",
                "product",
                "max-download-size",
                "kernel"
            };

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"\(bootloader\) (?<key>[^:]+):(?<value>.+)");
                if (match.Success && usefulKeys.Contains(match.Groups["key"].Value))
                {
                    sb.AppendLine($"{match.Groups["key"].Value}: {match.Groups["value"].Value.Trim()}");
                }
            }

            return sb.ToString().Trim();
        }
    }

    internal enum DeviceMode
    {
        RECOVERY,
        SYSTEM,
        UNAUTHORIZED,
        SIDELOAD,
        UNKNOWN,
        FASTBOOT
    }

    internal enum DeviceConnection
    {
        CABLE,
        WIRELESS
    }

    internal enum AppMode
    {
        SYSTEM,
        NONSYSTEM,
        ALL,
        DISABLE,
        UNINSTALLED
    }
}
