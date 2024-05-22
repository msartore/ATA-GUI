using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATA_GUI.Utils
{
    internal class ConsoleProcess
    {
        public static string fastbootProcess(string command)
        {
            return adbFastbootCommandR(command, 1);
        }

        public static string adbProcess(string command)
        {
            return adbFastbootCommandR(command, 0);
        }

        public static string adbFastbootCommandR(string command, int type)
        {
            string[] r = adbFastbootCommandR(new[] { command }, type);

            return r.Length > 0 ? r[0] : string.Empty;
        }

        public static string[] adbFastbootCommandR(string[] args, int type)
        {
            List<string> ret = new List<string>();
            Cursor.Current = Cursors.WaitCursor;

            string executable = type switch
            {
                0 => "adb.exe",
                1 => "fastboot.exe",
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid type specified.")
            };

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

        public static string scrcpyVersion(string arguments)
        {
            return systemProcess("", "scrcpy.exe", arguments);
        }

        public static void scrcpyProcess(string arguments)
        {
            _ = Task.Run(() =>
            {
                return systemProcess("", "scrcpy.exe", arguments);
            });
        }

        public static string systemProcess(string command, string exe, string arguments)
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

        public static void openLink(string link)
        {
            _ = Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
        }

        public static string systemCommand(string command)
        {
            return systemProcess(command, "cmd.exe", "");
        }

        public static Task<string> systemCommandAsync(string command)
        {
            return Task.Run(() =>
            {
                return systemCommand(command);
            });
        }
    }
}
