﻿using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class TpaDeny : Command
    {
        public void Invoke(ShPlayer player)
        {
            var ePlayer = player.GetExtendedPlayer();
            if (ePlayer.TpaUser == null)
            {
                player.TS("no_tpa_requests");
                return;
            }
            ePlayer.TpaUser.TS("TpaUser_tpa_denied", player.username.CleanerMessage());
            player.TS("player_tpa_denied", ePlayer.TpaUser.username.CleanerMessage());
            ePlayer.TpaUser = null;
        }
    }
}
