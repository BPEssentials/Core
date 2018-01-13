using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Feed : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdFeedExecutableBy == "admin" || CmdFeedExecutableBy == "everyone")
                {
                    string arg1 = GetArgument.Run(1, false, true, message).Trim();
                    const string msg = "Maxed stats for {0}.";
                    if (String.IsNullOrWhiteSpace(arg1))
                    {
                        for (byte i = 0; i < 4; i++)
                            player.UpdateStat(i, 100);
                        player.SendToSelf(Channel.Unsequenced, 10, String.Format(msg, "yourself"));
                    }
                    else
                    {
                        bool found = false;
                        foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                            if (shPlayer.svPlayer.playerData.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                                if (shPlayer.IsRealPlayer())
                                {
                                    for (byte i = 0; i < 4; i++)
                                        shPlayer.svPlayer.UpdateStat(i, 100);
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