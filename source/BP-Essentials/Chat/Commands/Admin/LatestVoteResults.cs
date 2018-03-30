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
        public static bool Run(object oPlayer)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdLatestVoteResultsExecutableBy))
                {
                    if (!LatestVotePeople.Any())
                        player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>The list seems empty.</color>");
                    else
                    {
                        string content = string.Join("\r\n", LatestVotePeople.ToArray());
                        player.SendToSelf(Channel.Unsequenced, 50, "\r\nPlayers that voted 'yes' on the latest votekick: \r\n\r\n" + content);
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
