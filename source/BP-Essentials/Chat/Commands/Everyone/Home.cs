using System;
using static BP_Essentials.Variables;
using System.IO;
namespace BP_Essentials.Commands
{
    public static class Home
    {
        public static void Run(SvPlayer player, string message)
        {
            var shPlayer = player.player;
            if (!shPlayer.ownedApartment)
            {
                player.SendChatMessage($"<color={warningColor}>You don't have a apartment to teleport to!</color>");
                return;
            }
            player.ResetAndSavePosition(shPlayer.ownedApartment.svDoor.transform.position, shPlayer.ownedApartment.svDoor.transform.rotation, 0);
            player.SendChatMessage($"<color={infoColor}>Teleported to your apartment.</color>");
        }
    }
}