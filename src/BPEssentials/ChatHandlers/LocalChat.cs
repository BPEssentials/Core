using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Utility.Networking;
using BPEssentials.Utils;

namespace BPEssentials.ChatHandlers
{
    public class LocalChat : IScript
    {
        [Target(GameSourceEvent.PlayerLocalChatMessage, ExecutionMode.Override)]
        public void OnEvent(ShPlayer player, string message)
        {
            if (player.manager.svManager.chatted.OverLimit(player))
            {
                return;
            }

            Core.Instance.Logger.LogInfo($"[LOCAL] {player.username.CleanerMessage()}: {message.CleanerMessage()}");


            if (CommandHandler.OnEvent(player, message)) // 'true' if message starts with command prefix
            {
                return;
            }

            player.manager.svManager.chatted.Add(player);

            player.svPlayer.Send(SvSendType.LocalOthers, Channel.Unsequenced, ClPacket.LocalChatMessage, player.ID, ChatUtils.FormatMessage(player, message, "localformat"));
        }
    }
}
