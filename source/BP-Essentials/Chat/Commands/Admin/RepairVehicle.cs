using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
	class RepairVehicle
	{
		public static void Run(SvPlayer player, string message)
		{
			var vehicle = player.player.curMount as ShTransport;
			if (!player.player.curMount || vehicle == null)
			{
				player.SendChatMessage($"<color={errorColor}>You must be in a vehicle to execute this command.</color>");
				return;
			}
			vehicle.RestoreStats();
			player.SendChatMessage($"<color={infoColor}>Your vehicle has been repaired.</color>");
		}
	}
}
