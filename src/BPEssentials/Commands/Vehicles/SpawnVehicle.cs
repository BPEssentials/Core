using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;
using System.Linq;
using BrokeProtocol.Managers;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class SpawnVehicle : BpeCommand
    {
        public void Invoke(ShPlayer player, string name)
        {
            var vehicle = Core.Instance.EntityHandler.Vehicles.OrderByDescending(x => LevenshteinDistance.CalculateSimilarity(x.Key, name)).FirstOrDefault().Value;
            var pos = player.GetPosition;
            SvManager.Instance.AddNewEntity(vehicle, player.GetPlace, new Vector3(pos.x, pos.y + 7F, pos.z), player.GetRotation, respawnable: false);
            player.TS("player_vehicle_spawned", vehicle.name);
        }
    }
}
