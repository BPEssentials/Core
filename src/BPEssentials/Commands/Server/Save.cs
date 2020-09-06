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
           
                var Tmer = new System.Timers.Timer(); // TODO: Disposing the timer seems to break
                Tmer.Elapsed += (sender, e) => Run();
                Tmer.Interval = 15 * 60 * 1000;
                Tmer.Enabled = true;
            
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
