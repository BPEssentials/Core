// Essentials created by UserR00T & DeathByKorea
using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Linq;

public class EssentialsPlugin
{
    // Folder locations --------------------------------------------------------------------
    private static string DirectoryFolder = Directory.GetCurrentDirectory() + " \\Essentials";
    private static string SettingsFile = DirectoryFolder + "settings.txt";
    private static string LanguageBlockFile = DirectoryFolder + "languageblock.txt";
    private static string ChatBlockFile = DirectoryFolder + "chatblock.txt";

    private static string IPListFile = "ip_list.txt";
    private static string AdminListFile = "admin_list.txt";
    // Edit them if you like ---------------------------------------------------------------

    #region predefining variables

    // General
    private static string version;
    private static string msgUnknownCommand;
    private static string ChatBlock;
    private static string LanguageBlock;
    // Block arrays
    private static string[] ChatBlockWords;
    private static string[] LanguageBlockWords;

    // Messages
    private static string msgNoPerm;

    // Commands
    private static string cmdCommandCharacter;

    private static string cmdClearChat;
    private static string cmdClearChat2;

    private static string cmdReload;
    private static string cmdReload2;
    #endregion










    /*
    
    
            Code below here, Don't edit unless you know what you're doing.
            Information about the api @ https://github.com/deathbykorea/universalunityhooks

    
     */


    [Hook("SvNetMan.StartServerNetwork")]
    public static void StartServerNetwork(SvNetMan netMan)
    {
        if (!Directory.Exists(DirectoryFolder))
        {
            Directory.CreateDirectory(DirectoryFolder);
            Thread.Sleep(20);
            File.Create(SettingsFile);
            Debug.Log("[WARNING] Essentials - Settings file does not exist! Creating one.");
            DownloadFile("https://UserR00T.com/dev/BPEssentials/settings.txt",SettingsFile);
        }
        if (!File.Exists(SettingsFile))
        {
            File.Create(SettingsFile);
            Debug.Log("[WARNING] Essentials - Settings file does not exist! Creating one.");
            DownloadFile("https://UserR00T.com/dev/BPEssentials/settings.txt", SettingsFile);
        }
        Debug.Log("[INFO] Essentials - version: " + version + " Loaded in successfully!");
    }


    // /////////////////////// //
    //       ChatEvents        //
    // /////////////////////// //
    [Hook("SvPlayer.SvGlobalChatMessage")]
    public static bool SvGlobalChatMessage(SvPlayer player, ref string message)
    {
        #region Message
        if (!message.StartsWith(cmdCommandCharacter))
        {
            Debug.Log("[MESSAGE]" + player.playerData.username + ": " + message);

            #region ChatBlock message handler
            if (ChatBlock == true)
            {
                
                if (ChatBlockWords.Any(message.ToLower().Contains))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Please don't say a blocked word, the message has been blocked.");
                    Debug.Log(player.playerData.username + " Said a word that is blocked.");
                    return true;
                }
            }
            #endregion

            #region LanguageBlock message handler
            if (LanguageBlock == true)
            {
                if (LanguageBlockWords.Any(message.ToLower().Contains))
                {
                    if (System.IO.File.ReadAllText(adminlist).Contains(player.playerData.username))
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
            #endregion

            return false;
        }
        #endregion


        #region Command
        else
        {
            Debug.Log("[COMMAND]" + player.playerData.username + ": " + message);

            #region ClearChat command handler
            // ClearChat //
            if (message.StartsWith(cmdClearChat) || message.StartsWith(cmdClearChat2))
            {
                try
                {
                    if (message.Length == cmdClearChat.Length || message.Length == cmdClearChat.Length + 1 || message.Length == cmdClearChat2.Length || message.Length == cmdClearChat2.Length + 1)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, " ");
                        }
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Cleared the chat for yourself.");
                        return true;
                    }
                    string arg1ClearChat = message.Substring(11);
                    if (arg1ClearChat == "self" || arg1ClearChat == "own")
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, " ");
                        }
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "Cleared the chat for yourself.");
                    }
                    else if (arg1ClearChat == "all" || arg1ClearChat == "everyone")
                    {
                        if (System.IO.File.ReadAllText(AdminListFile).Contains(player.playerData.username))
                        {
                            SvPlayer svPlayer = (SvPlayer)player;
                            for (int i = 0; i < 6; i++)
                            {
                                player.SendToAll(Channel.Unsequenced, (byte)10, " ");
                            }
                            player.SendToAll(Channel.Unsequenced, (byte)10, "Chat has been cleared for everyone.");

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
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.Log("Something went wrong while trying to make a SubString: Expection: " + ex);
                    Debug.Log("Please DM the error to UserR00T!");
                    Debug.Log("Try reinstalling the newest version.");
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Unknown error occured. Please check output_log.txt for more info.");
                }
            }
            #endregion

            #region Reload command handler
            // Reload //
            if (message.StartsWith(cmdReload) || message.StartsWith(cmdReload2))
            {
                if (System.IO.File.ReadAllText(AdminListFile).Contains(player.playerData.username))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Reloading config files...");
                    ReadFile(SettingsFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "[OK] Config file reloaded");
                    Thread.Sleep(50);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Reloading language and chat block files..");
                    ReadFile(LanguageBlockFile);
                    Thread.Sleep(10);
                    ReadFile(ChatBlockFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "[OK] Language and chat block files reloaded");
                    Thread.Sleep(10);
                    ReadFile(GodListFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "[OK] GodList file reloaded");
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                }
            }
            #endregion

            #region GodMode command handler
            if (message.StartsWith(cmdGodmode) || message.StartsWith(cmdGodmode2))
            {
                try
                {
                    if (System.IO.File.ReadAllText(AdminListFile).Contains(player.playerData.username))
                    {
                        if (GodListFile.Contains(player.playerData.username))
                        {
                            ReadFile(GodListFile);

                            player.SendToSelf(channel.unsquenced, (btye)10, "Godmode disabled.");
                        }
                        else
                        {
                            File.AppendAllText(player.playerData.username + Environment.NewLine);
                            player.SendToSelf(channel.unsquenced, (btye)10, "Godmode enabled.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("[ERROR] [GODMODE] Expection: " + ex.ToString());
                    player.SendToSelf(channel.unsquenced, (btye)10, "Unknown error. Check console for more info");
                    return true;
                }
            }
            #endregion

            #region Say command handler
            if (message.StartsWith(cmdSay1) || (message.StartsWith(cmdSay2)))
            {
                try
                {
                    if (System.IO.File.ReadAllText(AdminListFile).Contains(player.playerData.username))
                    {
                        if ((message.Length == cmdSay1.Length) || (message.Length == cmdSay2.Length))
                        {
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "An argument is required for this command.");
                        }
                        else
                        {
                            string arg1;
                            if (message.StartsWith(cmdSay1))
                            {
                                arg1 = message.SubString(cmdSay1.Length);
                            }
                            else if (message.StartsWith(cmdSay2))
                            {
                                arg1 = message.SubString(cmdSay2.Length);
                            }
                            player.SendToAll(Channel.Unsequenced, (byte)10, msgSayPrefix + player.playerData.username + ": " + arg1);
                            return true;
                        }
                    }
                    else
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, msgNoPerm);
                        return false;
                    }
                }
                catch (Expection ex)
                {
                    Debug.Log("[ERROR] [SAY] Expection: " + ex.ToString());
                    player.SendToSelf(channel.unsquenced, (btye)10, "Unknown error. Check console for more info");
                    return true;
                }
            }
            #endregion

            #region Essentials command handler
            if (message.StartsWith("/essentials") || message.StartsWith("/ess"))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Essentials Created by UserR00T");

                //TODO: Subcommands like /essentials reload : executes cmdReload
                
            }
            #endregion


            //#region .. command handler
            //else if (message.StartsWith() || message.StarsWith())
            //{
            //
            //}
            //#endregion
            #region else
            else
            {
                if (msgUnknownCommand == true)
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Unknown command");
                    return true;
                }
                return false;
            }
            #endregion
        }
        #endregion
    }


    // /////////////////////// //
    //          IpLog          //
    // /////////////////////// //
    [Hook("SvPlayer.Initialize")]
    public static void Initialize(SvPlayer player)
    {
        if (player.playerData.username != null)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(WriteIPToFile));
            thread.Start(player);
        }
    }
    private static void WriteIPToFile(object oPlayer)
    {
        Thread.Sleep(500);
        SvPlayer player = (SvPlayer)oPlayer;
        Debug.Log("[INFO] " + "[JOIN] " + player.playerData.username + " IP is: " + player.netMan.GetAddress(player.connection));
        try
        {
            if (!File.ReadAllText(IPListFile).Contains(player.playerData.username + ": " + player.netMan.GetAddress(player.connection)))
            {
                File.AppendAllText(IPListFile, player.playerData.username + ": " + player.netMan.GetAddress(player.connection) + Environment.NewLine);

            }
        }
        catch (Exception ex)
        {
            Debug.Log("[ERROR] Unknown error occured, please send the following to UserR00T:" + ex);
        }

    }


    // /////////////////////// //
    //      DownloadFile       //
    // /////////////////////// //
    public static void DownloadFile(string SrvLoc, string LocalLoc)
    {
        WebClient client = new WebClient();
        Stream stream = client.OpenRead(SrvLoc);
        StreamReader reader = new StreamReader(stream);
        String content = reader.ReadToEnd();
        File.WriteAllText(LocalLoc, content);
    }


    // /////////////////////// //
    //        ReadFile         //
    // /////////////////////// //
    public static void ReadFile(string FileName)
    {
        if (FileName == SettingsFile)
        {
            var lines = File.ReadAllLines(FileName);
            foreach (var line in lines)
            {
                if (line.StartsWith("#") || line.Contains("#"))
                {
                    continue;
                }
                else
                {
                    // TODO: make this better/compacter
                    if (line.Contains("version: "))
                    {
                        version = line.Substring(9);
                    }
                    else if (line.Contains("CommandCharacter: "))
                    {
                        cmdCommandCharacter = line.Substring(17);
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
                    else if (line.Contains("UnknownCommand: "))
                    {
                        msgUnknownCommand = line.Substring(16);
                    }
                    else if (line.Contains("enableChatBlock: "))
                    {
                        ChatBlock = line.Substring(17);
                    }
                    else if (line.Contains("enableLanguageBlock: "))
                    {
                        LanguageBlock = line.Substring(21);
                    }
                    else if (line.Contains("ReloadCommand: "))
                    {
                        cmdReload = cmdCommandCharacter + line.Substring(15);
                    }
                    else if (line.Contains("ReloadCommand2: "))
                    {
                        cmdReload2 = cmdCommandCharacter + line.Substring(16);
                    }

                    //else if (line.Contains(""))
                    //{
                    //    = line.Substring();
                    //}
                }
            }
        }
        else if (FileName == LanguageBlockFile)
        {
            LanguageBlockWords = System.IO.File.ReadAllLines(FileName);
        }
        else if (FileName == ChatBlockFile)
        {
            ChatBlockWords = System.IO.File.ReadAllLines(FileName);
        }
    }
}