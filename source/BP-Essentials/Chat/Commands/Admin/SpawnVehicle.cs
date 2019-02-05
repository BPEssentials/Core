using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
	class SpawnVehicle
	{
		public static void Run(SvPlayer player, string message)
		{
			string arg1 = GetArgument.Run(1, false, true, message);
			if (string.IsNullOrEmpty(arg1))
			{
				player.SendChatMessage(ArgRequired);
				return;
			}
			bool parsedSuccessfully = int.TryParse(arg1, out int arg1int);
			if (!parsedSuccessfully)
			{
				player.SendChatMessage(NotValidArg);
				return;
			}
			if (arg1int < 0 || arg1int > IDs_Vehicles.Length)
			{
				player.SendChatMessage($"<color={errorColor}>Error: The ID must be between 1 and {IDs_Vehicles.Length}.</color>");
				return;
			}
			var shPlayer = player.player;
			var pos = shPlayer.GetPosition();
			if (!player.player.IsOutside())
			{
				player.SendChatMessage($"<color={errorColor}>Cannot spawn inside a building.</color>");
				return;
			}
			if (arg1.Length > 4)
				SvMan.AddNewEntity(ShManager.GetEntity(arg1int), shPlayer.manager.places[0], new Vector3(pos.x, pos.y + 10F, pos.z), shPlayer.GetRotation(), false);
			else
				SvMan.AddNewEntity(ShManager.GetEntity(IDs_Vehicles[arg1int - 1]), shPlayer.manager.places[0], new Vector3(pos.x, pos.y + 7F, pos.z), shPlayer.GetRotation(), false);
			player.SendChatMessage($"<color={infoColor}>Spawning in vehicle with the ID: </color><color={argColor}>{arg1}</color>");
		}
	}
}
