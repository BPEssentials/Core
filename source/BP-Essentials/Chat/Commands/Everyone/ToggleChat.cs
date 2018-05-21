using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ToggleChat : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdAtmExecutableBy))
                {
                    foreach (var player2 in playerList.Values)
                    {
                        if (player2.shplayer.svPlayer == player)
                        {
                            if (player2.chatEnabled)
                            {
                                player2.chatEnabled = false;
                                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Chat </color><color={argColor}>disabled</color><color={infoColor}>.</color>");
                            }
                            else
                            {
                                player2.chatEnabled = true;
                                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Chat </color><color={argColor}>enabled</color><color={infoColor}>.</color>");
                            }
                            break;
                        }
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
