using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Jail : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdJailExecutableBy))
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (!string.IsNullOrEmpty(arg1))
                    {
                        int lastIndex = arg1.LastIndexOf(" ");
                        if (lastIndex != -1)
                            arg1 = arg1.Remove(lastIndex).Trim();
                        ExecuteOnPlayer.Run(player, message, arg1);
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
