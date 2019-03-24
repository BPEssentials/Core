using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BP_Essentials.Commands
{
    public class CreateWarp
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, false, message); // (int)DelayInSeconds
            var arg2 = GetArgument.Run(2, false, false, message); // (int)Price
            var arg3 = GetArgument.Run(3, false, true, message); // (string)KitName
            if (string.IsNullOrEmpty(arg1.Trim()) || string.IsNullOrEmpty(arg2.Trim()) || string.IsNullOrEmpty(arg3.Trim()))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            if (!int.TryParse(arg1, out int arg1i) || !int.TryParse(arg2, out int arg2i))
            {
                player.SendChatMessage($"<color={errorColor}>Cannot convert {arg1} or {arg2} to a integer.</color>");
                return;
            }
            if (arg1i < 0)
            {
                player.SendChatMessage($"<color={errorColor}>The delay must be a positive number (0 to disable)</color>");
                return;
            }
            if (player.InApartment())
            {
                player.SendChatMessage($"<color={errorColor}>You cannot set a warp inside any apartment, as they are not fixed places.</color>");
                return;
            }
            var file = Path.Combine(WarpDirectory, $"{arg3}.json");
            if (File.Exists(file))
            {
                player.SendChatMessage($"<color={errorColor}>A warp already exists with that name.</color>");
                return;
            }
            var obj = new WarpHandler.JsonModel
            {
                Delay = arg1i,
                Price = arg2i < 0 ? 0 : arg2i,
                Name = arg3,
                ExecutableBy = "everyone",
                Position = new WarpHandler.Position { X = player.player.GetPosition().x, Y = player.player.GetPosition().y, Z = player.player.GetPosition().z, PlaceIndex = player.player.GetPlaceIndex() },
                Rotation = new WarpHandler.Rotation { X = player.player.GetRotation().x, Y = player.player.GetRotation().y, Z = player.player.GetRotation().z, W = player.player.GetRotation().w }
            };
            Variables.WarpHandler.CreateNew(obj, arg3);
            player.SendChatMessage($"<color={infoColor}>Warp created. Please edit </color><color={argColor}>{file}</color> <color={infoColor}>to add ExecuteableBy.</color>");
        }
    }
}