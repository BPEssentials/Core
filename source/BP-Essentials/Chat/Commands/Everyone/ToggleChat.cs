using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class ToggleChat
    {
        public static void Run(SvPlayer player, string message)
        {
            PlayerList[player.player.ID].ChatEnabled = !PlayerList[player.player.ID].ChatEnabled;
            player.SendChatMessage($"<color={infoColor}>Chat</color> <color={argColor}>{(PlayerList[player.player.ID].ChatEnabled ? "enabled" : "disabled")}</color><color={infoColor}>.</color>");
        }
    }
}
