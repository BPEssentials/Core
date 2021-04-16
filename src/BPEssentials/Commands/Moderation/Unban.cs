using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Unban : Command
    {
        public void Invoke(ShPlayer player, string targetAccount)
        {
            if (!Core.Instance.SvManager.TryGetUserData(targetAccount, out var target))
            {
                player.TS("user_not_found", targetAccount.CleanerMessage());
                return;
            }
            Core.Instance.SvManager.database.Bans.Delete(target.IP);
            player.TS("unbanned", targetAccount.CleanerMessage());
        }
    }
}