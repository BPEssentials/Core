using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Reload
    {
        public static void Run(SvPlayer player, string message)
        {
            BP_Essentials.Reload.Run(false, player);
        }
    }
}
