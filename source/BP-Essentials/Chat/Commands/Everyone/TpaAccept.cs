using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class TpaAccept
    {
        public static void Run(SvPlayer player, string message)
        {
            var tpaUser = playerList[player.player.ID].TpaUser;
            if (tpaUser == null || !IsOnline.Run(tpaUser))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>There are no TPA accepts. (User could've went offline.)</color>");
                return;
            }
            tpaUser.svPlayer.SvReset(player.player.GetPosition(), player.player.GetRotation(), player.player.GetPlaceIndex());
            playerList[player.player.ID].TpaUser = null;
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>You accepted the TPA request of</color> <color={argColor}>{tpaUser.username}</color><color={infoColor}>.</color>");
            tpaUser.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{player.player.username}</color> <color={infoColor}>Accepted your TPA request.</color>");
        }
    }
}
