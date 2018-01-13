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
        public static bool Run(object oPlayer)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdAtmExecutableBy == "admin" || CmdAtmExecutableBy == "everyone")
                {
                    foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                        if (shPlayer.svPlayer == player)
                            if (shPlayer.IsRealPlayer())
                                if (shPlayer.wantedLevel == 0)
                                {
                                    player.SendToSelf(Channel.Unsequenced, 10, "Opening ATM menu..");
                                    player.SendToSelf(Channel.Reliable, 40, player.bankBalance);
                                }
                                else if (shPlayer.wantedLevel != 0)
                                    player.SendToSelf(Channel.Unsequenced, 10, "Criminal Activity: Account Locked");
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
