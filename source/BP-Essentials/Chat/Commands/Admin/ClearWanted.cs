using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ClearWanted
    {
        public static void Run(SvPlayer player, string message)
        {
            bool found = false;
            string arg1 = GetArgument.Run(1, false, true, message);
            string msg;
            if (String.IsNullOrEmpty(arg1))
            {
                arg1 = player.playerData.username;
                msg = "yourself";
            }
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.username == arg1 && !shPlayer.svPlayer.serverside || shPlayer.ID.ToString() == arg1.ToString() && !shPlayer.svPlayer.serverside)
                {
                    msg = shPlayer.username;
                    shPlayer.ClearCrimes();
                    shPlayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, 33, shPlayer.ID);
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Cleared crimes of '" + msg + "'.</color>");
                    found = true;
                }
            if (!found)
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
        }
    }
}
