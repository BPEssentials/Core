using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Kill : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.svPlayer.SvDestroySelf();
            player.TS("killed", target.username.CleanerMessage());
        }
    }
}
