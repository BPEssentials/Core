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
            player.SendChatMessage($"Sent a TPA request to {target.username.SanitizeString()}.{(eTarget.CurrentChat == Chat.Disabled ? " Their chat is currently disabled, they will not recieve any message about your request." : "")}");
            if (eTarget.CurrentChat == Chat.Disabled)
            {
                return;
            }
            // TODO: Softcode value
            target.SendChatMessage($"{player.username.SanitizeString()} sent you a TPA request! Type /tpaccept to accept it.");
        }
    }
}
