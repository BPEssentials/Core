using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TeleportHere : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.GetExtendedPlayer().ResetAndSavePosition(player);
            player.SendChatMessage($"You've teleported {target.username.SanitizeString()} to you.");
            target.SendChatMessage($"{player.username.SanitizeString()} has teleported you to him.");
        }
    }
}
