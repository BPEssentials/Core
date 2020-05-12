using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Ban : Command
    {
        public override bool LastArgSpaces { get; } = true;

        public void Invoke(ShPlayer player, string targetAccount, string reason = "No reason provided.")
        {
            if (player.HasPermissionBP(PermEnum.UnbanAccount) && Core.Instance.SvManager.TryGetUserData(targetAccount, out var target))
            {
                target.Unban();
                Core.Instance.SvManager.database.Users.Upsert(target);
                player.svPlayer.Send(SvSendType.Self, Channel.Reliable, 98, targetAccount, false);
            }
        }
    }
}