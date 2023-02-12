using System.IO;

namespace BPEssentials
{
    public static class Paths
    {
        public static string EssentialsFolder { get; } = "Essentials";

        public static string SettingsFile { get; } = Path.Combine(EssentialsFolder, "settings.json");

        public static string CustomCommandsFile { get; } = Path.Combine(EssentialsFolder, "CustomCommands.json");

        public static string LocalizationFile { get; } = Path.Combine(EssentialsFolder, "localization.json");

        public static string WarpsFolder { get; } = Path.Combine(EssentialsFolder, "Warps");

        public static string KitsFolder { get; } = Path.Combine(EssentialsFolder, "Kits");

    }
}
