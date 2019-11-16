using BPEssentials.Abstractions;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Feed : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.RestoreStats();
            player.SendChatMessage($"Replenished stats for '{target.username.SanitizeString()}'.");
        }
    }
}
