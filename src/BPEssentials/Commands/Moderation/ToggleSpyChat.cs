using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleSpyChat : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            var ePlayer = player.GetExtendedPlayer();
            ePlayer.EnabledSpychat = !ePlayer.EnabledSpychat;
            if (ePlayer.EnabledSpychat)
            {
                player.TS("spy_chat_enabled");

            }
            else
            {
                player.TS("spy_chat_disabled");

            }
        }
    }
}
