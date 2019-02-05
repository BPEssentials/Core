using System;
using System.Collections.Generic;
using static BP_Essentials.Variables;
namespace BP_Essentials.Commands
{
    public class Promote
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
			if (currPlayer.rank >= 2)
			{
				player.SendChatMessage($"<color={argColor}>{currPlayer.username}</color> <color={errorColor}>Already has the highest rank!</color>");
				return;
			}
			currPlayer.svPlayer.Reward(currPlayer.maxExperience - currPlayer.experience + 1, 0);
			player.SendChatMessage($"<color={infoColor}>Promoted</color> <color={argColor}>{currPlayer.username}</color> <color={infoColor}>to rank</color> <color={argColor}>{currPlayer.rank + 1}</color><color={infoColor}>.</color>");
		}
    }
}