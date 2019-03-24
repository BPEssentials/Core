using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class LatestVoteResults
    {
        public static void Run(SvPlayer player, string message)
        {
            if (!LatestVotePeople.Any())
            {
                player.SendChatMessage($"<color={infoColor}>The list seems empty.</color>");
                return;
            }
            string content = string.Join("\r\n", LatestVotePeople);
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.ServerInfo, "\r\nPlayers that voted 'yes' on the latest votekick: \r\n\r\n" + content);
        }
    }
}
