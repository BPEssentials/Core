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
            GameSourceHandler.Add(BrokeProtocol.API.Events.Player.OnLocalChatMessage, new Func<ShPlayer, string, bool>(OnEvent));
        }

        public bool OnEvent(ShPlayer player, string message)
        {
            Core.Instance.Logger.LogInfo($"[LOCAL] {player.username.SanitizeString()}: {message.SanitizeString()}");
            return false;
        }
    }
}
