using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Restrain
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
			if (currPlayer == null)
			{
				player.SendChatMessage(NotFoundOnline);
				return;
			}
			currPlayer.svPlayer.Restrain(currPlayer.manager.handcuffed);
			var shRetained = currPlayer.curEquipable as ShRestrained;
			currPlayer.svPlayer.SvSetEquipable(shRetained.otherRestrained.index);
			if (!currPlayer.svPlayer.serverside)
				currPlayer.svPlayer.SendChatMessage("You've been restrained");
			player.SendChatMessage($"<color={infoColor}>Restrained</color> <color={argColor}>{currPlayer.username}</color><color={infoColor}>.</color>");
		}
    }
}
