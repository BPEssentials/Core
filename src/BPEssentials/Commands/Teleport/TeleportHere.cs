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
            player.TS("player_tph", target.username.SanitizeString());
            target.TS("target_tph", player.username.SanitizeString());
        }
    }
}
