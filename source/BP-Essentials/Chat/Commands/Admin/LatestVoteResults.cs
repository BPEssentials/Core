using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class LatestVoteResults : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player)
        {
            if (!LatestVotePeople.Any())
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>The list seems empty.</color>");
            else
            {
                string content = string.Join("\r\n", LatestVotePeople.ToArray());
                player.SendToSelf(Channel.Unsequenced, ClPacket.ServerInfo, "\r\nPlayers that voted 'yes' on the latest votekick: \r\n\r\n" + content);
            }
        }
    }
}
