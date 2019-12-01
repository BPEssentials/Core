using BrokeProtocol.API;
using BrokeProtocol.Managers;
using System;

namespace BPEssentials.RegisteredEvents
{
    public class OnStarted : IScript
    {
        public OnStarted()
        {
            GameSourceHandler.Add(BrokeProtocol.API.Events.Manager.OnStarted, new Action<SvManager>(OnEvent));
        }

        public void OnEvent(SvManager svManager)
        {
            Core.Instance.SvManager = svManager;

            Core.Instance.CooldownHandler.Load();
        }
    }
}
