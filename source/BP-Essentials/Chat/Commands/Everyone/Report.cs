using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;

namespace BP_Essentials.Commands
{
    class Report
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (!String.IsNullOrEmpty(arg1))
            {
                foreach (KeyValuePair<int, _PlayerList> item in playerList)
                    if (item.Value.Shplayer.svPlayer == player)
                    {
                        if (item.Value.Shplayer.username != arg1)
                        {
                            if (!item.Value.Shplayer.admin)
                            {
                                bool playerfound = false;
                                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                                    if (shPlayer.username == arg1 && !shPlayer.svPlayer.IsServerside() || shPlayer.ID.ToString() == arg1 && !shPlayer.svPlayer.IsServerside())
                                    {
                                        playerfound = true;
                                        if (!shPlayer.admin)
                                        {
                                            var builder = new StringBuilder();
                                            builder.Append("<color=#00ffffff>Reporting</color> <color=#ea8220>" + arg1 + "</color>\n<color=#00ffffff>Reason:</color>\n\n");
                                            for (int i = 0; i < ReportReasons.Length; i++)
                                                builder.Append("<color=#00ffffff>F" + (i + 2) + ":</color> " + ReportReasons[i] + "\n");
                                            player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowFunctionMenu, builder + "\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                            item.Value.LastMenu = CurrentMenu.Report;
                                            item.Value.ReportedPlayer = shPlayer;
                                        }
                                        else
                                            player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report an admin.</color>");
                                        break;
                                    }
                                if (!playerfound)
                                    player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, NotFoundOnline);
                            }
                            else
                                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report as admin.</color>");
                        }
                        else
                            player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report yourself, dummy.</color>");
                    }
            }
            else
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Missing argument! Correct syntax:</color> <color={warningColor}>{GetArgument.Run(0, false, false, message)} [player]</color>");
        }
    }
}
