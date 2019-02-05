using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class TpaDeny
    {
        public static void Run(SvPlayer player, string message)
        {
            var tpaUser = PlayerList[player.player.ID].TpaUser;
            if (tpaUser == null || !IsOnline.Run(tpaUser))
            {
                player.SendChatMessage($"<color={errorColor}>There are no TPA accepts. (User could've went offline.)</color>");
                return;
            }
            PlayerList[player.player.ID].TpaUser = null;
            player.SendChatMessage($"<color={errorColor}>You denied the TPA request of</color> <color={argColor}>{tpaUser.username}</color><color={errorColor}>.</color>");
            tpaUser.svPlayer.SendChatMessage($"<color={argColor}>{player.player.username}</color> <color={errorColor}>Denied your TPA request.</color>");
        }
    }
}
