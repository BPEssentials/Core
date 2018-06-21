using System;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands {
    public class Ban : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (!string.IsNullOrEmpty(arg1))
            {
                var shPlayer = GetShByStr.Run(arg1);
                if (shPlayer == null)
                {
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                    return;
                }
                player.svManager.AddBanned(shPlayer);
                player.svManager.Disconnect(shPlayer.svPlayer.connection);
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Banned</color> <color={argColor}>" + shPlayer.username + $"</color><color={infoColor}>.</color>");
            }
            else
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
        }
    }
}