﻿using System;
using System.Collections.Generic;
using System.IO;
using ATA_GUI.Classes;

namespace ATA_GUI.Utils
{
    internal class APK
    {
        public static void installApk(string[] apksPath, string user, Action<string> log)
        {
            string message = "";
            List<bool> r = new();

            foreach (string fileLoc in apksPath)
            {
                if (fileLoc.ToLower().Contains(".apk"))
                {
                    if (File.Exists(fileLoc))
                    {
                        log.Invoke(fileLoc[(fileLoc.LastIndexOf('\\') + 1)..]);
                        r.Add(ConsoleProcess.adbFastbootCommandR("-s " + ATA.CurrentDeviceSelected.ID + " install -r --user " + user + " \"" + fileLoc + "\"", 0).Contains("Success"));
                    }
                }
            }

            for (int i = 0; i < apksPath.Length; i++)
            {
                message += apksPath[i][(apksPath[i].LastIndexOf('\\') + 1)..] + (r[i] ? " installed!" : " installation failed") + "\n";
            }

            MainForm.MessageShowBox(message, 2);
        }
    }
}
