using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ToggleChat
    {
        public static void Run(SvPlayer player, string message)
        {
            foreach (var player2 in playerList.Values)
            {
                if (player2.Shplayer.svPlayer == player)
                {
                    if (player2.chatEnabled)
                    {
                        player2.chatEnabled = false;
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Chat </color><color={argColor}>disabled</color><color={infoColor}>.</color>");
                    }
                    else
                    {
                        player2.chatEnabled = true;
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Chat </color><color={argColor}>enabled</color><color={infoColor}>.</color>");
                    }
                    break;
                }
            }
        }
    }
}
