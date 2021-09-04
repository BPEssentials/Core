using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleGodmode : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;

            target.svPlayer.godMode = !target.svPlayer.godMode;
            player.TS("godmode_toggle", target.username.CleanerMessage(),
               target.svPlayer.godMode.Stringify(player.T("enabled"), player.T("disabled"))
            );
        }
    }
}