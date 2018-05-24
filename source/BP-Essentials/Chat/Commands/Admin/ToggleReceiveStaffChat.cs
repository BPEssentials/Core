using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ToggleReceiveStaffChat : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            var player = (SvPlayer)oPlayer;
            if (HasPermission.Run(player, CmdStaffChatMessagesExecutableBy))
            {
                var shplayer = GetShBySv.Run(player);
                if (playerList[shplayer.ID].receiveStaffChat)
                {
                    playerList[shplayer.ID].receiveStaffChat = false;
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat messages disabled. You'll not receive any staff chat messages anymore.</color>");
                }
                else
                {
                    playerList[shplayer.ID].receiveStaffChat = true;
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat message enabled. You'll now receive staff messages.</color>");
                }
            }
            else
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            return true;
        }
    }
}
