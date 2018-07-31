using static BP_Essentials.EssentialsVariablesPlugin;
using UnityEngine;
namespace BP_Essentials.Commands
{
    public class TpToApartment : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            switch (arg1)
            {
                case "1k":
                case "1.2k":
                case "1":
                case "1.200":
                case "1000":
                    player.SvReset(new Vector3(643.1F, 0, -61.4F), Quaternion.identity, 0);
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported to 1.2k apartment.</color>");
                    break;
                case "5k":
                case "2":
                case "5000":
                    player.SvReset(new Vector3(518.8F, 0, -127.2F), Quaternion.identity, 0);
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported to 5k apartment.</color>");
                    break;
                case "10k":
                case "10":
                case "3":
                case "10000":
                    player.SvReset(new Vector3(1.7F, 0, -92.8F), Quaternion.identity, 0);
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Teleported to 10k apartment.</color>");
                    break;
                default:
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"{GetArgument.Run(0, false, false, message)} [1.2k, 5k, 10k]");
                    break;
            }
        }
    }
}
