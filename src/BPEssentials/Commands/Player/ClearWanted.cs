using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class ClearWanted : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.ClearCrimes();
            target.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ClearCrimes, target.ID);
            player.TS("cleared_crimes", target.username.CleanerMessage());
        }
    }
}
