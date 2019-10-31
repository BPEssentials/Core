using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.Interfaces;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Atm : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory PlayerFactory { get; set; }

        public void Invoke(ShPlayer player)
        {
            player.SendChatMessage("Opening ATM Menu..");
            player.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowATMMenu, player.svPlayer.bankBalance);
        }
    }
}
