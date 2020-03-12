using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Utility;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Feed : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.RestoreStats();
            player.TS("feed", target.username.CleanerMessage());
        }
    }
}
