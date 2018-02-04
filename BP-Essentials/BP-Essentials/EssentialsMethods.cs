using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
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
                player.SendToSelf(Channel.Unsequenced, 10, $"<color={warningColor}>Are you sure you want to sell your apartment? Type '</color> <color={argColor}>{CmdConfirm}</color><color={warningColor}>' to confirm.</color>");
            }
            return false;
        }

        [Hook("SvPlayer.Initialize")]
        public static void Initialize(SvPlayer player)
        {
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.svPlayer == player && shPlayer.IsRealPlayer())
                {
                    var thread1 = new Thread(new ParameterizedThreadStart(CheckBanned.Run));
                    thread1.Start(player);
                    var thread2 = new Thread(new ParameterizedThreadStart(WriteIpToFile.Run));
                    thread2.Start(player);
                    var thread3 = new Thread(new ParameterizedThreadStart(CheckAltAcc.Run));
                    thread3.Start(player);
                }
        }

        [Hook("SvPlayer.Damage")]
        public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider) {
            return CheckGodMode.Run(player, amount);
        }

        [Hook("SvPlayer.SpawnBot")]
        public static bool SpawnBot(SvPlayer player, ref Vector3 position, ref Quaternion rotation, ref Place place, ref WaypointNode node, ref ShTransport vehicle, ref ShPlayer enemy, ref byte spawnJobIndex) {
            return EnableBlockSpawnBot == true && BlockedSpawnIds.Contains(spawnJobIndex);
        }

        [Hook("ShRetainer.HitEffect")]
        public static bool HitEffect(ShRetainer player, ref ShEntity hitTarget, ref ShPlayer source, ref Collider collider)
        {

            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                if (shPlayer.IsRealPlayer()) {
                    if (shPlayer != hitTarget) continue;
                    if (!GodListPlayers.Contains(shPlayer.username)) continue;
                    shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, "<color=#b7b5b5>Being handcuffed Blocked!</color>");
                    return true;
                }
            return false;
        }

        [Hook("SvPlayer.SvBan")]
        public static void SvBan(SvPlayer player, ref int otherID)
        {
            foreach (var shPlayer in GameObject.FindObjectsOfType<ShPlayer>())
                if (shPlayer.ID == otherID)
                    if (shPlayer.IsRealPlayer() && !shPlayer.svPlayer.IsServerside())
                        player.SendToAll(Channel.Unsequenced, 10, $"<color={argColor}>{shPlayer.username}</color> <color={warningColor}>Just got banned by</color> <color={argColor}>{player.playerData.username}</color>");
        }

        [Hook("SvPlayer.SvStartVote")]
        public static bool SvStartVote(SvPlayer player, ref byte voteIndex, ref int ID)
        {
            if (voteIndex == 1)
                if (!VoteKickDisabled)
                {
                    foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                        if (shPlayer.ID == ID)
                            foreach (var shIssuer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                if (shIssuer.svPlayer == player)
                                {
                                    if (player.netMan.vote != null || voteIndex >= shIssuer.gameMan.votes.Length || player.netMan.startedVote.Contains(shIssuer))
                                        return false;
                                    player.SendToAll(Channel.Unsequenced, 10, $"<color={argColor}>{player.playerData.username} </color><color={warningColor}>Has issued a vote kick against</color><color={argColor}> {shPlayer.username}</color>");
                                    LatestVotePeople.Clear();
                                }
                }
                else
                {
                    player.SendToAll(Channel.Unsequenced, 10, $"<color={errorColor}>Vote kicking has been disabled on this server.</color>");
                    return true;
                }
            return false;
        }

        [Hook("SvPlayer.SvVoteYes", true)]
        public static void SvVoteYes(SvPlayer player)
        {
            LatestVotePeople.Add(player.playerData.username);
        }
    }
}