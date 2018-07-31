using System;
using System.Collections.Generic;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands
{
    public class Promote : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (!string.IsNullOrEmpty(arg1))
            {
                var shPlayer = GetShByStr.Run(arg1);
                if (shPlayer == null)
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                    return;
                }
                if (shPlayer.rank >= 2)
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{shPlayer.username}</color> <color={errorColor}>Already has the highest rank!</color>");
                    return;
                }
                shPlayer.svPlayer.Reward(shPlayer.maxExperience - shPlayer.experience + 1, 0);
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Promoted</color> <color={argColor}>{shPlayer.username}</color> <color={infoColor}>to rank</color> <color={argColor}>{shPlayer.rank +1}</color><color={infoColor}>.</color>");
            }
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
        }
    }
}