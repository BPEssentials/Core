using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Utils;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleStaffChat : Command
    {
        public void Invoke(ShPlayer player, string text = "")
        {
            var ePlayer = player.GetExtendedPlayer();
            if (string.IsNullOrWhiteSpace(text))
            {
                if (ePlayer.CurrentChat == Enums.Chat.StaffChat)
                {
                    ePlayer.CurrentChat = Enums.Chat.Global;
                    player.TS("staff_chat_disabled");
                }
                else
                {
                    ePlayer.CurrentChat = Enums.Chat.StaffChat;
                    player.TS("staff_chat_enabled");
                }
                return;
            }
            ChatUtils.SendStaffChatMessage(player, text);
        }
    }
}
