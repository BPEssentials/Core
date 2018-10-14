using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class FakeJoin
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (!String.IsNullOrEmpty(arg1))
                player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, arg1 + " connected");
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
        }
    }
}
