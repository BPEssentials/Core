using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class ReplyUser
    {
        public static void Run(SvPlayer player, string message)
        {
            var replyUser = PlayerList[player.player.ID].ReplyToUser;
            if (replyUser == null || !IsOnline.Run(replyUser))
            {
                player.SendChatMessage($"<color={errorColor}>There is nobody to respond to. (User could've went offline.)</color>");
                return;
            }
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            PlayerList[replyUser.ID].ReplyToUser = player.player;
            player.SendChatMessage($"<color={infoColor}>[PM]</color> <color={argColor}>{replyUser.username}</color> <color={warningColor}>></color> <color={infoColor}>{arg1}</color>");
            replyUser.svPlayer.SendChatMessage($"<color={infoColor}>[PM]</color> <color={argColor}>{player.player.username}</color> <color={warningColor}><</color> <color={infoColor}>{arg1}</color>");
        }
    }
}
