using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATA_GUI.Classes;

namespace ATA_GUI.Utils
{
    internal class ConsoleProcess
    {
        public static string FastbootProcess(string command)
        {
            return AdbFastbootCommandR(command, 1);
        }

        public static string AdbProcess(string command)
        {
            return AdbFastbootCommandR(command, 0);
        }

        public static string AdbFastbootCommandR(string command, int type)
        {
            string[] r = AdbFastbootCommandR(new[] { command }, type);

            return r.Length > 0 ? r[0] : string.Empty;
        }

        public static string[] AdbFastbootCommandR(string[] args, int type)
        {
            List<string> ret = new List<string>();
            Cursor.Current = Cursors.WaitCursor;

            string executable = type == 0 ? ATA.ADBPath : ATA.FASTBOOTPath;

            if (executable == null)
            {
                return Array.Empty<string>();
            }

            ProcessStartInfo startInfo = new()
            {
                FileName = executable,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            foreach (string arg in args)
            {
                startInfo.Arguments = arg;
                using Process process = new() { StartInfo = startInfo };
                process.Start();

                if (type == 1)
                {
                    string error = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(error))
                    {
                        ret.Add(error);
                    }
                }
                string output = process.StandardOutput.ReadToEnd();

                if (!string.IsNullOrEmpty(output))
                {
                    ret.Add(output);
                }

                process.WaitForExit();
            }

            Cursor.Current = Cursors.Default;
            return ret.ToArray();
        }

        public static string ScrcpyVersion(string arguments)
        {
            return SystemProcess("", "scrcpy.exe", arguments);
        }

        public static void ScrcpyProcess(string arguments)
        {
            _ = Task.Run(() =>
            {
                return SystemProcess("", "scrcpy.exe", arguments);
            });
        }

        public static string SystemProcess(string command, string exe, string arguments)
        {
            Process cmd = new();
            cmd.StartInfo.FileName = exe;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = arguments;
            _ = cmd.Start();
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            string result = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();
            cmd.Close();
            return result;
        }

        public static void OpenLink(string link)
        {
            _ = Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
        }

        public static string SystemCommand(string command)
        {
            return SystemProcess(command, "cmd.exe", "");
        }

        public static Task<string> SystemCommandAsync(string command)
        {
            return Task.Run(() =>
            {
                return SystemCommand(command);
            });
        }
    }
}
