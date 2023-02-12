using BPEssentials.Abstractions;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;

namespace BPEssentials.Commands
{
    public class Save : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            Run();
        }

        public void Run()
        {
            Core.Instance.Logger.LogWithTimestamp("Saving game status");
            SvManager.Instance.SaveAll();
        }
    }
}
