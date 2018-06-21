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
                    if (shPlayer.IsRealPlayer())
                        if (shPlayer.wantedLevel == 0)
                        {
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Opening ATM menu..</color>");
                            player.SendToSelf(Channel.Reliable, 40, player.bankBalance);
                        }
                        else if (shPlayer.wantedLevel != 0)
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Criminal Activity: Account Locked</color>");
        }
    }
}
