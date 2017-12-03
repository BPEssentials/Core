// Essentials created by UserR00T & DeathByKorea & BP

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using System.Text.RegularExpressions;

namespace BP_Essentials {
    public class EssentialsPlugin
    {
        private const string FileDirectory = "Essentials/";
        private const string LogDirectory = FileDirectory + "logs/";

        private const string SettingsFile = FileDirectory + "essentials_settings.txt";
        private const string LanguageBlockFile = FileDirectory + "languageblock.txt";
        private const string ChatBlockFile = FileDirectory + "chatblock.txt";
        private const string AnnouncementsFile = FileDirectory + "announcements.txt";
        private const string IpListFile = FileDirectory + "ip_list.txt";
        private const string GodListFile = FileDirectory + "godlist.txt";
        private const string AfkListFile = FileDirectory + "afklist.txt";
        private const string MuteListFile = FileDirectory + "mutelist.txt";
        private const string ExeptionFile = FileDirectory + "exceptions.txt";
        private const string CustomCommandsFile = FileDirectory + "CustomCommands.txt";

        private const string AdminListFile = "admin_list.txt";
        private const string RulesFile = "server_info.txt";

        private const string LogFile = LogDirectory + "all.txt";
        private const string ChatLogFile = LogDirectory + "chat.txt";
        private const string CommandLogFile = LogDirectory + "commands.txt";

        #region predefining variables

        // General
        private const string Ver = "PRE-2.0.0";

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
        private static bool enableATMCommand;
        private static bool Confirmed = false;
        private static bool? EnableBlockSpawnBot = null;
        // Lists
        private static readonly List<string> Commands = new List<string>();
        private static readonly List<string> Responses = new List<string>();
        private static List<string> ChatBlockWords = new List<string>();
        private static List<string> LanguageBlockWords = new List<string>();
        private static readonly List<string> AdminsListPlayers = new List<string>();
        private static List<string> GodListPlayers = new List<string>();
        private static List<string> AfkPlayers = new List<string>();
        private static List<string> MutePlayers = new List<string>();

        // Arrays
        private static string[] announcements;

        private static readonly string[] Jobs = { "Citizen", "Criminal", "Prisoner", "Police", "Paramedic", "Firefighter", "Gangster: Red", "Gangster: Green", "Gangster: Blue" };

        // Messages
        private static string msgSayPrefix;
        private static string msgNoPerm;
        private static string msgDiscord;
        private const string DisabledCommand = "The server owner disabled this command.";

        // Strings
        private static string rules;
        private static string DisabledSpawnBots;

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
        private static string cmdPlayers;
        private static string cmdPlayers2;
        private static string cmdInfo;
        private static string cmdInfo2;
        private static string cmdMoney;
        private static string cmdMoney2;
        private static string cmdATM;

        private static string cmdSave;
        private static string cmdPay;
        private static string cmdPay2;
        private static string cmdHelp;
        private static string cmdConfirm;
        private static string cmdLogs;

        private static string cmdTp;
        private static string cmdTpHere;
        private static string cmdTpHere2;
        private static string cmdKill;
        private static string cmdArrest;
        private static string cmdRestrain;
        private static string cmdFree;
        private static string cmdDbug;
        // Ints
        private const int SaveTime = 60 * 5;

        private static int announceIndex = 0;
        private static int TimeBetweenAnnounce;
        private static int[] BlockedSpawnIDS;
        #endregion


        //Code below here, Don't edit unless you know what you're doing.
        //Information about the api @ https://github.com/DeathByKorea/UniversalUnityhooks



        #region Event: StartServerNetwork
        [Hook("SvNetMan.StartServerNetwork")]
        public static void StartServerNetwork(SvNetMan netMan)
        {

            try
            {
                Reload(true);

                if (Ver != version)
                {
                    Debug.Log("[ERROR] Essentials - Versions do not match!");
                    Debug.Log("[ERROR] Essentials - Essentials version:" + Ver);
                    Debug.Log("[ERROR] Essentials - Settings file version" + version);
                    Debug.Log("");
                    Debug.Log("");
                    Debug.Log("[ERROR] Essentials - Recreating settings file!");
                    if (File.Exists(SettingsFile + ".OLD"))
                    {
                        File.Delete(SettingsFile + ".OLD");
                    }
                    File.Move(SettingsFile, SettingsFile + ".OLD");
                    Reload(true);
                }
                var thread = new Thread(SavePeriodically);
                thread.Start();
                Debug.Log("-------------------------------------------------------------------------------");
                Debug.Log("    ");
                Debug.Log("[INFO] Essentials - version: " + version + " Loaded in successfully!");
                Debug.Log("    ");
                Debug.Log("-------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Debug.Log("-------------------------------------------------------------------------------");
                Debug.Log("    ");
                Debug.Log("[ERROR]   Essentials - A file cannot be loaded in!");
                Debug.Log("[ERROR]   Essentials - Please check the error below for more info,");
                Debug.Log("[ERROR]   Essentials - And it would be highly if you would send the error to the developers of this plugin!");
                Debug.Log("    ");
                Debug.Log(ex);
                Debug.Log(ex.ToString());
                Debug.Log("-------------------------------------------------------------------------------");
            
            }

            if (announcements.Length != 0)
            {
                var thread = new Thread(new ParameterizedThreadStart(AnnounceThread));
                thread.Start(netMan);
                Debug.Log(SetTimeStamp() + "[INFO] Announcer started successfully!");
            }
            else
                Debug.Log(SetTimeStamp() + "[WARNING] No announcements found in the file!");

        }
        #endregion
        #region Event: ChatMessage
        //Chat Events
        [Hook("SvPlayer.SvGlobalChatMessage")]
        public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
        {
            // TODO: Improve ;)
            if (!(MutePlayers.Contains(player.playerData.username)))
            {
                MessageLog(message, player);
            }
            // If player is afk, unafk him
            if (AfkPlayers.Contains(player.playerData.username))
            {
                Afk(message, player);
            }
            if (message.Trim().StartsWith(cmdCommandCharacter))
            {
                if (message.StartsWith("/arg")) // TODO
                {
                    try {
                        GetArgument(1, message);
                       // GetArgument(Convert.ToByte(GetArgument(1, message)), message);
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException || ex is OverflowException)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "ERROR: " + ex.Message);
                        }
                    }
               
                    return true;
                }
                if (message.StartsWith(cmdPay) || (message.StartsWith(cmdPay2)))
                {
                    string arg1 = null;
                    string arg2 = null;
                    try
                    {
                        if (message.StartsWith(cmdPay))
                        {
                            arg1 = message.Substring(cmdPay.Length + 1).Trim();
                        }
                        else if (message.StartsWith(cmdPay2))
                        {
                            arg1 = message.Substring(cmdPay2.Length + 1).Trim();
                        }
                        arg2 = message.Split(' ').Last().Trim();
                        arg1 = arg1.Substring(0, arg1.Length - arg2.Length).Trim();
                        if (string.IsNullOrEmpty(arg1))
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, cmdPay + " / " + cmdPay2 + " [Player] [Amount]");
                            return true;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, cmdPay + " / " + cmdPay2 + " [Player] [Amount]");
                        return true;
                    }
                    if (!(String.IsNullOrEmpty(arg2)))
                    {
                        int arg2Int;
                        var isNumeric = int.TryParse(arg2, out arg2Int);

                        if (isNumeric)
                        {
                            var error = false;
                            var found = false;
                            if (arg2Int == 0)
                            {
                                error = true;
                            }
                            if (!(error))
                            {
                                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                                {
                                    if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                    {
                                        if (shPlayer.IsRealPlayer())
                                        {
                                            foreach (var _shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                                            {
                                                if (_shPlayer.svPlayer == player)
                                                {
                                                    if (_shPlayer.IsRealPlayer())
                                                    {
                                                        Debug.Log(_shPlayer.playerInventory.MyMoneyCount());
                                                        if (_shPlayer.playerInventory.MyMoneyCount() >= arg2Int)
                                                        {
                                                            _shPlayer.playerInventory.TransferMoney(2, arg2Int, true);
                                                        }
                                                        else
                                                        {
                                                            error = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!(error))
                                            {
                                                shPlayer.playerInventory.TransferMoney(1, arg2Int, true);
                                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Succesfully transfered " + arg2Int + "$ to " + shPlayer.svPlayer.playerData.username + "!");
                                                shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " gave you " + arg2Int + "$!");
                                                found = true;
                                            }
                                            else
                                            {
                                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer money, do you have " + arg2Int + "$ in your inventory?");
                                                found = true;
                                            }
                                        }
                                    }
                                }
                                if (!(found))
                                {
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + @" Not found/online.");
                                }
                            }
                            else
                            {
                                if (arg2Int == 0)
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer 0$.");
                                else
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer money, do you have " + arg2Int + "$ in your inventory?");

                            }
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, cmdPay + " / " + cmdPay2 + " [Player] [Amount] (incorrect argument!)");
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, cmdPay + " / " + cmdPay2 + " [Player] [Amount]");
                    }
                    return true;
                }
                // TODO ------------------------------------------------------------------------------------------
                if (message.StartsWith("/location") || (message.StartsWith("/loc")))
                {
                    if (message.StartsWith("/location") || (message.StartsWith("/loc")))
                    {
                        if (AdminsListPlayers.Contains(player.playerData.username))
                        {
                            player.Save();
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Your location: " + player.playerData.position);
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }
                if (message.StartsWith("/getplayerhash") || (message.StartsWith("/gethash")))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var TempMSG = message.Trim();
                        if (TempMSG != "/getplayerhash" || TempMSG != "/gethash")
                        {
                            var arg1 = player.playerData.username;
                            if (TempMSG.StartsWith("/gethash"))
                                arg1 = TempMSG.Substring(8 + 1);
                            if (TempMSG.StartsWith("/getplayerhash"))
                                arg1 = TempMSG.Substring(14 + 1);
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "StringToHash: " + Animator.StringToHash(arg1).ToString());
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                    return true;
                }
                // Command: SpaceIndex
                if (message.StartsWith("/spaceindex"))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.svPlayer == player)
                                if (shPlayer.IsRealPlayer())
                                {
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "SpaceIndex: " + shPlayer.GetSpaceIndex());
                                }
                        return true;

                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }
                // TODO ------------------------------------------------------------------------------------------
                // Command: Save
                if (message.StartsWith(cmdSave))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var thread = new Thread(SaveNow);
                        thread.Start();
                        return true;
                    }
                    player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                    return true;
                }
                // Tp
                if (message.StartsWith(cmdTpHere) || message.StartsWith(cmdTpHere2))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != cmdTpHere || tempMsg != cmdTpHere2)
                    {
                        var arg1 = String.Empty;
                        if (tempMsg.StartsWith(cmdTpHere + " "))
                        {
                            arg1 = tempMsg.Substring(cmdTpHere.Length + 1);
                        }
                        else if (tempMsg.StartsWith(cmdTpHere2 + " "))
                        {
                            arg1 = tempMsg.Substring(cmdTpHere2.Length + 1);
                        }
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }
                else if (message.StartsWith("/tp"))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != "/tp")
                    {
                        var arg1 = tempMsg.Substring(3 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }
                // Ban
                if (message.StartsWith("/ban"))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != "/ban")
                    {
                        var arg1 = tempMsg.Substring(4 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }

                // Kick
                if (message.StartsWith("/kick"))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != "/kick")
                    {
                        var arg1 = tempMsg.Substring(5 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                    return true;
                }
                // Arrest
                if (message.StartsWith("/arrest"))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != "/arrest")
                    {
                        var arg1 = tempMsg.Substring(7 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }
                // Restrain
                if (message.StartsWith("/restrain"))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != "/restrain")
                    {
                        var arg1 = tempMsg.Substring(9 + 1);
                        ExecuteOnPlayer(player, message, arg1);

                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }
                // Kill
                if (message.StartsWith("/kill"))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != "/kill")
                    {
                        var arg1 = tempMsg.Substring(5 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }
                // Free
                if (message.StartsWith("/free"))
                {
                    var tempMsg = message.Trim();
                    if (tempMsg != "/free")
                    {
                        var arg1 = tempMsg.Substring(5 + 1);
                        ExecuteOnPlayer(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                    return true;
                }

                // Command: Confirm
                if (message.ToLower().StartsWith("/confirm"))
                {
                    player.Save();
                    if (player.playerData.ownedApartment)
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Selling apartment...");
                        Confirmed = true;
                        player.SvSellApartment();
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "You don't have a apartment to sell!");
                    }
                    return true;
                }

                // Command: Logs
                if (message.StartsWith("/logs"))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        GetLogs(player, ChatLogFile);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }
                // Command: ATM
                if (message.ToLower().StartsWith(cmdATM))
                {
                    if (enableATMCommand)
                    {
                        player.Save();
                        foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                        {
                            if (shPlayer.svPlayer == player)
                            {
                                if (shPlayer.IsRealPlayer())
                                {
                                    if (shPlayer.wantedLevel == 0)
                                    {
                                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Opening ATM menu..");
                                        player.SendToSelf(Channel.Reliable, (byte)40, player.playerData.bankBalance);
                                        return true;
                                    }
                                    else if (shPlayer.wantedLevel != 0)
                                    {
                                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Criminal Activity: Account Locked");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, DisabledCommand);
                    }
                    return true;
                }
                // Command: players
                if (message.StartsWith(cmdPlayers) || message.StartsWith(cmdPlayers2))
                {
                    var realPlayers = GameObject.FindObjectsOfType<ShPlayer>().Count(shPlayer => shPlayer.IsRealPlayer());
                    switch (realPlayers) {
                        case 1:
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "There is " + realPlayers + " player online");
                            break;
                        default:
                            if (realPlayers < 1)
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + realPlayers + " play- wait, how is that possible");
                            else
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + realPlayers + " player(s) online");
                            break;
                    }
                    return true;
                }

                // Command: ClearChat
                if (message.StartsWith(cmdClearChat) || message.StartsWith(cmdClearChat2))
                {
                    if (message.Contains("all") || message.Contains("everyone"))
                    {
                        if (AdminsListPlayers.Contains(player.playerData.username))
                        {
                            all = true;
                            ClearChat(message, player, all);
                            return true;
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                            return true;
                        }

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
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        Reload(false, message, player);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }
                // Command: Mute
                if (message.StartsWith(cmdUnMute))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        unmute = true;
                        Mute(message, player, unmute);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }

                }
                else if (message.StartsWith(cmdMute))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        unmute = false;
                        Mute(message, player, unmute);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }

                }
                // Command: Say
                if (message.StartsWith(cmdSay) || (message.StartsWith(cmdSay2)))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        Say(message, player);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }

                }
                // Command: GodMode
                if (message.StartsWith(cmdGodmode) || message.StartsWith(cmdGodmode2))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        godMode(message, player);
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }
                // Command: AFK
                if (message.StartsWith(cmdAfk) || message.StartsWith(cmdAfk2))
                {
                    Afk(message, player);
                    return true;
                }
                // Command: Main/essentials
                if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
                {
                    Essentials(message, player);
                    return true;
                }
                // Command: Rules
                if (message.StartsWith(cmdRules))
                {
                    player.SendToSelf(Channel.Reliable, (byte)50, rules);
                    return true;
                }
                // Command: CheckIP
                if (message.StartsWith(cmdCheckIP))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var tempMsg = message.Trim();
                        if (tempMsg != cmdCheckIP)
                        {
                            var arg1 = tempMsg.Substring(cmdCheckIP.Count() + 1);
                            CheckIp(tempMsg, player, arg1);
                            return true;
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                            return true;
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }
                // Command: CheckPlayer
                if (message.StartsWith(cmdCheckPlayer))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var tempMsg = message.Trim();
                        if (tempMsg != cmdCheckPlayer)
                        {
                            var arg1 = tempMsg.Substring(cmdCheckPlayer.Count() + 1);
                            CheckPlayer(tempMsg, player, arg1);
                            return true;
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                            return true;
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }

                }
                // Command: Fake Join/Leave
                if (message.StartsWith(cmdFakeJoin) || (message.StartsWith(cmdFakeLeave)))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        var tempMsg = message.Trim();
                        if (!(tempMsg == cmdFakeJoin || tempMsg == cmdFakeLeave))
                        {
                            string arg1 = null;
                            if (tempMsg.StartsWith(cmdFakeJoin))
                            {
                                arg1 = tempMsg.Substring(cmdFakeJoin.Length + 1);
                                player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " connected");
                            }
                            else if (tempMsg.StartsWith(cmdFakeLeave))
                            {
                                arg1 = tempMsg.Substring(cmdFakeLeave.Length + 1);
                                player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " disconnected");
                            }
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                        }

                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }

                }
                // Command: Discord
                if (message.StartsWith(cmdDiscord))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Discord: " + msgDiscord);
                    return true;
                }

                // Command: Help
                if (message.StartsWith("/help"))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Up to date help can be found at http://bit.do/BPEssentials");
                    return true;
                }

                // CustomCommands
                if (Commands.Any(message.Contains))
                {
                    var i = 0;
                    foreach (var command in Commands)
                    {
                        if (message.StartsWith(command))
                        {
                            i = Commands.IndexOf(command);
                        }
                    }
                    player.SendToSelf(Channel.Unsequenced, (byte)10, GetPlaceHolders(i, player));
                    return true;
                }
                if (message.StartsWith(cmdInfo) || message.StartsWith(cmdInfo2))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        try
                        {
                            var arg1 = "null";
                            if (message.StartsWith(cmdInfo))
                            {
                                arg1 = message.Substring(cmdInfo.Length + 1).Trim();
                            }
                            else if (message.StartsWith(cmdInfo2))
                            {
                                arg1 = message.Substring(cmdInfo2.Length + 1).Trim();
                            }

                            if (!(String.IsNullOrEmpty(arg1)))
                            {
                                new Thread(delegate () { GetPlayerInfo(player, arg1); }).Start();
                            }
                            else
                            {
                                player.SendToSelf(Channel.Unsequenced, (byte)10, cmdInfo + " / " + cmdInfo2 +" [Username]");
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, cmdInfo + " / " + cmdInfo2 + " [Username]");
                        }
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }
                if (message.StartsWith(cmdMoney) || message.StartsWith(cmdMoney2))
                {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        string arg1 = null;
                        string arg2 = null;
                        try
                        {
                            if (message.StartsWith(cmdMoney))
                            {
                                arg1 = message.Substring(cmdMoney.Length + 1).Trim();
                            }
                            else if (message.StartsWith(cmdMoney2))
                            {
                                arg1 = message.Substring(cmdMoney2.Length + 1).Trim();
                            }
                            arg2 = message.Split(' ').Last().Trim();
                            arg1 = arg1.Substring(0, arg1.Length - arg2.Length).Trim();
                            if (string.IsNullOrEmpty(arg1))
                            {
                                player.SendToSelf(Channel.Unsequenced, (byte)10, cmdMoney + "/ " + cmdMoney2 + " [Player] [Amount]");
                                return true;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, cmdMoney + "/ " + cmdMoney2 + " [Player] [Amount]");
                            return true;
                        }
                        if (!(string.IsNullOrEmpty(arg2)))
                        {
                            int arg2Int;
                            var isNumeric = int.TryParse(arg2, out arg2Int);

                            if (isNumeric)
                            {
                                var found = false;
                                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                                {
                                    if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                    {
                                        if (shPlayer.IsRealPlayer())
                                        {
                                            shPlayer.playerInventory.TransferMoney(1, arg2Int, true);
                                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Succesfully gave " + shPlayer.svPlayer.playerData.username + " " + arg2Int + "$");
                                            shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " gave you " + arg2Int + "$!");
                                            found = true;
                                        }
                                    }
                                }
                                if (!(found))
                                {
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + @" Not found/online.");
                                }

                            }
                            else
                            {
                                player.SendToSelf(Channel.Unsequenced, (byte)10, cmdMoney + "/ " + cmdMoney2 + " [Player] [Amount] (incorrect argument!)");
                            }
                        }
                        else
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, cmdMoney + "/ " + cmdMoney2 + " [Player] [Amount]");
                        }
                        return true;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return true;
                    }
                }

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
            else
            {
                if (MutePlayers.Contains(player.playerData.username))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "You are muted.");
                    return true;
                }
                // Check if message contains a player that is AFK
                if (AfkPlayers.Any(message.Contains))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "That player is AFK.");
                    return true;
                }
                //Checks if the message is a blocked one, if it is, block it.
                if (!ChatBlock && !LanguageBlock) return false;
                return BlockMessage(message, player);
            }
            return false;

        }
        #endregion
        #region Event: SellApartment
        [Hook("SvPlayer.SvSellApartment")]
        public static bool SvSellApartment(SvPlayer player)
        {
            if (Confirmed)
            {
                Confirmed = false;
                return false;
            }
            else if (!(Confirmed))
            {
                Confirmed = false;
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Are you sure you want to sell your apartment? Type '/confirm' to confirm.");
                return true;
            }
            return false;
        }
        #endregion
        #region Event: Initialize
        [Hook("SvPlayer.Initialize")]
        public static void Initialize(SvPlayer player)
        {
            if (player.playerData.username == null) return;
            var thread1 = new Thread(new ParameterizedThreadStart(CheckBanned));
            thread1.Start(player);
            var thread2 = new Thread(new ParameterizedThreadStart(WriteIPToFile));
            thread2.Start(player);
            var thread3 = new Thread(new ParameterizedThreadStart(CheckAltAcc));
            thread3.Start(player);
        }
        #endregion
        #region Event: Damage
        [Hook("SvPlayer.Damage")]
        public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider) {
            return CheckGodmode(player, amount);
        }
        #endregion
        #region Event: SpawnBot
        [Hook("SvPlayer.SpawnBot")]
        public static bool SpawnBot(SvPlayer player, ref Vector3 position, ref Quaternion rotation, ref WaypointNode node, ref ShTransport vehicle, ref byte spawnJobIndex)
        {
            if (EnableBlockSpawnBot != true) return false;
            return BlockedSpawnIDS.Contains(spawnJobIndex);
        }
        #endregion
        #region Event: HitEffect
        [Hook("ShRetainer.HitEffect")] // Blocks handcuff
        public static bool HitEffect(ShRetainer player, ref ShEntity hitTarget, ref ShPlayer source, ref Collider collider)
        {
            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                if (shPlayer.IsRealPlayer()) {
                    if (shPlayer != hitTarget) continue;
                    if (!GodListPlayers.Contains(shPlayer.svPlayer.playerData.username)) continue;
                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Being handcuffed Blocked!");
                    return true;
                }
            return false;
        }
        #endregion
        #region Event: SvBan
        [Hook("SvPlayer.SvBan")]
        public static void SvBan(SvPlayer player, ref int otherID)
        {
            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                if (shPlayer.ID == otherID)
                    if (shPlayer.IsRealPlayer())
                    {
                        if (!shPlayer.svPlayer.IsServerside())
                        {
                            player.SendToAll(Channel.Unsequenced, (byte)10, shPlayer.svPlayer.playerData.username + " Just got banned by " + player.playerData.username);
                        }
                    }
        }
        #endregion
        #region Methods
        private static void SavePeriodically()
        {
            while (true)
            {
                Debug.Log("[INFO] Saving game..");
                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.IsRealPlayer())
                    {
                        if (shPlayer.GetSpaceIndex() >= 13) continue;
                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Saving game.. This can take up to 5 seconds.");
                        shPlayer.svPlayer.Save();
                    }
                Thread.Sleep(SaveTime * 1000);
            }
        }
        private static void SaveNow()
        {
            Debug.Log("[INFO] Saving game..");
            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                if (shPlayer.IsRealPlayer())
                {
                    if (shPlayer.GetSpaceIndex() >= 13) continue;
                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Saving game.. This can take up to 5 seconds.");
                    shPlayer.svPlayer.Save();
                }
        }
        private static void ExecuteOnPlayer(object oPlayer, string message, string arg1)
        {
            var player = (SvPlayer)oPlayer;
            if (AdminsListPlayers.Contains(player.playerData.username))
            {
                var found = false;
                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                        if (shPlayer.IsRealPlayer())
                        {
                            shPlayer.svPlayer.Save();
                            if (message.StartsWith("/tp")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                // TODO: 
                                // - Rotation doesn't work all the time
                                // - Doesn't always TP
                                if (message.StartsWith("/tph") || message.StartsWith("/tphere")) // CHANGE THIS TO SOFTCODED ONE
                                {
                                    player.Save();
                                    Debug.Log("1 " +shPlayer.svPlayer.playerData.rotation);
                                    Debug.Log("2 " + player.playerData.rotation);
                                    shPlayer.SetPosition(player.playerData.position);
                                    shPlayer.SetRotation(player.playerData.rotation);
                                    Debug.Log("3 " + shPlayer.svPlayer.playerData.rotation);
                                    Debug.Log("4 " + player.playerData.rotation);
                                    //  shPlayer.svPlayer.playerData.position = player.playerData.position;
                                    // shPlayer.svPlayer.playerData.rotation = player.playerData.rotation;
                                    // Debug.Log("5 " + shPlayer.svPlayer.playerData.rotation);
                                    // Debug.Log("6 " + player.playerData.rotation);
                                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " Teleported you to him.");
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Teleported " + shPlayer.svPlayer.playerData.username + " to you.");
                                }
                                else if (message.StartsWith("/tp")) // CHANGE THIS TO SOFTCODED ONE
                                {
                                    player.SvTeleport(shPlayer.ID);
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Teleported to " + shPlayer.svPlayer.playerData.username + ".");
                                }
                            }
                            else if (message.Contains("/ban")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvBan(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Banned " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/kick")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvKick(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Kicked " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/arrest")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvArrest(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Arrested " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/restrain")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                player.SvArrest(shPlayer.ID);
                                player.SvRestrain(shPlayer.ID);
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Restrained " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/kill")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                shPlayer.svPlayer.SvSuicide();
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Killed " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            else if (message.Contains("/free")) // CHANGE THIS TO SOFTCODED ONE
                            {
                                shPlayer.svPlayer.Unhandcuff();
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Freed " + shPlayer.svPlayer.playerData.username + ".");
                            }
                            found = true;
                        }
                        else
                            player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " Is not a real player.");
                if (!(found))
                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " Is not online.");
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
            }
        }

        private static void GetLogs(object oPlayer, string LogFile)
        {
            var player = (SvPlayer)oPlayer;
            if (LogFile != ChatLogFile) return;
            string content = null;
            using (var reader = new StreamReader(LogFile))
            {
                for (var i = 0; i < 31; i++) {
                    string line = null;
                    if ((line = reader.ReadLine()) != null)
                        content = content + "\r\n" + line;
                    else
                        break;
                }
            }
            player.SendToSelf(Channel.Reliable, (byte)50, content);
        }

        private static void GetPlayerInfo(object oPlayer, string arg1)
        {
            var found = false;
            var player = (SvPlayer)oPlayer;
            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>()) {
                if (shPlayer.svPlayer.playerData.username != arg1 &&
                    shPlayer.ID.ToString() != arg1.ToString()) continue;
                if (!shPlayer.IsRealPlayer()) continue;
                player.Save();
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Info about:                                        " + shPlayer.svPlayer.playerData.username);
                string[] contentarray = {
                    "Username:              " +  shPlayer.svPlayer.playerData.username,
                    "",
                    "",
                    "Job:                         " + Jobs[shPlayer.svPlayer.playerData.jobIndex],
                    "Health:                    " + shPlayer.svPlayer.playerData.health,
                    "OwnsApartment:   " + shPlayer.svPlayer.playerData.ownedApartment,
                    "Position:                 " + shPlayer.svPlayer.playerData.position,
                    "WantedLevel:         " + shPlayer.wantedLevel,
                    "IsAdmin:                 " + shPlayer.admin,
                    "IP:                            " + shPlayer.svPlayer.netMan.GetAddress(shPlayer.svPlayer.connection)
                };


                var content = string.Join("\r\n", contentarray);

                player.SendToSelf(Channel.Reliable, (byte)50, content);

                found = true;
            }
            if (!(found))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " Not found/online.");
            }
        }

        private static string GetPlaceHolders(int i, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            var src = DateTime.Now;
            var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
            var placeHolderText = Responses[i];
            var minutes = hm.ToString("mm");
            var seconds = hm.ToString("ss");

            if (Responses[i].Contains("{YYYY}"))
            {
                placeHolderText = placeHolderText.Replace("{YYYY}", hm.ToString("yyyy"));
            }
            if (Responses[i].Contains("{DD}"))
            {
                placeHolderText = placeHolderText.Replace("{DD}", hm.ToString("dd"));
            }
            if (Responses[i].Contains("{DDDD}"))
            {
                placeHolderText = placeHolderText.Replace("{DDDD}", hm.ToString("dddd"));
            }
            if (Responses[i].Contains("{MMMM}"))
            {
                placeHolderText = placeHolderText.Replace("{MMMM}", hm.ToString("MMMM"));
            }
            if (Responses[i].Contains("{MM}"))
            {
                placeHolderText = placeHolderText.Replace("{MM}", hm.ToString("MM"));
            }

            if (Responses[i].Contains("{H}"))
            {
                placeHolderText = placeHolderText.Replace("{H}", hm.ToString("HH"));
            }
            if (Responses[i].Contains("{h}"))
            {
                placeHolderText = placeHolderText.Replace("{h}", hm.ToString("hh"));
            }
            if (Responses[i].Contains("{M}") || Responses[i].Contains("{m}"))
            {
                placeHolderText = placeHolderText.Replace("{M}", minutes);
            }
            if (Responses[i].Contains("{S}") || Responses[i].Contains("{s}"))
            {
                placeHolderText = placeHolderText.Replace("{S}", seconds);
            }
            if (Responses[i].Contains("{T}"))
            {
                placeHolderText = placeHolderText.Replace("{T}", hm.ToString("tt"));
            }
            if (Responses[i].ToLower().Contains("{username}"))
            {
                placeHolderText = placeHolderText.Replace("{username}", player.playerData.username);
            }
            return placeHolderText;
        }
        private static void AnnounceThread(object man)
        {
            var netMan = (SvNetMan)man;
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

        private static void CheckIp(string message, object oPlayer, string arg1)
        {
            var player = (SvPlayer)oPlayer;
            var found = 0;
            //  player.SendToSelf(Channel.Unsequenced, (byte)10, "IP for player: " + arg1);
            var content = String.Empty;
        
            foreach (var line in File.ReadAllLines(IpListFile))
            {
                if (line.Contains(arg1) || line.Contains(arg1 + ":"))
                {
                    ++found;
                    // player.SendToSelf(Channel.Unsequenced, (byte)10, line.Substring(arg1.Length + 1));
                    content = line.Substring(arg1.Length +1) + "\r\n" + content;
                }
            }

            content = content + "\r\n\r\n" + arg1 + " occurred " + found + " times in the iplog file." + "\r\n";
            player.SendToSelf(Channel.Reliable, (byte)50, content);
            //player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " occurred " + found + " times in the iplog file.");
        }

        private static void CheckPlayer(string message, object oPlayer, string arg1)
        {
            var player = (SvPlayer)oPlayer;
            var found = 0;
            string arg1temp = null;
            player.SendToSelf(Channel.Unsequenced, (byte)10, "Accounts for IP: " + arg1);
            foreach (var line in File.ReadAllLines(IpListFile))
            {
                if (line.Contains(arg1) || line.Contains(" " + arg1))
                {
                    ++found;
                    arg1temp = line.Replace(": " + arg1, " ");
                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1temp);
                }
            }
            player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " occurred " + found + " times in the iplog file.");
        }

        private static void MessageLog(string message, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            if (!message.StartsWith(cmdCommandCharacter))
            {
                var mssge = SetTimeStamp() + "[MESSAGE] " + player.playerData.username + ": " + message;
                Debug.Log(mssge);
                File.AppendAllText(ChatLogFile, mssge + Environment.NewLine);
                File.AppendAllText(LogFile, mssge + Environment.NewLine);
            }
            else if (message.StartsWith(cmdCommandCharacter))
            {
                var mssge = (SetTimeStamp() + "[COMMAND] " + player.playerData.username + ": " + message);
                Debug.Log(mssge);
                File.AppendAllText(CommandLogFile, mssge + Environment.NewLine);
                File.AppendAllText(LogFile, mssge + Environment.NewLine);
            }
        }

        private static bool Mute(string message, object oPlayer, bool unmute)
        {
            var player = (SvPlayer)oPlayer;
            string muteuser = null;
            var found = false;
            if (message.StartsWith(cmdMute))
                muteuser = message.Substring(cmdMute.Count() + 1);
            else if (message.StartsWith(cmdUnMute))
                muteuser = message.Substring(cmdUnMute.Count() + 1);

            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
            {
                if (shPlayer.svPlayer.playerData.username == muteuser.ToString() || shPlayer.ID.ToString() == muteuser.ToString())
                {
                    if (shPlayer.IsRealPlayer())
                    {
                        muteuser = shPlayer.svPlayer.playerData.username;
                        found = true;
                    }
                }
            }
            if (!found)
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, muteuser + " is not found, this means that if it is a -");
                player.SendToSelf(Channel.Unsequenced, (byte)10, "ID it can't convert it to a username.");
            }
            if (AdminsListPlayers.Contains(player.playerData.username))
            {

                ReadFile(MuteListFile);

                if (unmute)
                {
                    if (!MutePlayers.Contains(muteuser))
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, muteuser + " is not muted!");
                        return true;

                    }
                    if (MutePlayers.Contains(muteuser))
                    {
                        RemoveStringFromFile(MuteListFile, muteuser);
                        ReadFile(MuteListFile);
                        player.SendToSelf(Channel.Unsequenced, (byte)10, muteuser + " Unmuted");
                        return true;
                    }

                }
                else {
                    MutePlayers.Add(muteuser);
                    File.AppendAllText(MuteListFile, muteuser + Environment.NewLine);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, muteuser + " Muted");
                    return true;
                }
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                return false;
            }
            return false;
        }

        private static void Afk(string message, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            ReadFile(AfkListFile);
            if (AfkPlayers.Contains(player.playerData.username))
            {
                RemoveStringFromFile(AfkListFile, player.playerData.username);
                ReadFile(AfkListFile);
                player.SendToSelf(Channel.Unsequenced, (byte)10, "You are no longer AFK");
            }
            else
            {
                File.AppendAllText(AfkListFile, player.playerData.username + Environment.NewLine);
                AfkPlayers.Add(player.playerData.username);
                player.SendToSelf(Channel.Unsequenced, (byte)10, "You are now AFK");
            }
        }

        private static bool BlockMessage(string message, object oPlayer)
        {
            message = message.ToLower();
            var player = (SvPlayer)oPlayer;
            if (ChatBlock == true)
            {
                if (ChatBlockWords.Contains(message))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Please don't say a blacklisted word, the message has been blocked.");
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
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Because you are staff, your message has NOT been blocked.");
                        return false;
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "--------------------------------------------------------------------------------------------");
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "             ?olo ingl�s! Tu mensaje ha sido bloqueado.");
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "             Only English! Your message has been blocked.");
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "--------------------------------------------------------------------------------------------");
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool godMode(string message, object oPlayer)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                ReadFile(GodListFile);

                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    if (GodListPlayers.Contains(player.playerData.username))
                    {
                        RemoveStringFromFile(GodListFile, player.playerData.username);
                        ReadFile(GodListFile);
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Godmode disabled.");
                        return true;
                    }
                    else
                    {
                        File.AppendAllText(GodListFile, player.playerData.username + Environment.NewLine);
                        GodListPlayers.Add(player.playerData.username);
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Godmode enabled.");
                        return true;
                    }
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                    return false;
                }

            }
            catch (Exception ex)
            {

                ErrorLogging(ex);

                return true;
            }

        }

        private static void ClearChat(string message, object oPlayer, bool all)
        {

            try
            {
                var player = (SvPlayer)oPlayer;
                if (!all)
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Clearing the chat for yourself...");
                    Thread.Sleep(500);
                    for (var i = 0; i < 6; i++)
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, " ");
                    }
                }
                else {
                    if (AdminsListPlayers.Contains(player.playerData.username))
                    {
                        player.SendToAll(Channel.Unsequenced, (byte)10, "Clearing chat for everyone...");
                        Thread.Sleep(500);
                        for (var i = 0; i < 6; i++)
                        {
                            player.SendToAll(Channel.Unsequenced, (byte)10, " ");
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);

            }

        }

        private static void CheckFiles(string FileName)
        {
            if (FileName == "all")
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
                    if (FileName == CustomCommandsFile)
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

        private static void CreateFile(string FileName)
        {
            switch (FileName) {
                case SettingsFile: {
                    string[] content = { "# ---------------------------------------------------------------------------------------- #", "#                             Broke Protocol: Essentials                                   #", "#                        Created by UserR00T and DeathByKorea and BP                       #", "# ---------------------------------------------------------------------------------------- #", "#                                                                                          #", "#                                                                                          #", "#                                                                                          #", "# NOTE:                                                                                    #", "# CommandCharacter will be automatically added to the commands! No need to do that!        #", "# Example:                                                                                 #", "# INCORRECT: ClearChatCommand: /clearchat                                                  #", "# CORRECT: ClearChatCommand: clearchat                                                     #", "# if CommandCharacter is / it will automatically add a /                                   #", "#                                                                                          #", "# ---------------------------------------------------------------------------------------- #", "", "", "", "", "", "#----------------------------------------------------------#", "#                           General                        #", "#----------------------------------------------------------#", "version: " + Ver, "", "# Character used for commands", "#----------------------------------", "CommandCharacter: /", "", @"# Will display ""unknown command"" if the command is not found in this plugin", "#----------------------------------", "UnknownCommand: true", "", "noperm: No permission.", "", "", "", "", "", "#----------------------------------------------------------#", "#                          Commands                        #", "#----------------------------------------------------------#", "", "# Reload command", "#----------------------------------", "ReloadCommand: reload", "ReloadCommand2: rl", "", "# Clearchat command", "#----------------------------------", "ClearChatCommand: clearchat", "ClearChatCommand2: cc", "", "# Say command", "#----------------------------------", "SayCommand: say", "SayCommand2: broadcast", "", "# CheckIP/Player command", "#----------------------------------", "CheckIPCommand: checkip", "CheckPlayerCommand: checkplayer", "", "# Fake Join/Leave command", "#----------------------------------", "FakeLeaveCommand: fakeleave", "FakeJoinCommand: fakejoin", "", "# Player info command", "#----------------------------------", "InfoPlayerCommand: info", "InfoPlayerCommand2: stats", "", "# Money command", "#----------------------------------", "MoneyCommand: money", "MoneyCommand2: setbal", "", "# Rules command", "#----------------------------------", "RulesCommand: rules", "", "", "# Discord command", "#----------------------------------", "DiscordCommand: discord", "", "# Online Players command", "#----------------------------------", "PlayersOnlineCommand: players", "PlayersOnlineCommand2: online", "", "# Godmode command --- Godmode default saving location - 'godmode.txt'", "#----------------------------------", "GodmodeCommand: god", "GodmodeCommand2: godmode", "", "# AFK command --- AFK default saving location - 'afkplayers.txt'", "#----------------------------------", "AFKCommand: afk", "AFKCommand2: brb", "", "# Mute command --- Mute default saving location - 'muteplayers.txt'", "#----------------------------------", "MuteCommand: mute", "# Unmute Command", "UnMuteCommand: unmute", "", "# ATM command", "#----------------------------------", "ATMCommand: atm", "", "", "", "#----------------------------------------------------------#", "#                          Messages                        #", "#----------------------------------------------------------#", "# Say prefix: Will show the string infront of the message", "# Example: [!!!] UserR00T: This is a message!", "# A space is not required at the end.", "#----------------------------------", "msgSayPrefix: [!!!]", "", "# Discord invite link", "#----------------------------------", "DiscordLink: https://discord.gg/Test", "", "", "", "#----------------------------------------------------------#", "#                    Additional Settings                   #", "#----------------------------------------------------------#", "", "", "# ChatBlock --- Words default location - 'chatblock.txt'", "# Enable chatblock", "#----------------------------------", "enableChatBlock: true", "", "", "# LanguageBlock --- Words default location - 'languageblock.txt'", "# Enable LanguageBlock", "#----------------------------------", "enableLanguageBlock: true", "", "# Enable ATM command", "#----------------------------------", "EnableATMCommand: true", "", "# This will check for alt accounts with the same IP.", "# NOTE: If someone in the same home connects with the same IP it will be detected as alt.", "# Enable CheckForAlts", "#----------------------------------", "CheckForAlts: true", "", "", "# Seconds between annoucements in seconds -- Announcements in 'Annoucements.txt'", "#----------------------------------", "TimeBetweenAnnounce: 360", "", "", "# TimestampFormat; This means what you will see infront of a message: ", "# E.g: [{H}:{M}:{S}] and your time is 12:05:59 it will show [12:05:59]", "# E.g Time/Date:                                 | [Year] [Month] [Day] [Hour] [Minute] [Second] [AM/PM]", "#                                                |  2017    9       22    22     18        6        PM", "# Placeholders:", "# {YYYY} Year                                    |  2017", "# {MM} Month (2 numbers)                         |          9", "# {MMMM} Month (full name)                       |        September", "# {DD} Day (2 numbers)                           |                  22", "# {DDDD} Day (full name)                         |                Friday", "# {H} Hours (24 hour clock)                      |                        22", "# {h} Hours (12 hour clock)                      |                        10", "# {M} Minutes                                    |                               18", "# {S} Seconds                                    |                                         6", "# {T} AM/PM (used in {h} 12 hour clock)          |                                                  PM", "# So, this would be:", "# [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]", "# [2017:09:22] [22:18:06]", "#----------------------------------", "TimestapFormat: [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]" };
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

        private static void Reload(bool silentExecution, string message = null, object oPlayer = null)
        {
            if (!silentExecution)
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Checking if file's exist...");
                    CheckFiles("all");
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Reloading config files...");
                    ReadFile(SettingsFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "[OK] Config file reloaded");
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Reloading critical .txt files...");
                    ReadCustomCommands();
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
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "[OK] Critical .txt files reloaded");
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                }
            }
            else {
                CheckFiles("all");
                ReadFile(SettingsFile);
                ReadFileStream(LanguageBlockFile, LanguageBlockWords);
                ReadFileStream(ChatBlockFile, ChatBlockWords);
                ReadFileStream(AdminListFile, AdminsListPlayers);
                ReadCustomCommands();
                LanguageBlockWords = LanguageBlockWords.ConvertAll(d => d.ToLower());
                ChatBlockWords = ChatBlockWords.ConvertAll(d => d.ToLower());
                ReadFile(AnnouncementsFile);
                ReadFile(GodListFile);
                ReadFile(MuteListFile);
                ReadFile(AfkListFile);
                ReadFile(RulesFile);
            }
        }

        private static void Essentials(string message, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            player.SendToSelf(Channel.Unsequenced, (byte)10, "Essentials Created by UserR00T & DeathByKorea & BP");
            player.SendToSelf(Channel.Unsequenced, (byte)10, "Version " + Ver);
        }
        private static bool CheckGodmode(object oPlayer, float amount)
        {
            var player = (SvPlayer)oPlayer;
            if (GodListPlayers.Contains(player.playerData.username))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, amount + " DMG Blocked!");
                return true;
            }
            return false;
        }

        private static bool Say(string message, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            try
            {

                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    if ((message.Length == cmdSay.Length) || (message.Length == cmdSay2.Length))
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "An argument is required for this command.");
                        return true;
                    }
                    else
                    {
                        var arg1 = "blank";
                        if (message.StartsWith(cmdSay))
                        {
                            arg1 = message.Substring(cmdSay.Length);
                        }
                        else if (message.StartsWith(cmdSay2))
                        {
                            arg1 = message.Substring(cmdSay2.Length);
                        }
                        player.SendToAll(Channel.Unsequenced, (byte)10, msgSayPrefix + " " + player.playerData.username + ": " + arg1);
                        return true;
                    }
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorLogging(ex);

                return true;
            }
        }
        private static void CheckBanned(object oPlayer)
        {
            Thread.Sleep(3000);
            try
            {
                var player = (SvPlayer)oPlayer;
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
            }
        }
        private static void CheckAltAcc(object oPlayer)
        {
            if (CheckAlt)
            {
                Thread.Sleep(3000);
                try
                {
                    var player = (SvPlayer)oPlayer;
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
                }
            }
        }
        private static void WriteIPToFile(object oPlayer)
        {
            Thread.Sleep(500);
            var player = (SvPlayer)oPlayer;
            Debug.Log(SetTimeStamp() + "[INFO] " + "[JOIN] " + player.playerData.username + " IP is: " + player.netMan.GetAddress(player.connection));
            try
            {
                if (!File.ReadAllText(IpListFile).Contains(player.playerData.username + ": " + player.netMan.GetAddress(player.connection)))
                {
                    File.AppendAllText(IpListFile, player.playerData.username + ": " + player.netMan.GetAddress(player.connection) + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
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

        private static string SetTimeStamp()
        {
            try
            {
                var src = DateTime.Now;
                var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
                var placeHolderText = TimestampFormat;
                var minutes = hm.ToString("mm");
                var seconds = hm.ToString("ss");

                if (TimestampFormat.Contains("{YYYY}"))
                {
                    placeHolderText = placeHolderText.Replace("{YYYY}", hm.ToString("yyyy"));
                }
                if (TimestampFormat.Contains("{DD}"))
                {
                    placeHolderText = placeHolderText.Replace("{DD}", hm.ToString("dd"));
                }
                if (TimestampFormat.Contains("{DDDD}"))
                {
                    placeHolderText = placeHolderText.Replace("{DDDD}", hm.ToString("dddd"));
                }
                if (TimestampFormat.Contains("{MMMM}"))
                {
                    placeHolderText = placeHolderText.Replace("{MMMM}", hm.ToString("MMMM"));
                }
                if (TimestampFormat.Contains("{MM}"))
                {
                    placeHolderText = placeHolderText.Replace("{MM}", hm.ToString("MM"));
                }


                if (TimestampFormat.Contains("{H}"))
                {
                    placeHolderText = placeHolderText.Replace("{H}", hm.ToString("HH"));
                }
                if (TimestampFormat.Contains("{h}"))
                {
                    placeHolderText = placeHolderText.Replace("{h}", hm.ToString("hh"));
                }
                if (TimestampFormat.Contains("{M}") || TimestampFormat.Contains("{m}"))
                {
                    placeHolderText = placeHolderText.Replace("{M}", minutes);
                }
                if (TimestampFormat.Contains("{S}") || TimestampFormat.Contains("{s}"))
                {
                    placeHolderText = placeHolderText.Replace("{S}", seconds.ToString());
                }
                if (TimestampFormat.Contains("{T}"))
                {
                    placeHolderText = placeHolderText.Replace("{T}", hm.ToString("tt"));
                }
                placeHolderText = placeHolderText + " ";
                return placeHolderText;
            }
            catch
            {
                return "[Failed] ";
            }
        }

        private static void ReadCustomCommands()
        {
            string line;
            var file = new StreamReader(CustomCommandsFile);
            while ((line = file.ReadLine()) != null)
            {
                if (line.ToLower().StartsWith("#") || line == null || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    if (line.ToLower().StartsWith("command: "))
                    {
                        Commands.Add(cmdCommandCharacter + line.Substring(9));
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

        private static void ReadFileStream(string FileName, List<string> output)
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

        private static void ReadFile(string FileName)
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
                        else if (line.Contains("PlayersOnlineCommand: "))
                        {
                            cmdPlayers = cmdCommandCharacter + line.Substring(22);
                        }
                        else if (line.Contains("PlayersOnlineCommand2: "))
                        {
                            cmdPlayers2 = cmdCommandCharacter + line.Substring(23);
                        }
                        else if (line.Contains("InfoPlayerCommand: "))
                        {
                            cmdInfo = cmdCommandCharacter + line.Substring(19);
                        }
                        else if (line.Contains("InfoPlayerCommand2: "))
                        {
                            cmdInfo2 = cmdCommandCharacter + line.Substring(20);
                        }
                        else if (line.Contains("MoneyCommand: "))
                        {
                            cmdMoney = cmdCommandCharacter + line.Substring(14);
                        }
                        else if (line.Contains("MoneyCommand2: "))
                        {
                            cmdMoney2 = cmdCommandCharacter + line.Substring(15);
                        }
                        else if (line.Contains("DiscordLink: "))
                        {
                            msgDiscord = line.Substring(13);
                        }
                        else if (line.StartsWith("ATMCommand: "))
                        {
                            cmdATM = cmdCommandCharacter + line.Substring(12);
                        }
                        else if (line.StartsWith("EnableATMCommand: "))
                        {
                            enableATMCommand = Convert.ToBoolean(line.Substring(18));
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
                                        BlockedSpawnIDS = DisabledSpawnBots.Split(',').Select(int.Parse).ToArray();
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
                            cmdHelp = line.Substring(13);
                        }
                        else if (line.StartsWith("SaveCommand: "))
                        {
                            cmdSave = cmdCommandCharacter + line.Substring(13);
                        }
                        else if (line.StartsWith("FreeCommand: "))
                        {
                            cmdFree = cmdCommandCharacter + line.Substring(13);
                        }
                        else if (line.StartsWith("KillCommand: "))
                        {
                            cmdKick = cmdCommandCharacter + line.Substring(13);
                        }
                        else if (line.StartsWith("KickCommand: "))
                        {
                            cmdKick = cmdCommandCharacter + line.Substring(13);
                        }
                        else if (line.StartsWith("LogsCommand: "))
                        {
                            cmdLogs = cmdCommandCharacter + line.Substring(13);
                        }
                        else if (line.StartsWith("TpCommand: "))
                        {
                            cmdTp = cmdCommandCharacter + line.Substring(11);
                        }
                        else if (line.StartsWith("TpHereCommand: "))
                        {
                            cmdTpHere = cmdCommandCharacter + line.Substring(15);
                        }
                        else if (line.StartsWith("TpHereCommand2: "))
                        {
                            cmdTpHere2 = cmdCommandCharacter + line.Substring(16);
                        }
                        else if (line.StartsWith("SaveCommand: "))
                        {
                            cmdSave = cmdCommandCharacter + line.Substring(13);
                        }
                        else if (line.StartsWith("PayCommand: "))
                        {
                            cmdPay = cmdCommandCharacter + line.Substring(12);
                        }
                        else if (line.StartsWith("PayCommand2: "))
                        {
                            cmdPay2 = cmdCommandCharacter + line.Substring(13);
                        }
                        else if (line.StartsWith("BanCommand: "))
                        {
                            cmdBan = cmdCommandCharacter + line.Substring(12);
                        }
                        else if (line.StartsWith("ConfirmCommand: "))
                        {
                            cmdConfirm = cmdCommandCharacter + line.Substring(16);
                        }
                        else if (line.StartsWith("ArrestCommand: "))
                        {
                            cmdArrest = cmdCommandCharacter + line.Substring(15);
                        }
                        else if (line.StartsWith("RestrainCommand: "))
                        {
                            cmdRestrain = cmdCommandCharacter + line.Substring(17);
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
                rules = File.ReadAllText(FileName);
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

        private static void ErrorLogging(Exception ex)
        {
            if (!File.Exists(ExeptionFile))
            {
                File.Create(ExeptionFile).Close();
            }
            Thread.Sleep(20);
            string[] content =
            {
                Environment.NewLine,
                "Expection START ---------------- Date: " + DateTime.Now,
                Environment.NewLine,
                "Error Message: " + ex.Message,
                "Stack Trace: " + ex.StackTrace,
                "Full error: " + ex,
                Environment.NewLine,
                "Expection STOP ---------------- Date: " + DateTime.Now
            };
            File.AppendAllLines(ExeptionFile, content);
            Debug.Log(ex);
            Debug.Log("[ERROR]   Essentials - An exception occured, Check the Exceptions file for more info.");
            Debug.Log("[ERROR]   Essentials - Or check the error above for more info,");
            Debug.Log("[ERROR]   Essentials - And it would be highly if you would send the error to the developers of this plugin!");
        }

        // TODO ---------------------------------------------------------------------------------
        private static string GetArgument(int nr, string message) {
            var list = Regex.Matches(message, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            return list[nr];
        }
        #endregion
    }
}