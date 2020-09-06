using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BPEssentials.Utils;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using static BPEssentials.ExtensionMethods.Warns.ExtensionPlayerWarns;

namespace BPEssentials.Commands
{
    public class Warn : Command
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, string target, string reason, int length = -1)
        {
            if (EntityCollections.TryGetPlayerByNameOrID(target, out var shTarget))
            {
                shTarget.AddWarn(player, reason, length);
                ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), shTarget.username.CleanerMessage(), reason.CleanerMessage());
                player.TS("player_warn", shTarget.username.CleanerMessage(), reason.CleanerMessage());
                shTarget.TS("target_warn", shTarget.username.CleanerMessage(), reason.CleanerMessage());
                return;
            }

            if (Core.Instance.SvManager.TryGetUserData(target, out var user))
            {
                user.AddWarn(player, reason, length);
                ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), target.CleanerMessage(), reason.CleanerMessage());
                player.TS("player_warn", target.CleanerMessage(), reason.CleanerMessage());
                return;
            }

            player.TS("user_not_found", target.CleanerMessage());
        }
    }
}