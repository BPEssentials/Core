﻿using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;

namespace BPEssentials.Commands
{
    public class ClearWanted : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            target.LifePlayer().ClearCrimes();
            player.TS("cleared_crimes", target.username.CleanerMessage());
        }
    }
}
