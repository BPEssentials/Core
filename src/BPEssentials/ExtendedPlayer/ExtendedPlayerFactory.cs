using BPCoreLib.Abstractions;
using BrokeProtocol.Entities;
using System.Linq;

namespace BPEssentials.ExtendedPlayer
{
    public class ExtendedPlayerFactory : BPCoreLib.PlayerFactory.ExtendedPlayerFactory<PlayerItem>
    {
        public override void AddOrReplace(ShPlayer player)
        {
            Players[player.ID] = new PlayerItem(player);
        }
    }
}
