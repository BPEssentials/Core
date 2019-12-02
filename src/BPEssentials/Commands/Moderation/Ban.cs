using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Ban : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, string reason = "No reason provided.")
        {
            ChatUtils.SendToAllEnabledChatT("all_ban", player.username.SanitizeString(), target.username.SanitizeString(), reason.SanitizeString());
            player.TS("player_banned", target.username.SanitizeString(), reason.SanitizeString());
            player.svPlayer.SvBan(target.ID, reason);
        }
    }
}
