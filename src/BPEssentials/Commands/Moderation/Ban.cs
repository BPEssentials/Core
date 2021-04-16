using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Ban : Command
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, ShPlayer target, string reason = "No reason provided.")
        {
            ChatUtils.SendToAllEnabledChatT("all_ban", player.username.CleanerMessage(), target.username.CleanerMessage(), reason.CleanerMessage());
            player.TS("player_banned", target.username.CleanerMessage(), reason.CleanerMessage());
            player.svPlayer.SvBan(target, reason);
        }
    }
}
