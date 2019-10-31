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
        public const string EssentialsFolder = "Essentials";

        public string SettingsFile { get; } = Path.Combine(EssentialsFolder, "settings.json");

        public string CustomCommandsFile { get; } = Path.Combine(EssentialsFolder, "CustomCommands.json");
    }
}
