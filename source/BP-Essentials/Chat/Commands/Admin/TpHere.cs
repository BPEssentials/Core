using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    public class TpHere
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            var shPlayer1 = player.player;
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            currPlayer.svPlayer.ResetAndSavePosition(shPlayer1.GetPosition(), shPlayer1.GetRotation(), shPlayer1.GetPlaceIndex());
            currPlayer.svPlayer.SendChatMessage($"<color={argColor}>{shPlayer1.username}</color><color={infoColor}> Teleported you to him.</color>");
            player.SendChatMessage($"<color={infoColor}>Teleported</color> <color={argColor}>{currPlayer.username}</color><color={infoColor}> To you.</color>");
        }
    }
}