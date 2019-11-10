using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BPEssentials.Interfaces;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Teleport : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target)
        {
            player.GetExtendedPlayer().ResetAndSavePosition(target);
            player.SendChatMessage($"You've been teleported to {target.username.SanitizeString()}.");
        }
    }
}
