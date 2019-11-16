using BPEssentials.Abstractions;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Save : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.SendChatMessage("Saving game..");
            player.manager.svManager.SaveAll();
        }
    }
}
