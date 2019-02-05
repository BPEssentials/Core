using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;

namespace BP_Essentials.Commands
{
	class Mute
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
			ReadFile.Run(MuteListFile);
			if (!MutePlayers.Contains(currPlayer.username))
			{
				MutePlayers.Add(currPlayer.username);
				File.AppendAllText(MuteListFile, currPlayer.username + Environment.NewLine);
				player.SendChatMessage($"<color={infoColor}>Muted </color><color={argColor}>{currPlayer.username}</color><color={infoColor}>.</color>");
				return;
			}
			RemoveStringFromFile.Run(MuteListFile, currPlayer.username);
			ReadFile.Run(MuteListFile);
			player.SendChatMessage($"<color={infoColor}>Unmuted </color><color={argColor}>{currPlayer.username}</color><color={infoColor}>.</color>");
		}
	}
}
