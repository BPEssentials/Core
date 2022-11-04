using BPCoreLib.Util;
using BrokeProtocol.API;
using BrokeProtocol.Managers;

namespace BPEssentials.RegisteredEvents
{
    public class OnStarted : IScript
    {
        [Target(GameSourceEvent.ManagerStart, ExecutionMode.Event)]
        public void OnEvent()
        {
            SvManager.Instance.StartCoroutine(Interval.Start(Core.Instance.Settings.General.SaveInterval * 60, new Commands.Save().Run));
        }
    }
}
