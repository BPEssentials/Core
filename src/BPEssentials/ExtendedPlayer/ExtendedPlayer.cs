using BPEssentials.Models;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System;

namespace BPEssentials.ExtendedPlayer
{
    [Serializable]
    public class PlayerItem : BPCoreLib.Abstractions.ExtendedPlayer
    {
        public PlayerItem(ShPlayer player) : base(player)
        {
        }

        public bool EnabledBypass { get; set; }

        public bool EnabledSpychat { get; set; }

        public Chat CurrentChat { get; set; } = Chat.Global;

        public bool CanRecieveStaffChat => Client.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.receivestaffchat");

        public ShPlayer ReplyToUser { get; set; }

        public ShPlayer TpaUser { get; set; }

        public LastLocation LastLocation { get; } = new LastLocation();

        public void SendPmMessage(PlayerItem target, string message)
        {
            ReplyToUser = target.Client;
            target.ReplyToUser = Client;
            Client.TS("pm>", ReplyToUser.username.CleanerMessage(), message.CleanerMessage());
            ReplyToUser.TS("pm<", Client.username.CleanerMessage(), message.CleanerMessage());
        }

        public void SendSpyChatMessage(ShPlayer target, string command)
        {
            Client.SendChatMessage($"[SPYCHAT] {target.username.CleanerMessage()}: {command.CleanerMessage()}");
        }
    }
}
