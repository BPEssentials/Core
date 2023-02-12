using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;

namespace BPEssentials.Commands
{
    public class Knockout : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.svPlayer.SvForceStance(StanceIndex.KnockedOut);
            player.TS("ko", target.username.CleanerMessage());
        }
    }
}


