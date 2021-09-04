using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BPEssentials.Models;
using BrokeProtocol.Entities;
using System;

namespace BPEssentials.ExtendedPlayer
{
    [Serializable]
    public class PlayerItem : BPCoreLib.Abstractions.ExtendedPlayer
    {
        public PlayerItem(ShPlayer player) : base(player)
        {
        }

        public bool Muted { get; set; }

        public bool EnabledBypass { get; set; }

        public bool EnabledSpychat { get; set; }

        public Chat CurrentChat { get; set; } = Chat.Global;

        public bool CanRecieveStaffChat => Client.svPlayer.HasPermission($"{Core.Instance.Info.GroupNamespace}.receivestaffchat");

        public ShPlayer ReplyToUser { get; set; }

        public TpaUserStore TpaUser { get; set; }

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

        public void SetTpaUser(ShPlayer target, bool TpHere = false)
        {
            TpaUser = new TpaUserStore() { Player = target, TpHere = TpHere };
        }
    }


    [Serializable]
    public class TpaUserStore
    {
        public ShPlayer Player { get; set; }

        public bool TpHere { get; set; }
    }
}