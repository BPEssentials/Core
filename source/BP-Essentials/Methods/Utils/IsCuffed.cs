using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class IsCuffed
    {
        public static bool Run(SvPlayer player, bool allow = true)
        {
            if (allow || player.player.IsRestrained())
                return true;
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"{MsgNoCuffedAllowed}");
            return false;
        }
    }
}
