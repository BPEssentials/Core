using BPEssentials.Abstractions;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Atm : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.SendChatMessage("Opening ATM Menu..");
            player.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowATMMenu, player.svPlayer.bankBalance);
        }
    }
}
