using System;
using System.Collections.Generic;
using System.Linq;
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
                if (!currPlayer.GetExtendedPlayer().CanRecieveStaffChat)
                {
                    continue;
                }
                currPlayer.SendChatMessage($"[STAFFCHAT] {player.username.SanitizeString()}: {message}");
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

        public static string FormatMessage(ShPlayer player, string message)
        {
            var formatGroup = player.svPlayer.Groups.FirstOrDefault(group => group.CustomData.Data.ContainsKey("bpe:format"));
            if (formatGroup != null && formatGroup.CustomData.TryFetchCustomData("bpe:format", out string formatter))
            {
                try
                {
                    return string.Format(new CustomFormatter(), formatter.ParseColorCodes(), player.ID, player.username.SanitizeString(), message);
                }
                catch (Exception err)
                {
                    Core.Instance.Logger.LogException(err);
                }
            }
            return $"{player.username}: {message.SanitizeString()}";
        }

        public static void SendToAllEnabledChat(string message, bool useColors = true)
        {
            foreach (var currPlayer in EntityCollections.Humans)
            {
                if (currPlayer.GetExtendedPlayer().CurrentChat == Chat.Disabled)
                {
                    continue;
                }
                currPlayer.SendChatMessage(message, useColors);
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
