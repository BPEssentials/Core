using static BP_Essentials.Variables;
using UnityEngine;
using System;

namespace BP_Essentials.Commands
{
	[Obsolete]
	public class TpToApartment
    {
		[Obsolete]
		public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            switch (arg1)
            {
                case "1k":
                case "1.2k":
                case "1":
                case "1.200":
                case "1000":
                    player.ResetAndSavePosition(new Vector3(643.1F, 0, -61.4F), Quaternion.identity, 0);
                    player.SendChatMessage($"<color={infoColor}>Teleported to 1.2k apartment.</color>");
                    break;
                case "5k":
                case "2":
                case "5000":
                    player.ResetAndSavePosition(new Vector3(518.8F, 0, -127.2F), Quaternion.identity, 0);
                    player.SendChatMessage($"<color={infoColor}>Teleported to 5k apartment.</color>");
                    break;
                case "10k":
                case "10":
                case "3":
                case "10000":
                    player.ResetAndSavePosition(new Vector3(1.7F, 0, -92.8F), Quaternion.identity, 0);
                    player.SendChatMessage($"<color={infoColor}>Teleported to 10k apartment.</color>");
                    break;
                default:
                    player.SendChatMessage($"{GetArgument.Run(0, false, false, message)} [1.2k, 5k, 10k]");
                    break;
            }
        }
    }
}
