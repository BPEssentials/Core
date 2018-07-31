using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BP_Essentials.Commands
{
    public class TpLocation : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            foreach (var currKey in PlaceDictionary.Keys)
                if (currKey.Contains(arg1))
                {
                    player.SvReset(PlaceDictionary[currKey], player.player.GetRotation(), 0);
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported to</color> <color={argColor}>{currKey[1]}</color><color={infoColor}>.</color>");
                    return;
                }
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Invalid place name!</color>");
        }
    }
}