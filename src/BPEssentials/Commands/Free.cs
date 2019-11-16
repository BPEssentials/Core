using BPEssentials.Abstractions;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Free : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.svPlayer.UnRestrain();
            player.SendChatMessage($"Freed {target.username.SanitizeString()}.");
        }
    }
}
