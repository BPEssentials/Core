using BrokeProtocol.Entities;

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
