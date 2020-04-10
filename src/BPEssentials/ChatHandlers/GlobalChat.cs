using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System;

namespace BPEssentials.ChatHandlers
{
    public class GlobalChat : IScript
    {
        [Target(GameSourceEvent.PlayerGlobalChatMessage, ExecutionMode.Override)]
        public void OnEvent(ShPlayer player, string message)
        {
            if (player.manager.svManager.chatted.Limit(player))
            {
                return;
            }

            if (CommandHandler.OnEvent(player, message)) // 'true' if message starts with command prefix
            {
                return;
            }

            Core.Instance.Logger.LogInfo($"[GLOBAL] {player.username}: {message}");

            switch (player.GetExtendedPlayer().CurrentChat)
            {
                case Chat.StaffChat:
                    ChatUtils.SendStaffChatMessage(player, message);
                    return;
                case Chat.Muted:
                    player.TS("muted_player");
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
