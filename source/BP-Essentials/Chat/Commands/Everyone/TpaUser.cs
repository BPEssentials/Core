using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class TpaUser
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var shPlayer = GetShByStr.Run(arg1);
            if (shPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
			if (shPlayer == player.player)
			{
				player.SendChatMessage($"<color={errorColor}>You cannot teleport to yourself.</color>");
				return;
			}
            PlayerList[shPlayer.ID].TpaUser = player.player;
            player.SendChatMessage($"<color={infoColor}>Sent a TPA request to</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
            shPlayer.svPlayer.SendChatMessage($"<color={argColor}>{player.player.username}</color> <color={infoColor}>sent you a tpa request. Type</color> <color={argColor}>{CmdCommandCharacter}{CmdTpaaccept}</color> <color={infoColor}>to accept it.</color>");
        }
    }
}
