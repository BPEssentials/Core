using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Kick : BpeCommand
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, ShPlayer target, string reason = "No reason provided.")
        {
            ChatUtils.SendToAllEnabledChatT("all_kick", player.username.CleanerMessage(), target.username.CleanerMessage(), reason.CleanerMessage());
            player.TS("player_kick", target.username.CleanerMessage(), reason.CleanerMessage());
            player.svPlayer.SvKick(target, reason);
        }
    }
}
