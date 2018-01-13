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
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdMuteExecutableBy == "admin" || CmdMuteExecutableBy == "everyone")
                {
                    string muteuser = null;
                    var found = false;
                    muteuser = GetArgument.Run(1, false, true, message);
                    foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                        if (shPlayer.svPlayer.playerData.username == muteuser.ToString() || shPlayer.ID.ToString() == muteuser.ToString())
                            if (shPlayer.IsRealPlayer())
                            {
                                muteuser = shPlayer.svPlayer.playerData.username;
                                found = true;
                            }
                    if (!found)
                    {
                        player.SendToSelf(Channel.Unsequenced, 10, "User or ID '" + muteuser + "' is not found.");
                        return true;
                    }
                    ReadFile.Run(MuteListFile);
                    if (!MutePlayers.Contains(muteuser))
                    {
                        MutePlayers.Add(muteuser);
                        File.AppendAllText(MuteListFile, muteuser + Environment.NewLine);
                        player.SendToSelf(Channel.Unsequenced, 10, muteuser + " Muted");

                    }
                    else
                    {
                        RemoveStringFromFile.Run(MuteListFile, muteuser);
                        ReadFile.Run(MuteListFile);
                        player.SendToSelf(Channel.Unsequenced, 10, muteuser + " Unmuted");
                    }
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
