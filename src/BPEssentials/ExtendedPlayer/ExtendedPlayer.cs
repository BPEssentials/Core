using BPEssentials.Models;
﻿using BPEssentials.Enums;
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

        public LastLocation LastLocation { get; } = new LastLocation();

        public void SendPmMessage(ShPlayer target, string message)
        {
            Client.SendChatMessage($"[PM] {target.username.SanitizeString()} > {message.SanitizeString()}");
            target.SendChatMessage($"[PM] {Client.username.SanitizeString()} < {message.SanitizeString()}");
        }

        public void SendSpyChatMessage(ShPlayer target, string command)
        {
            Client.SendChatMessage($"[SPYCHAT] {target.username.SanitizeString()}: {command.SanitizeString()}");
        }
    }
}
