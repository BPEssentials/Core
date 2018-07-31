using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ListKits : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player)
        {
            var kits = listKits.Where(x => !x.Disabled && HasPermission.Run(player, x.ExecutableBy)).Select(n=>n.Name).ToArray();
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Kits you can obtain ({kits.Length}):</color> <color={argColor}>{(kits == null || kits.Length == 0 ? "none" : string.Join(", ", kits))}</color>");
        }
    }
}
