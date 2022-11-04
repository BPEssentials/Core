using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;

namespace BPEssentials.Commands
{
    public class Unban : BpeCommand
    {
        public void Invoke(ShPlayer player, string targetAccount)
        {
            if (!SvManager.Instance.TryGetUserData(targetAccount, out var target))
            {
                player.TS("user_not_found", targetAccount.CleanerMessage());
                return;
            }
            SvManager.Instance.database.Bans.Delete(target.IP);
            player.TS("unbanned", targetAccount.CleanerMessage());
        }
    }
}
