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
            if (Core.Instance.Settings.General.SaveIntervalInMinutes <= 0)
            {
                return;
            }
            var interval = Core.Instance.Settings.General.SaveIntervalInMinutes * 60;
            Core.Instance.CooldownHandler.StartMethodTimer(interval, Run);
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
