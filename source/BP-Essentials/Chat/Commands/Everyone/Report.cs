using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;

namespace BP_Essentials.Commands
{
    class Report : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                string arg1 = GetArgument.Run(1, false, true, message);
                if (!String.IsNullOrEmpty(arg1))
                {
                    foreach (KeyValuePair<int, _PlayerList> item in playerList)
                        if (item.Value.shplayer.svPlayer == player)
                        {
                            if (item.Value.shplayer.username != arg1)
                            {
                                if (!item.Value.shplayer.admin)
                                {
                                    bool playerfound = false;
                                    foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                                        if (shPlayer.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1 && shPlayer.IsRealPlayer())
                                        {
                                            playerfound = true;
                                            if (!shPlayer.admin)
                                            {
                                                var builder = new StringBuilder();
                                                builder.Append("<color=#00ffffff>Reporting</color> <color=#ea8220>" + arg1 + "</color>\n<color=#00ffffff>Reason:</color>\n\n");
                                                for (int i = 0; i < ReportReasons.Length; i++)
                                                {
                                                    builder.Append("<color=#00ffffff>F" + (i + 2) + ":</color> " + ReportReasons[i] + "\n");
                                                }
                                                player.SendToSelf(Channel.Reliable, ClPacket.ShowFunctionMenu, builder + "\n<color=#00ffffff>Press</color> <color=#ea8220>F11</color> <color=#00ffffff>To close this (G)UI</color>");
                                                item.Value.LastMenu = CurrentMenu.Report;
                                                item.Value.reportedPlayer = shPlayer;
                                            }
                                            else
                                                player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report an admin.</color>");
                                            break;
                                        }
                                    if (!playerfound)
                                        player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, NotFoundOnline);
                                }
                                else
                                    player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report as admin.</color>");
                            }
                            else
                                player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Hey! You cannot report yourself, dummy.</color>");
                        }
                }
                else
                {
                    player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, $"<color={errorColor}>Missing argument! Correct syntax:</color> <color={warningColor}>{GetArgument.Run(0, false, false, message)} [player]</color>");
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
