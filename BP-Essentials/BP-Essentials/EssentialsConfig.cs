using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace BP_Essentials {
    public class EssentialsConfigPlugin : EssentialsCorePlugin{
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

        public static  string[] Jobs = { "Citizen", "Criminal", "Prisoner", "Police", "Paramedic", "Firefighter", "Gangster: Red", "Gangster: Green", "Gangster: Blue" };

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




        public static void ReadFile(string fileName)
        {
            switch (fileName)
            {
                case SettingsFile:
                    foreach (var line in File.ReadAllLines(SettingsFile)) //TODO: THIS METHOD IS A MESS, THERE IS PROBABLY A BETTER WAY.
                    {
                        if (line.StartsWith("#"))
                        {

                        }
                        else
                        {
                            if (line.Contains("version: "))
                            {
                                LocalVersion = line.Substring(9);
                            }
                            else if (line.Contains("CommandCharacter: "))
                            {
                                CmdCommandCharacter = line.Substring(18);
                            }
                            else if (line.Contains("noperm: "))
                            {
                                MsgNoPerm = line.Substring(8);
                            }
                            else if (line.Contains("ClearChatCommand: "))
                            {
                                CmdClearChat = CmdCommandCharacter + line.Substring(18);
                            }
                            else if (line.Contains("ClearChatCommand2: "))
                            {
                                CmdClearChat2 = CmdCommandCharacter + line.Substring(19);
                            }
                            else if (line.Contains("SayCommand: "))
                            {
                                CmdSay = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.Contains("SayCommand2:"))
                            {
                                CmdSay2 = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.Contains("msgSayPrefix: "))
                            {
                                MsgSayPrefix = line.Substring(14);
                            }
                            else if (line.StartsWith("GodmodeCommand: "))
                            {
                                CmdGodmode = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.StartsWith("GodmodeCommand2: "))
                            {
                                CmdGodmode2 = CmdCommandCharacter + line.Substring(17);
                            }
                            else if (line.StartsWith("MuteCommand: "))
                            {
                                CmdMute = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("UnMuteCommand: "))
                            {
                                CmdUnMute = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.Contains("UnknownCommand: "))
                            {
                                MsgUnknownCommand = Convert.ToBoolean(line.Substring(16));
                            }
                            else if (line.Contains("enableChatBlock: "))
                            {
                                ChatBlock = Convert.ToBoolean(line.Substring(17));
                            }
                            else if (line.Contains("enableLanguageBlock: "))
                            {
                                LanguageBlock = Convert.ToBoolean(line.Substring(21));
                            }
                            else if (line.Contains("RulesCommand: "))
                            {
                                CmdRules = CmdCommandCharacter + line.Substring(14);
                            }
                            else if (line.Contains("ReloadCommand: "))
                            {
                                CmdReload = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.Contains("ReloadCommand2: "))
                            {
                                CmdReload2 = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.Contains("TimeBetweenAnnounce: "))
                            {
                                TimeBetweenAnnounce = Int32.Parse(line.Substring(21));
                            }
                            else if (line.Contains("TimestapFormat: "))
                            {
                                TimestampFormat = line.Substring(16);
                            }
                            else if (line.Contains("AFKCommand: "))
                            {
                                CmdAfk = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.Contains("AFKCommand2: "))
                            {
                                CmdAfk2 = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.Contains("CheckIPCommand: "))
                            {
                                CmdCheckIp = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.Contains("CheckPlayerCommand: "))
                            {
                                CmdCheckPlayer = CmdCommandCharacter + line.Substring(20);
                            }
                            else if (line.Contains("FakeJoinCommand: "))
                            {
                                CmdFakeJoin = CmdCommandCharacter + line.Substring(17);
                            }
                            else if (line.Contains("FakeLeaveCommand: "))
                            {
                                CmdFakeLeave = CmdCommandCharacter + line.Substring(18);
                            }
                            else if (line.Contains("CheckForAlts: "))
                            {
                                CheckAlt = Convert.ToBoolean(line.Substring(14));
                            }
                            else if (line.Contains("DiscordCommand: "))
                            {
                                CmdDiscord = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.Contains("PlayersOnlineCommand: "))
                            {
                                CmdPlayers = CmdCommandCharacter + line.Substring(22);
                            }
                            else if (line.Contains("PlayersOnlineCommand2: "))
                            {
                                CmdPlayers2 = CmdCommandCharacter + line.Substring(23);
                            }
                            else if (line.Contains("InfoPlayerCommand: "))
                            {
                                CmdInfo = CmdCommandCharacter + line.Substring(19);
                            }
                            else if (line.Contains("InfoPlayerCommand2: "))
                            {
                                CmdInfo2 = CmdCommandCharacter + line.Substring(20);
                            }
                            else if (line.Contains("MoneyCommand: "))
                            {
                                CmdMoney = CmdCommandCharacter + line.Substring(14);
                            }
                            else if (line.Contains("MoneyCommand2: "))
                            {
                                CmdMoney2 = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.Contains("DiscordLink: "))
                            {
                                MsgDiscord = line.Substring(13);
                            }
                            else if (line.StartsWith("ATMCommand: "))
                            {
                                CmdAtm = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.StartsWith("EnableATMCommand: "))
                            {
                                EnableAtmCommand = Convert.ToBoolean(line.Substring(18));
                            }
                            else if (line.StartsWith("EnableBlockSpawnBot: "))
                            {
                                EnableBlockSpawnBot = Convert.ToBoolean(line.Substring(21));
                            }
                            else if (line.StartsWith("BlockSpawnBot: "))
                            {
                                if (EnableBlockSpawnBot == null)
                                {
                                    foreach (var line2 in File.ReadAllLines(SettingsFile)) //TODO: Bad way of doing it, but it works i guess
                                    {
                                        if (line2.StartsWith("#"))
                                        { }
                                        else
                                        {
                                            if (line2.StartsWith("EnableBlockSpawnBot: "))
                                            {
                                                EnableBlockSpawnBot = Convert.ToBoolean(line2.Substring(21));
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EnableBlockSpawnBot == true)
                                {
                                    DisabledSpawnBots = line.Substring(15);
                                    DisabledSpawnBots = DisabledSpawnBots.Replace(" ", String.Empty);
                                    if (DisabledSpawnBots.EndsWith(","))
                                    {
                                        EnableBlockSpawnBot = false;
                                        Debug.Log("[ERROR] BlockSpawnBot Cannot end with a comma!");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            BlockedSpawnIds = DisabledSpawnBots.Split(',').Select(int.Parse).ToArray();
                                        }
                                        catch (Exception ex)
                                        {
                                            EnableBlockSpawnBot = false;
                                            ErrorLogging(ex);
                                        }
                                    }
                                }
                            }
                            else if (line.Contains("HelpCommand: "))
                            {
                                CmdHelp = line.Substring(13);
                            }
                            else if (line.StartsWith("SaveCommand: "))
                            {
                                CmdSave = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("FreeCommand: "))
                            {
                                CmdFree = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("KillCommand: "))
                            {
                                CmdKill = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("KickCommand: "))
                            {
                                CmdKick = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("LogsCommand: "))
                            {
                                CmdLogs = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("TpCommand: "))
                            {
                                CmdTp = CmdCommandCharacter + line.Substring(11);
                            }
                            else if (line.StartsWith("TpHereCommand: "))
                            {
                                CmdTpHere = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.StartsWith("TpHereCommand2: "))
                            {
                                CmdTpHere2 = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.StartsWith("PayCommand: "))
                            {
                                CmdPay = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.StartsWith("PayCommand2: "))
                            {
                                CmdPay2 = CmdCommandCharacter + line.Substring(13);
                            }
                            else if (line.StartsWith("BanCommand: "))
                            {
                                CmdBan = CmdCommandCharacter + line.Substring(12);
                            }
                            else if (line.StartsWith("ConfirmCommand: "))
                            {
                                CmdConfirm = CmdCommandCharacter + line.Substring(16);
                            }
                            else if (line.StartsWith("ArrestCommand: "))
                            {
                                CmdArrest = CmdCommandCharacter + line.Substring(15);
                            }
                            else if (line.StartsWith("RestrainCommand: "))
                            {
                                CmdRestrain = CmdCommandCharacter + line.Substring(17);
                            }
                        }

                    }

                    break;
                case AnnouncementsFile:
                    Announcements = File.ReadAllLines(fileName);
                    break;
                case RulesFile:
                    Rules = File.ReadAllText(fileName);
                    break;
                case GodListFile:
                    GodListPlayers = File.ReadAllLines(fileName).ToList();
                    break;
                case AfkListFile:
                    AfkPlayers = File.ReadAllLines(fileName).ToList();
                    break;
                case MuteListFile:
                    MutePlayers = File.ReadAllLines(fileName).ToList();
                    break;
            }
        }

        public static void ReadFileStream(string fileName, List<string> output)
        {
            foreach (var line in File.ReadAllLines(fileName))
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                else
                {
                    output.Add(line);
                }
            }
        }

        public static void CheckFiles(string fileName)
        {
            if (fileName == "all")
            {
                if (!Directory.Exists(FileDirectory))
                {
                    Directory.CreateDirectory(FileDirectory);
                    Debug.Log(FileDirectory + " Does not exist! Creating one.");
                }
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                    Debug.Log(LogDirectory + " Does not exist! Creating one.");
                }
                if (!File.Exists(AfkListFile))
                {
                    File.Create(AfkListFile).Close();
                    Debug.Log(AfkListFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(MuteListFile))
                {
                    File.Create(MuteListFile).Close();
                    Debug.Log(MuteListFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(ChatBlockFile))
                {
                    File.Create(ChatBlockFile).Close();
                    CreateFile(ChatBlockFile);
                    Debug.Log(ChatBlockFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(GodListFile))
                {
                    File.Create(GodListFile).Close();
                    Debug.Log(GodListFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(IpListFile))
                {
                    File.Create(IpListFile).Close();
                    Debug.Log(IpListFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(LanguageBlockFile))
                {
                    File.Create(LanguageBlockFile).Close();
                    CreateFile(LanguageBlockFile);
                    Debug.Log(LanguageBlockFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(MuteListFile))
                {
                    File.Create(MuteListFile).Close();
                    Debug.Log(MuteListFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(SettingsFile))
                {
                    File.Create(SettingsFile).Close();
                    CreateFile(SettingsFile);
                    Debug.Log(SettingsFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(CustomCommandsFile))
                {
                    File.Create(CustomCommandsFile).Close();
                    CreateFile(CustomCommandsFile);
                    Debug.Log(CustomCommandsFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(AnnouncementsFile))
                {
                    File.Create(AnnouncementsFile).Close();
                    Debug.Log(AnnouncementsFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(LogFile))
                {
                    File.Create(LogFile).Close();
                    Debug.Log(LogFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(ChatLogFile))
                {
                    File.Create(ChatLogFile).Close();
                    Debug.Log(ChatLogFile + " Does not exist! Creating one.");
                }
                if (!File.Exists(CommandLogFile))
                {
                    File.Create(CommandLogFile).Close();
                    Debug.Log(CommandLogFile + " Does not exist! Creating one.");
                }
            }
            else
            {
                if (!File.Exists(fileName))
                {

                    if (fileName == SettingsFile)
                    {
                        File.Create(fileName).Close();
                        CreateFile(fileName);
                        Debug.Log(fileName + " Does not exist! Creating one.");
                    }
                    if (fileName == ChatBlockFile)
                    {
                        File.Create(fileName).Close();
                        CreateFile(fileName);
                        Debug.Log(fileName + " Does not exist! Creating one.");
                    }
                    if (fileName == LanguageBlockFile)
                    {
                        File.Create(fileName).Close();
                        CreateFile(fileName);
                        Debug.Log(fileName + " Does not exist! Creating one.");
                    }
                    if (fileName == CustomCommandsFile)
                    {
                        File.Create(fileName).Close();
                        CreateFile(fileName);
                        Debug.Log(fileName + " Does not exist! Creating one.");
                    }
                    else
                    {
                        File.Create(fileName).Close();
                        Debug.Log(fileName + " Does not exist! Creating one.");
                    }
                }
            }
        }

        private static void CreateFile(string fileName)
        {
            switch (fileName) {
                case SettingsFile: {
                    string[] content = { "# ---------------------------------------------------------------------------------------- #", "#                             Broke Protocol: Essentials                                   #", "#                        Created by UserR00T and DeathByKorea and BP                       #", "# ---------------------------------------------------------------------------------------- #", "#                                                                                          #", "#                                                                                          #", "#                                                                                          #", "# NOTE:                                                                                    #", "# CommandCharacter will be automatically added to the commands! No need to do that!        #", "# Example:                                                                                 #", "# INCORRECT: ClearChatCommand: /clearchat                                                  #", "# CORRECT: ClearChatCommand: clearchat                                                     #", "# if CommandCharacter is / it will automatically add a /                                   #", "#                                                                                          #", "# ---------------------------------------------------------------------------------------- #", "", "", "", "", "", "#----------------------------------------------------------#", "#                           General                        #", "#----------------------------------------------------------#", "version: " + Version, "", "# Character used for commands", "#----------------------------------", "CommandCharacter: /", "", @"# Will display ""unknown command"" if the command is not found in this plugin", "#----------------------------------", "UnknownCommand: true", "", "noperm: No permission.", "", "", "", "", "", "#----------------------------------------------------------#", "#                          Commands                        #", "#----------------------------------------------------------#", "", "# Reload command", "#----------------------------------", "ReloadCommand: reload", "ReloadCommand2: rl", "", "# Clearchat command", "#----------------------------------", "ClearChatCommand: clearchat", "ClearChatCommand2: cc", "", "# Say command", "#----------------------------------", "SayCommand: say", "SayCommand2: broadcast", "", "# CheckIP/Player command", "#----------------------------------", "CheckIPCommand: checkip", "CheckPlayerCommand: checkplayer", "", "# Fake Join/Leave command", "#----------------------------------", "FakeLeaveCommand: fakeleave", "FakeJoinCommand: fakejoin", "", "# Player info command", "#----------------------------------", "InfoPlayerCommand: info", "InfoPlayerCommand2: stats", "", "# Money command", "#----------------------------------", "MoneyCommand: money", "MoneyCommand2: setbal", "", "# Rules command", "#----------------------------------", "RulesCommand: rules", "", "", "# Discord command", "#----------------------------------", "DiscordCommand: discord", "", "# Online Players command", "#----------------------------------", "PlayersOnlineCommand: players", "PlayersOnlineCommand2: online", "", "# Godmode command --- Godmode default saving location - 'godmode.txt'", "#----------------------------------", "GodmodeCommand: god", "GodmodeCommand2: godmode", "", "# AFK command --- AFK default saving location - 'afkplayers.txt'", "#----------------------------------", "AFKCommand: afk", "AFKCommand2: brb", "", "# Mute command --- Mute default saving location - 'muteplayers.txt'", "#----------------------------------", "MuteCommand: mute", "# Unmute Command", "UnMuteCommand: unmute", "", "# ATM command", "#----------------------------------", "ATMCommand: atm", "", "", "", "#----------------------------------------------------------#", "#                          Messages                        #", "#----------------------------------------------------------#", "# Say prefix: Will show the string infront of the message", "# Example: [!!!] UserR00T: This is a message!", "# A space is not required at the end.", "#----------------------------------", "msgSayPrefix: [!!!]", "", "# Discord invite link", "#----------------------------------", "DiscordLink: https://discord.gg/Test", "", "", "", "#----------------------------------------------------------#", "#                    Additional Settings                   #", "#----------------------------------------------------------#", "", "", "# ChatBlock --- Words default location - 'chatblock.txt'", "# Enable chatblock", "#----------------------------------", "enableChatBlock: true", "", "", "# LanguageBlock --- Words default location - 'languageblock.txt'", "# Enable LanguageBlock", "#----------------------------------", "enableLanguageBlock: true", "", "# Enable ATM command", "#----------------------------------", "EnableATMCommand: true", "", "# This will check for alt accounts with the same IP.", "# NOTE: If someone in the same home connects with the same IP it will be detected as alt.", "# Enable CheckForAlts", "#----------------------------------", "CheckForAlts: true", "", "", "# Seconds between annoucements in seconds -- Announcements in 'Annoucements.txt'", "#----------------------------------", "TimeBetweenAnnounce: 360", "", "", "# TimestampFormat; This means what you will see infront of a message: ", "# E.g: [{H}:{M}:{S}] and your time is 12:05:59 it will show [12:05:59]", "# E.g Time/Date:                                 | [Year] [Month] [Day] [Hour] [Minute] [Second] [AM/PM]", "#                                                |  2017    9       22    22     18        6        PM", "# Placeholders:", "# {YYYY} Year                                    |  2017", "# {MM} Month (2 numbers)                         |          9", "# {MMMM} Month (full name)                       |        September", "# {DD} Day (2 numbers)                           |                  22", "# {DDDD} Day (full name)                         |                Friday", "# {H} Hours (24 hour clock)                      |                        22", "# {h} Hours (12 hour clock)                      |                        10", "# {M} Minutes                                    |                               18", "# {S} Seconds                                    |                                         6", "# {T} AM/PM (used in {h} 12 hour clock)          |                                                  PM", "# So, this would be:", "# [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]", "# [2017:09:22] [22:18:06]", "#----------------------------------", "TimestapFormat: [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]" };
                    File.WriteAllLines(SettingsFile, content);
                    break;
                }
                case ChatBlockFile: {
                    string[] content = { "nigger", "nigga", "nigg3r", "NIGGER", "NI99ER", "ni99er", "nigger.", "nigga.", "nigg3r.", "N199ER", "n1gger", "N1GGER", "NIGGA", "NIGGA." };
                    File.WriteAllLines(ChatBlockFile, content);
                    break;
                }
                case LanguageBlockFile: {
                    string[] content = { "bombas", "hola", "alguien", "habla", "espanol", "español", "estoy", "banco", "voy", "consegi", "donde", "quedamos", "banko", "afuera", "estas", "alguem", "donde", "nos", "vemos", "soy ", "vueno", "como", "carro", "cabros", "miren", "hacha", "laar", "corri", "sacame", "aqui", "policia", "trajo", "encerro", "bomba", "beuno", "pantalones", "dinero", "porque", "tengo", "escopetaa", "escopeta" };
                    File.WriteAllLines(LanguageBlockFile, content);
                    break;
                }
                case CustomCommandsFile: {
                    string[] content = { "# Custom commands file", "# Place your custom commands here", "#", "# Format:", "# Command: example", "# Respone: The time is {H}:{M}:{S}. ", "#", "# ", "# Placeholders:", "# ", "# Current time:", "# E.g Time/Date:                                 | [Year] [Month] [Day] [Hour] [Minute] [Second] [AM/PM]", "#                                                |  2017    9       22    22     18        6        PM", "# Placeholders:", "# {YYYY} Year                                    |  2017", "# {MM} Month (2 numbers)                         |          9", "# {MMMM} Month (full name)                       |        September", "# {DD} Day (2 numbers)                           |                  22", "# {DDDD} Day (full name)                         |                Friday", "# {H} Hours (24 hour clock)                      |                        22", "# {h} Hours (12 hour clock)                      |                        10", "# {M} Minutes                                    |                               18", "# {S} Seconds                                    |                                         6", "# {T} AM/PM (used in {h} 12 hour clock)          |                                                  PM", "# So, this would be:", "# [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]", "# [2017:09:22] [22:18:06]", "#", "# Player placeholders:", "# {username} Players username", "# ----------------------------------------------", "", "Command: servertime", "Response: The time on the server(pc) is: {YYYY}/{MM}/{DD}, {H}:{M}:{S}.", "", "Command: example", "Response: Your username is {username}!" };
                    File.WriteAllLines(CustomCommandsFile, content);
                    break;
                }
            }
        }

        public static void ReadCustomCommands()
        {
            string line;
            using (var file = new StreamReader(CustomCommandsFile))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.ToLower().StartsWith("#") || string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    else
                    {
                        if (line.ToLower().StartsWith("command: "))
                        {
                            Commands.Add(CmdCommandCharacter + line.Substring(9));
                            line = file.ReadLine();
                            if (line.ToLower().StartsWith("response: "))
                            {
                                Responses.Add(line.Substring(10));
                            }
                        }
                    }
                }
                file.Close();
            }
        }


    }
}