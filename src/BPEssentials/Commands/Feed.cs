using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.Interfaces;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Feed : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.RestoreStats();
            player.SendChatMessage($"Replenished stats for '{target.username.SanitizeString()}'.");
        }
    }
}
