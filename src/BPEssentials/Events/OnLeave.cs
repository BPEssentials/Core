using BrokeProtocol.API;
using BrokeProtocol.Entities;

namespace BPEssentials.Events
{
    public class OnLeave : IScript
    {
        [Target(GameSourceEvent.PlayerDestroy, ExecutionMode.Event)]
        public void OnEvent(ShPlayer player)
        {
            if (!player.isHuman)
            {
                return;
            }

            Core.Instance.Logger.LogInfo($"[DISCONNECT] {player.username} disconnected.");
        }
    }
}
