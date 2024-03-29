﻿using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;

namespace BPEssentials.Commands
{
    public class GiveAmmo : BpeCommand
    {
        public void Invoke(ShPlayer player, int amount = 1, ShPlayer target = null)
        {
            target = target ?? player;
            if (!target.curEquipable || !target.curEquipable.AmmoItem)
            {
                player.TS("player_giveammo_fail");
                return;
            }

            ShItem item = target.curEquipable.AmmoItem;
            target.TransferItem(DeltaInv.AddToMe, item.index, amount, true);
            player.TS("player_give", target.username.CleanerMessage(), item.itemName, amount.ToString());
        }
    }
}
