using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class FakeLeave : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdFakeLeaveExecutableBy))
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (!String.IsNullOrEmpty(arg1))
                    {
                        player.SendToAll(Channel.Unsequenced, ClPacket.GameMessage, arg1 + " disconnected");
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
