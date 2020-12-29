using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaHereAll : Command
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("player_tpahere_all");
            foreach (var eTarget in Core.Instance.PlayerHandler.Players)
            {
                if (eTarget.Key == player.ID) return;
                eTarget.Value.SetTpaUser(player, true);
                if (eTarget.Value.CurrentChat == Chat.Disabled)
                {
                    return;
                }
                eTarget.Value.Client.TS("target_tpahere_sent", player.username.CleanerMessage());
            }
        }
    }
}
