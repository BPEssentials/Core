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
            var player = (SvPlayer)oPlayer;
            if (AdminsListPlayers.Contains(player.playerData.username))
            {
                string arg1 = GetArgument.Run(1, false, true, message);
                if (!String.IsNullOrWhiteSpace(arg1))
                {
                    player.SendToAll(Channel.Unsequenced, (byte)10, arg1 + " disconncted");
                }
                else
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
            }
            else
                player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
            return true;
        }
    }
}
