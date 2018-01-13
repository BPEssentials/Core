using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Rules : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                player.SendToSelf(Channel.Unsequenced, 10, "Work in progress.");
                //  player.SendToSelf(Channel.Reliable, 50, );
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
