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
        private readonly int[] LicenseIDs = new[] { -700261193, 607710552, 499504400, 1695812550, -568534809 };

        [Target(GameSourceEvent.PlayerRemoveItemsDeath, ExecutionMode.Override)]
        protected void OnRemoveItemsDeath(ShPlayer player)
        {
            // -- BPE EXTEND
            if (BPEssentials.Core.Instance.Settings.KeptItemsOnDeath.KeepAllItemsOnDeath) { return; }
            // BPE EXTEND --

            // Allows players to keep items/rewards from job ranks
            foreach (InventoryItem myItem in player.myItems.Values.ToArray())
            {
                // -- BPE EXTEND
                if (BPEssentials.Core.Instance.Settings.KeptItemsOnDeath.KeptItemIds.Contains(myItem.item.index)) { continue; }
                if (BPEssentials.Core.Instance.Settings.KeptItemsOnDeath.KeptItemNames.Contains(myItem.item.itemName)) { continue; }
                if (BPEssentials.Core.Instance.Settings.KeptItemsOnDeath.KeepAllPhones && myItem.item is ShPhone) { continue; }
                if (BPEssentials.Core.Instance.Settings.KeptItemsOnDeath.KeepAllLicenses && LicenseIDs.Contains(myItem.item.index)) { continue; }

                // BPE EXTEND --

                int extra = myItem.count;
                if (player.svPlayer.job.info.upgrades.Length > player.rank)
                {
                    for (int rankIndex = player.rank; rankIndex >= 0; rankIndex--)
                    {
                        foreach (var i in player.svPlayer.job.info.upgrades[rankIndex].items)
                        {
                            if (myItem.item.name == i.itemName)
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