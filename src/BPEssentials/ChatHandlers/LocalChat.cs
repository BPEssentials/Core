using BrokeProtocol.API;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.API.Types;
using BrokeProtocol.Entities;
using System;

namespace BPEssentials.ChatHandlers
{
    public class LocalChat : IScript
    {
        public LocalChat()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Player.OnLocalChatMessage, new Action<ShPlayer, string>(OnEvent));
        }

        public void OnEvent(ShPlayer player, string message)
        {
            if (message.StartsWith(CommandHandler.Prefix))
            {
                return;
            }
            Core.Instance.Logger.LogInfo($"[LOCAL] {player.username.SanitizeString()}: {message.SanitizeString()}");
        }
    }
}
