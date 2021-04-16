using BPEssentials.Abstractions;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Sudo : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, string message)
        {
            target.svPlayer.SvGlobalChatMessage(message);
        }
    }
}
