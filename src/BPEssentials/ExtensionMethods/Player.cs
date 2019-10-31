using BPEssentials.ExtendedPlayer;
using BrokeProtocol.API.Types;
using BrokeProtocol.Entities;

namespace BPEssentials.ExtensionMethods
{
    public static class ExtensionPlayer
    {
        public static PlayerItem GetExtendedPlayer(this ShPlayer player)
        {
            return Core.Instance.PlayerHandler.GetUnsafe<PlayerItem>(player.ID);
        }
    }
}
