﻿using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class ToggleMute : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            var eTarget = target.GetExtendedPlayer();
            if (eTarget.Muted)
            {
                player.TS("player_unmuted", target.username);
            }
            else
            {
                player.TS("player_muted", target.username);
            }

            eTarget.Muted = !eTarget.Muted;
        }
    }
}
