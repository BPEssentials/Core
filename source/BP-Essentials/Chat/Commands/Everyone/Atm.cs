using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Atm
    {
        public static void Run(SvPlayer player, string message)
        {
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Opening ATM menu..</color>");
            player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowATMMenu, player.bankBalance);
        }
    }
}
