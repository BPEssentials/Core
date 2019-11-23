using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System.IO;

namespace BPEssentials.Commands
{
    public class WarpCreate : Command
    {
        private string Type = "warp";

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
            if (player.svPlayer.InApartment())
            {
                player.TS("warpCreate_error_inApartment");
                return;
            }
            var file = Path.Combine(Core.Instance.Paths.WarpsFolder, $"{name}.json");
            if (File.Exists(file))
            {
                player.TS("expFileHandler_create_error_alreadyExists", player.T(Type), name);
                return;
            }
            var obj = new WarpHandler.JsonModel
            {
                Delay = delay < 0 ? 0 : delay,
                Price = price < 0 ? 0 : price,
                Name = name,
                Position = new WarpHandler.Position { X = player.GetPosition().x, Y = player.GetPosition().y, Z = player.GetPosition().z, PlaceIndex = player.GetPlaceIndex() },
                Rotation = new WarpHandler.Rotation { X = player.GetRotation().x, Y = player.GetRotation().y, Z = player.GetRotation().z, W = player.GetRotation().w }
            };
            Core.Instance.WarpHandler.CreateNew(obj, name);
            player.TS("warpCreate_created", name, price.ToString(), delay.ToString());
        }
    }
}
