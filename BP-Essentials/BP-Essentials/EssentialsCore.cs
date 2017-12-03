using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsConfig;
using static BP_Essentials.EssentialsCore;
using static BP_Essentials.EssentialsChat;
using static BP_Essentials.EssentialsCmd;
using static BP_Essentials.EssentialsMethods;
namespace BP_Essentials {
    public static class EssentialsCore {
        
        //Initalization
        [Hook("SvNetMan.StartServerNetwork")]
        public static void StartServerNetwork(SvNetMan netMan)
        {

            try
            {
                Reload(true);

                if (EssentialsConfig.Version != LocalVersion)
                {
                    Debug.Log("[ERROR] Essentials - Versions do not match!");
                    Debug.Log("[ERROR] Essentials - Essentials version:" + EssentialsConfig.Version);
                    Debug.Log("[ERROR] Essentials - Settings file version" + LocalVersion);
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
                Debug.Log("[INFO] Essentials - version: " + LocalVersion + " Loaded in successfully!");
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

            if (Announcements.Length != 0)
            {
                var thread = new Thread(new ParameterizedThreadStart(AnnounceThread));
                thread.Start(netMan);
                Debug.Log(SetTimeStamp() + "[INFO] Announcer started successfully!");
            }
            else
                Debug.Log(SetTimeStamp() + "[WARNING] No announcements found in the file!");

        }

        //Reload
        public static void Reload(bool silentExecution, string message = null, object oPlayer = null)
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
                    player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
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
        
        
        
        //Saving
        public static void SavePeriodically()
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
        public static void SaveNow()
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
        
        //Logging
        public static void GetLogs(object oPlayer, string logFile)
        {
            var player = (SvPlayer)oPlayer;
            if (logFile != ChatLogFile) return;
            string content = null;
            using (var reader = new StreamReader(logFile))
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
        
        //Files
        public static void RemoveStringFromFile(string FileName, string RemoveString)
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
        
        //Timestamps
        public static string SetTimeStamp()
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
    }
}