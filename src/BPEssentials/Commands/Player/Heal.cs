﻿using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Utility;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Heal : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.svPlayer.Heal(target.maxStat);
            target.ClearInjuries();
            player.TS("healed", target.username.CleanerMessage());
        }
    }
}
