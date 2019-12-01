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
                player.TS("no_user_found_or_offline");
                return;
            }
            var eTarget = ePlayer.ReplyToUser.GetExtendedPlayer();
            if (eTarget.CurrentChat == Chat.Disabled)
            {
                player.SendChatMessage($"{player.T("user_has_chat_disabled")} {player.T("message_not_sent")}");
                return;
            }
            ePlayer.ReplyToUser = eTarget.Client;
            player.GetExtendedPlayer().SendPmMessage(eTarget.Client, message);
        }
    }
}
