using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Give : Command
    {
        public void Invoke(ShPlayer player, string name, int amount = 1, ShPlayer target = null)
        {
            target = target ?? player;
            var item = Core.Instance.EntityHandler.Items.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Key, name)).FirstOrDefault();
            target.TransferItem(DeltaInv.AddToMe, item.Value.index, amount, true);
            player.TS("player_give", target.username.SanitizeString(), name, amount.ToString());
        }
    }
}
