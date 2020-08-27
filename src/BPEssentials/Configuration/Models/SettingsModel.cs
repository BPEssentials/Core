using System.Collections.Generic;

namespace BPEssentials.Configuration.Models.SettingsModel
{
    public class Settings
    {
        public General General { get; set; }


        public Messages Messages { get; set; }

        public Levenshtein Levenshtein { get; set; }

        public FunctionUI FunctionUI { get; set; }

        public ReportOptions ReportOptions { get; set; }

        public List<Command> Commands { get; set; }
    }

    public class Command
    {
        public string CommandName { get; set; }

        public List<string> Commands { get; set; }

        public string ExecutableBy { get; set; }

        public bool Disabled { get; set; }

        public bool AllowWhileDead { get; set; } = true;

        public bool AllowWithCrimes { get; set; } = true;

        public bool AllowWhileCuffed { get; set; } = true;

        public bool AllowWhileJailed { get; set; } = true;
    }

    public enum LevenshteinMode {
        Automatic,
        Suggest,
        None
    }

    public class Levenshtein 
    {
        public LevenshteinMode GiveMode { get; set; }
        public LevenshteinMode WarpMode { get; set; }
        public LevenshteinMode KitMode { get; set; }
        public LevenshteinMode SetJobMode { get; set; }
    }

    public class ReportOptions
    {
    }

    public class FunctionUI
    {
    }

    public class Messages
    {
        public string DiscordLink { get; set; }

        public string InfoColor { get; set; } = "#179b43";

        public string ArgColor { get; set; } = "#178d9b";

    }

    public class General
    {
        public string Version { get; set; }

        public int AnnounceInterval { get; set; }

        public int[] KeptItemsOnDeath { get; set; }

        public bool KeepAllItemsOnDeath { get; set; }

        public bool LocalChatOverHead { get; set; } = true;

        public bool LocalChatInChat { get; set; }

        public bool LimitNames { get; set; }
    }
}
