using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
	// lol
	class Snap
	{
		public static void Run(SvPlayer player, string message)
		{
			var playersToBePwned = (new System.Random().NextDouble() >= 0.5 ? PlayerList.Take(PlayerList.Count / 2) : PlayerList.Skip(PlayerList.Count / 2)).Select(x => x.Value.ShPlayer).ToList();
			for (int i = 0; i < playersToBePwned.Count; i++)
				playersToBePwned[i].svPlayer.SvSuicide();
			player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Perfectly balanced, as all things should be.</color>");
		}
	}
}
