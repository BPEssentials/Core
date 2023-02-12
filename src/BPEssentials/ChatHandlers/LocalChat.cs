using System;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.ChatHandlers
{
    public class LocalChat : IScript
    {
        private static LimitQueue<ShPlayer> chatted = new LimitQueue<ShPlayer>(8, 20f);

        [Target(GameSourceEvent.PlayerChatLocal, ExecutionMode.Override)]
        public void OnEvent(ShPlayer player, string message)
        {
            if (chatted.Limit(player))
            {
                return;
            }
            if (CommandHandler.OnEvent(player, message))
            {
                return;
            }

            Core.Instance.Logger.LogInfo($"[LOCAL] {player.username}: {message}");
            switch (player.chatMode)
            {
                case ChatMode.Public:
                    player.svPlayer.Send(SvSendType.Local, Channel.Reliable, ClPacket.ChatLocal, player.ID, ChatUtils.FormatMessage(player, message, "format.local.public"));
                    break;

                case ChatMode.Job:
                    foreach (ShPlayer p in player.svPlayer.job.info.members)
                    {
                        if (!p.isHuman)
                        {
                            continue;
                        }

                        p.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ChatJob, player.ID, ChatUtils.FormatMessage(player, message, "format.local.job"));
                    }
                    break;

                case ChatMode.Channel:
                    foreach (ShPlayer p in EntityCollections.Humans)
                    {
                        if (p.chatChannel != player.chatChannel)
                        {
                            continue;
                        }

                        p.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ChatChannel, player.ID, ChatUtils.FormatMessage(player, message, "format.local.channel"));
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(player.chatMode));
            }
            if (Core.Instance.Settings.General.LocalChatInChat)
            {
                player.svPlayer.Send(SvSendType.LocalOthers, Channel.Unsequenced, ClPacket.GameMessage, ChatUtils.FormatMessage(player, message, "format.local"));
            }
        }
    }
}
