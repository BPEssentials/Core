using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BPEssentials.Interfaces;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ReplyUser : ICommand
    {
        public bool LastArgSpaces { get; } = true;

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, string message)
        {
            var ePlayer = player.GetExtendedPlayer();
            if (ePlayer.ReplyToUser == null)
            {
                player.SendChatMessage("There is nobody to respond to, or the user went offline.");
                return;
            }
            var eTarget = ePlayer.ReplyToUser.GetExtendedPlayer();
            if (eTarget.CurrentChat == ExtendedPlayer.PlayerItem.Chat.Disabled)
            {
                player.SendChatMessage("This user disabled their chat. Your message will not be sent.");
                return;
            }
            ePlayer.ReplyToUser = eTarget.Client;
            player.GetExtendedPlayer().SendPmMessage(eTarget.Client, message);
        }
    }
}
