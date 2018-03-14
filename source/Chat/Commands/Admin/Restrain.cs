﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Restrain : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                string arg1 = GetArgument.Run(1, false, true, message);
                if (!string.IsNullOrEmpty(arg1))
                    ExecuteOnPlayer.Run(player, message, arg1);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, ArgRequired);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
