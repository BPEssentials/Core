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
            var item = Core.Instance.EntityHandler.Items.FirstOrDefault(x => x.Key == "Money");
            if (player.MyItemCount(item.Value.index) < amount)
            {
                player.TS("you_dont_own", amount.ToString());
                return;
            }
            player.TransferItem(DeltaInv.RemoveFromMe, item.Value.index, amount, true);
            target.TransferItem(DeltaInv.AddToMe, item.Value.index, amount, true);
            player.TS("paid_to", amount.ToString(), target.username);
            target.TS("received_from", amount.ToString(), player.username);
        }
    }
}
