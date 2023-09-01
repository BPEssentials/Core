using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class DestroyVehicle : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            var vehicle = player.curMount as ShTransport;
            if (vehicle == null)
            {
                player.TS("player_notInVehicle");
                return;
            }
            vehicle.Destroy();
            player.TS("player_vehicle_destroyed", vehicle.name);
        }
    }
}
