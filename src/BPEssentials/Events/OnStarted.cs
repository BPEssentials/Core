using BrokeProtocol.API;
using BrokeProtocol.Managers;
using System;

namespace BPEssentials.RegisteredEvents
{
    public class OnStarted : IScript
    {
        [Target(GameSourceEvent.ManagerStart, ExecutionMode.Event)]
        public void OnEvent(SvManager svManager)
        {
            Core.Instance.SvManager = svManager;

            Core.Instance.CooldownHandler.Load();
            Core.Instance.SetupAnnouncer();
            new Commands.Save().StartSaveTimer();
        }
    }
}