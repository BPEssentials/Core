using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class UnBan : Command
    {
        public void Invoke(ShPlayer player, string targetAccount)
        {
            if (player.HasPermissionBP(PermEnum.UnbanAccount) && Core.Instance.SvManager.TryGetUserData(targetAccount, out var target))
            {
                target.Unban();
                Core.Instance.SvManager.database.Users.Upsert(target);
            }
        }
    }
}
