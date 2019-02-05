using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
	public class Tp
	{
		public static void Run(SvPlayer player, string message)
		{
			string arg1 = GetArgument.Run(1, false, true, message);
			if (string.IsNullOrEmpty(arg1))
			{
				player.SendChatMessage(ArgRequired);
				return;
			}
			var shPlayer = GetShByStr.Run(arg1);
			if (shPlayer == null)
			{
				player.SendChatMessage(NotFoundOnline);
				return;
			}
			player.ResetAndSavePosition(shPlayer.GetPosition(), shPlayer.GetRotation(), shPlayer.GetPlaceIndex());
			player.SendChatMessage($"<color={infoColor}>Teleported to</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
		}
	}
}