using BrokeProtocol.Entities;

namespace BPEssentials.ExtendedPlayer
{
    public class ExtendedPlayerFactory : BPCoreLib.PlayerFactory.ExtendedPlayerFactory<PlayerItem>
    {
        public override void AddOrReplace(ShPlayer player)
        {
            this[player.ID] = new PlayerItem(player);
        }
    }
}
