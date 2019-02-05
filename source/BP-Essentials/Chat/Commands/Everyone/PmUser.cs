using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class PmUser
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, false, message);
            string arg2 = GetArgument.Run(2, false, true, message);
            if (string.IsNullOrEmpty(arg1) || string.IsNullOrEmpty(arg2))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var shPlayer = GetShByStr.Run(arg1, true);
            if (shPlayer == null)
            {
                player.SendChatMessage(NotFoundOnlineIdOnly);
                return;
            }
            PlayerList[shPlayer.ID].ReplyToUser = player.player;
            player.SendChatMessage($"<color={infoColor}>[PM]</color> <color={argColor}>{shPlayer.username}</color> <color={warningColor}>></color> <color={infoColor}>{arg2}</color>");
            shPlayer.svPlayer.SendChatMessage($"<color={infoColor}>[PM]</color> <color={argColor}>{player.player.username}</color> <color={warningColor}><</color> <color={infoColor}>{arg2}</color>");
        }
    }
}
