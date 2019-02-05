using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;
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
                player.SendChatMessage(ArgRequired);
                return;
            }
            var file = Path.Combine(KitDirectory, $"{arg1}.json");
            if (!File.Exists(file))
            {
                player.SendChatMessage($"<color={errorColor}>No kits exist with that name.</color>");
                return;
            }
            Variables.WarpHandler.DeleteExisting(arg1);
            player.SendChatMessage($"<color={infoColor}>Kit deleted.</color>");
        }
    }
}