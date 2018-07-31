using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Atm : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player)
        {
            foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                if (shPlayer.svPlayer == player)
                    if (!shPlayer.svPlayer.IsServerside())
                        if (shPlayer.wantedLevel == 0)
                        {
                            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Opening ATM menu..</color>");
                            player.Send(SvSendType.Self, Channel.Reliable, 40, player.bankBalance);
                        }
                        else if (shPlayer.wantedLevel != 0)
                            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Criminal Activity: Account Locked</color>");
        }
    }
}
