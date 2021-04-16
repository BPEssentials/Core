using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;


namespace BPEssentials.Commands
{
    public class Teleport : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            player.GetExtendedPlayer().ResetAndSavePosition(target);
            player.TS("tp_to", target.username.CleanerMessage());
        }
    }
}
