using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ReplyUser : Command
    {
        public override bool LastArgSpaces => true;

        public void Invoke(ShPlayer player, string message)
        {
            var ePlayer = player.GetExtendedPlayer();
            if (ePlayer.ReplyToUser == null)
            {
                player.SendChatMessage("There is nobody to respond to, or the user went offline.");
                return;
            }
            var eTarget = ePlayer.ReplyToUser.GetExtendedPlayer();
            if (eTarget.CurrentChat == Chat.Disabled)
            {
                player.SendChatMessage("This user disabled their chat. Your message will not be sent.");
                return;
            }
            ePlayer.ReplyToUser = eTarget.Client;
            player.GetExtendedPlayer().SendPmMessage(eTarget.Client, message);
        }
    }
}
