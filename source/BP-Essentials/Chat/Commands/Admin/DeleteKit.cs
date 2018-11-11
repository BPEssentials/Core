using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BP_Essentials.Commands
{
    public class DeleteKit
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            var file = Path.Combine(KitDirectory, $"{arg1}.json");
            if (!File.Exists(file))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>No kits exist with that name.</color>");
                return;
            }
            Kits.DeleteKit(player, file, arg1);
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Kit deleted.</color>");
        }
    }
}