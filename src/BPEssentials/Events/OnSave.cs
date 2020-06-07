using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;

namespace BPEssentials.RegisteredEvents
{
    public class OnSave : IScript
    {
        [Target(GameSourceEvent.ManagerSave, ExecutionMode.Event)]
        public void OnEvent(SvManager svManager)
        {
            Core.Instance.CooldownHandler.SaveCooldowns();
        }
    }
}