using System;
using static BP_Essentials.EssentialsVariablesPlugin;
using System.IO;
namespace BP_Essentials.Commands
{
    public class Home
    {
        public static void Run(SvPlayer player, string message)
        {
            var shPlayer = player.player;
            if (!shPlayer.ownedApartment)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>You don't have a apartment to teleport to!</color>");
                return;
            }
            player.SvReset(shPlayer.ownedApartment.svDoor.other.spawnPoint.position, shPlayer.ownedApartment.svDoor.other.spawnPoint.rotation, 0);
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported to your apartment.</color>");
        }
    }
}