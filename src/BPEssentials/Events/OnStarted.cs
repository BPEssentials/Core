using BPCoreLib.Interval;
using BrokeProtocol.API;

namespace BPEssentials.Events
{
    public class OnStarted : IScript
    {
        [Target(GameSourceEvent.ManagerStart, ExecutionMode.Event)]
        public void OnEvent()
        {
            Interval.StartNew(Core.Instance.Settings.General.SaveInterval * 60, Commands.Save.Run);
        }
    }
}
