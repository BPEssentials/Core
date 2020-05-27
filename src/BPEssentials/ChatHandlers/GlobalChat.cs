using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BPEssentials.ExtensionMethods.Warns;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System;

namespace BPEssentials.ChatHandlers
{
    public class GlobalChat : IScript
    {
        [Target(GameSourceEvent.PlayerGlobalChatMessage, ExecutionMode.Override)]
        public void OnEvent(ShPlayer player, string message)
        {
            if (player.manager.svManager.chatted.Limit(player))
            {
                return;
            }

            if (CommandHandler.OnEvent(player, message)) // 'true' if message starts with command prefix
            {
                return;
            }

            Core.Instance.Logger.LogInfo($"[GLOBAL] {player.username}: {message}");

            switch (player.GetExtendedPlayer().CurrentChat)
            {
                case Chat.StaffChat:
                    ChatUtils.SendStaffChatMessage(player, message);
                    return;

                case Chat.Muted:
                    player.TS("muted_player");
                    return;

                case Chat.Disabled:
                    player.TS("chat_disabled");
                    return;

                default:
                    if (ChatUtils.MessageInBlacklist(message, Core.Instance.WordBlacklist.AutoBan))
                    {
                        string reason = player.T("auto_banchat");
                        ChatUtils.SendToAllEnabledChatT("all_ban", player.username.CleanerMessage(), player.username.CleanerMessage(), reason.CleanerMessage());
                        player.svPlayer.SvBan(player, reason);
                        return;
                    }
                    if (ChatUtils.MessageInBlacklist(message, Core.Instance.WordBlacklist.AutoKick))
                    {
                        string reason = player.T("auto_kickchat");
                        ChatUtils.SendToAllEnabledChatT("all_kick", player.username.CleanerMessage(), player.username.CleanerMessage(), reason.CleanerMessage());
                        player.svPlayer.SvKick(player, reason);
                        return;
                    }
                    if (ChatUtils.MessageInBlacklist(message, Core.Instance.WordBlacklist.AutoWarn))
                    {
                        string reason = player.T("auto_warnchat");
                        player.AddWarn(player, reason);
                        ChatUtils.SendToAllEnabledChatT("all_warned", player.username.CleanerMessage(), player.username.CleanerMessage(), reason.CleanerMessage());
                        player.TS("target_warn", player.username.CleanerMessage(), reason.CleanerMessage());
                        return;
                    }
                    ChatUtils.SendToAllEnabledChat(ChatUtils.FormatMessage(player, message), false);
                    return;
            }
        }
    }
}