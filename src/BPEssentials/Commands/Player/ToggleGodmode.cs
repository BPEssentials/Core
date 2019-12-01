using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands.Player
{
    public class ToggleGodmode : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            var eTarget = target.GetExtendedPlayer();
            eTarget.HasGodmode = !eTarget.HasGodmode;
            player.TS("godmode_toggle", target.username.SanitizeString(),
                eTarget.HasGodmode.Stringify(player.T("enabled"), player.T("disabled"))
            );
        }
    }
}
