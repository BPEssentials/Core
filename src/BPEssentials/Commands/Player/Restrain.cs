﻿using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Restrain : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            target.svPlayer.Restrain(player, target.Handcuffs.restrained);
            ShRestrained shRetained = target.curEquipable as ShRestrained;
            target.svPlayer.SvSetEquipable(shRetained);
            target.TS("target_restrained");
            player.TS("player_restrained", target.username.CleanerMessage());
        }
    }
}
