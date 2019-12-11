using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Kick : Command
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, ShPlayer target, string reason = "No reason provided.")
        {
            ChatUtils.SendToAllEnabledChatT("all_kick", player.username.SanitizeString(), target.username.SanitizeString(), reason.SanitizeString());
            player.TS("player_kick", target.username.SanitizeString(), reason.SanitizeString());
            player.svPlayer.SvKick(target.ID, reason);
        }
    }
}
