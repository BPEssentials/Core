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
    public class TpaUser : ICommand
    {
        public bool LastArgSpaces { get; }

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target)
        {
            var eTarget = target.GetExtendedPlayer();
            eTarget.TpaUser = player;
            player.SendChatMessage($"Sent a TPA request to {target.username.SanitizeString()}.{(eTarget.CurrentChat == ExtendedPlayer.PlayerItem.Chat.Disabled ? " Their chat is currently disabled, they will not recieve any message about your request." : "")}");
            if (eTarget.CurrentChat == ExtendedPlayer.PlayerItem.Chat.Disabled)
                return;
            // TODO: Softcode value
            target.SendChatMessage($"{player.username.SanitizeString()} sent you a TPA request! Type /tpaccept to accept it.");
        }
    }
}
