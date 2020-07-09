using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleBypass : Command
    {
        public void Invoke(ShPlayer player)
        {
            var ePlayer = player.GetExtendedPlayer();
            ePlayer.EnabledBypass = !ePlayer.EnabledBypass;
            if (ePlayer.EnabledBypass)
            {
                player.TS("bypass_enabled");

            }
            else
            {
                player.TS("bypass_disabled");

            }
        }
    }
}
