using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class CheckGodMode : EssentialsCorePlugin
    {
        public static bool Run(object oPlayer, float amount)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (GodListPlayers.Contains(player.playerData.username))
                {
                    if (ShowDMGMessage)
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color=#b7b5b5>{amount} DMG Blocked!</color>");
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return false;
        }
    }
}
