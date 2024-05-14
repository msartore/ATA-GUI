using System;
using System.Diagnostics;
using System.Text;
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
            return adbFastbootCommandR(new[] { command }, type);
        }

        public static string adbFastbootCommandR(string[] args, int type)
        {
            StringBuilder ret = new();
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

                string error = process.StandardError.ReadToEnd();
                string output = process.StandardOutput.ReadToEnd();

                if (!string.IsNullOrEmpty(output))
                {
                    ret.AppendLine(output);
                }
                if (!string.IsNullOrEmpty(error))
                {
                    ret.AppendLine(error);
                }

                process.WaitForExit();
            }

            Cursor.Current = Cursors.Default;
            return ret.ToString().Trim();
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
