using BPEssentials.Abstractions;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Arrest : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.svPlayer.Restrain(target.manager.handcuffed);
            player.SendChatMessage($"Arrested {target.username.SanitizeString()}.");
        }
    }
}
