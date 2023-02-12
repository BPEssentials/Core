using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Disconnect : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            player.svPlayer.svManager.Disconnect(target.svPlayer.connection, DisconnectTypes.Normal);
            player.TS("force_disconnect", target.username.CleanerMessage());
        }
    }
}
