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
            if (player.manager.svManager.chatted.Limit(player))
            {
                return;
            }

            Core.Instance.Logger.LogInfo($"[LOCAL] {player.username.CleanerMessage()}: {message.CleanerMessage()}");


            if (CommandHandler.OnEvent(player, message)) // 'true' if message starts with command prefix
            {
                return;
            }

            if (Core.Instance.Settings.General.LocalChatOverHead)
            {
                player.svPlayer.Send(SvSendType.LocalOthers, Channel.Unsequenced, ClPacket.LocalChatMessage, player.ID, message.CleanerMessage());
            }
            if (Core.Instance.Settings.General.LocalChatInChat)
            {
                player.svPlayer.Send(SvSendType.LocalOthers, Channel.Unsequenced, ClPacket.GameMessage, ChatUtils.FormatMessage(player, message, "localformat"));
            }
        }
    }
}
