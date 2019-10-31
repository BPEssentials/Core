using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPEssentials.RegisteredEvents
{
    public class OnLogin : IScript
    {
        public OnLogin()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Player.OnInitialize, new Action<ShPlayer>(OnEvent));
        }

        public void OnEvent(ShPlayer player)
        {
            Core.Instance.PlayerHandler.AddOrReplace(player);
            Core.Instance.Logger.LogInfo($"[CONNECT] {player.username} Connected to the server.");
        }
    }
}
