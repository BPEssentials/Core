using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP_Essentials
{
    public class EssentialsVariablesPlugin : EssentialsCorePlugin
    {
        // Generic Constants
        public const string FileDirectory = "Essentials/";
        public const string LogDirectory = FileDirectory + "logs/";

        public const string SettingsFile = FileDirectory + "essentials_settings.txt";
        public const string LanguageBlockFile = FileDirectory + "languageblock.txt";
        public const string ChatBlockFile = FileDirectory + "chatblock.txt";
        public const string AnnouncementsFile = FileDirectory + "announcements.txt";
        public const string IpListFile = FileDirectory + "ip_list.txt";
        public const string GodListFile = FileDirectory + "godlist.txt";
        public const string AfkListFile = FileDirectory + "afklist.txt";
        public const string MuteListFile = FileDirectory + "mutelist.txt";
        public const string ExeptionFile = FileDirectory + "exceptions.txt";
        public const string CustomCommandsFile = FileDirectory + "CustomCommands.txt";

        public const string AdminListFile = "admin_list.txt";
        public const string RulesFile = "server_info.txt";

        public const string LogFile = LogDirectory + "all.txt";
        public const string ChatLogFile = LogDirectory + "chat.txt";
        public const string CommandLogFile = LogDirectory + "commands.txt";

        public const string Version = "PRE-2.0.0";

        // Bools

        public static bool MsgUnknownCommand;
        public static bool ChatBlock;
        public static bool LanguageBlock;
        public static bool CheckAlt;
        public static bool All;
        public static bool Unmute;
        public static bool MessageToLower;
        public static bool EnableAtmCommand;
        public static bool Confirmed;
        public static bool? EnableBlockSpawnBot;

        // Lists

        public static List<string> Commands = new List<string>();
        public static List<string> Responses = new List<string>();
        public static List<string> ChatBlockWords = new List<string>();
        public static List<string> LanguageBlockWords = new List<string>();
        public static List<string> AdminsListPlayers = new List<string>();
        public static List<string> GodListPlayers = new List<string>();
        public static List<string> AfkPlayers = new List<string>();
        public static List<string> MutePlayers = new List<string>();

        // Arrays
        public static string[] Announcements;

        public static string[] Jobs = { "Citizen", "Criminal", "Prisoner", "Police", "Paramedic", "Firefighter", "Gangster: Red", "Gangster: Green", "Gangster: Blue" };

        // Messages
        public static string MsgSayPrefix;
        public static string MsgNoPerm;
        public static string MsgDiscord;
        public const string DisabledCommand = "The server owner disabled this command.";

        // Strings
        public static string Rules;
        public static string DisabledSpawnBots;
        public static string LocalVersion;


        #region Commands
        // Commands

        public static string CmdCommandCharacter;
        public static string CmdClearChat;
        public static string CmdClearChat2;
        public static string CmdSay;
        public static string CmdSay2;
        public static string CmdGodmode;
        public static string CmdGodmode2;
        public static string CmdMute;
        public static string CmdUnMute;
        public static string CmdReload;
        public static string CmdReload2;
        public static string CmdAfk;
        public static string CmdAfk2;
        public static string CmdRules;
        public static string CmdCheckIp;
        public static string CmdCheckPlayer;
        public static string CmdFakeJoin;
        public static string CmdFakeLeave;
        public static string CmdDiscord;
        public static string Arg1ClearChat;
        public static string CmdPlayers;
        public static string CmdPlayers2;
        public static string CmdInfo;
        public static string CmdInfo2;
        public static string CmdMoney;
        public static string CmdMoney2;
        public static string CmdAtm;
        public static string CmdSave;
        public static string CmdPay;
        public static string CmdPay2;
        public static string CmdTpHere;
        public static string CmdTpHere2;
        public static string CmdKill;
        public static string CmdDbug;
        public static string CmdBan;
        public static string CmdKick;
        public static string CmdHelp;
        public static string CmdConfirm;
        public static string CmdLogs;
        public static string CmdArrest;
        public static string CmdRestrain;
        public static string CmdFree;
        public static string CmdTp;

        #endregion

        // Ints
        public const int SaveTime = 60 * 5;

        public static int AnnounceIndex;
        public static int TimeBetweenAnnounce;
        public static int[] BlockedSpawnIds;

        // Misc.
        public static string TimestampFormat;

    }
}
