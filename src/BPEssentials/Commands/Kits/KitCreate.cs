using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System;
using System.IO;

namespace BPEssentials.Commands
{
    public class KitCreate : Command
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
                price = 0;
                foreach (var item in player.myItems.Values)
                {
                    price += item.count * item.currentValue;
                }
                player.TS("kit_price_automatically_calculated", price);
            }
            var file = Path.Combine(Core.Instance.Paths.KitsFolder, $"{kit}.json");
            if (File.Exists(file))
            {
                player.TS("expFileHandler_create_error_alreadyExists", player.T(Core.Instance.KitHandler.Name), kit);
                return;
            }
            var obj = new KitHandler.JsonModel
            {
                Delay = Math.Max(0, delay),
                Price = Math.Max(0, price),
                Name = kit,
            };
            foreach (var item in player.myItems.Values)
            {
                obj.Items.Add(new KitHandler.KitsItem { Amount = item.count, Id = item.item.index });
            }
            Core.Instance.KitHandler.CreateNew(obj, kit);
            player.TS("kitCreate_created", kit, price.ToString(), delay.ToString());
        }
    }
}
