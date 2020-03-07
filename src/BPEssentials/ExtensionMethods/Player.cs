using BPEssentials.ExtendedPlayer;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.ExtensionMethods
{
    public static class ExtensionPlayer
    {
        public static PlayerItem GetExtendedPlayer(this ShPlayer player)
        {
            return Core.Instance.PlayerHandler.GetSafe(player.ID);
        }

        public static string T(this SvPlayer player, string node, params string[] formatting) { 
            return Core.Instance.I18n.Localize(player.language.code, node, formatting) ?? Core.Instance.I18n.Localize("EN", node, formatting) ?? $"{node} [{string.Join(", ", formatting)}]";
        }

        public static string T(this ShPlayer player, string node, params string[] formatting)
        {
            return player.svPlayer.T(node, formatting);
        }

        public static void TS(this ShPlayer player, string node, params string[] formatting)
        {
            player.SendChatMessage(player.T(node, formatting));
        }

        public static void SendChatMessage(this ShPlayer player, string message, bool useColors = false)
        {
            message = useColors ? message.ParseColorCodes() : message;
            player.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, 10, message);
        }
    }
}
