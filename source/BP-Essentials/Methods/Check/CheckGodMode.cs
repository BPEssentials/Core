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
        public static bool Run(SvPlayer player, float? amount = null, string customMessage = null)
        {
            try
            {
                if (GodListPlayers.Contains(player.playerData.username))
                {
                    if (amount != null && ShowDMGMessage)
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color=#b7b5b5>{amount} DMG Blocked!</color>");
                    if (customMessage != null)
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, customMessage);
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
