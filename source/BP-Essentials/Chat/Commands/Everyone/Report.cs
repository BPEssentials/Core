using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;

namespace BP_Essentials.Commands
{
    class Report
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Missing argument! Correct syntax:</color> <color={warningColor}>{GetArgument.Run(0, false, false, message)} [player]</color>");
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            if (currPlayer == player.player)
            {
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report yourself, dummy.</color>");
                return;
            }
            if (player.player.admin)
            {
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report as admin.</color>");
                return;
            }
            if (currPlayer.admin)
            {
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report an admin.</color>");
                return;
            }
            var builder = new StringBuilder();
            builder.Append("<color=#00ffffff>Reporting</color> <color=#ea8220>" + arg1 + "</color>\n<color=#00ffffff>Reason:</color>\n\n");
            for (int i = 0; i < ReportReasons.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(ReportReasons[i]))
                    continue;
                builder.Append("<color=#00ffffff>F" + (i + 2) + ":</color> " + ReportReasons[i] + "\n");
            }
            player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, builder + "\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
            PlayerList[player.player.ID].LastMenu = CurrentMenu.Report;
            PlayerList[player.player.ID].ReportedPlayer = currPlayer;
        }
    }
}
