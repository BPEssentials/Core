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
    class Mute
    {
        public static void Run(SvPlayer player, string message)
        {
            string muteuser = null;
            var found = false;
            muteuser = GetArgument.Run(1, false, true, message);
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.username == muteuser.ToString() || shPlayer.ID.ToString() == muteuser.ToString())
                    if (!shPlayer.svPlayer.IsServerside())
                    {
                        muteuser = shPlayer.username;
                        found = true;
                    }
            if (!found)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                return;
            }
            ReadFile.Run(MuteListFile);
            if (!MutePlayers.Contains(muteuser))
            {
                MutePlayers.Add(muteuser);
                File.AppendAllText(MuteListFile, muteuser + Environment.NewLine);
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Muted </color><color={argColor}>" + muteuser + "</color>");

            }
            else
            {
                RemoveStringFromFile.Run(MuteListFile, muteuser);
                ReadFile.Run(MuteListFile);
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Unmuted </color><color={argColor}>" + muteuser + "</color>");
            }
        }
    }
}
