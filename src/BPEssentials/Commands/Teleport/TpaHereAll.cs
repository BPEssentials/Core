using System.Collections.Generic;
using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.Enums;
using BPEssentials.ExtendedPlayer;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaHereAll : BpeCommand
    {
        public void Invoke(ShPlayer player)
        {
            player.TS("player_tpahere_all");
            foreach (KeyValuePair<int, PlayerItem> eTarget in PlayerFactory)
            {
                if (eTarget.Key == player.ID)
                {
                    return;
                }

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
