using System.Collections.Generic;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;


namespace BPEssentials.Utils
{
    public static class ChatUtils
    {
        public static void SendStaffChatMessage(ShPlayer player, string message)
        {
            foreach (var currPlayer in EntityCollections.Humans)
            {
                if (currPlayer == player || !currPlayer.GetExtendedPlayer().CanRecieveStaffChat)
                {
                    continue;
                }
                currPlayer.SendChatMessage($"[STAFFCHAT] {player.username.SanitizeString()}: {message}"); // to Username
            }
        }

        public static void SendToAllEnabledChatT(string node, params string[] formatting)
        {
            foreach (var currPlayer in EntityCollections.Humans)
            {
                if (currPlayer.GetExtendedPlayer().CurrentChat == Chat.Disabled)
                {
                    continue;
                }
                currPlayer.TS(node, formatting);
            }
        }

        public static void SendToAllEnabledChat(string message)
        {
            foreach (var currPlayer in EntityCollections.Humans)
            {
                if (currPlayer.GetExtendedPlayer().CurrentChat == Chat.Disabled)
                {
                    continue;
                }
                currPlayer.SendChatMessage(message);
            }
        }

        public static void SendToAllEnabledChat(string[] messages)
        {
            foreach (var currPlayer in EntityCollections.Humans)
            {
                if (currPlayer.GetExtendedPlayer().CurrentChat == Chat.Disabled)
                {
                    continue;
                }
                foreach (var message in messages)
                {
                    currPlayer.SendChatMessage(message);
                }
            }
        }

        public static void SendToAllEnabledChat(List<string> messages) => SendToAllEnabledChat(messages.ToArray());
    }
}
