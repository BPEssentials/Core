using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using Newtonsoft.Json;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Save : Command
    {
        public void StartSaveTimer()
        {
            int interval = 15 * 60 * 1000;
            Core.Instance.CooldownHandler.StartInfiniteTimer(interval, Run);
        }

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