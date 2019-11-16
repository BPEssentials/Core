using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Save : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("saving_game");
            player.manager.svManager.SaveAll();
        }
    }
}
