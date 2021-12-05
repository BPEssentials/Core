using BPCoreLib.Serializable;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System;
using System.IO;

namespace BPEssentials.Commands
{
    public class WarpCreate : Command
    {
        public void Invoke(ShPlayer player, string warp, int price = 0, int delay = 0)
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
            if (player.InApartment)
            {
                player.TS("warpCreate_error_inApartment");
                return;
            }
            var file = Path.Combine(Core.Instance.Paths.WarpsFolder, $"{warp}.json");
            if (File.Exists(file))
            {
                player.TS("expFileHandler_create_error_alreadyExists", player.T(Core.Instance.WarpHandler.Name), warp);
                return;
            }
            var obj = new WarpHandler.JsonModel
            {
                Delay = Math.Max(0, delay),
                Price = Math.Max(0, price),
                Name = warp,
                Position = new WarpHandler.Position { SerializableVector3 = new SerializableVector3(player.GetPosition), PlaceIndex = player.GetPlaceIndex },
                SerializableQuaternion = new SerializableQuaternion(player.GetRotation)
            };
            Core.Instance.WarpHandler.CreateNew(obj, warp);
            player.TS("warpCreate_created", warp, price.ToString(), delay.ToString());
        }
    }
}
