using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Text.RegularExpressions;

namespace BP_Essentials.Commands
{
    class ToggleStaffChat : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            var shplayer = player.player;
            if (string.IsNullOrEmpty(arg1))
            {
                if (playerList[shplayer.ID].staffChatEnabled)
                {
                    playerList[shplayer.ID].staffChatEnabled = false;
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat disabled.</color>");
                }
                else
                {
                    playerList[shplayer.ID].staffChatEnabled = true;
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Staff chat enabled.</color>");
                }
            }
            else
            {
                _msg = AdminChatMessage;
                _msg = _msg.Replace("{username}", new Regex("(<)").Replace(shplayer.username, "<<b></b>"));
                _msg = _msg.Replace("{id}", new Regex("(<)").Replace($"{shplayer.ID}", "<<b></b>"));
                _msg = _msg.Replace("{jobindex}", new Regex("(<)").Replace($"{shplayer.job.jobIndex}", "<<b></b>"));
                _msg = _msg.Replace("{jobname}", new Regex("(<)").Replace($"{shplayer.job.info.jobName}", "<<b></b>"));
                _msg = _msg.Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(arg1), "<<b></b>"));
                SendChatMessageToAdmins.Run(_msg);
            }
        }
    }
}
