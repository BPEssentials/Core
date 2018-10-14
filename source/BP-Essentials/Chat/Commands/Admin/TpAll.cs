using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    public class TpAll
    {
        public static void Run(SvPlayer player, string message)
        {
            var pos = player.player.GetPosition();
            var rot = player.player.GetRotation();
            var pIndex = player.player.GetPlaceIndex();
            foreach (var currPlayer in SvMan.players.Values)
                currPlayer.svPlayer.SvReset(pos, rot, pIndex);
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported</color> <color={argColor}>everyone</color> <color={infoColor}>to your location.</color>");
        }
    }
}