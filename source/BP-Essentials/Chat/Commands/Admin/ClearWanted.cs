using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class ClearWanted : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;

                if (HasPermission.Run(player, CmdClearWantedExecutableBy))
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
                        if (shPlayer.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1.ToString() && shPlayer.IsRealPlayer())
                        {
                            msg = shPlayer.username;
                            shPlayer.ClearCrimes();
                            shPlayer.svPlayer.SendToSelf(Channel.Reliable, 33, shPlayer.ID);
                            player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Cleared crimes of '" + msg + "'.</color>");
                            found = true;
                        }
                    if (!found)
                        player.SendToSelf(Channel.Unsequenced, 10, NotFoundOnline);
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
