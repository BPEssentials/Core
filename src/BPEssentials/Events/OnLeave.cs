using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPEssentials.RegisteredEvents
{
    public class OnLeave : IScript
    {
        public OnLeave()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Player.OnDestroy, new Action<ShPlayer>(OnEvent));
        }

        public void OnEvent(ShPlayer player)
        {
            Core.Instance.PlayerHandler.Remove(player.ID);
            Core.Instance.Logger.LogInfo($"[DISCONNECT] {player.username} disconnected.");
        }
    }
}
