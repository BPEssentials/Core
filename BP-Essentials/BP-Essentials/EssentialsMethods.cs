using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsConfigPlugin;
using static BP_Essentials.EssentialsCmdPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
using System.Text.RegularExpressions;
namespace BP_Essentials
{
    public class EssentialsMethodsPlugin : EssentialsCorePlugin{


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

        [Hook("SvPlayer.Initialize")]
        public static void Initialize(SvPlayer player)
        {
            if (player.playerData.username == null) return;
            var thread1 = new Thread(new ParameterizedThreadStart(CheckBanned));
            thread1.Start(player);
            var thread2 = new Thread(new ParameterizedThreadStart(WriteIpToFile));
            thread2.Start(player);
            var thread3 = new Thread(new ParameterizedThreadStart(CheckAltAcc));
            thread3.Start(player);
        }

        [Hook("SvPlayer.Damage")]
        public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider) {
            return CheckGodmode(player, amount);
        }

        [Hook("SvPlayer.SpawnBot")]
        public static bool SpawnBot(SvPlayer player, ref Vector3 position, ref Quaternion rotation, ref WaypointNode node, ref ShTransport vehicle, ref byte spawnJobIndex) {
            return EnableBlockSpawnBot == true && BlockedSpawnIds.Contains(spawnJobIndex);
        }

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



        public static void ExecuteOnPlayer(object oPlayer, string message, string arg1)
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
                                if (message.StartsWith(CmdTpHere) || message.StartsWith("/tphere")) // CHANGE THIS TO SOFTCODED ONE
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
                player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
            }
        }

        public static void MessageLog(string message, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            if (!message.StartsWith(CmdCommandCharacter))
            {
                var mssge = SetTimeStamp() + "[MESSAGE] " + player.playerData.username + ": " + message;
                Debug.Log(mssge);
                File.AppendAllText(ChatLogFile, mssge + Environment.NewLine);
                File.AppendAllText(LogFile, mssge + Environment.NewLine);
            }
            else if (message.StartsWith(CmdCommandCharacter))
            {
                var mssge = (SetTimeStamp() + "[COMMAND] " + player.playerData.username + ": " + message);
                Debug.Log(mssge);
                File.AppendAllText(CommandLogFile, mssge + Environment.NewLine);
                File.AppendAllText(LogFile, mssge + Environment.NewLine);
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

        private static void WriteIpToFile(object oPlayer)
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

        public static void AnnounceThread(object man)
        {
            var netMan = (SvNetMan)man;
            while (true)
            {
                foreach (var player in netMan.players)
                {
                    player.svPlayer.SendToSelf(Channel.Reliable, ClPacket.GameMessage, Announcements[AnnounceIndex]);
                }
                Debug.Log(SetTimeStamp() + "[INFO] Announcement made...");

                AnnounceIndex += 1;
                if (AnnounceIndex > Announcements.Length - 1)
                    AnnounceIndex = 0;
                Thread.Sleep(TimeBetweenAnnounce * 1000);
            }
        }

        public static string GetArgument(int nr, bool UseRegex, bool IncludeSpaces, string message)
        {
            if (UseRegex)
            {
                var args = Regex.Matches(message, @"[\""].+?[\""]|[^ ]+")
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .ToList();

                return args[nr];
            }
            else
            {
                if (IncludeSpaces)
                {
                    string tmessage = message + " ";
                    string[] args = tmessage.Split(' ');
                    return tmessage.Substring(tmessage.IndexOf(args[nr]));
                }
                else
                {
                    string tmessage = message + " ";
                    string[] args = tmessage.Split(' ');
                    return args[nr];
                }
            }
        }
    }
}