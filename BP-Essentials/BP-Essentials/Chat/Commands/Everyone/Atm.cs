﻿using System;
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
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdAtmExecutableBy == "admins" || CmdAtmExecutableBy == "everyone")
                {
                    foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                        if (shPlayer.svPlayer == player)
                            if (shPlayer.IsRealPlayer())
                                if (shPlayer.wantedLevel == 0)
                                {
                                    player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Opening ATM menu..</color>");
                                    player.SendToSelf(Channel.Reliable, 40, player.bankBalance);
                                }
                                else if (shPlayer.wantedLevel != 0)
                                    player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Criminal Activity: Account Locked</color>");
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
