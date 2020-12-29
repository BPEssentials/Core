using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace BPEssentials.RegisteredEvents
{
    public class OnLeave : IScript
    {
        [Target(GameSourceEvent.PlayerDestroy, ExecutionMode.Event)]
        public void OnEvent(ShPlayer player)
        {
            if (!player.isHuman) { return; }
            Core.Instance.PlayerHandler.Remove(player.ID);
            Core.Instance.Logger.LogInfo($"[DISCONNECT] {player.username} disconnected.");
        }
    }
}
