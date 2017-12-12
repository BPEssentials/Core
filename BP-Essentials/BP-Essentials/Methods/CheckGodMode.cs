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
            var player = (SvPlayer)oPlayer;
            if (GodListPlayers.Contains(player.playerData.username))
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, amount + " DMG Blocked!");
                return true;
            }
            return false;
        }
    }
}
