using BrokeProtocol.API;
using BrokeProtocol.Entities;

namespace BPEssentials.Events
{
    public class OnLogin : IScript
    {
        [Target(GameSourceEvent.PlayerInitialize, ExecutionMode.Event)]
        public void OnEvent(ShPlayer player)
        {
            if (!player.isHuman)
            {
                return;
            }

            Core.Instance.Logger.LogInfo($"[CONNECT] {player.username} Connected to the server.");
        }
    }
}
