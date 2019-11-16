using BPEssentials.Abstractions;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Disconnect : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            player.svPlayer.svManager.Disconnect(target.svPlayer.connection, DisconnectTypes.Normal);
            player.SendChatMessage($"Disconnected '{target.username.SanitizeString()}'.");
        }
    }
}
