using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaUser : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            var eTarget = target.GetExtendedPlayer();
            eTarget.TpaUser = player;
            player.SendChatMessage($"{player.T("player_tpa_sent", target.username.SanitizeString())}  {(eTarget.CurrentChat == Chat.Disabled ? player.T("player_tpa_chat_disabled") : "")}");
            if (eTarget.CurrentChat == Chat.Disabled)
            {
                return;
            }
            target.TS("target_tpa_sent", player.username.SanitizeString());
        }
    }
}
