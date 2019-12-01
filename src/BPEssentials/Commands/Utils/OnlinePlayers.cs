using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.Commands
{
    public class OnlinePlayers : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("online_players", PlayerFactory.Players.Count.ToString(), string.Join(", ", PlayerFactory.Players.Select(x => x.Key + ": " + x.Value.Client.username.SanitizeString())));
        }
    }
}
