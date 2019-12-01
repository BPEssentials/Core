using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Kick : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, string reason = "none")
        {
            Util.SendToAllEnabledChatT("all_kick", player.username.SanitizeString(), target.username.SanitizeString(), reason.SanitizeString());
            player.TS("player_kick", target.username.SanitizeString(), reason.SanitizeString());
            player.svPlayer.SvKick(target.ID, reason);
        }
    }
}
