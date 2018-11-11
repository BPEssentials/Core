﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ListWarps
    {
        public static void Run(SvPlayer player, string message)
        {
            var warps = listWarps.Where(x => !x.Disabled && HasPermission.Run(player, x.ExecutableBy, false, player.player.job.jobIndex)).Select(n=>n.Name + $"{(n.Price != 0 ? $" ({n.Price})" : "")}").ToArray();
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Warps you can teleport to ({warps.Length}):</color> <color={argColor}>{(warps == null || warps.Length == 0 ? "none" : string.Join(", ", warps))}</color>");
        }
    }
}
