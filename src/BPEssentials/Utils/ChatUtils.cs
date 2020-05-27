using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
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
                currPlayer.SendChatMessage($"[STAFFCHAT] {player.username.CleanerMessage()}: {message}");
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

        public static string FormatMessage(ShPlayer player, string message, string formatKey = "format")
        {
            var formatGroup = player.svPlayer.Groups.FirstOrDefault(group => group.Value.CustomData.Data.ContainsKey($"{Core.Instance.Info.GroupNamespace}:{formatKey}"));
            if (formatGroup.Value != null && formatGroup.Value.CustomData.TryFetchCustomData($"{Core.Instance.Info.GroupNamespace}:{formatKey}", out string formatter))
            {
                try
                {
                    return string.Format(new CustomFormatter(), formatter, player.ID, player.username, message);
                }
                catch (Exception err)
                {
                    Core.Instance.Logger.LogException(err);
                }
            }
            return $"{player.username}: {message.CleanerMessage()}";
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

        public static bool MessageInBlacklist(string message, List<string> blacklist)
        {
            foreach (string s in blacklist)
            {
                if (s.Contains(message))
                {
                    return true;
                }
            }
            return false;
        }
    }
}