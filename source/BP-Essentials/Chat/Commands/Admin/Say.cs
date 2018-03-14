using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Text.RegularExpressions;

namespace BP_Essentials.Commands
{
    class Say : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdSayExecutableBy == "admins" || CmdSayExecutableBy == "everyone")
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (String.IsNullOrEmpty(arg1))
                        player.SendToSelf(Channel.Unsequenced, 10, ArgRequired);
                    else
                    {
                        arg1 = new Regex("(<)").Replace(arg1, "<<b></b>");
                        player.SendToAll(Channel.Unsequenced, 10, $"<color={MsgSayColor}>{MsgSayPrefix} {player.playerData.username}: {arg1}</color>");
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
