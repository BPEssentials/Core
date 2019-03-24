using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class ClearWanted
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
                arg1 = player.player.username;
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            currPlayer.ClearCrimes();
            currPlayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ClearCrimes, currPlayer.ID);
            player.SendChatMessage($"<color={infoColor}>Cleared crimes of '{currPlayer.username}'.</color>");
        }
    }
}
