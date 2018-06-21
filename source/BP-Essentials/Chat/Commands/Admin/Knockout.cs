using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Knockout : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (!string.IsNullOrEmpty(arg1))
            {
                var shPlayer = GetShByStr.Run(arg1);
                if (shPlayer == null)
                {
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                    return;
                }
                shPlayer.svPlayer.SvForceStance(StanceIndex.KnockedOut);
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Knocked out</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
            }
            else
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
        }
    }
}
