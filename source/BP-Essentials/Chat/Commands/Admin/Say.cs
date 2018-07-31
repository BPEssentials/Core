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
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (String.IsNullOrEmpty(arg1))
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
            else
            {
                arg1 = new Regex("(<)").Replace(arg1, "<<b></b>");
                player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, $"<color={MsgSayColor}>{MsgSayPrefix} {player.playerData.username}: {arg1}</color>");
            }
        }
    }
}
