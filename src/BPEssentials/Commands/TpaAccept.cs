using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaAccept : Command
    {
        public void Invoke(ShPlayer player)
        {
            var ePlayer = player.GetExtendedPlayer();
            if (ePlayer.TpaUser == null)
            {
                player.SendChatMessage("You have no TPA requests, or the user went offline.");
                return;
            }
            ePlayer.TpaUser.SendChatMessage($"{player.username.SanitizeString()} Accepted your TPA request.");
            player.SendChatMessage($"You accepted the TPA request of {ePlayer.TpaUser.username.SanitizeString()}.");
            ePlayer.TpaUser.GetExtendedPlayer().ResetAndSavePosition(player);
            ePlayer.TpaUser = null;
        }
    }
}
