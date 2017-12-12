using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsConfigPlugin;
using System.IO;

namespace BP_Essentials.Commands
{
    class Mute : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            var player = (SvPlayer)oPlayer;
            if (AdminsListPlayers.Contains(player.playerData.username))
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
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "User or ID '" + muteuser + "' is not found.");
                    return true;
                }
                ReadFile(MuteListFile);
                if (!MutePlayers.Contains(muteuser))
                {
                    MutePlayers.Add(muteuser);
                    File.AppendAllText(MuteListFile, muteuser + Environment.NewLine);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, muteuser + " Muted");

                }
                else
                {
                    RemoveStringFromFile(MuteListFile, muteuser);
                    ReadFile(MuteListFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, muteuser + " Unmuted");
                }
            }
            else
                player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
            return true;
        }
    }
}
