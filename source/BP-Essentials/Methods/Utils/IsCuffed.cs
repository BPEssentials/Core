using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials
{
    class IsCuffed
    {
        public static bool Run(SvPlayer player, bool allow = true)
        {
            if (allow || !player.player.IsRestrained())
                return true;
            player.SendChatMessage($"{MsgNoCuffedAllowed}");
            return false;
        }
    }
}
