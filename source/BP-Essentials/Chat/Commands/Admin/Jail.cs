using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Jail
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            arg1 = arg1.Remove(arg1.LastIndexOf(" ")).Trim();
            var shPlayer = GetShByStr.Run(arg1);
            if (shPlayer == null)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                return;
            }
            if (!float.TryParse(message.Split(' ').Last().Trim(), out float t))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            if (!SendToJail.Run(shPlayer, t))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Cannot send</color> <color={argColor}>{shPlayer.username}</color> <color={errorColor}>To jail.</color>");
                return;
            }
            shPlayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{player.player.username}</color> <color={infoColor}>sent you to jail.</color>");
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Sent</color> <color={argColor}>{shPlayer.username}</color> <color={infoColor}>To jail.</color>");
        }
    }
}
