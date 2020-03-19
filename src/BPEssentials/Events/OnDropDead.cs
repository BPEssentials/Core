using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.AI;
using System;
using System.Linq;
using UnityEngine;


namespace BPEssentials.RegisteredEvents
{
    public class OnDropDead : IScript
    {
        [Target(GameSourceEvent.PlayerRemoveItemsDeath, ExecutionMode.Override)]
        protected void OnRemoveItemsDeath(ShPlayer player)
        {
            // -- BPE EXTEND
            if (BPEssentials.Core.Instance.Settings.General.KeepAllItemsOnDeath) { return; }
            // BPE EXTEND --

            // Allows players to keep items/rewards from job ranks
            foreach (InventoryItem myItem in player.myItems.Values.ToArray())
            {
                // -- BPE EXTEND
                if (BPEssentials.Core.Instance.Settings.General.KeptItemsOnDeath.Contains(myItem.item.index)) { continue; }
                // BPE EXTEND --

                int extra = myItem.count;
                if (player.job.info.rankItems.Length > player.rank)
                {
                    for (int rankIndex = player.rank; rankIndex >= 0; rankIndex--)
                    {
                        foreach (InventoryItem i in player.job.info.rankItems[rankIndex].items)
                        {
                            if (myItem.item.index == i.item.index)
                            {
                                extra = Mathf.Max(0, myItem.count - i.count);
                            }
                        }
                    }
                }

                // Remove everything except legal items currently worn
                if (extra > 0 && (myItem.item.illegal || !(myItem.item is ShWearable w) || player.curWearables[(int)w.type].index != w.index))
                {
                    player.TransferItem(DeltaInv.RemoveFromMe, myItem.item.index, extra, true);
                }
            }
        }
    }
}

