using System.Text.RegularExpressions;

namespace ATA_GUI.Utils
{
    class Network
    {
        private static readonly string regexIP = "(\\b25[0-5]|\\b2[0-4][0-9]|\\b[01]?[0-9][0-9]?)(\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}";

        public static bool IsValidIP(string IP)
        {
            return Regex.Match(IP, regexIP).Success;
        }
    }
}
