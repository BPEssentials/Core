using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Atm : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("open_atm");
            player.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowATMMenu, player.svPlayer.bankBalance);
        }
    }
}
