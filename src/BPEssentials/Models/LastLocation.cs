using BrokeProtocol.Entities;
using UnityEngine;

namespace BPEssentials.Models
{
    public class LastLocation
    {
        public Vector3 Position { get; private set; }

        public Quaternion Rotation { get; private set; }

        public int PlaceIndex { get; private set; }

        public void Update(Vector3 position, Quaternion rotation, int index)
        {
            Position = position;
            Rotation = rotation;
            PlaceIndex = index;
        }

        public void Update(ShPlayer player)
        {
            Update(player.Position, player.Rotation, player.GetPlaceIndex());
        }

        public bool HasPositionSet()
        {
            return Position != default(Vector3) && Rotation != default(Quaternion);
        }

        public LastLocation(Vector3 position, Quaternion rotation, int index)
        {
            Update(position, rotation, index);
        }

        public LastLocation()
        {

        }
    }
}
