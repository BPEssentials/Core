using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ToggleReceiveStaffChat
    {
        public static void Run(SvPlayer player, string message)
        {
            var shplayer = player.player;
            if (playerList[shplayer.ID].receiveStaffChat)
            {
                playerList[shplayer.ID].receiveStaffChat = false;
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat messages disabled. You'll not receive any staff chat messages anymore.</color>");
            }
            else
            {
                playerList[shplayer.ID].receiveStaffChat = true;
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat message enabled. You'll now receive staff messages.</color>");
            }
        }
    }
}
