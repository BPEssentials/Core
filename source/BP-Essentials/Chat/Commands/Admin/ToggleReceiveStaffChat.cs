using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class ToggleReceiveStaffChat
    {
        public static void Run(SvPlayer player, string message)
        {
            var shplayer = player.player;
            if (PlayerList[shplayer.ID].ReceiveStaffChat)
            {
                PlayerList[shplayer.ID].ReceiveStaffChat = false;
                player.SendChatMessage($"<color={infoColor}>Staff chat messages disabled. You'll not receive any staff chat messages anymore.</color>");
            }
            else
            {
                PlayerList[shplayer.ID].ReceiveStaffChat = true;
                player.SendChatMessage($"<color={infoColor}>Staff chat messages enabled. You'll now receive staff messages.</color>");
            }
        }
    }
}
