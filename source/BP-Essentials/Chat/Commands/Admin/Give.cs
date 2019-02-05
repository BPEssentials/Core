using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Give
    {
		public static void Run(SvPlayer player, string message)
		{
			string arg1 = GetArgument.Run(1, false, false, message);
			string arg2 = GetArgument.Run(2, false, false, message);
			if (string.IsNullOrEmpty(arg1))
			{
				player.SendChatMessage(ArgRequired);
				return;
			}
			if (string.IsNullOrWhiteSpace(arg2))
				arg2 = "1";
			bool parsedSuccessfully = int.TryParse(arg1, out int arg1int);
			parsedSuccessfully = int.TryParse(arg2, out int arg2int);
			if (!parsedSuccessfully)
			{
				player.SendChatMessage(NotValidArg);
				return;
			}
			if (arg1int < 0 || arg1int > IDs_Items.Length)
			{
				player.SendChatMessage($"<color={errorColor}>Error: The ID must be between 1 and {IDs_Items.Length}.</color>");
				return;
			}
			player.player.TransferItem(DeltaInv.AddToMe, IDs_Items[arg1int - 1], arg2int, true);
			player.SendChatMessage($"<color={infoColor}>Giving you item ID: </color><color={argColor}>{arg1}</color>");
		}
    }
}
