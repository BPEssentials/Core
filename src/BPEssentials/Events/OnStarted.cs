using BPCoreLib.Util;
using BrokeProtocol.API;
using BrokeProtocol.Managers;

namespace BPEssentials.RegisteredEvents
{
    public class OnStarted : IScript
    {
        [Target(GameSourceEvent.ManagerStart, ExecutionMode.Event)]
        public void OnEvent(SvManager svManager)
        {
            Core.Instance.SvManager = svManager;
            Core.Instance.SvManager.StartCoroutine(Interval.Start(Core.Instance.Settings.General.SaveInterval * 60 * 1000, new Commands.Save().Run));
        }
    }
}
