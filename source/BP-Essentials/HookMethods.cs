using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using static BP_Essentials.Variables;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using UniversalUnityHooks;

namespace BP_Essentials
{
    public class HookMethods : Core
    {
        [Hook("SvPlayer.SvSellApartment")]
        public static bool SvSellApartment(SvPlayer player)
        {
            player.SendChatMessage($"<color={warningColor}>Are you sure you want to sell your apartment? Type '</color><color={argColor}>{CmdCommandCharacter}{CmdConfirm}</color><color={warningColor}>' to confirm.</color>"); //softcode command
            return true;
        }

        [Hook("SvEntity.Initialize", true)]
        public static void Initialize(SvEntity player)
        {
            if (player.serverside)
                return;
            var svPlayer = player as SvPlayer;
            if (svPlayer == null)
                return;
            PlayerList.Add(svPlayer.player.ID, new PlayerListItem(svPlayer.player));
            Task.Run(() =>
            {
                WriteIpToFile.Run(svPlayer);
                CheckBanned.Run(svPlayer);
            });
        }

        [Hook("SvPlayer.Destroy")]
        public static void Destroy(SvPlayer player)
        {
            Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] [LEAVE] {player.player.username}");
            if (!PlayerList.ContainsKey(player.player.ID))
                return;
            PlayerList.Remove(player.player.ID);
        }

        [Hook("SvPlayer.Damage")]
        public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider)
        {
            if (CheckGodMode.Run(player, amount))
                return true;
            if (player.WillDieByDamage(amount))
                OnDeath(player, ref type, ref amount, ref attacker, ref collider);
            return false;
        }
        public static void OnDeath(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider)
        {
            player.SavePosition();
        }

        [Hook("SvPlayer.SpawnBot")]
        public static bool SpawnBot(SvPlayer player,
            ref Vector3 position,
            ref Quaternion rotation,
            ref Place place,
            ref Waypoint node,
            ref ShPlayer spawner,
            ref ShMountable mount,
            ref ShPlayer enemy)
        {
            return EnableBlockSpawnBot == true && BlockedSpawnIds.Contains(player.player.spawnJobIndex);
        }

        [Hook("ShRestraint.HitEffect")]
        public static bool HitEffect(ShRestraint restraint, ref ShDestroyable hitTarget, ref ShPlayer source, ref Collider collider)
        {
            var shPlayer = hitTarget as ShPlayer;
            if (!shPlayer || shPlayer.IsDead())
                return true;

            if (!PlayerList.TryGetValue(hitTarget.ID, out var hitPlayer))
                return false;
            if (!GodListPlayers.Contains(hitPlayer.ShPlayer.username))
                return false;
            hitPlayer.ShPlayer.svPlayer.SendChatMessage("<color=#b7b5b5>Being handcuffed Blocked!</color>");
            return true;
        }

        [Hook("SvPlayer.SvBan")]
        public static bool SvBan(SvPlayer player, ref int otherID)
        {
            if (player.player.admin)
                return true;
            if (BlockBanButtonTabMenu)
            {
                player.SendChatMessage($"<color={errorColor}>This button has been disabled. Please use the ban commands. ID: {otherID}</color>");
                return true;
            }
            var targetPlayer = player.entity.manager.FindByID<ShPlayer>(otherID);
            if (!targetPlayer || targetPlayer.svPlayer.serverside)
            {
                player.SendChatMessage("Cannot ban NPC");
                return true;
            }
            LogMessage.LogOther($"{PlaceholderParser.ParseTimeStamp()} [INFO] {targetPlayer.username} Got banned by {player.playerData.username}");
            player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{targetPlayer.username}</color> <color={warningColor}>Just got banned by</color> <color={argColor}>{player.player.username}</color>");
            SendDiscordMessage.BanMessage(targetPlayer.username, player.playerData.username);
            player.svManager.AddBanned(targetPlayer);
            player.svManager.Disconnect(targetPlayer.svPlayer.connection, DisconnectTypes.Banned);
            return true;
        }

        [Hook("SvPlayer.SvStartVote")]
        public static bool SvStartVote(SvPlayer player, ref byte voteIndex, ref int ID)
        {
            switch (voteIndex)
            {
                case VoteIndex.Mission:
                    if (!BlockMissions)
                        return false;
                    player.SendChatMessage($"<color={errorColor}>All missions have been disabled on this server.</color>");
                    return true;
                case VoteIndex.Kick:
                    if (VoteKickDisabled)
                    {
                        player.SendChatMessage($"<color={errorColor}>Vote kicking has been disabled on this server.</color>");
                        return true;
                    }
                    if (!PlayerList.TryGetValue(ID, out var shPlayer))
                        return true;
                    if (player.svManager.vote != null || voteIndex >= player.player.manager.votes.Length || player.svManager.startedVote.OverLimit(player.player))
                        return true;
                    player.svManager.startedVote.Add(player.player);
                    player.svManager.vote = player.player.manager.votes[voteIndex];
                    if (!player.svManager.vote.CheckVote(ID))
                        player.svManager.vote = null;
                    player.Send(SvSendType.All, Channel.Reliable, 60, voteIndex, ID);
                    player.svManager.StartCoroutine(player.svManager.StartVote());
                    Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.playerData.username} Has issued a votekick against {player.player.username}");
                    player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{player.playerData.username} </color><color={warningColor}>Has issued a vote kick against</color><color={argColor}> {shPlayer.ShPlayer.username}</color>");
                    LatestVotePeople.Clear();
                    return true;
                default:
                    return false;
            }
        }

        [Hook("SvPlayer.SvVoteYes", true)]
        public static void SvVoteYes(SvPlayer player)
        {
            LatestVotePeople.Add(player.playerData.username);
        }

        [Hook("SvPlayer.SvFunctionKey")]
        public static bool SvFunctionKey(SvPlayer player, ref byte key)
        {
            var lastMenu = PlayerList[player.player.ID].LastMenu;
            if (lastMenu == CurrentMenu.Report && key > 1 && key < 11)
            {
                PlayerList[player.player.ID].LastMenu = Methods.Utils.FunctionMenu.ReportMenu(player, key);
                return true;
            }
            if (key == 11)
            {
                player.CloseFunctionMenu();
                PlayerList[player.player.ID].LastMenu = CurrentMenu.None;
                return true;
            }
            if (!FunctionMenuKeys.TryGetValue(key, lastMenu, out var method))
                return true;
            PlayerList[player.player.ID].LastMenu = method.Invoke(player);
            return true;
        }

        [Hook("SvPlayer.SvSuicide")]
        public static bool SvSuicide(SvPlayer player)
        {
            var shPlayer = player.player;
            if (!BlockSuicide)
                return false;
            player.SendChatMessage($"<color={errorColor}>You cannot suicide on this server because the server owner disabled it.</color>");
            return true;
        }
        [Hook("SvPlayer.SvGetJob")]
        public static bool SvGetJob(SvPlayer player, ref int employerID)
        {
            try
            {
                var shPlayer = player.player;
                var shEmployer = shPlayer.manager.FindByID<ShPlayer>(employerID);
                if (!WhitelistedJobs.ContainsKey(shEmployer.job.jobIndex))
                    return false;
                if (HasPermission.Run(player, WhitelistedJobs[shEmployer.job.jobIndex], false, shPlayer.job.jobIndex))
                    return false;
                player.SendChatMessage(MsgNoPermJob);
                return true;
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
                    player.svPlayer.SendChatMessage(BlockedItemMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return false;
        }


        [Hook("ShPlayer.RemoveItemsDeath")]
        public static bool RemoveItemsDeath(ShPlayer player)
        {
            if (!blockLicenseRemoved)
                return false;
            foreach (InventoryItem inventoryItem in player.myItems.Values.ToArray())
            {
                if (blockLicenseRemoved && inventoryItem.item.name.StartsWith("License", StringComparison.CurrentCulture))
                    continue;
                int extraCount = GetExtraCount.Run(player, inventoryItem);
                if (extraCount > 0)
                {
                    var shWearable = inventoryItem.item as ShWearable;
                    if (!shWearable || shWearable.illegal || player.curWearables[(int)shWearable.type].index != shWearable.index)
                        player.TransferItem(2, inventoryItem.item.index, extraCount, true);
                }
            }
            if (blockLicenseRemoved)
                player.svPlayer.SendChatMessage($"<color={warningColor}>This server disabled losing licenses on death.</color>");
            return true;
        }
        [Hook("SvPlayer.SvPlaceInJail")]
        public static bool SvPlaceInJail(SvPlayer player, ref int criminalID)
        {
            var shPlayer = player.player;
            if (shPlayer.manager.jail && shPlayer.job is Police)
            {
                var crimShPlayer = player.entity.manager.FindByID<ShPlayer>(criminalID);
                if (!crimShPlayer)
                    return true;
                if ((player.serverside || crimShPlayer.DistanceSqr(player.player.manager.jail) < 14400f) &&
                    crimShPlayer.IsRestrained() && !crimShPlayer.IsDead() && !(crimShPlayer.job is Prisoner))
                {
                    var jailTime = 0f;
                    var fine = 0;
                    foreach (var offense in crimShPlayer.offenses)
                    {
                        jailTime += offense.GetCrime().jailtime;
                        fine += offense.GetCrime().fine;
                    }
                    SendToJail.Run(crimShPlayer, jailTime);
                    if (fine > 0)
                        player.Reward(3, fine);
                    if (ShowJailMessage && !crimShPlayer.svPlayer.serverside && player.serverside)
                        player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{player.player.username}</color> <color={infoColor}>sent</color> <color={argColor}>{crimShPlayer.username}</color> <color={infoColor}>to jail{(fine > 0 ? $" for a fine of</color> <color={argColor}>${fine}</color>" : ".</color>")}");
                    return true;
                }
                player.SendChatMessage("Confirm criminal is cuffed and near jail");
            }
            return true;
        }
        [Hook("SvPlayer.SvTimescale")]
        public static bool SvTimescale(SvPlayer player, ref float timescale)
        {
            if (!player.player.admin)
                return true;
            if (TimescaleDisabled)
            {
                player.SendChatMessage($"<color={errorColor}>You cannot set the timescale because the server owner disabled this.</color>");
                return true;
            }
            Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] {player.player.username} set the timescale to {timescale}");
            return false;
        }
    }
}