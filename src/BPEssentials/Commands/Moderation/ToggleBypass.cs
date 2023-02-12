using BPEssentials.Abstractions;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleBypass : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            PlayerItem ePlayer = player.GetExtendedPlayer();
            ePlayer.EnabledBypass = !ePlayer.EnabledBypass;
            player.TS(ePlayer.EnabledBypass ? "bypass_enabled" : "bypass_disabled");
        }
    }
}
