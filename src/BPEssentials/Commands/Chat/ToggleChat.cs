using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleChat : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            PlayerItem ePlayer = player.GetExtendedPlayer();
            ePlayer.CurrentChat = ePlayer.CurrentChat == Chat.Disabled ? Chat.Global : Chat.Disabled;
            player.TS("chat_mode_set", ePlayer.CurrentChat.ToString().ToLowerInvariant());
        }
    }
}
