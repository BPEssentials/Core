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
            if (!Core.Instance.SvManager.TryGetUserData(targetAccount, out var target))
            {
                player.TS("user_not_found", targetAccount.CleanerMessage());
                return;  
            }
            if (!target.BanInfo.IsBanned)
            {
                player.TS("user_not_ban", targetAccount.CleanerMessage());
                return;
            }
            target.Unban();
            Core.Instance.SvManager.database.Users.Upsert(target);
            player.TS("unbanned", targetAccount.CleanerMessage());
        }
    }
}
