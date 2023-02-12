using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Feed : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.svPlayer.UpdateStatsDelta(1f, 1f, 1f);
            player.TS("feed", target.username.CleanerMessage());
        }
    }
}
