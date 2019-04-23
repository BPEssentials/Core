using System;
using static BP_Essentials.Variables;
namespace BP_Essentials.Commands
{
    class Ban
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, false, message);
            var arg2 = GetArgument.Run(2, false, true, message);
            if (string.IsNullOrEmpty(arg1) || string.IsNullOrEmpty(arg2))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var shPlayer = GetShByStr.Run(arg1, true);
            if (shPlayer == null)
            {
                player.SendChatMessage(NotFoundOnlineIdOnly);
                return;
            }
            LogMessage.LogOther($"{PlaceholderParser.ParseTimeStamp()} [INFO] {shPlayer.username} Got banned by {player.playerData.username} (Reason: {arg2}");
            player.Send(SvSendType.All, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>{shPlayer.username}</color> <color={warningColor}>Just got banned by</color> <color={argColor}>{player.playerData.username}</color> <color={warningColor}>(Reason: </color><color={argColor}>{arg2}</color><color={warningColor}>)</color>");
            SendDiscordMessage.BanMessage(shPlayer.username, player.playerData.username, arg2);
            player.SendChatMessage($"<color={infoColor}>Banned</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>. (Reason: {arg2})</color>");
            player.svManager.AddBanned(shPlayer);
            player.svManager.Disconnect(shPlayer.svPlayer.connection, DisconnectTypes.Banned);
        }
    }
}