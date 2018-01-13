using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ClearWanted : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                bool found = false;
                string arg1 = GetArgument.Run(1, false, true, message);
                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.svPlayer.playerData.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1.ToString() && shPlayer.IsRealPlayer())
                    {
                        shPlayer.ClearCrimes();
                        shPlayer.svPlayer.SendToSelf(Channel.Reliable, 33, shPlayer.ID);
                        player.SendToSelf(Channel.Unsequenced, 10, "Cleared crimes of '" + shPlayer.svPlayer.playerData.username + "'.");
                        found = true;
                    }
                if (!found)
                    player.SendToSelf(Channel.Unsequenced, 10, "Player not found/online.");

            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
