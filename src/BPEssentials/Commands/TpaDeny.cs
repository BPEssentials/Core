using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtensionMethods;
using BPEssentials.Interfaces;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaDeny : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory PlayerFactory { get; set; }

        public void Invoke(ShPlayer player)
        {
            var ePlayer = player.GetExtendedPlayer();
            if (ePlayer.TpaUser == null)
            {
                player.SendChatMessage("You have no TPA requests, or the user went offline.");
                return;
            }
            ePlayer.TpaUser.SendChatMessage($"{player.username.SanitizeString()} Denied your TPA request.");
            player.SendChatMessage($"You denied the TPA request of {ePlayer.TpaUser.username.SanitizeString()}.");
            ePlayer.TpaUser = null;
        }
    }
}
