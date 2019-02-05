using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Atm
    {
        public static void Run(SvPlayer player, string message)
        {
            player.SendChatMessage($"<color={infoColor}>Opening ATM menu..</color>");
            player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowATMMenu, player.bankBalance);
        }
    }
}
