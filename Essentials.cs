// Essentials created by UserR00T & DeathByKorea
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EssentialsPlugin
{
    #region Folder Locations
    private static string FileDirectory = "Essentials/";

    private static string SettingsFile = FileDirectory + "essentials_settings.txt";
    private static string LanguageBlockFile = FileDirectory + "languageblock.txt";
    private static string ChatBlockFile = FileDirectory + "chatblock.txt";
    private static string AnnouncementsFile = FileDirectory + "announcements.txt";
    private static string IPListFile = FileDirectory + "ip_list.txt";
    private static string AdminListFile = "admin_list.txt";
    private static string GodListFile = FileDirectory + "godlist.txt";
    private static string AfkListFile = FileDirectory + "afklist.txt";
    private static string MuteListFile = FileDirectory + "mutelist.txt";
    private static string RulesFile = FileDirectory + "rules.txt";
    private static string ExeptionFile = FileDirectory + "exceptions.txt";
    #endregion

    #region predefining variables

    // General
    private static string version;
    private static string command;
    private static SvPlayer player;
    private static ShPlayer Splayer;
    private static string TimestampFormat;

    // Booleans
    private static bool msgUnknownCommand;
    private static bool ChatBlock;
    private static bool LanguageBlock;
    private static bool CheckAlt;
    private static bool all;
    private static bool unmute;
    private static bool MessageToLower;

    // Lists
    private static List<string> ChatBlockWords = new List<string>();
    private static List<string> LanguageBlockWords = new List<string>();
    private static List<string> AdminsListPlayers = new List<string>();
    private static List<string> GodListPlayers = new List<string>();
    private static List<string> AfkPlayers = new List<string>();
    private static List<string> MutePlayers = new List<string>();

    // Arrays
    private static string[] rules;
    private static string[] rules2;
    private static string[] announcements;

    // Messages
    private static string msgSayPrefix;
    private static string msgNoPerm;
    private static string msgDiscord;

    // Commands
    private static string cmdCommandCharacter;
    private static string cmdClearChat;
    private static string cmdClearChat2;
    private static string cmdSay;
    private static string cmdSay2;
    private static string cmdGodmode;
    private static string cmdGodmode2;
    private static string cmdMute;
    private static string cmdUnMute;
    private static string cmdReload;
    private static string cmdReload2;
    private static string cmdAfk;
    private static string cmdAfk2;
    private static string cmdBan;
    private static string cmdKick;
    private static string cmdRules;
    private static string cmdCheckIP;
    private static string cmdCheckPlayer;
    private static string cmdFakeJoin;
    private static string cmdFakeLeave;
    private static string cmdDiscord;
    private static string arg1ClearChat;

    // Ints
    private static int announceIndex = 0;
    private static int TimeBetweenAnnounce;

    #endregion

    //Code below here, Don't edit unless you know what you're doing.
    //Information about the api @ https://github.com/DeathByKorea/UniversalUnityhooks

    [Hook("SvNetMan.StartServerNetwork")]
    public static void StartServerNetwork(SvNetMan netMan)
    {

        try
        {
            Reload(true);
            Debug.Log("-------------------------------------------------------------------------------");
            Debug.Log(" ");
            Debug.Log("[INFO] Essentials - version: " + version + " Loaded in successfully!");
            Debug.Log(" ");
            Debug.Log("-------------------------------------------------------------------------------");
        }
        catch(Exception ex)
        {
            Debug.Log("-------------------------------------------------------------------------------");
            Debug.Log(" ");
            Debug.Log("[WARNING] Essentials - Settings file does not exist! Make sure to place settings.txt in the game directory!");
            Debug.Log(" ");
            Debug.Log("-------------------------------------------------------------------------------");
            Debug.Log(ex);
        }

        if (!File.Exists(AnnouncementsFile))
        {
            Debug.Log(SetTimeStamp() + "[WARNING] Annoucements file doesn't exist! Please create " + AnnouncementsFile + " in the game directory");
        }
        Thread thread = new Thread(new ParameterizedThreadStart(AnnounceThread));
        thread.Start(netMan);
        Debug.Log(SetTimeStamp() + "[INFO] Announcer started successfully!");
    }

    //Chat Events
    [Hook("SvPlayer.SvGlobalChatMessage")]
    public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
    {
        // Log message
        MessageLog(message, player);
        // If MessageToLower is enabled and its a command, lower the command but not the argument
        if (MessageToLower)
        {
            if (message.StartsWith(cmdCommandCharacter))
            {
                if (message.Contains(" "))
                {
                    string TempArg = null;
                    string TempMessage = null;

                    TempMessage = message.Substring(0, message.LastIndexOf(" ") + 1);
                    if (message.EndsWith(" "))
                    {
                        message = TempMessage.ToLower().Trim();
                    }
                    else
                    {
                        TempArg = message.Substring(message.IndexOf(" ") + 1);
                        message = TempMessage.ToLower() + TempArg.Trim();
                    }
                }
                else
                {
                    message = message.ToLower();
                }
            }
        }


        // If player is afk, unafk him
        if (AfkPlayers.Contains(player.playerData.username))
        {
            afk(message, player);
        }
        //Checks if player is muted, if so, cancel message
        if (MutePlayers.Contains(player.playerData.username))
        {
            player.SendToSelf(Channel.Unsequenced, (byte)10, "You are muted.");
            return true;
        }
        // Command: ClearChat
        if (message.StartsWith(cmdClearChat) || message.StartsWith(cmdClearChat2))
        {
            if (message.Contains("all") || message.Contains("everyone"))
            {
                all = true;
                ClearChat(message, player, all);
                return true;

            }
            else
            {
                all = false;
                ClearChat(message, player, all);
                return true;
            }
        }
        // Command: Reload
        if (message.StartsWith(cmdReload) || message.StartsWith(cmdReload2))
        {
            Reload(false, message, player);
            return true;
        }
        // Command: Mute
        if (message.StartsWith(cmdUnMute))
        {
            unmute = true;
            Mute(message, player, unmute);
            return true;

        }
        else if (message.StartsWith(cmdMute))
        {
            unmute = false;
            Mute(message, player, unmute);
            return true;
        }
        // Command: Say
        if (message.StartsWith(cmdSay) || (message.StartsWith(cmdSay2)))
        {
            say(message, player);
            return true;
        }
        // Command: GodMode
        if (message.StartsWith(cmdGodmode) || message.StartsWith(cmdGodmode2))
        {
            godMode(message, player);
            return true;
        }
        // Command: AFK
        if (message.StartsWith(cmdAfk) || message.StartsWith(cmdAfk2))
        {
            afk(message, player);
            return true;
        }
        // Command: Main/essentials
        if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
        {
            essentials(message, player);
            return true;
        }
        // Command: Rules
        if (message.StartsWith(cmdRules))
        {
            string arg1 = message.Substring(cmdRules.Length);
            int arg1int;
            bool isNumeric = int.TryParse(arg1, out arg1int);
            if (isNumeric)
            {
                printRules(message, player, arg1int);
                return true;
            }
            else if (!(isNumeric))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "That is not a valid argument.");
                return true;
            }
        }
        // Command: CheckIP
        if (message.StartsWith(cmdCheckIP))
        {
            if (message != cmdCheckIP)
            {
                string arg1 = message.Substring(cmdCheckIP.Count() + 1);
                CheckIP(message, player, arg1);
                return true;
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                return true;
            }
        }
        // Command: CheckPlayer
        if (message.StartsWith(cmdCheckPlayer))
        {
            if (message != cmdCheckPlayer)
            {
                string arg1 = message.Substring(cmdCheckPlayer.Count() + 1);
                CheckPlayer(message, player, arg1);
                return true;
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                return true;
            }
        }
        // Command: Fake Join/Leave
        if (message.StartsWith(cmdFakeJoin) || (message.StartsWith(cmdFakeLeave)))
        {
            if (!(message == cmdFakeJoin || message == cmdFakeLeave))
            {
                string arg1 = null;
                arg1 = message.Split(' ').Last();
                if (message.StartsWith(cmdFakeJoin))
                {
                    player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " Connected");
                }
                else if (message.StartsWith(cmdFakeLeave))
                {
                    player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " Disconnected");
                }
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
            }

            return true;
        }
        // Command: Discord
        if (message.StartsWith(cmdDiscord))
        {
            player.SendToSelf(Channel.Unsequenced, (byte)10, "Discord: " + msgDiscord);
            return true;
        }
        // Message: Unkonwn command
        if (message.StartsWith(cmdCommandCharacter))
        {
            if (msgUnknownCommand)
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Unknown command");
                return true;
            }
            else
            {
                return false;
            }
        }

        //Checks if the message is a blocked one, if it is, block it.
        if (ChatBlock || LanguageBlock)
        {
            bool blocked = BlockMessage(message, player);
            if (blocked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Check if message contains a player that is AFK
        if (AfkPlayers.Any(message.Contains))
        {
            player.SendToSelf(Channel.Unsequenced, (byte)10, "That player is AFK.");
            return true;
        }
        return false;

    }
    private static void AnnounceThread(object man)
    {
        SvNetMan netMan = (SvNetMan) man;
        while (true)
        {
            foreach (var player in netMan.players)
            {
                player.svPlayer.SendToSelf(Channel.Reliable, ClPacket.GameMessage, announcements[announceIndex]);
            }
            Debug.Log(SetTimeStamp() + "[INFO] Announcement made...");

            announceIndex += 1;
            if (announceIndex > announcements.Length - 1)
                announceIndex = 0;
            Thread.Sleep(TimeBetweenAnnounce * 1000);
        }
    }
    public static void CheckIP(string message, object oPlayer, string arg1)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        int found = 0;
        player.SendToSelf(Channel.Unsequenced, (byte) 10, "IP for player: " + arg1);
        foreach (var line in File.ReadAllLines(IPListFile))
        {
            if (line.Contains(arg1) || line.Contains(arg1 + ":"))
            {
                ++found;
                player.SendToSelf(Channel.Unsequenced, (byte) 10, line.Substring(arg1.Length + 1));
            }
        }
        player.SendToSelf(Channel.Unsequenced, (byte) 10, arg1 + " occurred " + found + " times in the iplog file.");
    }
    public static void CheckPlayer(string message, object oPlayer, string arg1)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        int found = 0;
        string arg1temp = null;
        player.SendToSelf(Channel.Unsequenced, (byte) 10, "Accounts for IP: " + arg1);
        foreach (var line in File.ReadAllLines(IPListFile))
        {
            if (line.Contains(arg1) || line.Contains(" " + arg1))
            {
                ++found;
                arg1temp = line.Replace(": " + arg1, " ");
                player.SendToSelf(Channel.Unsequenced, (byte) 10, arg1temp);
            }
        }
        player.SendToSelf(Channel.Unsequenced, (byte) 10, arg1 + " occurred " + found + " times in the iplog file.");
    }
    public static void MessageLog(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        if (!message.StartsWith(cmdCommandCharacter))
        {
            Debug.Log(SetTimeStamp() + "[MESSAGE] " + player.playerData.username + ": " + message);
        }
        else if (message.StartsWith(cmdCommandCharacter))
        {
            Debug.Log(SetTimeStamp() + "[COMMAND] " + player.playerData.username + ": " + message);
        }
    }

    public static void printRules(string message, object oPlayer, int pagenumber = 1)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        rules2 = rules.Skip(5 * pagenumber - 5).ToArray();
        int linecnt = rules.Count();
        if (linecnt > 5)
        {
        player.SendToSelf(Channel.Unsequenced, (byte) 10, "Rules: Type " + cmdRules + " (pagenumber) for the next page");
        }
        else if (linecnt < 5)
        {
            player.SendToSelf(Channel.Unsequenced, (byte) 10, "Rules:");
        }
        int id = 0;
        foreach (string rule in rules2)
        {
            ++id;
            player.SendToSelf(Channel.Unsequenced, (byte) 10, rule);
            if (id > 5)
            {
                id = 0;
                break;
            }
        }

    }

    public static bool Mute(string message, object oPlayer, bool unmute)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        string muteuser = message.Remove(0, message.IndexOf(' ') + 1 );
        if (AdminsListPlayers.Contains(player.playerData.username))
        {

            ReadFile(MuteListFile);

            if (unmute)
            {
                if (!MutePlayers.Contains(muteuser))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, muteuser + " is not muted!");
                    return true;

                }
                if (MutePlayers.Contains(muteuser))
                {
                    RemoveStringFromFile(MuteListFile, muteuser);
                    ReadFile(MuteListFile);
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, muteuser + " Unmuted");
                    return true;
                }

            }
            else if (!unmute)
            {
                MutePlayers.Add(muteuser);
                File.AppendAllText(MuteListFile, muteuser + Environment.NewLine);
                player.SendToSelf(Channel.Unsequenced, (byte) 10, muteuser + " Muted");
                return true;
            }
        }
        else
        {
            player.SendToSelf(Channel.Unsequenced, (byte) 10, msgNoPerm);
            return false;
        }
        return false;
    }

    public static void afk(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        ReadFile(AfkListFile);
        if (AfkPlayers.Contains(player.playerData.username))
        {
            RemoveStringFromFile(AfkListFile, player.playerData.username);
            ReadFile(AfkListFile);
            player.SendToSelf(Channel.Unsequenced, (byte) 10, "You are no longer AFK");
        }
        else
        {
            File.AppendAllText(AfkListFile, player.playerData.username + Environment.NewLine);
            AfkPlayers.Add(player.playerData.username);
            player.SendToSelf(Channel.Unsequenced, (byte) 10, "You are now AFK");
        }
    }
    public static bool BlockMessage(string message, object oPlayer)
    {
        message = message.ToLower();
        SvPlayer player = (SvPlayer) oPlayer;
        if (ChatBlock == true)
        {
            if (ChatBlockWords.Contains(message))
            {
                player.SendToSelf(Channel.Unsequenced, (byte) 10, "Please don't say a blacklisted word, the message has been blocked.");
                Debug.Log(SetTimeStamp() + player.playerData.username + " Said a word that is blocked.");
                return true;
            }
        }
        if (LanguageBlock == true)
        {
            if (LanguageBlockWords.Contains(message))
            {
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "Because you are staff, your message has NOT been blocked.");
                    return false;
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "--------------------------------------------------------------------------------------------");
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "             ?olo ingl�s! Tu mensaje ha sido bloqueado.");
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "             Only English! Your message has been blocked.");
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "--------------------------------------------------------------------------------------------");
                    return true;
                }
            }
        }
        return false;
    }

    public static bool godMode(string message, object oPlayer)
    {
        try
        {
            SvPlayer player = (SvPlayer) oPlayer;
            ReadFile(GodListFile);

            if (AdminsListPlayers.Contains(player.playerData.username))
            {
                if (GodListPlayers.Contains(player.playerData.username))
                {
                    RemoveStringFromFile(GodListFile, player.playerData.username);
                    ReadFile(GodListFile);
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "Godmode disabled.");
                    return true;
                }
                else
                {
                    File.AppendAllText(GodListFile, player.playerData.username + Environment.NewLine);
                    GodListPlayers.Add(player.playerData.username);
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "Godmode enabled.");
                    return true;
                }
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte) 10, msgNoPerm);
                return false;
            }

        }
        catch (Exception ex)
        {

            ErrorLogging(ex);
            ReadError();
            return true;
        }

    }
    public static void ClearChat(string message, object oPlayer, bool all)
    {

        try
        {
            SvPlayer player = (SvPlayer) oPlayer;
            if (!all)
            {
                player.SendToSelf(Channel.Unsequenced, (byte) 10, "Clearing the chat for yourself...");
                Thread.Sleep(500);
                for (int i = 0; i < 6; i++)
                {
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, " ");
                }
            }
            else if (all)
            {
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    player.SendToAll(Channel.Unsequenced, (byte) 10, "Clearing chat for everyone...");
                    Thread.Sleep(500);
                    for (int i = 0; i < 6; i++)
                    {
                        player.SendToAll(Channel.Unsequenced, (byte) 10, " ");
                    }
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, msgNoPerm);
                }
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte) 10, @"'" + arg1ClearChat + @"'" + " Is not a valid argument.");
            }
        }
        catch (Exception ex)
        {
            ErrorLogging(ex);
            ReadError();
        }

    }

    public static void CheckFiles(string FileName)
    {
        if (FileName == "all")
        {
            if (!Directory.Exists(FileDirectory))
            {
                Directory.CreateDirectory(FileDirectory);
                Debug.Log(FileDirectory + " Does not exist! Creating one.");
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
            if (!File.Exists(IPListFile))
            {
                File.Create(IPListFile).Close();
                Debug.Log(IPListFile + " Does not exist! Creating one.");
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
            if (!File.Exists(RulesFile))
            {
                File.Create(RulesFile).Close();
                CreateFile(RulesFile);
                Debug.Log(RulesFile + " Does not exist! Creating one.");
            }
            if (!File.Exists(SettingsFile))
            {
                File.Create(SettingsFile).Close();
                CreateFile(SettingsFile);
                Debug.Log(RulesFile + " Does not exist! Creating one.");
            }
            if (!File.Exists(AnnouncementsFile))
            {
                File.Create(AnnouncementsFile).Close();
                CreateFile(AnnouncementsFile);
                Debug.Log(AnnouncementsFile + " Does not exist! Creating one.");
            }
        }
        else
        {
            if (!File.Exists(FileName))
            {

                if (FileName == SettingsFile)
                {
                    File.Create(FileName).Close();
                    CreateFile(FileName);
                    Debug.Log(FileName + " Does not exist! Creating one.");
                }
                if (FileName == ChatBlockFile)
                {
                    File.Create(FileName).Close();
                    CreateFile(FileName);
                    Debug.Log(FileName + " Does not exist! Creating one.");
                }
                if (FileName == LanguageBlockFile)
                {
                    File.Create(FileName).Close();
                    CreateFile(FileName);
                    Debug.Log(FileName + " Does not exist! Creating one.");
                }
                if (FileName == RulesFile)
                {
                    File.Create(FileName).Close();
                    CreateFile(FileName);
                    Debug.Log(FileName + " Does not exist! Creating one.");
                }
                else
                {
                    File.Create(FileName).Close();
                    Debug.Log(FileName + " Does not exist! Creating one.");
                }
            }
        }
    }
    public static void CreateFile(string FileName)
    {
        if (FileName == SettingsFile)
        {
            string[] content = { "# ---------------------------------------------------------------------------------------- #", "#                             Broke Protocol: Essentials                                   #", "#                        Created by UserR00T and DeathByKorea                              #", "# ---------------------------------------------------------------------------------------- #", "#                                                                                          #", "#                                                                                          #", "#                                                                                          #", "# NOTE:                                                                                    #", "# CommandCharacter will be automatically added to the commands! No need to do that!        #", "# Example:                                                                                 #", "# INCORRECT: ClearChatCommand: /clearchat                                                  #", "# CORRECT: ClearChatCommand: clearchat                                                     #", "# if CommandCharacter is / it will automatically add a /                                   #", "#                                                                                          #", "# ---------------------------------------------------------------------------------------- #", "", "", "", "", "", "#----------------------------------------------------------#", "#                           General                        #", "#----------------------------------------------------------#", "version: 1.0.1", "", "# Character used for commands", "#----------------------------------", "CommandCharacter: /", "", @"# Will display ""unknown command"" if the command is not found in this plugin", "#----------------------------------", "UnknownCommand: true", "", "noperm: No permission.", "", "", "", "", "", "#----------------------------------------------------------#", "#                          Commands                        #", "#----------------------------------------------------------#", "", "# Reload command", "#----------------------------------", "ReloadCommand: reload", "ReloadCommand2: rl", "", "# Clearchat command", "#----------------------------------", "ClearChatCommand: clearchat", "ClearChatCommand2: cc", "", "# Say command", "#----------------------------------", "SayCommand: say", "SayCommand2: broadcast", "", "# CheckIP/Player command", "#----------------------------------", "CheckIPCommand: checkip", "CheckPlayerCommand: checkplayer", "", "# Fake Join/Leave command", "#----------------------------------", "FakeLeaveCommand: fakeleave", "FakeJoinCommand: fakejoin", "", "# Rules command -- Rules default location - 'rules.txt'", "#----------------------------------", "RulesCommand: rules", "", "# Discord command", "#----------------------------------", "DiscordCommand: discord", "", "# Godmode command --- Godmode default saving location - 'godmode.txt'", "#----------------------------------", "GodmodeCommand: god", "GodmodeCommand2: godmode", "", "# AFK command --- AFK default saving location - 'afkplayers.txt'", "#----------------------------------", "AFKCommand: afk", "AFKCommand2: brb", "", "# Mute command --- Mute default saving location - 'muteplayers.txt'", "#----------------------------------", "MuteCommand: mute", "# UnMute Command", "UnMuteCommand: unmute", "", "", "", "", "", "#----------------------------------------------------------#", "#                          Messages                        #", "#----------------------------------------------------------#", "# Say prefix: Will show the string infront of the message", "# Example: [!!!] UserR00T: This is a message!", "# A space is not required at the end.", "#----------------------------------", "msgSayPrefix: [!!!]", "", "# Discord invite link", "#----------------------------------", "DiscordLink: https://discord.gg/Test", "", "", "", "#----------------------------------------------------------#", "#                    Additional Settings                   #", "#----------------------------------------------------------#", "", "", "# ChatBlock --- Words default location - 'chatblock.txt'", "# Enable chatblock", "#----------------------------------", "enableChatBlock: true", "", "", "# LanguageBlock --- Words default location - 'languageblock.txt'", "# Enable LanguageBlock", "#----------------------------------", "enableLanguageBlock: true", "", "", "# This will check for alt accounts with the same IP.", "# NOTE: If someone in the same home connects with the same IP it will be detected as alt.", "# Enable CheckForAlts", "#----------------------------------", "CheckForAlts: true", "", "# This will lower any command ", "# so /DiScOrD will still be registed as /discord", "# This does not work on arguments.", "# Enable ToLowerCommands", "#----------------------------------", "ToLowerCommands: true", "", "# Seconds between annoucements in seconds -- Announcements in 'Annoucements.txt'", "#----------------------------------", "TimeBetweenAnnounce: 360", "", "", "# TimestampFormat; This means what you will see infront of a message: ", "# E.g: [{H}:{M}:{S}] and your time is 12:5:59 it will show [12:05:59]", "# Placeholders:", "# {H} Hours (24 hour clock)", "# {h} Hours (12 hour clock)", "# {M} Minutes", "# {S} Seconds", "# {T} AM/PM (used in {h} 12 hour clock)", "#----------------------------------", "TimestapFormat: [{H}:{M}:{S}]" };
            File.WriteAllLines(SettingsFile, content);
        }
        if (FileName == ChatBlockFile)
        {
            string[] content = { "nigger", "nigga", "nigg3r", "NIGGER", "NI99ER", "ni99er", "nigger.", "nigga.", "nigg3r.", "N199ER", "n1gger", "N1GGER", "NIGGA", "NIGGA." };
            File.WriteAllLines(ChatBlockFile, content);
        }
        if (FileName == LanguageBlockFile)
        {
            string[] content = { "bombas","hola","alguien","habla","espanol","español","estoy","banco","voy","consegi","donde","quedamos","banko","afuera","estas","alguem","donde","nos","vemos","soy ","vueno","como","carro","cabros","miren","hacha","laar","corri","sacame","aqui","policia","trajo","encerro","bomba","beuno","pantalones","dinero","porque","tengo","escopetaa","escopeta" };
            File.WriteAllLines(LanguageBlockFile, content);
        }
        if (FileName == RulesFile)
        {
            string[] content = {"Please tell the owner to edit the rules.txt file", "in the essentials folder!"};
            File.WriteAllLines(RulesFile, content);
        }

    }
    public static void Reload(bool silentExecution, string message = null, object oPlayer = null)
    {
        if (!silentExecution)
        {
            SvPlayer player = (SvPlayer) oPlayer;
            if (AdminsListPlayers.Contains(player.playerData.username))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Checking if file's exist...");
                CheckFiles("all");
                player.SendToSelf(Channel.Unsequenced, (byte) 10, "Reloading config files...");
                ReadFile(SettingsFile);
                player.SendToSelf(Channel.Unsequenced, (byte) 10, "[OK] Config file reloaded");
                player.SendToSelf(Channel.Unsequenced, (byte) 10, "Reloading critical .txt files...");
                ReadFileStream(LanguageBlockFile, LanguageBlockWords);
                ReadFileStream(ChatBlockFile, ChatBlockWords);
                ReadFileStream(AdminListFile, AdminsListPlayers);
                LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                ReadFile(AnnouncementsFile);
                ReadFile(GodListFile);
                ReadFile(MuteListFile);
                ReadFile(AfkListFile);
                ReadFile(RulesFile);
                player.SendToSelf(Channel.Unsequenced, (byte) 10, "[OK] Critical .txt files reloaded");
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte) 10, msgNoPerm);
            }
        }
        else if (silentExecution)
        {
            CheckFiles("all");
            ReadFile(SettingsFile);
            ReadFileStream(LanguageBlockFile, LanguageBlockWords);
            ReadFileStream(ChatBlockFile, ChatBlockWords);
            ReadFileStream(AdminListFile, AdminsListPlayers);
            LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
            ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
            ReadFile(AnnouncementsFile);
            ReadFile(GodListFile);
            ReadFile(MuteListFile);
            ReadFile(AfkListFile);
            ReadFile(RulesFile);
        }
    }

    public static void essentials(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        player.SendToSelf(Channel.Unsequenced, (byte) 10, "Essentials Created by UserR00T & DeathByKorea");
        player.SendToSelf(Channel.Unsequenced, (byte) 10, "Version " + version);
    }

    [Hook("SvPlayer.Damage")]
    public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider)
    {
        if (CheckGodmode(player, amount) == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private static bool CheckGodmode(object oPlayer, float amount)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        if (GodListPlayers.Contains(player.playerData.username))
        {
            player.SendToSelf(Channel.Unsequenced, (byte) 10, amount + " DMG Blocked!");
            return true;
        }
        return false;
    }
    public static bool say(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer) oPlayer;
        try
        {

            if (AdminsListPlayers.Contains(player.playerData.username))
            {
                if ((message.Length == cmdSay.Length) || (message.Length == cmdSay2.Length))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "An argument is required for this command.");
                    return true;
                }
                else
                {
                    string arg1 = "blank";
                    if (message.StartsWith(cmdSay))
                    {
                        arg1 = message.Substring(cmdSay.Length);
                    }
                    else if (message.StartsWith(cmdSay2))
                    {
                        arg1 = message.Substring(cmdSay2.Length);
                    }
                    player.SendToAll(Channel.Unsequenced, (byte) 10, msgSayPrefix + " " + player.playerData.username + ": " + arg1);
                    return true;
                }
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte) 10, msgNoPerm);
                return false;
            }

        }
        catch (Exception ex)
        {
            ErrorLogging(ex);
            ReadError();
            return true;
        }
    }

    [Hook("SvPlayer.Initialize")]
    public static void Initialize(SvPlayer player)
    {

        if (player.playerData.username != null)
        {
            Thread thread1 = new Thread(new ParameterizedThreadStart(CheckBanned));
            thread1.Start(player);
            Thread thread2 = new Thread(new ParameterizedThreadStart(WriteIPToFile));
            thread2.Start(player);
            Thread thread3 = new Thread(new ParameterizedThreadStart(CheckAltAcc));
            thread3.Start(player);
        }
    }
    private static void CheckBanned(object oPlayer)
    {
        Thread.Sleep(3000);
        try
        {
            SvPlayer player = (SvPlayer) oPlayer;
            if (File.ReadAllText("ban_list.txt").Contains(player.playerData.username))
            {
                Debug.Log("[WARNING] " + player.playerData.username + " Joined while banned! IP: " + player.netMan.GetAddress(player.connection));
                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                {
                    if (shPlayer.svPlayer == player)
                    {
                        if (shPlayer.IsRealPlayer())
                        {
                            player.netMan.AddBanned(shPlayer);
                            player.netMan.Disconnect(player.connection);
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ErrorLogging(ex);
            ReadError();
                    }
    }
    private static void CheckAltAcc(object oPlayer)
    {
        if (CheckAlt)
        {
            Thread.Sleep(3000);
            try
            {
                SvPlayer player = (SvPlayer)oPlayer;
                if (File.ReadAllText("ban_list.txt").Contains(player.netMan.GetAddress(player.connection)))
                {
                    Debug.Log("[WARNING] " + player.playerData.username + " Joined with a possible alt! IP: " + player.netMan.GetAddress(player.connection));
                    foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                    {
                        if (shPlayer.svPlayer == player)
                        {
                            if (shPlayer.IsRealPlayer())
                            {
                                player.netMan.AddBanned(shPlayer);
                                player.netMan.Disconnect(player.connection);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            ErrorLogging(ex);
            ReadError();
                        }
        }
    }
    private static void WriteIPToFile(object oPlayer)
    {
        Thread.Sleep(500);
        SvPlayer player = (SvPlayer) oPlayer;
        Debug.Log(SetTimeStamp() + "[INFO] " + "[JOIN] " + player.playerData.username + " IP is: " + player.netMan.GetAddress(player.connection));
        try
        {
            if (!File.ReadAllText(IPListFile).Contains(player.playerData.username + ": " + player.netMan.GetAddress(player.connection)))
            {
                File.AppendAllText(IPListFile, player.playerData.username + ": " + player.netMan.GetAddress(player.connection) + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            ErrorLogging(ex);
            ReadError();
                    }

    }
    private static void RemoveStringFromFile(string FileName, string RemoveString)
    {
        string content = null;
        foreach (var line in File.ReadAllLines(FileName))
        {
            if (!line.Contains(RemoveString))
            {
                content = content + line + Environment.NewLine;
            }
        }
        File.WriteAllText(FileName, content);
    }
    public static string SetTimeStamp()
    {
        try
        {
            var src = DateTime.Now;
            var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
            string PlaceHolderText = TimestampFormat;
            string Hours = hm.ToString("HH");
            string Minutes = hm.ToString("mm");
            string Seconds = hm.ToString("ss");

            if (TimestampFormat.Contains("{H}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{H}", hm.ToString("HH"));
            }
            if (TimestampFormat.Contains("{h}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{h}", hm.ToString("hh"));
            }
            if (TimestampFormat.Contains("{M}") || TimestampFormat.Contains("{m}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{M}", Minutes);
            }
            if (TimestampFormat.Contains("{S}") || TimestampFormat.Contains("{s}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{S}", Seconds.ToString());
            }
            if (TimestampFormat.Contains("{T}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{T}", hm.ToString("tt"));
            }
            PlaceHolderText = PlaceHolderText + " ";
            return PlaceHolderText;
        }
        catch (Exception ex)
        {
            ErrorLogging(ex);
            ReadError();
            return "[Failed] ";
        }
    }
    public static void ReadFileStream(string FileName, List<string> output)
    {
        foreach (var line in File.ReadAllLines(FileName))
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
    static void ReadFile(string FileName)
    {
        #region SettingsFile
        if (FileName == SettingsFile)
        {
            foreach (var line in File.ReadAllLines(SettingsFile)) //TODO: THIS METHOD IS A MESS, THERE IS PROBABLY A BETTER WAY.
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                else
                {
                    if (line.Contains("version: "))
                    {
                        version = line.Substring(9);
                    }
                    else if (line.Contains("CommandCharacter: "))
                    {
                        cmdCommandCharacter = line.Substring(18);
                    }
                    else if (line.Contains("noperm: "))
                    {
                        msgNoPerm = line.Substring(8);
                    }
                    else if (line.Contains("ClearChatCommand: "))
                    {
                        cmdClearChat = cmdCommandCharacter + line.Substring(18);
                    }
                    else if (line.Contains("ClearChatCommand2: "))
                    {
                        cmdClearChat2 = cmdCommandCharacter + line.Substring(19);
                    }
                    else if (line.Contains("SayCommand: "))
                    {
                        cmdSay = cmdCommandCharacter + line.Substring(12);
                    }
                    else if (line.Contains("SayCommand2:"))
                    {
                        cmdSay2 = cmdCommandCharacter + line.Substring(13);
                    }
                    else if (line.Contains("msgSayPrefix: "))
                    {
                        msgSayPrefix = line.Substring(14);
                    }
                    else if (line.StartsWith("GodmodeCommand: "))
                    {
                        cmdGodmode = cmdCommandCharacter + line.Substring(16);
                    }
                    else if (line.StartsWith("GodmodeCommand2: "))
                    {
                        cmdGodmode2 = cmdCommandCharacter + line.Substring(17);
                    }
                    else if (line.StartsWith("MuteCommand: "))
                    {
                        cmdMute = cmdCommandCharacter + line.Substring(13);
                    }
                    else if (line.StartsWith("UnMuteCommand: "))
                    {
                        cmdUnMute = cmdCommandCharacter + line.Substring(15);
                    }
                    else if (line.Contains("UnknownCommand: "))
                    {
                        msgUnknownCommand = Convert.ToBoolean(line.Substring(16));
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
                        cmdRules = cmdCommandCharacter + line.Substring(14);
                    }
                    else if (line.Contains("ReloadCommand: "))
                    {
                        cmdReload = cmdCommandCharacter + line.Substring(15);
                    }
                    else if (line.Contains("ReloadCommand2: "))
                    {
                        cmdReload2 = cmdCommandCharacter + line.Substring(16);
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
                        cmdAfk = cmdCommandCharacter + line.Substring(12);
                    }
                    else if (line.Contains("AFKCommand2: "))
                    {
                        cmdAfk2 = cmdCommandCharacter + line.Substring(13);
                    }
                    else if (line.Contains("CheckIPCommand: "))
                    {
                        cmdCheckIP = cmdCommandCharacter + line.Substring(16);
                    }
                    else if (line.Contains("CheckPlayerCommand: "))
                    {
                        cmdCheckPlayer = cmdCommandCharacter + line.Substring(20);
                    }
                    else if (line.Contains("FakeJoinCommand: "))
                    {
                        cmdFakeJoin = cmdCommandCharacter + line.Substring(17);
                    }
                    else if (line.Contains("FakeLeaveCommand: "))
                    {
                        cmdFakeLeave = cmdCommandCharacter + line.Substring(18);
                    }
                    else if (line.Contains("CheckForAlts: "))
                    {
                        CheckAlt = Convert.ToBoolean(line.Substring(14));
                    }
                    else if (line.Contains("DiscordCommand: "))
                    {
                        cmdDiscord = cmdCommandCharacter + line.Substring(16);
                    }
                    else if (line.Contains("DiscordLink: "))
                    {
                        msgDiscord =  line.Substring(13);
                    }
                    else if (line.Contains("ToLowerCommands: "))
                    {
                        MessageToLower = Convert.ToBoolean(line.Substring(17));
                    }
                }
            }

        }
        #endregion
        else if (FileName == AnnouncementsFile)
        {
            announcements = File.ReadAllLines(FileName);
        }
        else if (FileName == RulesFile)
        {
            rules = File.ReadAllLines(FileName);
        }
        else if (FileName == GodListFile)
        {
            GodListPlayers = File.ReadAllLines(FileName).ToList();
        }
        else if (FileName == AfkListFile)
        {
            AfkPlayers = File.ReadAllLines(FileName).ToList();
        }
        else if (FileName == MuteListFile)
        {
            MutePlayers = File.ReadAllLines(FileName).ToList();
        }

    }
     public static void ErrorLogging(Exception ex)
    {
        string strPath = ExeptionFile;
        if (!File.Exists(strPath))
        {
            File.Create(strPath).Dispose();
        }
        using (StreamWriter sw = File.AppendText(strPath))
        {
            sw.WriteLine("=============Error Logging ===========");
            sw.WriteLine("===========Start============= " +       DateTime.Now);
            sw.WriteLine("Error Message: " + ex.Message);
            sw.WriteLine("Stack Trace: " + ex.StackTrace);
            sw.WriteLine("===========End============= " + DateTime.Now);

        }
    }

    public static void ReadError()
    {
        string strPath = ExeptionFile;
        using (StreamReader sr = new StreamReader(strPath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
      }
   }

