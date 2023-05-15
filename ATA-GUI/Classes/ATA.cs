﻿using System;
using System.Collections.Generic;

namespace ATA_GUI.Classes
{
    internal class ATA
    {
        public static readonly string CURRENTVERSION = "v2.5.0";
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
    }
}