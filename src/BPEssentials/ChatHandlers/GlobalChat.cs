using BPEssentials.Enums;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;

namespace BPEssentials.ChatHandlers
{
    public class GlobalChat : IScript
    {
        private static LimitQueue<ShPlayer> chatted = new LimitQueue<ShPlayer>(8, 20f);

        [Target(GameSourceEvent.PlayerChatGlobal, ExecutionMode.Override)]
        public void OnEvent(ShPlayer player, string message)
        {
            if (chatted.Limit(player))
            {
                return;
            }
            if (CommandHandler.OnEvent(player, message))
            {
                return;
            }

            PlayerItem ePlayer = player.GetExtendedPlayer();
            if (ePlayer.Muted)
            {
                player.TS("muted_player");
                return;
            }

            Core.Instance.Logger.LogInfo($"[GLOBAL] {player.username}: {message}");

            switch (ePlayer.CurrentChat)
            {
                case Chat.StaffChat:
                    ChatUtils.SendStaffChatMessage(player, message);
                    return;

                case Chat.Disabled:
                    player.TS("chat_disabled");
                    return;

                default:
                    ChatUtils.SendToAllEnabledChat(ChatUtils.FormatMessage(player, message), false);
                    return;
            }
        }
    }
}
