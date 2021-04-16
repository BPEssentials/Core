using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.ChatHandlers
{
    public class LocalChat : IScript
    {
        private static LimitQueue<ShPlayer> chatted = new LimitQueue<ShPlayer>(8, 20f);

        [Target(GameSourceEvent.PlayerLocalChatMessage, ExecutionMode.Override)]
        public void OnEvent(ShPlayer player, string message)
        {
            if (chatted.Limit(player))
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
