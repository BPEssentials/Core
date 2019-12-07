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
                player.TS("no_tpa_requests");
                return;
            }
            ePlayer.TpaUser.TS("TpaUser_tpa_accepted", player.username.SanitizeString());
            player.TS("player_tpa_accepted", ePlayer.TpaUser.username.SanitizeString());
            ePlayer.TpaUser.GetExtendedPlayer().ResetAndSavePosition(player);
            ePlayer.TpaUser = null;
        }
    }
}
