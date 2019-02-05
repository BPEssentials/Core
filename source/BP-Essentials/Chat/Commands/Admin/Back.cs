using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
	class Back
	{
		public static void Run(SvPlayer player, string message)
		{
			var lastLocation = PlayerList[player.player.ID].LastLocation;
			if (!lastLocation.HasPositionSet())
			{
				player.SendChatMessage($"<color={errorColor}>There is no location to teleport to.</color>");
				return;
			}
			player.ResetAndSavePosition(lastLocation.Position, lastLocation.Rotation, lastLocation.PlaceIndex);
			player.SendChatMessage($"<color={infoColor}>You've been teleported to your last known location.</color>");
		}
	}
}
