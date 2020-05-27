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
            if (player.HasPermissionBP(PermEnum.UnbanAccount))
            {
                if (Core.Instance.SvManager.TryGetUserData(targetAccount, out var target))
                {
                    if (target.BanInfo.IsBanned)
                    {
                        target.Unban();
                        Core.Instance.SvManager.database.Users.Upsert(target);
                        player.TS("unbanned", targetAccount.CleanerMessage());
                        return;
                    }
                    player.TS("usernot_ban", targetAccount.CleanerMessage());
                    return;
                }
                player.TS("usernot_found", targetAccount.CleanerMessage());
                return;
            }
            player.TS("no_permission");
        }
    }
}