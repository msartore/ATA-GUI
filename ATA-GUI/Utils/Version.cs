using System;
using System.Linq;

namespace ATA_GUI.Utils
{
    internal class Version
    {
        public enum VersionComparisonResult
        {
            LessThan = -1,
            EqualTo = 0,
            GreaterThan = 1
        }

        public static VersionComparisonResult CompareVersions(string versionA, string versionB)
        {
            var partsA = versionA.TrimStart('v').Split(new[] { "_Pre-release" }, StringSplitOptions.None);
            var partsB = versionB.TrimStart('v').Split(new[] { "_Pre-release" }, StringSplitOptions.None);

            int[] verA = partsA[0].Split('.').Select(int.Parse).ToArray();
            int[] verB = partsB[0].Split('.').Select(int.Parse).ToArray();

            int maxLength = Math.Max(verA.Length, verB.Length);
            Array.Resize(ref verA, maxLength);
            Array.Resize(ref verB, maxLength);

            for (int i = 0; i < maxLength; i++)
            {
                if (verA[i] != verB[i])
                {
                    return verA[i] < verB[i] ? VersionComparisonResult.LessThan : VersionComparisonResult.GreaterThan;
                }
            }

            bool isAPreRelease = partsA.Length > 1;
            bool isBPreRelease = partsB.Length > 1;

            if (!isAPreRelease && isBPreRelease)
            {
                return VersionComparisonResult.GreaterThan;
            }
            else if (isAPreRelease && !isBPreRelease)
            {
                return VersionComparisonResult.LessThan;
            }

            return VersionComparisonResult.EqualTo;
        }
    }
}
