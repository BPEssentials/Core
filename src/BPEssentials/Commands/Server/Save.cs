using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using Newtonsoft.Json;
using System.Linq;

namespace BPEssentials.Commands
{
    public class Save : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("saving_game");
            player.manager.svManager.SaveAll();

            // TODO: Move this into the Save Event
            Core.Instance.CooldownHandler.SaveCooldowns();
        }
    }
}
