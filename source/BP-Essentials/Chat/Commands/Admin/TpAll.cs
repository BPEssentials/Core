using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    public class TpAll
    {
        public static void Run(SvPlayer player, string message)
        {
            var pos = player.player.GetPosition();
            var rot = player.player.GetRotation();
            foreach (var currPlayer in SvMan.players.Values)
                currPlayer.svPlayer.ResetAndSavePosition(pos, rot, player.player.GetPlaceIndex());
            player.SendChatMessage($"<color={infoColor}>Teleported</color> <color={argColor}>everyone</color> <color={infoColor}>to your location.</color>");
        }
    }
}