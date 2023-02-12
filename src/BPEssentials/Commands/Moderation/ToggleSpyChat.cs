using BPEssentials.Abstractions;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleSpyChat : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            PlayerItem ePlayer = player.GetExtendedPlayer();
            ePlayer.EnabledSpychat = !ePlayer.EnabledSpychat;
            player.TS(ePlayer.EnabledSpychat ? "spy_chat_enabled" : "spy_chat_disabled");
        }
    }
}
