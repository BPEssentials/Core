using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;

namespace BPEssentials.Commands
{
    public class ToggleGodmode : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            var eTarget = target.GetExtendedPlayer();
            eTarget.HasGodmode = !eTarget.HasGodmode;
            player.TS("godmode_toggle", target.username.CleanerMessage(),
                eTarget.HasGodmode.Stringify(player.T("enabled"), player.T("disabled"))
            );
        }
    }
}
