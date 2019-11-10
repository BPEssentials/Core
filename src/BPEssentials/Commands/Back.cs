using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BPEssentials.Interfaces;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Back : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target)
        {
            var lastLocation = target.GetExtendedPlayer().lastLocation;
            if (!lastLocation.HasPositionSet())
            {
                player.SendChatMessage($"There is no location to teleport to.");
                return;
            }
            target.GetExtendedPlayer().ResetAndSavePosition(lastLocation.Position, lastLocation.Rotation, lastLocation.PlaceIndex);
            target.SendChatMessage("You've been teleported to your last known location.");
        }
    }
}
