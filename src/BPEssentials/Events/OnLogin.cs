using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace BPEssentials.RegisteredEvents
{
    public class OnLogin : IScript
    {
        [Target(GameSourceEvent.PlayerInitialize, ExecutionMode.Event)]
        public void OnEvent(ShPlayer player)
        {
            Core.Instance.PlayerHandler.AddOrReplace(player);
            Core.Instance.Logger.LogInfo($"[CONNECT] {player.username} Connected to the server.");
        }
    }
}
