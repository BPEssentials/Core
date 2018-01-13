using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class OnlinePlayers : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer)
        {
            try
            {

                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdPlayersExecutableBy == "admin" || CmdPlayersExecutableBy == "everyone")
                {
                    var realPlayers = FindObjectsOfType<ShPlayer>().Count(shPlayer => shPlayer.IsRealPlayer());
                    switch (realPlayers)
                    {
                        case 1:
                            player.SendToSelf(Channel.Unsequenced, 10, "There is " + realPlayers + " player online");
                            break;
                        default:
                            if (realPlayers < 1)
                                player.SendToSelf(Channel.Unsequenced, 10, "There are " + realPlayers + " play- wait, how is that possible");
                            else
                                player.SendToSelf(Channel.Unsequenced, 10, "There are " + realPlayers + " player(s) online");
                            break;
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
