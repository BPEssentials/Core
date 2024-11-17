using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils.Formatter.Chat;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using BPCoreLib.ExtensionMethods;
using BrokeProtocol.API;

namespace BPEssentials.Utils
{
    public static class ChatUtils
    {
        public static void SendStaffChatMessage(ShPlayer player, string message)
        {
            foreach (ShPlayer currPlayer in EntityCollections.Humans)
            {
                if (!currPlayer.GetExtendedPlayer().CanReceiveStaffChat)
                {
                    continue;
                }

                currPlayer.SendChatMessage(FormatMessage(player, message, "staffformat"), false);
            }
        }

        public static void SendToAllEnabledChatT(string node, params string[] formatting)
        {
            foreach (ShPlayer currPlayer in EntityCollections.Humans)
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
            Group formatGroup = player.svPlayer.Groups.FirstOrDefault(group => group.CustomData.Data.ContainsKey($"{Core.Instance.Info.GroupNamespace}:{formatKey}"));
            if (formatGroup != null && formatGroup.CustomData.TryGetValue($"{Core.Instance.Info.GroupNamespace}:{formatKey}", out string formatter))
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
            foreach (ShPlayer currPlayer in EntityCollections.Humans)
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
            foreach (ShPlayer currPlayer in EntityCollections.Humans)
            {
                if (currPlayer.GetExtendedPlayer().CurrentChat == Chat.Disabled)
                {
                    continue;
                }

                foreach (string message in messages)
                {
                    currPlayer.SendChatMessage(message);
                }
            }
        }

        public static void SendToAllEnabledChat(List<string> messages)
        {
            SendToAllEnabledChat(messages.ToArray());
        }
    }
}
