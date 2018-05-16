using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

namespace BP_Essentials.Commands
{
    class Mute : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdMuteExecutableBy))
                {
                    string muteuser = null;
                    var found = false;
                    muteuser = GetArgument.Run(1, false, true, message);
                    foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                        if (shPlayer.username == muteuser.ToString() || shPlayer.ID.ToString() == muteuser.ToString())
                            if (shPlayer.IsRealPlayer())
                            {
                                muteuser = shPlayer.username;
                                found = true;
                            }
                    if (!found)
                    {
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                        return true;
                    }
                    ReadFile.Run(MuteListFile);
                    if (!MutePlayers.Contains(muteuser))
                    {
                        MutePlayers.Add(muteuser);
                        File.AppendAllText(MuteListFile, muteuser + Environment.NewLine);
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Muted </color><color={argColor}>" + muteuser + "</color>");

                    }
                    else
                    {
                        RemoveStringFromFile.Run(MuteListFile, muteuser);
                        ReadFile.Run(MuteListFile);
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Unmuted </color><color={argColor}>" + muteuser + "</color>");
                    }
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
