using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsConfigPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
namespace BP_Essentials
{
    public class EssentialsCmdPlugin : EssentialsCorePlugin{
        public static void DebugCommands(string message, object oPlayer)
        {
            // TODO: Add Commands like
            // - Ram usage and CPU usage on PC as well as date and other useful information about it
            var player = (SvPlayer)oPlayer;
            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                if (shPlayer.svPlayer == player)
                    if (shPlayer.IsRealPlayer())
                    {
                        string arg = GetArgument(1, false, false, message).Trim().ToLower();
                        if (arg == "location" || arg == "getlocation")
                            player.SendToSelf(Channel.Unsequenced, (byte)10, "Your location: " + shPlayer.GetPosition());
                        else if (arg == "getplayerhash" || arg == "gethash")
                        {
                            string TempMSG = GetArgument(0, false, false, message).Trim().ToLower() + arg;
                            string arg2 = TempMSG.Substring(TempMSG.Length + 1);
                            if (!String.IsNullOrWhiteSpace(arg2))
                            {
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Hash of " + arg2 + " :" + Animator.StringToHash(arg2).ToString());
                            }
                            else
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Invalid arguments. /debug get(player)hash [username]");
                        }
                        else if (arg == "spaceindex" || arg == "getspaceindex")
                        {
                            string TempMSG = GetArgument(0, false, false, message).Trim().ToLower() + arg;
                            string arg2 = TempMSG.Substring(TempMSG.Length + 1);
                            if (!String.IsNullOrWhiteSpace(arg2))
                            {
                                bool found = false;
                                foreach (var shPlayer2 in GameObject.FindObjectsOfType<ShPlayer>())
                                    if (shPlayer2.svPlayer.playerData.username == arg2 || shPlayer2.ID.ToString() == arg2)
                                        if (shPlayer2.IsRealPlayer())
                                        {
                                            found = true;
                                            player.SendToSelf(Channel.Unsequenced, (byte)10, "SpaceIndex of '" + shPlayer2.svPlayer.playerData.username + "': " + shPlayer2.GetSpaceIndex());
                                        }
                                if (!found)
                                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Invalid arguments (Or user is not found online.) /debug (get)spaceindex [username] ");
                            }
                            else
                                player.SendToSelf(Channel.Unsequenced, (byte)10, "Your SpaceIndex: " + shPlayer.GetSpaceIndex());
                        }
                    }
        }

        public static void Afk(string message, object oPlayer)
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




        public static void ClearChat(string message, object oPlayer, bool all)
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
                        player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);

            }

        }

        public static bool Mute(string message, object oPlayer, bool unmute)
        {
            var player = (SvPlayer)oPlayer;
            string muteuser = null;
            var found = false;
            if (message.StartsWith(CmdMute))
                muteuser = message.Substring(CmdMute.Count() + 1);
            else if (message.StartsWith(CmdUnMute))
                muteuser = message.Substring(CmdUnMute.Count() + 1);

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
                player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                return false;
            }
            return false;
        }

        public static bool Say(string message, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            try
            {

                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    if ((message.Length == CmdSay.Length) || (message.Length == CmdSay2.Length))
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "An argument is required for this command.");
                        return true;
                    }
                    else
                    {
                        var arg1 = "blank";
                        if (message.StartsWith(CmdSay))
                        {
                            arg1 = message.Substring(CmdSay.Length);
                        }
                        else if (message.StartsWith(CmdSay2))
                        {
                            arg1 = message.Substring(CmdSay2.Length);
                        }
                        player.SendToAll(Channel.Unsequenced, (byte)10, MsgSayPrefix + " " + player.playerData.username + ": " + arg1);
                        return true;
                    }
                }
                else
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorLogging(ex);

                return true;
            }
        }

        //GodMode
        public static bool GodMode(string message, object oPlayer)
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
                    player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
                    return false;
                }

            }
            catch (Exception ex)
            {

                ErrorLogging(ex);

                return true;
            }

        }
        public static bool CheckGodmode(object oPlayer, float amount)
        {
            var player = (SvPlayer)oPlayer;
            if (GodListPlayers.Contains(player.playerData.username))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, amount + " DMG Blocked!");
                return true;
            }
            return false;
        }


        public static void Essentials(string message, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            player.SendToSelf(Channel.Unsequenced, (byte)10, "Essentials Created by UserR00T & DeathByKorea & BP");
            player.SendToSelf(Channel.Unsequenced, (byte)10, "Version " + EssentialsConfigPlugin.Version);
        }

        //Player Checking
        public static void CheckIp(string message, object oPlayer, string arg1)
        {
            var player = (SvPlayer)oPlayer;
            var found = 0;
            //  player.SendToSelf(Channel.Unsequenced, (byte)10, "IP for player: " + arg1);
            var content = string.Empty;

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
        public static void CheckPlayer(string message, object oPlayer, string arg1)
        {
            var player = (SvPlayer)oPlayer;
            var found = 0;
            player.SendToSelf(Channel.Unsequenced, (byte)10, "Accounts for IP: " + arg1);
            foreach (var line in File.ReadAllLines(IpListFile))
            {
                if (line.Contains(arg1) || line.Contains(" " + arg1))
                {
                    ++found;
                    var arg1temp = line.Replace(": " + arg1, " ");
                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1temp);
                }
            }
            player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " occurred " + found + " times in the iplog file.");
        }
        public static void GetPlayerInfo(object oPlayer, string arg1)
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


        public static string GetPlaceHolders(int i, object oPlayer)
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

        public static bool BlockMessage(string message, object oPlayer)
        {
            message = message.ToLower();
            var player = (SvPlayer)oPlayer;
            if (ChatBlock)
            {
                if (ChatBlockWords.Contains(message))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Please don't say a blacklisted word, the message has been blocked.");
                    Debug.Log(SetTimeStamp() + player.playerData.username + " Said a word that is blocked.");
                    return true;
                }
            }
            if (LanguageBlock)
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

    }
}