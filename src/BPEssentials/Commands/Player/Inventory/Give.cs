using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;

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
            var item = Core.Instance.EntityHandler.Items.FirstOrDefault(x => x.Key == name);
            if (item.Value == null)
            {
                if (Core.Instance.Settings.Levenshtein.GiveMode == Configuration.Models.SettingsModel.LevenshteinMode.None)
                {
                    player.TS("give_notFound", name);
                    return;
                }
                item = Core.Instance.EntityHandler.Items.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Key, name)).FirstOrDefault();
                
                if (Core.Instance.Settings.Levenshtein.GiveMode == Configuration.Models.SettingsModel.LevenshteinMode.Suggest)
                {
                    player.TS("give_notFound", name);
                    player.TS("levenshteinSuggest", item.Key);
                    return;
                }
            }
            target.TransferItem(DeltaInv.AddToMe, item.Value.index, amount, true);
            player.TS("player_give", target.username.CleanerMessage(), item.Key, amount.ToString());
        }
    }
}
