using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Slap
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
			int amount = new System.Random().Next(4, 15);
			currPlayer.svPlayer.Damage(DamageIndex.Null, amount, null, null);
			currPlayer.svPlayer.SvForce(new Vector3(500f, 0f, 500f));
			currPlayer.svPlayer.SendChatMessage($"<color={warningColor}>You got slapped by </color><color={argColor}>{player.player.username}</color><color={warningColor}>! [-{amount} HP]</color>");
			player.SendChatMessage($"<color={infoColor}>You've slapped </color><color={argColor}>{currPlayer.username}</color><color={infoColor}>. [-{amount} HP]</color>");
		}
    }
}
