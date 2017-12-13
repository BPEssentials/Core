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
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Are you sure you want to sell your apartment? Type '/confirm' to confirm.");
            }
            return false;
        }

        [Hook("SvPlayer.Initialize")]
        public static void Initialize(SvPlayer player)
        {
            if (player.playerData.username == null) return;
            var thread1 = new Thread(new ParameterizedThreadStart(CheckBanned.Run));
            thread1.Start(player);
            var thread2 = new Thread(new ParameterizedThreadStart(WriteIpToFile.Run));
            thread2.Start(player);
            var thread3 = new Thread(new ParameterizedThreadStart(CheckAltAcc.Run));
            thread3.Start(player);
        }

        [Hook("SvPlayer.Damage")]
        public static bool Damage(SvPlayer player, ref DamageIndex type, ref float amount, ref ShPlayer attacker, ref Collider collider) {
            return CheckGodMode.Run(player, amount);
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
                        if (!shPlayer.svPlayer.IsServerside())
                            player.SendToAll(Channel.Unsequenced, (byte)10, shPlayer.svPlayer.playerData.username + " Just got banned by " + player.playerData.username);
        }






    }
}