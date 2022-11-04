using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Arrest : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.svPlayer.Restrain(player, target.Handcuffs.restrained);
            player.TS("arrested", target.username.CleanerMessage());
        }
    }
}
