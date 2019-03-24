using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Sudo
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
            shPlayer.svPlayer.SvGlobalChatMessage(arg2);
            player.SendChatMessage($"<color={infoColor}>You sudo'ed</color> <color={argColor}>{shPlayer.username}</color> <color={infoColor}>to type</color> <color={argColor}>{arg2}</color> <color={infoColor}>in the global chat.</color>");
        }
    }
}
