using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BP_Essentials.Commands
{
    public class CreateWarp
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, false, message);
            var arg2 = GetArgument.Run(2, false, false, message);
            var arg3 = GetArgument.Run(3, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()) || string.IsNullOrEmpty(arg2.Trim()) || string.IsNullOrEmpty(arg3.Trim()))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            if (!int.TryParse(arg1, out int arg1i) || !int.TryParse(arg2, out int arg2i))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Cannot convert {arg1} or {arg2} to a integer.</color>");
                return;
            }
            if (arg1i < 0)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>The delay must be a positive number (0 to disable)</color>");
                return;
            }
            var file = Path.Combine(WarpDirectory, $"{arg3}.json");
            if (File.Exists(file))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>A warp already exists with that name.</color>");
                return;
            }
            Warps.CreateWarp(player, arg3, arg1i, arg2i, file);
            player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Warp created. Please edit </color><color={argColor}>{file}</color> <color={infoColor}>to add ExecuteableBy and Price.</color>");
        }
    }
}