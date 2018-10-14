using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BP_Essentials.Commands
{
    public class CreateKit
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, false, message);
            var arg2 = GetArgument.Run(2, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()) || string.IsNullOrEmpty(arg2.Trim()))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            if (!int.TryParse(arg1, out int arg1i))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Cannot convert {arg1} to a integer.</color>");
                return;
            }
            if (arg1i < 0)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>The delay must be a positive number (0 to disable)</color>");
                return;
            }
            var file = Path.Combine(KitDirectory, $"{arg2}.json");
            if (File.Exists(file))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>A kit already exists with that name.</color>");
                return;
            }
            Kits.CreateKit(player, arg2, arg1i, file);
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Kit created. Please edit </color><color={argColor}>{file}</color> <color={infoColor}>to add ExecuteableBy.</color>");
        }
    }
}