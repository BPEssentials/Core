using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BPEssentials.Utils;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Warn : Command
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, ShPlayer target, string reason)
        {
            target.AddWarn(player, reason);
            ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), target.username.CleanerMessage(), reason.CleanerMessage());
            player.TS("player_warn", target.username.CleanerMessage(), reason.CleanerMessage());
            player.TS("target_warn", player.username.CleanerMessage(), reason.CleanerMessage());
        }
    }
}
