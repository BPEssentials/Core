using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Kill : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.Die();
            player.TS("killed", target.username.CleanerMessage());
        }
    }
}
