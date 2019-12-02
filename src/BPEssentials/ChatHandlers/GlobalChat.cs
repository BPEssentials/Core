using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System;

namespace BPEssentials.ChatHandlers
{
    public class GlobalChat : IScript
    {
        public GlobalChat()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Player.OnGlobalChatMessage, new Action<ShPlayer, string>(OnEvent));
        }

        public void OnEvent(ShPlayer player, string message)
        {
            if (message.StartsWith(CommandHandler.Prefix))
            {
                return;
            }
            Core.Instance.Logger.LogInfo($"[GLOBAL] {player.username}: {message}");
            switch (player.GetExtendedPlayer().CurrentChat)
            {
                case Chat.StaffChat:
                    ChatUtils.SendStaffChatMessage(player, message);
                    return;

                case Chat.Disabled:
                    player.TS("chat_disabled");
                    return;

                default:
                    ChatUtils.SendToAllEnabledChat($"{player.username.SanitizeString()}: {message.SanitizeString()}");
                    return;
            }
        }
    }
}
