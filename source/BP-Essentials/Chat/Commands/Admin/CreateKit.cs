using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BP_Essentials.Commands
{
    public class CreateKit
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
            var file = Path.Combine(KitDirectory, $"{arg3}.json");
            if (File.Exists(file))
            {
                player.SendChatMessage($"<color={errorColor}>A kit already exists with that name.</color>");
                return;
            }
			var obj = new KitsHandler.JsonModel
			{
				Delay = arg1i,
				Price = arg2i < 0 ? 0 : arg2i,
				Name = arg3,
				ExecutableBy = "everyone"
			};
			foreach (var item in player.player.myItems.Values)
				obj.Items.Add(new KitsHandler.Kits_Item { Amount = item.count, Id = item.item.index });
			Variables.KitsHandler.CreateNew(obj, arg3);
            player.SendChatMessage($"<color={infoColor}>Kit created. Please edit </color><color={argColor}>{file}</color> <color={infoColor}>to add ExecuteableBy.</color>");
        }
    }
}