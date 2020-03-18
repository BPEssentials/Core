using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;

namespace BPEssentials.Commands
{
    class ToggleMute : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            var eTarget = target.GetExtendedPlayer();
            if (eTarget.CurrentChat == Enums.Chat.Muted)
            {
                eTarget.CurrentChat = Enums.Chat.Global;
                player.TS("player_unmuted", target.username);
            }
            else
            {
                eTarget.CurrentChat = Enums.Chat.Muted;
                player.TS("player_muted",  target.username);
            }
        }
    }
}
