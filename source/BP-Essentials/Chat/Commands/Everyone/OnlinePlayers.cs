using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class OnlinePlayers
    {
		public static void Run(SvPlayer player, string message)
		{
			player.SendChatMessage($"<color={infoColor}>There are/is </color><color={argColor}>{PlayerList.Count}</color><color={infoColor}> player(s) online</color>");
		}
    }
}
