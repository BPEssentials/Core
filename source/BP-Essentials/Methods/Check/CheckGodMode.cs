using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials
{
    class CheckGodMode : Core
    {
        public static bool Run(SvPlayer player, float? amount = null, string customMessage = null)
        {
            try
            {
                if (GodListPlayers.Contains(player.playerData.username))
                {
                    if (amount != null && ShowDMGMessage)
                        player.SendChatMessage($"<color=#b7b5b5>{amount} DMG Blocked!</color>");
                    if (customMessage != null)
                        player.SendChatMessage(customMessage);
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
