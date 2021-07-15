﻿using System.Collections.Generic;

namespace BPEssentials.Configuration.Models.SettingsModel
{
    public class Settings
    {
        public General General { get; set; }

        public KeptItemsOnDeath KeptItemsOnDeath { get; set; }

        public Messages Messages { get; set; }

        public Levenshtein Levenshtein { get; set; }

        public FunctionUI FunctionUI { get; set; }

        public ReportOptions ReportOptions { get; set; }

        public Warns Warns { get; set; }

        public List<Command> Commands { get; set; }
    }

    public class Command
    {
        public string CommandName { get; set; }

        public List<string> Commands { get; set; }

        public bool Disabled { get; set; }

        public bool AllowWhileDead { get; set; } = true;

        public bool AllowWhileKO { get; set; } = true;

        public bool AllowWithCrimes { get; set; } = true;

        public bool AllowWhileCuffed { get; set; } = true;

        public bool AllowWhileJailed { get; set; } = true;

        public int CoolDown { get; set; }
    }

    public enum LevenshteinMode
    {
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

    public class KeptItemsOnDeath
    {
        public bool KeepAllItemsOnDeath { get; set; }

        public int[] KeptItemIds { get; set; }

        public string[] KeptItemNames { get; set; }

        public bool KeepAllPhones { get; set; }

        public bool KeepAllLicenses { get; set; }
    }

    public class General
    {
        public string Version { get; set; }

        public int SaveInterval { get; set; } = 15;

        public bool LocalChatOverHead { get; set; } = true;

        public bool LocalChatInChat { get; set; }

        public bool LimitNames { get; set; }

        public bool DisableAccountOverwrite { get; set; }
    }

    public class Warns
    {
        public int WarnsExpirationInDays { get; set; } = 30;

        public bool DeleteExpiredWarns { get; set; }
    }
}