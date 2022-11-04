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
            Core.Instance.Logger.Log("Saving Game Status");
            Utils.ChatUtils.SendToAllEnabledChatT("saving_game");
            SvManager.Instance.SaveAll();
        }
    }
}
