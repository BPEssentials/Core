using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Jail : EssentialsChatPlugin
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
                if (float.TryParse(message.Split(' ').Last().Trim(), out float t))
                {
                    if (SendToJail.Run(shPlayer, t))
                    {
                        shPlayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{player.playerData.username}</color> <color={infoColor}>sent you to jail.</color>");
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Sent</color> <color={argColor}>{shPlayer.username}</color> <color={infoColor}>To jail.</color>");
                    }
                    else
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Cannot send </color> <color={argColor}>{shPlayer.username}</color> <color={errorColor}>To jail.</color>");
                }
                else
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
            }
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
        }
    }
}
