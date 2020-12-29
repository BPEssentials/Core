using BPEssentials.Abstractions;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Save : Command
    {
        public void Invoke(ShPlayer player)
        {
            Run();
        }

        public void Run()
        {
            Core.Instance.Logger.Log("Saving Game Status");
            Utils.ChatUtils.SendToAllEnabledChatT("saving_game");
            Core.Instance.SvManager.SaveAll();
        }
    }
}
