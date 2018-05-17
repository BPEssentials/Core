using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace BP_Essentials
{
    public class EssentialsMethodsPlugin : EssentialsCorePlugin
    {

        [Hook("SvPlayer.SvSellApartment")]
        public static bool SvSellApartment(SvPlayer player)
        {
            if (Confirmed)
            {
                Confirmed = false;
                return false;
            }
            Confirmed = false;
            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>Are you sure you want to sell your apartment? Type '</color><color={argColor}>{CmdConfirm}</color><color={warningColor}>' to confirm.</color>");
            return true;
        }

        [Hook("SvPlayer.Initialize")]
        public static void Initialize(SvPlayer player)
        {
            ShPlayer shPlayer = (ShPlayer)typeof(SvPlayer).GetField(nameof(player), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(player);

            if (shPlayer.IsRealPlayer())
            {
                var thread1 = new Thread(new ParameterizedThreadStart(WriteIpToFile.Run));
                thread1.Start(player);
                var thread2 = new Thread(new ParameterizedThreadStart(CheckBanned.Run));
                thread2.Start(player);
                var thread3 = new Thread(new ParameterizedThreadStart(CheckAltAcc.Run));
                thread3.Start(player);
                playerList.Add(new _PlayerList { shplayer = shPlayer }, shPlayer.ID);
            }
        }

        [Hook("SvPlayer.Destroy")]
        public static void Destroy(SvPlayer player)
        {
            foreach (KeyValuePair<_PlayerList, int> item in playerList)
                if (item.Key.shplayer.svPlayer == player && item.Key.shplayer.IsRealPlayer())
                {
                    Debug.Log(SetTimeStamp.Run() + "[INFO] [LEAVE] " + item.Key.shplayer.username);
                    playerList.Remove(item.Key);
                    break;
                }
        }

        [Hook("SvPlayer.Damage")]
        public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider)
        {
            return CheckGodMode.Run(player, amount);
        }

        [Hook("SvPlayer.SpawnBot")]
        public static bool SpawnBot(SvPlayer player, ref Vector3 position, ref Quaternion rotation, ref Place place, ref WaypointNode node, ref ShPlayer spawner, ref ShTransport transport, ref ShPlayer enemy)
        {
            ShPlayer shPlayer = (ShPlayer)typeof(SvPlayer).GetField(nameof(player), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(player);
            return EnableBlockSpawnBot == true && BlockedSpawnIds.Contains(shPlayer.spawnJobIndex);
        }

        [Hook("ShRetainer.HitEffect")]
        public static bool HitEffect(ShRetainer player, ref ShEntity hitTarget, ref ShPlayer source, ref Collider collider)
        {
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.IsRealPlayer())
                {
                    if (shPlayer != hitTarget) continue;
                    if (!GodListPlayers.Contains(shPlayer.username)) continue;
                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "<color=#b7b5b5>Being handcuffed Blocked!</color>");
                    return true;
                }
            return false;
        }

        [Hook("SvPlayer.SvBan")]
        public static void SvBan(SvPlayer player, ref int otherID)
        {
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.ID == otherID)
                    if (shPlayer.IsRealPlayer() && !shPlayer.svPlayer.IsServerside())
                    {
                        Debug.Log($"{SetTimeStamp.Run()}[INFO] {shPlayer.username} Got banned by {player.playerData.username}");
                        player.SendToAll(Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{shPlayer.username}</color> <color={warningColor}>Just got banned by</color> <color={argColor}>{player.playerData.username}</color>");
                    }
        }

        [Hook("SvPlayer.SvStartVote")]
        public static bool SvStartVote(SvPlayer player, ref byte voteIndex, ref int ID)
        {
            if (voteIndex == 1)
            {
                if (!VoteKickDisabled)
                {
                    foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                        if (shPlayer.ID == ID)
                            foreach (var shIssuer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                if (shIssuer.svPlayer == player)
                                {
                                    if (player.svManager.vote != null || voteIndex >= shIssuer.manager.votes.Length || player.svManager.startedVote.Contains(shIssuer))
                                        return true;
                                    player.svManager.startedVote.Add(shIssuer);
                                    player.svManager.vote = shIssuer.manager.votes[voteIndex];
                                    if (player.svManager.vote.CheckVote(ID))
                                    {
                                        player.SendToAll(Channel.Reliable, 60, voteIndex, ID);
                                        player.svManager.StartCoroutine(player.svManager.StartVote());
                                        Debug.Log($"{SetTimeStamp.Run()}[INFO] {player.playerData.username} Has issued a votekick against {shPlayer.username}");
                                        player.SendToAll(Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{player.playerData.username} </color><color={warningColor}>Has issued a vote kick against</color><color={argColor}> {shPlayer.username}</color>");
                                        LatestVotePeople.Clear();
                                    }
                                    else
                                        player.svManager.vote = null;
                                }
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Vote kicking has been disabled on this server.</color>");
                return true;
            }
            return false;
        }

        [Hook("SvPlayer.SvVoteYes", true)]
        public static void SvVoteYes(SvPlayer player)
        {
            LatestVotePeople.Add(player.playerData.username);
        }

        [Hook("SvPlayer.SvFunctionKey")]
        public static bool SvFunctionKey(SvPlayer player, ref byte key)
        {
            try
            {
                if (key < 11)
                {
                    foreach (KeyValuePair<_PlayerList, int> item in playerList)
                    {
                        if (item.Key.shplayer.svPlayer == player)
                        {
                            ShPlayer shPlayer = item.Key.shplayer;

                            #region Report
                            if (item.Key.LastMenu == CurrentMenu.Report && key > 1 && key < 11)
                            {
                                player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Reported \"</color><color={warningColor}>{item.Key.reportedPlayer.username}</color><color={infoColor}>\" With the reason \"</color><color={warningColor}>{ReportReasons[key - 2]}</color><color={infoColor}>\".</color>");
                                item.Key.reportedReason = ReportReasons[key - 2];
                                item.Key.LastMenu = CurrentMenu.Main;
                                ReportPlayer.Run(player.playerData.username, ReportReasons[key - 2], item.Key.reportedPlayer);
                                return true;
                            }
                            #endregion

                            switch (key)
                            {
                                case 1:
                                    if (HasPermission.Run(player, AccessMoneyMenu) || HasPermission.Run(player, AccessItemMenu) || HasPermission.Run(player, AccessSetHPMenu) || HasPermission.Run(player, AccessSetStatsMenu) || HasPermission.Run(player, AccessCWMenu))
                                        //player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F2:</color> Help menu\n<color=#00ffffff>F3:</color> Server info menu\n<color=#00ffffff>F10:</color> Staff menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F3:</color> Server info menu\n<color=#00ffffff>F10:</color> Extras menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                    else
                                        //player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F2:</color> Help menu\n<color=#00ffffff>F3:</color> Server info menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F3:</color> Server info menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                    item.Key.LastMenu = CurrentMenu.Main;
                                    break;
                                case 2:
                                    //if (item.Key.LastMenu == CurrentMenu.Main)
                                    //{
                                    //    player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>(example) Help menu:</color>\n\n<color=#00ffffff>F2:</color> Getting started\n<color=#00ffffff>F3:</color> How to earn money\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                    //    item.Key.LastMenu = CurrentMenu.Help;
                                    //}
                                    //else
                                    if (item.Key.LastMenu == CurrentMenu.ServerInfo)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        player.SendToSelf(Channel.Fragmented, ClPacket.ServerInfo, File.ReadAllText("server_info.txt"));
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }
                                    if (item.Key.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessMoneyMenu))
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Give Money menu:</color>\n\n<color=#00ffffff>F2:</color> Give <color=#ea8220>1.000 dollars (1k)</color>\n<color=#00ffffff>F3:</color> Give <color=#ea8220>10.000 dollars (10k)</color>\n<color=#00ffffff>F4:</color> Give <color=#ea8220>100.000 dollars (100k)</color>\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                        item.Key.LastMenu = CurrentMenu.GiveMoney;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.GiveMoney)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferMoney(1, 1000, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 1.000 dollars.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 1.000 dollars through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferItem(1, CommonIDs[0], 500, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 500 pistol ammo.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 500 pistol ammo through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.AdminReport && shPlayer.admin)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        if (IsOnline.Run(item.Key.reportedPlayer))
                                        {
                                            shPlayer.SetPosition(item.Key.reportedPlayer.GetPosition());
                                            player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Teleported to \"</color><color=#ea8220>{item.Key.reportedPlayer.username}</color><color={infoColor}>\".</color>");
                                        }
                                        else
                                            player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, "<color=#ff0000ff>Player not online anymore.</color>");
                                        item.Key.reportedPlayer = null;
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }

                                    break;
                                case 3:
                                    if (item.Key.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessItemMenu))
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Give Items menu:</color>\n\n<color=#00ffffff>F2:</color> Give <color=#ea8220>500</color> Pistol Ammo\n<color=#00ffffff>F3:</color> Give <color=#ea8220>20</color> Handcuffs\n<color=#00ffffff>F4:</color> Give <color=#ea8220>10</color> Taser ammo\n<color=#00ffffff>F5:</color> Give <color=#ea8220>all</color> Licenses\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                        item.Key.LastMenu = CurrentMenu.GiveItems;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.GiveMoney)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferMoney(1, 10000, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 10.000 dollars.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 10.000 dollars through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                        return true;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferItem(1, CommonIDs[1], 20, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 20 handcuffs.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 20 handcuffs through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                        return true;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.AdminReport && shPlayer.admin)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        item.Key.LastMenu = CurrentMenu.Main;
                                        return true;
                                    }
                                    if (item.Key.LastMenu == CurrentMenu.Main)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Server info menu:</color>\n\n<color=#00ffffff>F2:</color> Show rules\n<color=#00ffffff>F3:</color> Show admins\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                        item.Key.LastMenu = CurrentMenu.ServerInfo;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.ServerInfo)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);

                                        var builder = new StringBuilder();
                                        builder.Append("All admins on this server:\n\n");
                                        foreach (var line in File.ReadAllLines("admin_list.txt"))
                                            if (line.Trim() != null && !line.Trim().StartsWith("#", StringComparison.OrdinalIgnoreCase))
                                                builder.Append(line + "\r\n");
                                        player.SendToSelf(Channel.Fragmented, ClPacket.ServerInfo, builder.ToString());
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }

                                    break;
                                case 4:
                                    if (item.Key.LastMenu == CurrentMenu.GiveMoney)
                                    {
                                        item.Key.shplayer.TransferMoney(1, 100000, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 100.000 dollars.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 100.000 dollars through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessSetHPMenu))
                                    {
                                        player.Heal(100);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You've been healed.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " healed himself through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferItem(1, CommonIDs[2], ClPacket.GameMessage, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 10 Taser ammo.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 10 taser ammo through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }
                                    player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                    break;
                                case 5:
                                    if (item.Key.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessSetStatsMenu))
                                    {
                                        for (byte i = 0; i < 4; i++)
                                            player.UpdateStat(i, 100);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Maxed out stats for yourself.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Maxed out stats through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Key.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        for (int i = 3; i < 7; i++)
                                            shPlayer.TransferItem(1, CommonIDs[i], 1, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself all licenses.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in all licenses through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }

                                    player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                    break;
                                case 6:
                                    if (item.Key.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessCWMenu))
                                    {
                                        shPlayer.ClearCrimes();
                                        player.SendToSelf(Channel.Reliable, 33, shPlayer.ID);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Cleared wanted level.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Removed his wantedlevel through the functionUI");
                                        item.Key.LastMenu = CurrentMenu.Main;
                                    }

                                    player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                    break;
                                case 10:
                                    if (item.Key.LastMenu == CurrentMenu.Main)
                                    {
                                        var sb = new StringBuilder().Append("<color=#00ffffff>Staff menu:</color>\n\n");

                                        if (HasPermission.Run(player, AccessMoneyMenu))
                                            sb.Append("<color=#00ffffff>F2:</color> Give Money\n");
                                        if (HasPermission.Run(player, AccessItemMenu))
                                            sb.Append("<color=#00ffffff>F3:</color> Give Items\n");
                                        if (HasPermission.Run(player, AccessSetHPMenu))
                                            sb.Append("<color=#00ffffff>F4:</color> Set HP to full\n");
                                        if (HasPermission.Run(player, AccessSetStatsMenu))
                                            sb.Append("<color=#00ffffff>F5:</color> Set Stats to full\n");
                                        if (HasPermission.Run(player, AccessCWMenu))
                                            sb.Append("<color=#00ffffff>F6:</color> Clear wanted level\n\n");
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, $"{sb}<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                        item.Key.LastMenu = CurrentMenu.Staff;
                                    }
                                    break;
                            }
                        }
                    }
                    return true;
                }
                foreach (KeyValuePair<_PlayerList, int> item in playerList)
                    if (item.Key.shplayer.svPlayer == player)
                        item.Key.LastMenu = CurrentMenu.Main;
                player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }

        [Hook("SvPlayer.SvSuicide")]
        public static bool SvSuicide(SvPlayer player)
        {
            if (player.IsServerside())
                return true;

            var shPlayer = GetShBySv.Run(player);

            if (shPlayer.IsDead())
                return true;

            shPlayer.ShDie();
            player.SendToLocalAndSelf(Channel.Reliable, ClPacket.UpdateHealth, shPlayer.ID, shPlayer.health);
            return true;
        }
    }
}