using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials.Commands
{
    class Arrest
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
			shPlayer.svPlayer.Restrain(shPlayer.manager.handcuffed);
			player.SendChatMessage($"<color={infoColor}>Arrested</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
		}
    }
}
