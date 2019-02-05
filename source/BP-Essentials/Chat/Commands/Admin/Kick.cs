using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Kick
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var shPlayer = GetShByStr.Run(arg1);
            if (shPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            player.svManager.Kick(shPlayer.svPlayer.connection);
            player.SendChatMessage($"<color={infoColor}>Kicked</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
        }
    }
}
