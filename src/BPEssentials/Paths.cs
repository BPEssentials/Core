using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPEssentials
{
    public class Paths
    {
        public static string EssentialsFolder { get; } = "Essentials";

        public string SettingsFile { get; } = Path.Combine(EssentialsFolder, "settings.json");

        public string CustomCommandsFile { get; } = Path.Combine(EssentialsFolder, "CustomCommands.json");

        public string LocalizationFile { get; } = Path.Combine(EssentialsFolder, "localization.json");

        public string WarpsFolder { get; } = Path.Combine(EssentialsFolder, "Warps");

        public string KitsFolder { get; } = Path.Combine(EssentialsFolder, "Kits");

    }
}
