using BPEssentials.Abstractions;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Knockout : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.svPlayer.SvForceStance(BrokeProtocol.Utility.StanceIndex.KnockedOut);
            player.SendChatMessage($"Knocked out {target.username.SanitizeString()}.");
        }
    }
}


