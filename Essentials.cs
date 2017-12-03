// Essentials created by UserR00T & DeathByKorea & BP
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
    private static string LogDirectory = FileDirectory + "logs/";

    private static string SettingsFile = FileDirectory + "essentials_settings.txt";
    private static string LanguageBlockFile = FileDirectory + "languageblock.txt";
    private static string ChatBlockFile = FileDirectory + "chatblock.txt";
    private static string AnnouncementsFile = FileDirectory + "announcements.txt";
    private static string IPListFile = FileDirectory + "ip_list.txt";
    private static string GodListFile = FileDirectory + "godlist.txt";
    private static string AfkListFile = FileDirectory + "afklist.txt";
    private static string MuteListFile = FileDirectory + "mutelist.txt";
    private static string ExeptionFile = FileDirectory + "exceptions.txt";
    private static string CustomCommandsFile = FileDirectory + "CustomCommands.txt";

    private static string AdminListFile = "admin_list.txt";
    private static string RulesFile = "server_info.txt";

    private static string LogFile =  LogDirectory + "all.txt";
    private static string ChatLogFile =  LogDirectory + "chat.txt";
    private static string CommandLogFile =  LogDirectory + "commands.txt";

    #endregion

    #region predefining variables

    // General
    private static string ver = "PRE-2.0.0";
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
    private static List<string> Commands = new List<string>();
    private static List<string> Responses = new List<string>();
    private static List<string> ChatBlockWords = new List<string>();
    private static List<string> LanguageBlockWords = new List<string>();
    private static List<string> AdminsListPlayers = new List<string>();
    private static List<string> GodListPlayers = new List<string>();
    private static List<string> AfkPlayers = new List<string>();
    private static List<string> MutePlayers = new List<string>();

    // Arrays
    private static string[] announcements;
    public static string[] Jobs = { "Citizen", "Criminal", "Prisoner", "Police", "Paramedic", "Firefighter", "Gangster: Red", "Gangster: Green", "Gangster: Blue" };

    // Messages
    private static string msgSayPrefix;
    private static string msgNoPerm;
    private static string msgDiscord;
    private static string DisabledCommand = "The server owner disabled this command.";

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
    private static int SaveTime = 60 * 5;
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

            if (ver != version)
            {
                Debug.Log("[ERROR] Essentials - Versions do not match!");
                Debug.Log("[ERROR] Essentials - Essentials version:" + ver);
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
            Thread thread = new Thread(SavePeriodically);
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
            Thread thread = new Thread(new ParameterizedThreadStart(AnnounceThread));
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
            afk(message, player);
        }
        if (message.Trim().StartsWith(cmdCommandCharacter))
        {
            if (message.StartsWith("/arg")) // TODO
            {
                try
                {
                    GetArgument(Convert.ToByte(GetArgument(1, message)), message);
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
                    if (String.IsNullOrEmpty(arg1))
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
                    int arg2int;
                    bool isNumeric = int.TryParse(arg2, out arg2int);

                    if (isNumeric)
                    {
                        bool error = false;
                        bool found = false;
                        if (arg2int == 0)
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
                                                    if (_shPlayer.playerInventory.MyMoneyCount() >= arg2int)
                                                    {
                                                        _shPlayer.playerInventory.TransferMoney(2, arg2int, true);
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
                                            shPlayer.playerInventory.TransferMoney(1, arg2int, true);
                                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Succesfully transfered " + arg2int + "$ to " + shPlayer.svPlayer.playerData.username + "!");
                                            shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " gave you " + arg2int + "$!");
                                            found = true;
                                        }
                                        else
                                        {
                                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer money, do you have " + arg2int + "$ in your inventory?");
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
                            if (arg2int == 0)
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer 0$.");
                            else
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Cannot transfer money, do you have " + arg2int + "$ in your inventory?");

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
                    string TempMSG = message.Trim();
                    if (TempMSG != "/getplayerhash" || TempMSG != "/gethash")
                    {
                        string arg1 = player.playerData.username;
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
                    Thread thread = new Thread(SaveNow);
                    thread.Start();
                    return true;
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                    return true;
                }
            }
            // Tp
            if (message.StartsWith(cmdTpHere) || message.StartsWith(cmdTpHere2))
            {
                string TempMSG = message.Trim();
                if (TempMSG != cmdTpHere || TempMSG != cmdTpHere2)
                {
                    string arg1 = String.Empty;
                    if (TempMSG.StartsWith(cmdTpHere + " "))
                    {
                        arg1 = TempMSG.Substring(cmdTpHere.Length + 1);
                    }
                    else if (TempMSG.StartsWith(cmdTpHere2 + " "))
                    {
                        arg1 = TempMSG.Substring(cmdTpHere2.Length + 1);
                    }
                    ExecuteOnPlayer(player, message, arg1);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                return true;
            }
            else if (message.StartsWith("/tp"))
            {
                string TempMSG = message.Trim();
                if (TempMSG != "/tp")
                {
                    string arg1 = TempMSG.Substring(3 + 1);
                    ExecuteOnPlayer(player, message, arg1);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                return true;
            }
            // Ban
            if (message.StartsWith("/ban"))
            {
                string TempMSG = message.Trim();
                if (TempMSG != "/ban")
                {
                    string arg1 = TempMSG.Substring(4 + 1);
                    ExecuteOnPlayer(player, message, arg1);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                return true;
            }

            // Kick
            if (message.StartsWith("/kick"))
            {
                string TempMSG = message.Trim();
                if (TempMSG != "/kick")
                {
                    string arg1 = TempMSG.Substring(5 + 1);
                    ExecuteOnPlayer(player, message, arg1);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
                return true;
            }
            // Arrest
            if (message.StartsWith("/arrest"))
            {
                string TempMSG = message.Trim();
                if (TempMSG != "/arrest")
                {
                    string arg1 = TempMSG.Substring(7 + 1);
                    ExecuteOnPlayer(player, message, arg1);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                return true;
            }
            // Restrain
            if (message.StartsWith("/restrain"))
            {
                string TempMSG = message.Trim();
                if (TempMSG != "/restrain")
                {
                    string arg1 = TempMSG.Substring(9 + 1);
                    ExecuteOnPlayer(player, message, arg1);

                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                return true;
            }
            // Kill
            if (message.StartsWith("/kill"))
            {
                string TempMSG = message.Trim();
                if (TempMSG != "/kill")
                {
                    string arg1 = TempMSG.Substring(5 + 1);
                    ExecuteOnPlayer(player, message, arg1);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");

                return true;
            }
            // Free
            if (message.StartsWith("/free"))
            {
                string TempMSG = message.Trim();
                if (TempMSG != "/free")
                {
                    string arg1 = TempMSG.Substring(5 + 1);
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
                int RealPlayers = 0;
                foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                {
                    if (shPlayer.IsRealPlayer())
                    {
                        ++RealPlayers;
                    }
                }
                if (RealPlayers == 1)
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "There is " + RealPlayers + " player online");
                else if (RealPlayers < 1)
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + RealPlayers + " play- wait, how is that possible");
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "There are " + RealPlayers + " player(s) online");
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
                    say(message, player);
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
                player.SendToSelf(Channel.Reliable, (byte)50, rules);
                return true;
            }
            // Command: CheckIP
            if (message.StartsWith(cmdCheckIP))
            {
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    string TempMSG = message.Trim();
                    if (TempMSG != cmdCheckIP)
                    {
                        string arg1 = TempMSG.Substring(cmdCheckIP.Count() + 1);
                        CheckIP(TempMSG, player, arg1);
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
                    string TempMSG = message.Trim();
                    if (TempMSG != cmdCheckPlayer)
                    {
                        string arg1 = TempMSG.Substring(cmdCheckPlayer.Count() + 1);
                        CheckPlayer(TempMSG, player, arg1);
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
                    string TempMSG = message.Trim();
                    if (!(TempMSG == cmdFakeJoin || TempMSG == cmdFakeLeave))
                    {
                        string arg1 = null;
                        if (TempMSG.StartsWith(cmdFakeJoin))
                        {
                            arg1 = TempMSG.Substring(cmdFakeJoin.Length + 1);
                            player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " connected");
                        }
                        else if (TempMSG.StartsWith(cmdFakeLeave))
                        {
                            arg1 = TempMSG.Substring(cmdFakeLeave.Length + 1);
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
                int i = 0;
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
                        string arg1 = "null";
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
                        if (String.IsNullOrEmpty(arg1))
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
                    if (!(String.IsNullOrEmpty(arg2)))
                    {
                        int arg2int;
                        bool isNumeric = int.TryParse(arg2, out arg2int);

                        if (isNumeric)
                        {
                            bool found = false;
                            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                            {
                                if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                {
                                    if (shPlayer.IsRealPlayer())
                                    {
                                        shPlayer.playerInventory.TransferMoney(1, arg2int, true);
                                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Succesfully gave " + shPlayer.svPlayer.playerData.username + " " + arg2int + "$");
                                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, player.playerData.username + " gave you " + arg2int + "$!");
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
            if (ChatBlock || LanguageBlock)
            {
                if (BlockMessage(message, player))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
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
    #endregion
    #region Event: Damage
    [Hook("SvPlayer.Damage")]
    public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider)
    {
        if (!(CheckGodmode(player, amount)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
    #region Event: SpawnBot
    [Hook("SvPlayer.SpawnBot")]
    public static bool SpawnBot(SvPlayer player, ref Vector3 position, ref Quaternion rotation, ref WaypointNode node, ref ShTransport vehicle, ref byte spawnJobIndex)
    {
        if (EnableBlockSpawnBot == true)
        {
            if (BlockedSpawnIDS.Contains(spawnJobIndex))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    #region Event: HitEffect
    [Hook("ShRetainer.HitEffect")] // Blocks handcuff
    public static bool HitEffect(ShRetainer player, ref ShEntity hitTarget, ref ShPlayer source, ref Collider collider)
    {
        foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
            if (shPlayer.IsRealPlayer())
            {
                if (shPlayer == hitTarget)
                {
                    if (GodListPlayers.Contains(shPlayer.svPlayer.playerData.username))
                    {
                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Being handcuffed Blocked!");
                        return true;
                    }
                }
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
                    if (shPlayer.GetSpaceIndex() < 13)
                    {
                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Saving game.. This can take up to 5 seconds.");
                        shPlayer.svPlayer.Save();
                    }
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
                if (shPlayer.GetSpaceIndex() < 13)
                {
                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, (byte)10, "Saving game.. This can take up to 5 seconds.");
                    shPlayer.svPlayer.Save();
                }
            }
    }
    private static void ExecuteOnPlayer(object oPlayer, string message, string arg1)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        if (AdminsListPlayers.Contains(player.playerData.username))
        {
            bool found = false;
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
    public static void GetLogs(object oPlayer, string LogFile)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        if (LogFile == ChatLogFile)
        {
            string content = null;
            string line = null;
            using (var reader = new StreamReader(LogFile))
            {
                for (int i = 0; i < 31; i++)
                {
                    if ((line = reader.ReadLine()) != null)
                        content = content + "\r\n" + line;
                    else
                        break;
                }
            }
            player.SendToSelf(Channel.Reliable, (byte)50, content);
        }
    }
    public static void GetPlayerInfo(object oPlayer, string arg1)
    {
        bool found = false;
        SvPlayer player = (SvPlayer)oPlayer;
        foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
        {
            if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
            {
                if (shPlayer.IsRealPlayer())
                {
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


                    string content = string.Join("\r\n", contentarray);

                    player.SendToSelf(Channel.Reliable, (byte)50, content);

                    found = true;
                }
            }
        }
        if (!(found))
        {
            player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " Not found/online.");
        }
    }
    static string GetPlaceHolders(int i, object oPlayer)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        var src = DateTime.Now;
        var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
        string PlaceHolderText = Responses[i];
        string Hours = hm.ToString("HH");
        string Minutes = hm.ToString("mm");
        string Seconds = hm.ToString("ss");

        if (Responses[i].Contains("{YYYY}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{YYYY}", hm.ToString("yyyy"));
        }
        if (Responses[i].Contains("{DD}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{DD}", hm.ToString("dd"));
        }
        if (Responses[i].Contains("{DDDD}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{DDDD}", hm.ToString("dddd"));
        }
        if (Responses[i].Contains("{MMMM}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{MMMM}", hm.ToString("MMMM"));
        }
        if (Responses[i].Contains("{MM}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{MM}", hm.ToString("MM"));
        }

        if (Responses[i].Contains("{H}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{H}", hm.ToString("HH"));
        }
        if (Responses[i].Contains("{h}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{h}", hm.ToString("hh"));
        }
        if (Responses[i].Contains("{M}") || Responses[i].Contains("{m}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{M}", Minutes);
        }
        if (Responses[i].Contains("{S}") || Responses[i].Contains("{s}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{S}", Seconds.ToString());
        }
        if (Responses[i].Contains("{T}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{T}", hm.ToString("tt"));
        }
        if (Responses[i].ToLower().Contains("{username}"))
        {
            PlaceHolderText = PlaceHolderText.Replace("{username}", player.playerData.username);
        }
        return PlaceHolderText;
    }
    private static void AnnounceThread(object man)
    {
        SvNetMan netMan = (SvNetMan)man;
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
        SvPlayer player = (SvPlayer)oPlayer;
        int found = 0;
      //  player.SendToSelf(Channel.Unsequenced, (byte)10, "IP for player: " + arg1);
        string content = String.Empty;
        
        foreach (var line in File.ReadAllLines(IPListFile))
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
    public static void CheckPlayer(string message, object oPlayer, string arg1)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        int found = 0;
        string arg1temp = null;
        player.SendToSelf(Channel.Unsequenced, (byte)10, "Accounts for IP: " + arg1);
        foreach (var line in File.ReadAllLines(IPListFile))
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
    public static void MessageLog(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        if (!message.StartsWith(cmdCommandCharacter))
        {
            string mssge = SetTimeStamp() + "[MESSAGE] " + player.playerData.username + ": " + message;
            Debug.Log(mssge);
            File.AppendAllText(ChatLogFile, mssge + Environment.NewLine);
            File.AppendAllText(LogFile, mssge + Environment.NewLine);
        }
        else if (message.StartsWith(cmdCommandCharacter))
        {
            string mssge = (SetTimeStamp() + "[COMMAND] " + player.playerData.username + ": " + message);
            Debug.Log(mssge);
            File.AppendAllText(CommandLogFile, mssge + Environment.NewLine);
            File.AppendAllText(LogFile, mssge + Environment.NewLine);
        }
    }
    public static bool Mute(string message, object oPlayer, bool unmute)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        string muteuser = null;
        bool found = false;
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
            else if (!unmute)
            {
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
    public static void afk(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer)oPlayer;
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
    public static bool BlockMessage(string message, object oPlayer)
    {
        message = message.ToLower();
        SvPlayer player = (SvPlayer)oPlayer;
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
    public static bool godMode(string message, object oPlayer)
    {
        try
        {
            SvPlayer player = (SvPlayer)oPlayer;
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
    public static void ClearChat(string message, object oPlayer, bool all)
    {

        try
        {
            SvPlayer player = (SvPlayer)oPlayer;
            if (!all)
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Clearing the chat for yourself...");
                Thread.Sleep(500);
                for (int i = 0; i < 6; i++)
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, " ");
                }
            }
            else if (all)
            {
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    player.SendToAll(Channel.Unsequenced, (byte)10, "Clearing chat for everyone...");
                    Thread.Sleep(500);
                    for (int i = 0; i < 6; i++)
                    {
                        player.SendToAll(Channel.Unsequenced, (byte)10, " ");
                    }
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                }
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, @"'" + arg1ClearChat + @"'" + " Is not a valid argument.");
            }
        }
        catch (Exception ex)
        {
            ErrorLogging(ex);

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
    public static void CreateFile(string FileName)
    {
        if (FileName == SettingsFile)
        {
            string[] content = { "# ---------------------------------------------------------------------------------------- #", "#                             Broke Protocol: Essentials                                   #", "#                        Created by UserR00T and DeathByKorea and BP                       #", "# ---------------------------------------------------------------------------------------- #", "#                                                                                          #", "#                                                                                          #", "#                                                                                          #", "# NOTE:                                                                                    #", "# CommandCharacter will be automatically added to the commands! No need to do that!        #", "# Example:                                                                                 #", "# INCORRECT: ClearChatCommand: /clearchat                                                  #", "# CORRECT: ClearChatCommand: clearchat                                                     #", "# if CommandCharacter is / it will automatically add a /                                   #", "#                                                                                          #", "# ---------------------------------------------------------------------------------------- #", "", "", "", "", "", "#----------------------------------------------------------#", "#                           General                        #", "#----------------------------------------------------------#", "version: " + ver, "", "# Character used for commands", "#----------------------------------", "CommandCharacter: /", "", @"# Will display ""unknown command"" if the command is not found in this plugin", "#----------------------------------", "UnknownCommand: true", "", "noperm: No permission.", "", "", "", "", "", "#----------------------------------------------------------#", "#                          Commands                        #", "#----------------------------------------------------------#", "", "# Reload command", "#----------------------------------", "ReloadCommand: reload", "ReloadCommand2: rl", "", "# Clearchat command", "#----------------------------------", "ClearChatCommand: clearchat", "ClearChatCommand2: cc", "", "# Say command", "#----------------------------------", "SayCommand: say", "SayCommand2: broadcast", "", "# CheckIP/Player command", "#----------------------------------", "CheckIPCommand: checkip", "CheckPlayerCommand: checkplayer", "", "# Fake Join/Leave command", "#----------------------------------", "FakeLeaveCommand: fakeleave", "FakeJoinCommand: fakejoin", "", "# Player info command", "#----------------------------------", "InfoPlayerCommand: info", "InfoPlayerCommand2: stats", "", "# Money command", "#----------------------------------", "MoneyCommand: money", "MoneyCommand2: setbal", "", "# Rules command", "#----------------------------------", "RulesCommand: rules", "", "", "# Discord command", "#----------------------------------", "DiscordCommand: discord", "", "# Online Players command", "#----------------------------------", "PlayersOnlineCommand: players", "PlayersOnlineCommand2: online", "", "# Godmode command --- Godmode default saving location - 'godmode.txt'", "#----------------------------------", "GodmodeCommand: god", "GodmodeCommand2: godmode", "", "# AFK command --- AFK default saving location - 'afkplayers.txt'", "#----------------------------------", "AFKCommand: afk", "AFKCommand2: brb", "", "# Mute command --- Mute default saving location - 'muteplayers.txt'", "#----------------------------------", "MuteCommand: mute", "# Unmute Command", "UnMuteCommand: unmute", "", "# ATM command", "#----------------------------------", "ATMCommand: atm", "", "", "", "#----------------------------------------------------------#", "#                          Messages                        #", "#----------------------------------------------------------#", "# Say prefix: Will show the string infront of the message", "# Example: [!!!] UserR00T: This is a message!", "# A space is not required at the end.", "#----------------------------------", "msgSayPrefix: [!!!]", "", "# Discord invite link", "#----------------------------------", "DiscordLink: https://discord.gg/Test", "", "", "", "#----------------------------------------------------------#", "#                    Additional Settings                   #", "#----------------------------------------------------------#", "", "", "# ChatBlock --- Words default location - 'chatblock.txt'", "# Enable chatblock", "#----------------------------------", "enableChatBlock: true", "", "", "# LanguageBlock --- Words default location - 'languageblock.txt'", "# Enable LanguageBlock", "#----------------------------------", "enableLanguageBlock: true", "", "# Enable ATM command", "#----------------------------------", "EnableATMCommand: true", "", "# This will check for alt accounts with the same IP.", "# NOTE: If someone in the same home connects with the same IP it will be detected as alt.", "# Enable CheckForAlts", "#----------------------------------", "CheckForAlts: true", "", "", "# Seconds between annoucements in seconds -- Announcements in 'Annoucements.txt'", "#----------------------------------", "TimeBetweenAnnounce: 360", "", "", "# TimestampFormat; This means what you will see infront of a message: ", "# E.g: [{H}:{M}:{S}] and your time is 12:05:59 it will show [12:05:59]", "# E.g Time/Date:                                 | [Year] [Month] [Day] [Hour] [Minute] [Second] [AM/PM]", "#                                                |  2017    9       22    22     18        6        PM", "# Placeholders:", "# {YYYY} Year                                    |  2017", "# {MM} Month (2 numbers)                         |          9", "# {MMMM} Month (full name)                       |        September", "# {DD} Day (2 numbers)                           |                  22", "# {DDDD} Day (full name)                         |                Friday", "# {H} Hours (24 hour clock)                      |                        22", "# {h} Hours (12 hour clock)                      |                        10", "# {M} Minutes                                    |                               18", "# {S} Seconds                                    |                                         6", "# {T} AM/PM (used in {h} 12 hour clock)          |                                                  PM", "# So, this would be:", "# [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]", "# [2017:09:22] [22:18:06]", "#----------------------------------", "TimestapFormat: [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]" };
            File.WriteAllLines(SettingsFile, content);
        }
        if (FileName == ChatBlockFile)
        {
            string[] content = { "nigger", "nigga", "nigg3r", "NIGGER", "NI99ER", "ni99er", "nigger.", "nigga.", "nigg3r.", "N199ER", "n1gger", "N1GGER", "NIGGA", "NIGGA." };
            File.WriteAllLines(ChatBlockFile, content);
        }
        if (FileName == LanguageBlockFile)
        {
            string[] content = { "bombas", "hola", "alguien", "habla", "espanol", "español", "estoy", "banco", "voy", "consegi", "donde", "quedamos", "banko", "afuera", "estas", "alguem", "donde", "nos", "vemos", "soy ", "vueno", "como", "carro", "cabros", "miren", "hacha", "laar", "corri", "sacame", "aqui", "policia", "trajo", "encerro", "bomba", "beuno", "pantalones", "dinero", "porque", "tengo", "escopetaa", "escopeta" };
            File.WriteAllLines(LanguageBlockFile, content);
        }
        if (FileName == CustomCommandsFile)
        {
            string[] content = { "# Custom commands file", "# Place your custom commands here", "#", "# Format:", "# Command: example", "# Respone: The time is {H}:{M}:{S}. ", "#", "# ", "# Placeholders:", "# ", "# Current time:", "# E.g Time/Date:                                 | [Year] [Month] [Day] [Hour] [Minute] [Second] [AM/PM]", "#                                                |  2017    9       22    22     18        6        PM", "# Placeholders:", "# {YYYY} Year                                    |  2017", "# {MM} Month (2 numbers)                         |          9", "# {MMMM} Month (full name)                       |        September", "# {DD} Day (2 numbers)                           |                  22", "# {DDDD} Day (full name)                         |                Friday", "# {H} Hours (24 hour clock)                      |                        22", "# {h} Hours (12 hour clock)                      |                        10", "# {M} Minutes                                    |                               18", "# {S} Seconds                                    |                                         6", "# {T} AM/PM (used in {h} 12 hour clock)          |                                                  PM", "# So, this would be:", "# [{YYYY}:{MM}:{DD}] [{H}:{M}:{S}]", "# [2017:09:22] [22:18:06]", "#", "# Player placeholders:", "# {username} Players username", "# ----------------------------------------------", "", "Command: servertime", "Response: The time on the server(pc) is: {YYYY}/{MM}/{DD}, {H}:{M}:{S}.", "", "Command: example", "Response: Your username is {username}!" };
            File.WriteAllLines(CustomCommandsFile, content);
        }

    }
    public static void Reload(bool silentExecution, string message = null, object oPlayer = null)
    {
        if (!silentExecution)
        {
            SvPlayer player = (SvPlayer)oPlayer;
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
        else if (silentExecution)
        {
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
    public static void essentials(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        player.SendToSelf(Channel.Unsequenced, (byte)10, "Essentials Created by UserR00T & DeathByKorea & BP");
        player.SendToSelf(Channel.Unsequenced, (byte)10, "Version " + ver);
    }
    private static bool CheckGodmode(object oPlayer, float amount)
    {
        SvPlayer player = (SvPlayer)oPlayer;
        if (GodListPlayers.Contains(player.playerData.username))
        {
            player.SendToSelf(Channel.Unsequenced, (byte)10, amount + " DMG Blocked!");
            return true;
        }
        return false;
    }
    public static bool say(string message, object oPlayer)
    {
        SvPlayer player = (SvPlayer)oPlayer;
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
                    string arg1 = "blank";
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
            SvPlayer player = (SvPlayer)oPlayer;
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
            }
        }
    }
    private static void WriteIPToFile(object oPlayer)
    {
        Thread.Sleep(500);
        SvPlayer player = (SvPlayer)oPlayer;
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

            if (TimestampFormat.Contains("{YYYY}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{YYYY}", hm.ToString("yyyy"));
            }
            if (TimestampFormat.Contains("{DD}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{DD}", hm.ToString("dd"));
            }
            if (TimestampFormat.Contains("{DDDD}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{DDDD}", hm.ToString("dddd"));
            }
            if (TimestampFormat.Contains("{MMMM}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{MMMM}", hm.ToString("MMMM"));
            }
            if (TimestampFormat.Contains("{MM}"))
            {
                PlaceHolderText = PlaceHolderText.Replace("{MM}", hm.ToString("MM"));
            }


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
        catch
        {
            return "[Failed] ";
        }
    }
    public static void ReadCustomCommands()
    {
        string line;
        StreamReader file = new StreamReader(CustomCommandsFile);
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
                                {
                                    continue;
                                }
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
    public static void ErrorLogging(Exception ex)
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
    public static string GetArgument(byte nr, string message)
    {
        switch (nr)
        {
            case 1:
                string cmd = message.Substring(0, message.Split(' ')[0].Length);
                string arg1 = message.Split(' ')[1];
                Debug.Log("ArgNumber: " + nr);
                Debug.Log("Message: " + message);
                Debug.Log("Command: " + cmd);
                Debug.Log("Arg1: " + arg1);
                return arg1;
            case 2:
                string cmd = message.Substring(0, message.Split(' ')[0].Length);
                string arg2 = message.Split(' ').Last().Trim(); // TODO
                Debug.Log("ArgNumber: " + nr);
                Debug.Log("Message: " + message);
                Debug.Log("Command: " + cmd);
                Debug.Log("Arg2: " + arg2);
                return arg2;
            case 3:
                break;
           
        } return null;
    }
    #endregion
}