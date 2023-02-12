using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaHereUser : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            PlayerItem eTarget = target.GetExtendedPlayer();
            eTarget.SetTpaUser(player, true);
            player.SendChatMessage($"{player.T("player_tpahere_sent", target.username.CleanerMessage())}  {(eTarget.CurrentChat == Chat.Disabled ? player.T("player_tpa_chat_disabled") : "")}");
            if (eTarget.CurrentChat == Chat.Disabled)
            {
                return;
            }

            target.TS("target_tpahere_sent", player.username.CleanerMessage());
        }
    }
}
