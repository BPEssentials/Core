using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

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
            player.svMovable.Send(SvSendType.Self, Channel.Reliable, 98, new object[]
            {
                    target.IP,
                    false
            });
            player.TS("unbanned", targetAccount.CleanerMessage());
        }
    }
}