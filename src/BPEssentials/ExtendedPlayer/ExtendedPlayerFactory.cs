using BrokeProtocol.Entities;

namespace BPEssentials.ExtendedPlayer
{
    public class ExtendedPlayerFactory : BPCoreLib.PlayerFactory.ExtendedPlayerFactory
    {
        public override void AddOrReplace(ShPlayer player)
        {
            Players[player.ID] = new PlayerItem(player);
        }
    }
}
