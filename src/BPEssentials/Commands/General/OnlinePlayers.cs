using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Linq;
using BPCoreLib.ExtensionMethods;

namespace BPEssentials.Commands
{
    public class OnlinePlayers : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("online_players", PlayerFactory.Count.ToString(), string.Join(", ", PlayerFactory.Select(x =>
                $"{x.Key}: {x.Value.Client.username.CleanerMessage()}")));
        }
    }
}
