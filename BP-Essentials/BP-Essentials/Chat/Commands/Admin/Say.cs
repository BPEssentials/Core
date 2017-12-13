using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Say : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username))
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (String.IsNullOrWhiteSpace(arg1))
                        player.SendToSelf(Channel.Unsequenced, (byte)10, "An argument is required for this command.");
                    else
                        player.SendToAll(Channel.Unsequenced, (byte)10, MsgSayPrefix + " " + player.playerData.username + ": " + arg1);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
