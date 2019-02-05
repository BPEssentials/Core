using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Jail
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            arg1 = arg1.Remove(arg1.LastIndexOf(" ", StringComparison.CurrentCulture)).Trim();
            var shPlayer = GetShByStr.Run(arg1);
            if (shPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            if (!float.TryParse(message.Split(' ').Last().Trim(), out float time))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            if (!SendToJail.Run(shPlayer, time))
            {
                player.SendChatMessage($"<color={errorColor}>Cannot send</color> <color={argColor}>{shPlayer.username}</color> <color={errorColor}>To jail.</color>");
                return;
            }
            shPlayer.svPlayer.SendChatMessage($"<color={argColor}>{player.player.username}</color> <color={infoColor}>sent you to jail.</color>");
            player.SendChatMessage($"<color={infoColor}>Sent</color> <color={argColor}>{shPlayer.username}</color> <color={infoColor}>To jail.</color>");
        }
    }
}
