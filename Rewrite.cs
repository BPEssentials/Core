// Essentials created by UserR00T
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.WebClient;
using UnityEngine;

public class EssentialsPlugin {

    #region Folder Locations
    private static string DirectoryFolder = Directory.GetCurrentDirectory () + " \\Essentials";
    private static string SettingsFile = DirectoryFolder + "settings.txt";
    private static string LanguageBlockFile = DirectoryFolder + "languageblock.txt";
    private static string ChatBlockFile = DirectoryFolder + "chatblock.txt";

    private static string IPListFile = "ip_list.txt";
    private static string AdminListFile = "admin_list.txt";
    #endregion

    #region predefining variables

    // General
    private static string version;
    private static string msgUnknownCommand;
    private static string ChatBlock;
    private static string LanguageBlock;
    private static string Command;
    private static SvPlayer svplayer;

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

    //Startup script
    [Hook ("SvNetMan.StartServerNetwork")]
    public static void StartServerNetwork (SvNetMan netMan) {
        if (!Directory.Exists (DirectoryFolder)) {
            Directory.CreateDirectory (DirectoryFolder);
            Thread.Sleep (20);
            File.Create (SettingsFile);
            Debug.Log ("[WARNING] Essentials - Settings file does not exist! Creating one.");
            DownloadFile ("https://UserR00T.com/dev/BPEssentials/settings.txt", SettingsFile);
        }
        if (!File.Exists (SettingsFile)) {
            File.Create (SettingsFile);
            Debug.Log ("[WARNING] Essentials - Settings file does not exist! Creating one.");
            DownloadFile ("https://UserR00T.com/dev/BPEssentials/settings.txt", SettingsFile);
        }
        Debug.Log ("[INFO] Essentials - version: " + version + " Loaded in successfully!");
    }

    //Chat Events
    [Hook ("SvPlayer.SvGlobalChatMessage")]
    public static bool SvGlobalChatMessage (SvPlayer player, ref string message) {
        // Checks if the message is a command, if not, log it
        if (message.StartsWith (cmdCommandCharacter)) {
            command = message;

        } else {
            MessageLog (message);

        }
        // Checks if the message is a blocked one, if it is, block it.
        if (ChatBlock == true) {

            if (ChatBlockWords.Any (message.ToLower ().Contains)) {
                bool blocked = BlockMessage (message);
                if (blocked) {
                    return false;
                } else {
                    return true;
                }
            }
        }
        // Check if the message is English, if not, block it
        if (LanguageBlock == true) {
            if (LanguageBlockWords.Any (message.ToLower ().Contains)) {
                bool blocked = BlockMessage ();
                if (blocked) {
                    return true;
                } else {
                    return false;
                }
            }
            // Clear chat command, self and global
<<<<<<< HEAD
            if (command = cmdClearChat || command = cmdClearChat2) {
=======
            if (command == cmdClearChat || command == cmdClearChat2) {
>>>>>>> origin/master
            ClearChat (message, true);
            return true;
        }
        string arg1ClearChat = message.Substring (11);
        else if (arg1ClearChat == "all" || arg1ClearChat == "everyone") {
            ClearChat(message, false);
            return true;
        }
        // Reload command
        if (message.StartsWith (cmdReload) || message.StartsWith (cmdReload2)) {
            Reload (message);
        }

        if (message.StartsWith () || message.StartsWith ()) { } //TODO: Godmode

        if (message.StartsWith ("/say")) { //TODO: Figure out how this would work.
                say (message);

            }
            // Info command
            if (message.StartsWith ("/essentials") || message.StartsWith ("/ess")) {
                    if (msgUnknownCommand == true) {
                        player.SendToSelf (Channel.Unsequenced, (byte) 10, "Unknown command");
                        return true;
                    }
                    else{

                    
                    return false;
                    }

                }

            }
    }


        // These are the various functions for the commands.

        public static void MessageLog (string message) {
            if (!message.StartsWith (cmdCommandCharacter)) {
                Debug.Log ("[MESSAGE]" + player.playerData.username + ": " + message);
            }
        }

        public static bool BlockMessage (string message) {

            if (ChatBlock == true) {

                if (ChatBlockWords.Any (message.ToLower ().Contains)) {
                    player.SendToSelf (Channel.Unsequenced, (byte) 10, "Please don't say a blocked word, the message has been blocked.");
                    Debug.Log (player.playerData.username + " Said a word that is blocked.");
                    return true;
                }
            }
            return false;

            if (System.IO.File.ReadAllText (adminlist).Contains (player.playerData.username)) {
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "Because you are staff, your message has NOT been blocked.");
                return false;
            } else {
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "--------------------------------------------------------------------------------------------");
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "             ?olo ingl�s! Tu mensaje ha sido bloqueado.");
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "             Only English! Your message has been blocked.");
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "--------------------------------------------------------------------------------------------");
                return true;
            }
        }

        public static void ClearChat (string message, bool self) {
            try {

                if (self) {
                    player.SendToSelf (Channel.Unsequenced, (byte) 10, "Clearing the chat for yourself...");
                    Thread.Sleep (250)
                    for (int i = 0; i < 6; i++) {
                        player.SendToSelf (Channel.Unsequenced, (byte) 10, " ");
                    }
                } else if (!(self)) {
                    if (System.IO.File.ReadAllText (AdminListFile).Contains (player.playerData.username)) {
                        SvPlayer svPlayer = (SvPlayer) player;
                        player.SendToAll (Channel.Unsequenced, (byte) 10, "Clearing chat for everyone...");
                        Thread.Sleep (250)
                        for (int i = 0; i < 6; i++) {
                            player.SendToAll (Channel.Unsequenced, (byte) 10, " ");
                        }

                    } else {
                        player.SendToSelf (Channel.Unsequenced, (byte) 10, msgNoPerm);
                    }
                } else {
                    player.SendToSelf (Channel.Unsequenced, (byte) 10, @"'" + arg1ClearChat + @"'" + " Is not a valid argument.");
                }
            } catch (Exception ex) {
                Debug.Log ("Something went wrong while trying to make a SubString: Expection: " + ex);
                Debug.Log ("Please Post the error on GitHub!");
                Debug.Log ("Try reinstalling the newest version.");
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "Unknown error occured. Please check output_log.txt for more info.");
            }

        }

        public static void Reload (string message) {
            if (System.IO.File.ReadAllText (AdminListFile).Contains (player.playerData.username)) {
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "Reloading config files...");
                ReadFile (SettingsFile);
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "[OK] Config file reloaded");
                Thread.Sleep (50);
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "Reloading language and chat block files..");
                ReadFile (LanguageBlockFile);
                Thread.Sleep (10);
                ReadFile (ChatBlockFile);
                player.SendToSelf (Channel.Unsequenced, (byte) 10, "[OK] Language and chat block files reloaded");
            } else {
                player.SendToSelf (Channel.Unsequenced, (byte) 10, msgNoPerm);
            }
        }

        public static void essentials (string message) {
            player.SendToSelf (Channel.Unsequenced, (byte) 10, "Essentials Created by UserR00T");
            player.SendToSelf (Channel.Unsequenced, (byte) 10, "Version " + version);

            //TODO: Subcommands like /essentials reload : executes cmdReload

        }
        public static void say (string message) {

        }

        [Hook ("SvPlayer.Initialize")]
        public static void Initialize (SvPlayer player) {
            if (player.playerData.username != null) {
                Thread thread = new Thread (new ParameterizedThreadStart (WriteIPToFile));
                thread.Start (player);
            }
        }
        private static void WriteIPToFile (object oPlayer) {
            Thread.Sleep (500);
            SvPlayer player = (SvPlayer) oPlayer;
            Debug.Log ("[INFO] " + "[JOIN] " + player.playerData.username + " IP is: " + player.netMan.GetAddress (player.connection));
            try {
                if (!File.ReadAllText (IPListFile).Contains (player.playerData.username + ": " + player.netMan.GetAddress (player.connection))) {
                    File.AppendAllText (IPListFile, player.playerData.username + ": " + player.netMan.GetAddress (player.connection) + Environment.NewLine);

                }
            } catch (Exception ex) {
                Debug.Log ("[ERROR] Unknown error occured, please send the following to UserR00T:" + ex);
            }

        }

        public static void DownloadFile (string SrvLoc, string LocalLoc) {
            WebClient client = new WebClient ();
            Stream stream = client.OpenRead (SrvLoc);
            StreamReader reader = new StreamReader (stream);
            String content = reader.ReadToEnd ();
            File.WriteAllText (LocalLoc, content);
        }

        public static void ReadFile (string FileName) {
            if (FileName == SettingsFile) {
                var lines = File.ReadAllLines (FileName);
                foreach (var line in lines) {
                    if (line.StartsWith ("#") || line.Contains ("#")) {
                        continue;
                    } else {
                        // TODO: make this better/compacter
                        if (line.Contains ("version: ")) {
                            version = line.Substring (9);
                        } else if (line.Contains ("CommandCharacter: ")) {
                            cmdCommandCharacter = line.Substring (17);
                        } else if (line.Contains ("noperm: ")) {
                            msgNoPerm = line.Substring (8);
                        } else if (line.Contains ("ClearChatCommand: ")) {
                            cmdClearChat = cmdCommandCharacter + line.Substring (18);
                        } else if (line.Contains ("ClearChatCommand2: ")) {
                            cmdClearChat2 = cmdCommandCharacter + line.Substring (19);
                        } else if (line.Contains ("UnknownCommand: ")) {
                            msgUnknownCommand = line.Substring (16);
                        } else if (line.Contains ("enableChatBlock: ")) {
                            ChatBlock = line.Substring (17);
                        } else if (line.Contains ("enableLanguageBlock: ")) {
                            LanguageBlock = line.Substring (21);
                        } else if (line.Contains ("ReloadCommand: ")) {
                            cmdReload = cmdCommandCharacter + line.Substring (15);
                        } else if (line.Contains ("ReloadCommand2: ")) {
                            cmdReload2 = cmdCommandCharacter + line.Substring (16);
                        }

                        //else if (line.Contains(""))
                        //{
                        //    = line.Substring();
                        //}
                    }
                }
            } else if (FileName == LanguageBlockFile) {
                LanguageBlockWords = System.IO.File.ReadAllLines (FileName);
            } else if (FileName == ChatBlockFile) {
                ChatBlockWords = System.IO.File.ReadAllLines (FileName);
            }
        }
    }