using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.Interfaces;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.Commands
{
    public class OnlinePlayers : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory PlayerFactory { get; set; }

        public void Invoke(ShPlayer player)
        {
            player.SendChatMessage($"Online players ({PlayerFactory.Players.Count}): {string.Join(", ", PlayerFactory.Players.Select(x => x.Key + ": " + x.Value.Client.username.SanitizeString()))}");
        }
    }
}
