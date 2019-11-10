using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using UnityEngine;

namespace BPEssentials.ExtendedPlayer
{
    public class PlayerItem : BPCoreLib.Abstractions.ExtendedPlayer
    {
        public PlayerItem(ShPlayer player) : base(player)
        {
        }

        public bool HasGodmode { get; set; }

        public bool EnabledSpychat { get; set; }

        public Chat CurrentChat { get; set; } = Chat.Global;

        public bool CanRecieveStaffChat { get; set; }

        public ShPlayer ReplyToUser { get; set; }

        public ShPlayer TpaUser { get; set; }

        public LastLocation lastLocation { get; set; } = new LastLocation();

        public void SendPmMessage(ShPlayer target, string message)
        {
            Client.SendChatMessage($"[PM] {target.username.SanitizeString()} > {message.SanitizeString()}");
            target.SendChatMessage($"[PM] {Client.username.SanitizeString()} < {message.SanitizeString()}");
        }

        public void SendSpyChatMessage(ShPlayer target, string command)
        {
            Client.SendChatMessage($"[SPYCHAT] {target.username.SanitizeString()}: {command.SanitizeString()}");
        }

        public enum Chat
        {
            Disabled,
            Global,
            StaffChat
        }

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
                Update(player.GetPosition(), player.GetRotation(), player.GetPlaceIndex());
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
}
