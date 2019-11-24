using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class KitCreate : Command
    {
        private readonly string Type = "kit";

        public void Invoke(ShPlayer player, string name, int price = 0, int delay = 0)
        {
            if (delay < 0)
            {
                player.TS("delay_error_negative");
                return;
            }
            if (price < 0)
            {
                player.TS("price_error_negative");
                return;
            }
            var file = Path.Combine(Core.Instance.Paths.WarpsFolder, $"{name}.json");
            if (File.Exists(file))
            {
                player.TS("expFileHandler_create_error_alreadyExists", player.T(Type), name);
                return;
            }
            var obj = new KitHandler.JsonModel
            {
                Delay = delay < 0 ? 0 : price,
                Price = price < 0 ? 0 : price,
                Name = name,
            };
            foreach (var item in player.myItems.Values)
            {
                obj.Items.Add(new KitHandler.KitsItem { Amount = item.count, Id = item.item.index });
            }
            Core.Instance.KitHandler.CreateNew(obj, name);
            player.TS("kitCreate_created", name, price.ToString(), delay.ToString());
        }
    }
}
