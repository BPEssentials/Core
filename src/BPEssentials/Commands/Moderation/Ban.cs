using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Ban : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, string reason = "none")
        {
            Util.SendToAllEnabledChatT("all_ban", player.username.SanitizeString(), target.username.SanitizeString(), reason.SanitizeString());
            player.TS("player_banned", target.username.SanitizeString(), reason.SanitizeString());
            player.svPlayer.SvBan(target.ID, reason);
        }
    }
}
