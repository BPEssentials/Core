using System.Collections.Generic;

namespace BPEssentials.Configuration.Models.SettingsModel
{
    public class Settings
    {
        public General General { get; set; }

        public Messages Messages { get; set; }

        public List<string> Announcements { get; set; }

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

        public bool AllowWithCrimes { get; set; } = true;

        public bool AllowWhileCuffed { get; set; } = true;

        public bool AllowWhileJailed { get; set; } = true;
    }

    public class ReportOptions
    {
    }

    public class FunctionUI
    {
    }

    public class Messages
    {
    }

    public class General
    {
        public string Version { get; set; }
    }
}
