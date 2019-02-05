using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class TpaAccept
    {
        public static void Run(SvPlayer player, string message)
        {
            var tpaUser = PlayerList[player.player.ID].TpaUser;
            if (tpaUser == null || !IsOnline.Run(tpaUser))
            {
                player.SendChatMessage($"<color={errorColor}>There are no TPA accepts. (User could've went offline.)</color>");
                return;
            }
            tpaUser.svPlayer.ResetAndSavePosition(player.player.GetPosition(), player.player.GetRotation(), player.player.GetPlaceIndex());
            PlayerList[player.player.ID].TpaUser = null;
            player.SendChatMessage($"<color={infoColor}>You accepted the TPA request of</color> <color={argColor}>{tpaUser.username}</color><color={infoColor}>.</color>");
            tpaUser.svPlayer.SendChatMessage($"<color={argColor}>{player.player.username}</color> <color={infoColor}>Accepted your TPA request.</color>");
        }
    }
}
