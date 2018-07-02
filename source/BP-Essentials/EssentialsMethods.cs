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
            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>Are you sure you want to sell your apartment? Type '</color><color={argColor}>{CmdCommandCharacter}{CmdConfirm}</color><color={warningColor}>' to confirm.</color>"); //softcode command
            return true;
        }

        [Hook("SvPlayer.Initialize")]
        public static void Initialize(SvPlayer player)
        {
            var shPlayer = player.player;
            if (!player.IsServerside())
            {
                new Thread(() => WriteIpToFile.Run(player)).Start();
                new Thread(() => CheckBanned.Run(player)).Start();
                new Thread(() => CheckAltAcc.Run(player)).Start();
                playerList.Add(shPlayer.ID, new _PlayerList { shplayer = shPlayer });
            }
        }

        [Hook("SvPlayer.Destroy")]
        public static void Destroy(SvPlayer player)
        {
            foreach (KeyValuePair<int, _PlayerList> item in playerList)
                if (item.Value.shplayer.svPlayer == player && !item.Value.shplayer.svPlayer.IsServerside())
                {
                    Debug.Log(SetTimeStamp.Run() + "[INFO] [LEAVE] " + item.Value.shplayer.username);
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
            var shPlayer = player.player;
            return EnableBlockSpawnBot == true && BlockedSpawnIds.Contains(shPlayer.spawnJobIndex);
        }

        [Hook("ShRestraint.HitEffect")]
        public static bool HitEffect(ShRestraint player, ref ShEntity hitTarget, ref ShPlayer source, ref Collider collider)
        {
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (!shPlayer.svPlayer.IsServerside())
                {
                    if (shPlayer != hitTarget) continue;
                    if (!GodListPlayers.Contains(shPlayer.username)) continue;
                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "<color=#b7b5b5>Being handcuffed Blocked!</color>");
                    return true;
                }
            return false;
        }

        [Hook("SvPlayer.SvBan")]
        public static bool SvBan(SvPlayer player, ref int otherID)
        {
            if (BlockBanButtonTabMenu)
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>This button has been disabled. Please use the ban commands.</color>");
                return true;
            }
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.ID == otherID)
                    if (!shPlayer.svPlayer.IsServerside() && !shPlayer.svPlayer.IsServerside())
                    {
                        LogMessage.LogOther($"{SetTimeStamp.Run()}[INFO] {shPlayer.username} Got banned by {player.playerData.username}");
                        player.SendToAll(Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{shPlayer.username}</color> <color={warningColor}>Just got banned by</color> <color={argColor}>{player.playerData.username}</color>");
                        SendDiscordMessage.BanMessage(shPlayer.username, player.playerData.username);
                    }
            return false;
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
                    foreach (KeyValuePair<int, _PlayerList> item in playerList)
                    {
                        if (item.Value.shplayer.svPlayer == player)
                        {
                            ShPlayer shPlayer = item.Value.shplayer;

                            #region Report
                            if (item.Value.LastMenu == CurrentMenu.Report && key > 1 && key < 11)
                            {
                                player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Reported \"</color><color={warningColor}>{item.Value.reportedPlayer.username}</color><color={infoColor}>\" With the reason \"</color><color={warningColor}>{ReportReasons[key - 2]}</color><color={infoColor}>\".</color>");
                                item.Value.reportedReason = ReportReasons[key - 2];
                                item.Value.LastMenu = CurrentMenu.Main;
                                ReportPlayer.Run(player.playerData.username, ReportReasons[key - 2], item.Value.reportedPlayer);
                                return true;
                            }
                            #endregion

                            switch (key)
                            {
                                case 1:
                                    if (HasPermission.Run(player, AccessMoneyMenu) || HasPermission.Run(player, AccessItemMenu) || HasPermission.Run(player, AccessSetHPMenu) || HasPermission.Run(player, AccessSetStatsMenu) || HasPermission.Run(player, AccessCWMenu))
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F3:</color> Server info menu\n<color=#00ffffff>F10:</color> Extras menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                    else
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Main menu:</color>\n\n<color=#00ffffff>F3:</color> Server info menu\n\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                    item.Value.LastMenu = CurrentMenu.Main;
                                    break;
                                case 2:
                                    if (item.Value.LastMenu == CurrentMenu.ServerInfo)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        player.SendToSelf(Channel.Fragmented, ClPacket.ServerInfo, File.ReadAllText("server_info.txt"));
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }
                                    if (item.Value.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessMoneyMenu))
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Give Money menu:</color>\n\n<color=#00ffffff>F2:</color> Give <color=#ea8220>1.000 dollars (1k)</color>\n<color=#00ffffff>F3:</color> Give <color=#ea8220>10.000 dollars (10k)</color>\n<color=#00ffffff>F4:</color> Give <color=#ea8220>100.000 dollars (100k)</color>\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                        item.Value.LastMenu = CurrentMenu.GiveMoney;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.GiveMoney)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferMoney(1, 1000, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 1.000 dollars.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 1.000 dollars through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferItem(1, CommonIDs[0], 500, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 500 pistol ammo.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 500 pistol ammo through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.AdminReport && shPlayer.admin)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        if (IsOnline.Run(item.Value.reportedPlayer))
                                        {
                                            shPlayer.SetPosition(item.Value.reportedPlayer.GetPosition());
                                            player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Teleported to \"</color><color=#ea8220>{item.Value.reportedPlayer.username}</color><color={infoColor}>\".</color>");
                                        }
                                        else
                                            player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, "<color=#ff0000ff>Player not online anymore.</color>");
                                        item.Value.reportedPlayer = null;
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }

                                    break;
                                case 3:
                                    if (item.Value.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessItemMenu))
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Give Items menu:</color>\n\n<color=#00ffffff>F2:</color> Give <color=#ea8220>500</color> Pistol Ammo\n<color=#00ffffff>F3:</color> Give <color=#ea8220>20</color> Handcuffs\n<color=#00ffffff>F4:</color> Give <color=#ea8220>10</color> Taser ammo\n<color=#00ffffff>F5:</color> Give <color=#ea8220>all</color> Licenses\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                        item.Value.LastMenu = CurrentMenu.GiveItems;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.GiveMoney)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferMoney(1, 10000, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 10.000 dollars.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 10.000 dollars through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                        return true;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferItem(1, CommonIDs[1], 20, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 20 handcuffs.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 20 handcuffs through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                        return true;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.AdminReport && shPlayer.admin)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        item.Value.LastMenu = CurrentMenu.Main;
                                        return true;
                                    }
                                    if (item.Value.LastMenu == CurrentMenu.Main)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, "<color=#00ffffff>Server info menu:</color>\n\n<color=#00ffffff>F2:</color> Show rules\n<color=#00ffffff>F3:</color> Show admins\n\n<color=#00ffffff>Press</color><color=#ea8220> F11 </color><color=#00ffffff>To close this (G)UI</color>");
                                        item.Value.LastMenu = CurrentMenu.ServerInfo;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.ServerInfo)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);

                                        var builder = new StringBuilder();
                                        builder.Append("All admins on this server:\n\n");
                                        foreach (var line in File.ReadAllLines("admin_list.txt"))
                                            if (line.Trim() != null && !line.Trim().StartsWith("#", StringComparison.OrdinalIgnoreCase))
                                                builder.Append(line + "\r\n");
                                        player.SendToSelf(Channel.Fragmented, ClPacket.ServerInfo, builder.ToString());
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }

                                    break;
                                case 4:
                                    if (item.Value.LastMenu == CurrentMenu.GiveMoney)
                                    {
                                        item.Value.shplayer.TransferMoney(1, 100000, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 100.000 dollars.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 100.000 dollars through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessSetHPMenu))
                                    {
                                        player.Heal(100);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You've been healed.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " healed himself through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                        shPlayer.TransferItem(1, CommonIDs[2], ClPacket.GameMessage, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself 10 Taser ammo.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in 10 taser ammo through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }
                                    player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                    break;
                                case 5:
                                    if (item.Value.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessSetStatsMenu))
                                    {
                                        for (byte i = 0; i < 4; i++)
                                            player.UpdateStat(i, 100);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Maxed out stats for yourself.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Maxed out stats through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }
                                    else if (item.Value.LastMenu == CurrentMenu.GiveItems)
                                    {
                                        for (int i = 3; i < 7; i++)
                                            shPlayer.TransferItem(1, CommonIDs[i], 1, true);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>You have given yourself all licenses.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Spawned in all licenses through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }

                                    player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                    break;
                                case 6:
                                    if (item.Value.LastMenu == CurrentMenu.Staff && HasPermission.Run(player, AccessCWMenu))
                                    {
                                        shPlayer.ClearCrimes();
                                        player.SendToSelf(Channel.Reliable, 33, shPlayer.ID);
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={infoColor}>Cleared wanted level.</color>");
                                        Debug.Log(SetTimeStamp.Run() + "[INFO] " + player.playerData.username + " Removed his wantedlevel through the functionUI");
                                        item.Value.LastMenu = CurrentMenu.Main;
                                    }

                                    player.SendToSelf(Channel.Reliable, ClPacket.CloseFunctionMenu);
                                    break;
                                case 10:
                                    if (item.Value.LastMenu == CurrentMenu.Main)
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
                                        item.Value.LastMenu = CurrentMenu.Staff;
                                    }
                                    break;
                            }
                        }
                    }
                    return true;
                }
                foreach (KeyValuePair<int, _PlayerList> item in playerList)
                    if (item.Value.shplayer.svPlayer == player)
                        item.Value.LastMenu = CurrentMenu.Main;
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

            var shPlayer = player.player;

            if (shPlayer.IsDead())
                return true;

            shPlayer.ShDie();
            player.SendToLocal(Channel.Reliable, ClPacket.UpdateHealth, shPlayer.ID, shPlayer.health);
            return true;
        }
        [Hook("SvPlayer.SvGetJob")]
        public static bool SvGetJob(SvPlayer player, ref int employerID)
        {
            try
            {
                var shPlayer = player.player;
                var shEmployer = shPlayer.manager.FindByID<ShPlayer>(employerID);
                if (WhitelistedJobs.ContainsKey(shEmployer.job.jobIndex))
                    if (!HasPermission.Run(player, WhitelistedJobs[shEmployer.job.jobIndex], false, shPlayer.job.jobIndex))
                    {
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPermJob);
                        return true;
                    }
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
                return false;
            }
        }
        [Hook("SvPlayer.SvAddCrime")]
        public static bool SvAddCrime(SvPlayer player, ref byte crimeIndex, ref ShEntity victim)
        {
            try
            {
                if (GodModeLevel >= 1 && CheckGodMode.Run(player, null, "<color=#b7b5b5>Blocked crime and losing EXP!</color>"))
                    return true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return false;
        }
        [Hook("ShPlayer.TransferItem")]
        public static bool TransferItem(ShPlayer player, ref byte deltaType, ref int itemIndex, ref int amount, ref bool dispatch)
        {
            try
            {
                if (player != null && BlockedItems.Count > 0  && BlockedItems.Contains(itemIndex))
                {
                    player.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, BlockedItemMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return false;
        }

        [Hook("ShPlayer.ShDie")]
        public static bool ShDie(ShPlayer player)
        {
            if (player.manager.isClient)
            {
                if (player.clPlayer.isMain)
                {
                    foreach (PlayerEffect playerEffect in player.effects)
                        playerEffect.active = false;
                    player.manager.mainCamera.ClearEffects();
                    player.clPlayer.deathSound.Play();
                }
            }
            else
            {
                player.svPlayer.ClearWitnessed();
                foreach (PlayerEffect playerEffect2 in player.effects)
                    playerEffect2.active = false;
                if (player.svPlayer.IsServerside())
                    player.svPlayer.SetState(0);
                player.svPlayer.SendToSelf(Channel.Reliable, 45, new object[] {player.svPlayer.respawnDelay});
                if (Physics.Raycast(player.GetPosition() + Vector3.up, Vector3.down, out RaycastHit raycastHit, 10f, 1))
                {
                    var shEntity = player.manager.svManager.AddNewEntity(player.manager.svManager.GetRandomBriefcase(), player.GetPlace(), raycastHit.point, player.GetRotationT().rotation, false);
                    foreach (var keyValuePair in player.myItems)
                    {
                        if (UnityEngine.Random.value < 0.8f)
                        {
                            InventoryItem value = new InventoryItem(keyValuePair.Value.item, Mathf.CeilToInt(keyValuePair.Value.count * UnityEngine.Random.Range(0.05f, 0.3f)), 0);
                            shEntity.myItems.Add(keyValuePair.Key, value);
                        }
                    }
                }
                foreach (InventoryItem inventoryItem in player.myItems.Values.ToArray())
                {
                    bool flag = false;
                    int num = inventoryItem.count;
                    if (player.job.info.rankItems.Length > player.rank)
                    {
                        for (int l = player.rank; l >= 0; l--)
                        {
                            foreach (InventoryItem inventoryItem2 in player.job.info.rankItems[l].items)
                                if (inventoryItem.item.index == inventoryItem2.item.index)
                                {
                                    num = Mathf.Max(0, inventoryItem.count - inventoryItem2.count);
                                    flag = true;
                                    break;
                                }
                            if (flag)
                                break;
                        }
                    }
                    if (num > 0)
                    {
                        var shWearable = inventoryItem.item as ShWearable;
                        if (!shWearable || shWearable.illegal || player.curWearables[(int)shWearable.type].index != shWearable.index || (blockLicenseRemoved && !shWearable.name.StartsWith("License")))
                            player.TransferItem(2, inventoryItem.item.index, num, true);
                    }
                }
                if (blockLicenseRemoved)
                    player.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>This server disabled losing licenses on death.</color>");
            }

            player.CleanUp();
            player.health = 0f;
            if (player.manager.isServer)
                SvMan.StartCoroutine(player.svMovable.RespawnDelay());

            player.SetSimpleLayer(true);
            if (player.manager.isClient)
            {
                player.clPlayer.ClSetStance(5);
                if (player.clPlayer.isMain)
                    player.clPlayer.mainCamera.ResetCamera();
            }
            return true;
        }
    }
}