using BPCoreLib.PlayerFactory;
using BPEssentials.ExtendedPlayer;
using BPEssentials.Utils.Formatter.Response;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.ExtensionMethods
{
    public static class ExtensionPlayer
    {
        public static PlayerItem GetExtendedPlayer(this ShPlayer player)
        {
            return player.svPlayer.GetExtended<PlayerItem>();
        }

        public static string TC(this SvPlayer player, string node, params object[] formatting)
        {
            var formatter = new CustomFormatter(Core.Instance.Settings.Messages.ArgColor, Core.Instance.Settings.Messages.InfoColor);
            return $"<color={Core.Instance.Settings.Messages.InfoColor}>" +
                (Core.Instance.I18n.Localize(formatter, player.player.language.code, node, formatting) ?? Core.Instance.I18n.Localize(formatter, "EN", node, formatting) ?? $"{node} [{string.Join(", ", formatting)}]")
                + "</color>";
        }

        public static string TC(this ShPlayer player, string node, params object[] formatting)
        {
            return player.svPlayer.TC(node, formatting);
        }

        public static string T(this SvPlayer player, string node, params object[] formatting)
        {

            return Core.Instance.I18n.Localize(player.player.language.code, node, formatting) ?? Core.Instance.I18n.Localize("EN", node, formatting) ?? $"{node} [{string.Join(", ", formatting)}]";

        }

        public static string T(this ShPlayer player, string node, params object[] formatting)
        {
            return player.svPlayer.T(node, formatting);
        }

        public static void TS(this ShPlayer player, string node, params object[] formatting)
        {
            player.SendChatMessage(player.TC(node, formatting));
        }

        public static void SendChatMessage(this ShPlayer player, string message, bool useColors = false)
        {
            message = useColors ? message.ParseColorCodes() : message;
            player.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, message);
        }
    }
}
