using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class ListKits
    {
        public static void Run(SvPlayer player, string message)
        {
            var kits = Variables.KitsHandler.List.Where(x => !x.Disabled && HasPermission.Run(player, x.ExecutableBy, false, player.player.job.jobIndex)).Select(n=>n.Name + $"{(n.Price != 0 ? $" ({n.Price})" : "")}").ToArray(); //linq rocks, and yes this was an very important comment.
            player.SendChatMessage($"<color={infoColor}>Kits you can obtain ({kits.Length}):</color> <color={argColor}>{(kits == null || kits.Length == 0 ? "none" : string.Join(", ", kits))}</color>");
        }
    }
}
