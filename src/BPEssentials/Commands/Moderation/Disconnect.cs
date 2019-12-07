using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
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
            player.TS("force_disconnect", target.username.SanitizeString());
        }
    }
}
