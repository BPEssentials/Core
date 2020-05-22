using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;
namespace BPEssentials.Commands
{
    public class Pay : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, int amount=1)
        {
            if (0 >= amount)
            {
                player.TS("pay_error_negative");
                return;
            }
            if (player.MyMoneyCount() < amount)
            {
                player.TS("you_dont_own", amount.ToString());
                return;
            }
            player.TransferMoney(DeltaInv.RemoveFromMe, amount, true);
            target.TransferMoney(DeltaInv.AddToMe, amount, true);
            player.TS("paid_to", amount.ToString(), target.username);
            target.TS("received_from", amount.ToString(), player.username);
        }
    }
}
