using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class HasWantedLevel
    {
        public static bool Run(SvPlayer player, bool allow = true)
        {
            if (allow || player.player.wantedLevel <= 0)
                return true;
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"{MsgNoWantedAllowed}");
            return false;
        }
    }
}
