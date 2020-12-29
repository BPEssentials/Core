using BPCoreLib.Util;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;
using System;
using System.Linq;

namespace BPEssentials.RegisteredEvents
{
    public class OnStarted : IScript
    {
        [Target(GameSourceEvent.ManagerStart, ExecutionMode.Event)]
        public void OnEvent(SvManager svManager)
        {
            Core.Instance.SvManager = svManager;
            Core.Instance.SvManager.StartCoroutine(Interval.Start(Core.Instance.Settings.General.AnnounceInterval * 60 * 1000, new Commands.Save().Run));
        }
    }
}
