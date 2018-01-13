using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Heal : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdHealExecutableBy == "admin" || CmdHealExecutableBy == "everyone")
                {
                    string arg1 = GetArgument.Run(1, false, true, message).Trim();
                    const string msg = "Healed {0}.";
                    if (String.IsNullOrWhiteSpace(arg1))
                    {
                        player.Heal(100);
                        player.SendToSelf(Channel.Unsequenced, 10, String.Format(msg, "yourself"));
                    }
                    else
                    {
                        bool found = false;
                        foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                if (shPlayer.IsRealPlayer())
                                {
                                    shPlayer.svPlayer.Heal(100);
                                    player.SendToSelf(Channel.Unsequenced, 10, String.Format(msg, shPlayer.svPlayer.playerData.username));
                                    found = true;
                                }
                        if (!found)
                            player.SendToSelf(Channel.Unsequenced, 10, "Player '" + arg1 + "' Not found/online.");
                    }
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
