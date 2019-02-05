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
	class Say
	{
		public static void Run(SvPlayer player, string message)
		{
			string arg1 = GetArgument.Run(1, false, true, message);
			if (string.IsNullOrEmpty(arg1))
			{
				player.SendChatMessage(ArgRequired);
				return;
			}
			player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, $"<color={MsgSayColor}>{MsgSayPrefix} {player.playerData.username}: {arg1.FilterString()}</color>");
		}
	}
}
