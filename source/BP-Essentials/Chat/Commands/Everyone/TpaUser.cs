﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class TpaUser
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            var shPlayer = GetShByStr.Run(arg1);
            if (shPlayer == null)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                return;
            }
            playerList[shPlayer.ID].TpaUser = player.player;
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Sent a TPA request to</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
            shPlayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{player.player.username}</color> <color={infoColor}>sent you a tpa request. Type</color> <color={argColor}>{CmdCommandCharacter}{CmdTpaaccept}</color> <color={infoColor}>to accept it.</color>");
        }
    }
}
