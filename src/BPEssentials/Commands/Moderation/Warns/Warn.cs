using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BPEssentials.Utils;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;

namespace BPEssentials.Commands
{
    public class Warn : BpeCommand
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, string target, string reason)
        {
            if (EntityCollections.TryGetPlayerByNameOrID(target, out var shTarget))
            {
                shTarget.AddWarn(player, reason);
                ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), shTarget.username.CleanerMessage(), reason.CleanerMessage());
                player.TS("player_warn", shTarget.username.CleanerMessage(), reason.CleanerMessage());
                shTarget.TS("target_warn", shTarget.username.CleanerMessage(), reason.CleanerMessage());
                return;
            }

            if (SvManager.Instance.TryGetUserData(target, out var user))
            {
                user.AddWarn(player, reason);
                ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), target.CleanerMessage(), reason.CleanerMessage());
                player.TS("player_warn", target.CleanerMessage(), reason.CleanerMessage());
                SvManager.Instance.database.Users.Upsert(user);
                return;
            }

            player.TS("user_not_found", target.CleanerMessage());
        }
    }
}
