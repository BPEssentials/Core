using BPCoreLib.Interfaces;
using BPCoreLib.PlayerFactory;
using BPEssentials.Configuration.Models.SettingsModel;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BPEssentials.Interfaces;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class PmUser : ICommand
    {
        public bool LastArgSpaces { get; } = true;

        public ILogger Logger { get; set; }

        public Settings Settings { get; set; }

        public ExtendedPlayerFactory<PlayerItem> PlayerFactory { get; set; }

        public void Invoke(ShPlayer player, ShPlayer target, string message)
        {
            var eTarget = target.GetExtendedPlayer();
            if (eTarget.CurrentChat == ExtendedPlayer.PlayerItem.Chat.Disabled)
            {
                player.SendChatMessage("This user disabled their chat. Your message will not be sent.");
                return;
            }
            eTarget.ReplyToUser = player;
            player.GetExtendedPlayer().SendPmMessage(target, message);
        }
    }
}
