using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    public class TpHere
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
			var shPlayer1 = player.player;
			if (shPlayer == null)
			{
				player.SendChatMessage(NotFoundOnline);
				return;
			}
			shPlayer.svPlayer.ResetAndSavePosition(shPlayer1.GetPosition(), shPlayer1.GetRotation(), shPlayer1.GetPlaceIndex());
			shPlayer.svPlayer.SendChatMessage($"<color={argColor}>{shPlayer1.username}</color><color={infoColor}> Teleported you to him.</color>");
			player.SendChatMessage($"<color={infoColor}>Teleported</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}> To you.</color>");
		}
    }
}