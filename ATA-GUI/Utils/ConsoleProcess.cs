using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATA_GUI.Utils
{
    internal class ConsoleProcess
    {
        public static string adbFastbootCommandR(string command, int type)
        {
            return adbFastbootCommandR(new[] { command }, type);
        }

        public static string adbFastbootCommandR(string[] args, int type)
        {
            StringBuilder ret = new();
            string line;
            Cursor.Current = Cursors.WaitCursor;
            Process process = new();
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            switch (type)
            {
                case 0:
                    startInfo.FileName = "adb.exe";
                    process.StartInfo = startInfo;

                    foreach (string s in args)
                    {
                        startInfo.Arguments = s;
                        _ = process.Start();
                        line = process.StandardOutput.ReadToEnd();
                        if (line.Length > 0)
                        {
                            _ = ret.Append(line);
                        }
                    }
                    break;
                case 1:
                    startInfo.FileName = "fastboot.exe";
                    process.StartInfo = startInfo;

                    foreach (string s in args)
                    {
                        startInfo.Arguments = s;
                        _ = process.Start();
                        line = process.StandardError.ReadToEnd();
                        if (line.Length > 0) { _ = ret.Append(line); }

                        line = process.StandardOutput.ReadToEnd();
                        if (line.Length > 0)
                        {
                            _ = ret.Append(line);
                        }
                    }
                    break;
                default:
                    break;
            }
            process.Close();
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
