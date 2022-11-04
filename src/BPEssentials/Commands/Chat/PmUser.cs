using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class PmUser : BpeCommand
    {
        public override bool LastArgSpaces => true;

        public void Invoke(ShPlayer player, ShPlayer target, string message)
        {
            var eTarget = target.GetExtendedPlayer();
            if (eTarget.CurrentChat == Chat.Disabled)
            {
                player.SendChatMessage($"{player.T("user_has_chat_disabled")} {player.T("message_not_sent")}");
                return;
            }
            player.GetExtendedPlayer().SendPmMessage(eTarget, message);
        }
    }
}
