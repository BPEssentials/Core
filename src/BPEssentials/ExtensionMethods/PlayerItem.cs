using BPEssentials.ExtendedPlayer;
using BrokeProtocol.Entities;
using UnityEngine;

namespace BPEssentials.ExtensionMethods
{
    public static class ExtensionPlayerItem
    {
        public static void SavePosition(this PlayerItem player)
        {
            player.SavePosition(player.Client.GetPosition, player.Client.GetRotation, player.Client.GetPlaceIndex);
        }

        public static void SavePosition(this PlayerItem player, Vector3 position, Quaternion rotation, int index)
        {
            player.LastLocation.Update(position, rotation, index);
        }

        public static void ResetAndSavePosition(this PlayerItem player, Vector3 position, Quaternion rotation, int index)
        {
            player.SavePosition();
            player.Client.svPlayer.SvRestore(position, rotation, index);
        }

        public static void ResetAndSavePosition(this PlayerItem player, ShPlayer targetPlayer)
        {
            player.SavePosition();
            player.Client.svPlayer.SvRestore(targetPlayer.GetPosition, targetPlayer.GetRotation, targetPlayer.GetPlaceIndex);
        }
    }
}
