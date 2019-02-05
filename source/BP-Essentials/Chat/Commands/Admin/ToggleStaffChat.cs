using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Text.RegularExpressions;

namespace BP_Essentials.Commands
{
    class ToggleStaffChat
    {
		public static void Run(SvPlayer player, string message)
		{
			var arg1 = GetArgument.Run(1, false, true, message);
			var shplayer = player.player;
			if (string.IsNullOrEmpty(arg1))
			{
				if (PlayerList[shplayer.ID].StaffChatEnabled)
				{
					PlayerList[shplayer.ID].StaffChatEnabled = false;
					player.SendChatMessage($"<color={infoColor}>Staff chat disabled.</color>");
				}
				else
				{
					PlayerList[shplayer.ID].StaffChatEnabled = true;
					player.SendChatMessage($"<color={infoColor}>Staff chat enabled.</color>");
				}
				return;
			}
			SendChatMessageToAdmins.Run(PlaceholderParser.ParseUserMessage(player.player, AdminChatMessage, arg1));
		}
    }
}
