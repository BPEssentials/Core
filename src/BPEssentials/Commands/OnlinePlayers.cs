using BPEssentials.Abstractions;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.Commands
{
    public class OnlinePlayers : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.SendChatMessage($"Online players ({PlayerFactory.Players.Count}): {string.Join(", ", PlayerFactory.Players.Select(x => x.Key + ": " + x.Value.Client.username.SanitizeString()))}");
        }
    }
}
