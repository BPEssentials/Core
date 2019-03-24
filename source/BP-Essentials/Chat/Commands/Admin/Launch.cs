using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Launch
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            currPlayer.svPlayer.SvForce(new Vector3(0f, 6500f, 0f));
            currPlayer.svPlayer.SendChatMessage($"<color={warningColor}>Off you go!</color>");
            player.SendChatMessage($"<color={infoColor}>You've launched </color><color={argColor}>{currPlayer.username}</color><color={infoColor}> into space!</color>");
        }
    }
}
