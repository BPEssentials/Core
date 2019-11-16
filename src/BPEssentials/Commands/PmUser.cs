using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class PmUser : Command
    {
        public override bool LastArgSpaces => true;

        public void Invoke(ShPlayer player, ShPlayer target, string message)
        {
            var eTarget = target.GetExtendedPlayer();
            if (eTarget.CurrentChat == Chat.Disabled)
            {
                player.SendChatMessage("This user disabled their chat. Your message will not be sent.");
                return;
            }
            eTarget.ReplyToUser = player;
            player.GetExtendedPlayer().SendPmMessage(target, message);
        }
    }
}
