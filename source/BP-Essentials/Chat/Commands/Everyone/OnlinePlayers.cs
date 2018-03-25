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
                if (HasPermission.Run(player, CmdPlayersExecutableBy))
                {
                    var realPlayers = FindObjectsOfType<ShPlayer>().Count(shPlayer => shPlayer.IsRealPlayer());
                    switch (realPlayers)
                    {
                        case 1:
                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>There is </color><color={argColor}>{realPlayers}</color><color={infoColor}> player online</color>");
                            break;
                        default:
                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>There are </color><color={argColor}>{realPlayers}</color><color={infoColor}> players online</color>");
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
