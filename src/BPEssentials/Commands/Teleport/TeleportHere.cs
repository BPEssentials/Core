using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;

namespace BPEssentials.Commands
{
    public class TeleportHere : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.GetExtendedPlayer().ResetAndSavePosition(player);
            player.TS("player_tph", target.username.CleanerMessage());
            target.TS("target_tph", player.username.CleanerMessage());
        }
    }
}
