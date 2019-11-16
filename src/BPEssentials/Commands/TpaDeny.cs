using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaDeny : Command
    {
        public void Invoke(ShPlayer player)
        {
            var ePlayer = player.GetExtendedPlayer();
            if (ePlayer.TpaUser == null)
            {
                player.SendChatMessage("You have no TPA requests, or the user went offline.");
                return;
            }
            ePlayer.TpaUser.SendChatMessage($"{player.username.SanitizeString()} Denied your TPA request.");
            player.SendChatMessage($"You denied the TPA request of {ePlayer.TpaUser.username.SanitizeString()}.");
            ePlayer.TpaUser = null;
        }
    }
}
