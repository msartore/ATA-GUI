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
            StringBuilder ret = new StringBuilder();
            string line;
            Cursor.Current = Cursors.WaitCursor;
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
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
                        process.Close();
                    }
                    break;
                case 1:
                    startInfo.FileName = "fastboot.exe";
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
                        process.Close();
                    }
                    break;
                default:
                    break;
            }
            return ret.ToString().Trim();
        }

        public static async Task<string> systemCommandAsync(string command)
        {
            return await Task.Run(() =>
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                _ = cmd.Start();
                cmd.StandardInput.WriteLine(command);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                return cmd.StandardOutput.ReadToEnd();
            });
        }
    }
}
