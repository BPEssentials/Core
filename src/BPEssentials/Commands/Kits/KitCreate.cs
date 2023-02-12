using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System;
using System.IO;
using System.Linq;
using BrokeProtocol.Utility;

namespace BPEssentials.Commands
{
    public class KitCreate : BpeCommand
    {
        public void Invoke(ShPlayer player, string kit, int price = 0, int delay = 0)
        {
            if (delay < 0)
            {
                player.TS("delay_error_negative");
                return;
            }
            if (price < 0)
            {
                price = player.myItems.Values.Sum(item => item.count * item.currentValue);
                player.TS("kit_price_automatically_calculated", price);
            }

            string file = Path.Combine(Paths.KitsFolder, $"{kit}.json");
            if (File.Exists(file))
            {
                player.TS("expFileHandler_create_error_alreadyExists", player.T(Core.Instance.KitHandler.Name), kit);
                return;
            }

            KitHandler.JsonModel obj = new KitHandler.JsonModel
            {
                Delay = Math.Max(0, delay),
                Price = Math.Max(0, price),
                Name = kit,
            };
            foreach (InventoryItem item in player.myItems.Values)
            {
                obj.Items.Add(new KitHandler.KitsItem { Amount = item.count, Id = item.item.index });
            }

            Core.Instance.KitHandler.CreateNew(obj, kit);
            player.TS("kitCreate_created", kit, price.ToString(), delay.ToString());
        }
    }
}
