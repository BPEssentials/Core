using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace BPEssentials.RegisteredEvents
{
    public class OnDropDead : IScript
    {
        private readonly int[] LicenseIDs = new[] { -700261193, 607710552, 499504400, 1695812550, -568534809 };

        [Target(GameSourceEvent.PlayerRemoveItemsDeath, ExecutionMode.Override)]
        protected void OnRemoveItemsDeath(ShPlayer player, bool dropItems)
        {
            // -- BPE EXTEND
            if (Core.Instance.Settings.KeptItemsOnDeath.KeepAllItemsOnDeath) { return; }
            // BPE EXTEND --

            // List for removed items for dropping into briefcase
            List<InventoryItem> removedItems = new List<InventoryItem>();

            // Allows players to keep items/rewards from job ranks
            foreach (InventoryItem myItem in player.myItems.Values.ToArray())
            {
                // -- BPE EXTEND
                if (Core.Instance.Settings.KeptItemsOnDeath.KeptItemIds.Contains(myItem.item.index)) { continue; }
                if (Core.Instance.Settings.KeptItemsOnDeath.KeptItemNames.Contains(myItem.item.itemName)) { continue; }
                if (Core.Instance.Settings.KeptItemsOnDeath.KeepAllPhones && myItem.item is ShPhone) { continue; }
                if (Core.Instance.Settings.KeptItemsOnDeath.KeepAllLicenses && LicenseIDs.Contains(myItem.item.index)) { continue; }

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
                    removedItems.Add(new InventoryItem(myItem.item, extra));
                    player.TransferItem(DeltaInv.RemoveFromMe, myItem.item.index, extra, true);
                }
            }

            if (dropItems)
            {
                // Only drop items if attacker present, to prevent AI suicide item farming
                if (Physics.Raycast(
                    player.GetPosition + Vector3.up,
                    Vector3.down,
                    out RaycastHit hit,
                    10f,
                    MaskIndex.world))
                {
                    ShEntity briefcase = player.manager.svManager.AddNewEntity(
                        player.manager.svManager.briefcasePrefabs.GetRandom(),
                        player.GetPlace,
                        hit.point,
                        Quaternion.LookRotation(player.GetPositionT.forward),
                        false);

                    if (briefcase)
                    {
                        foreach (var invItem in removedItems)
                        {
                            if (Random.value < 0.8f)
                            {
                                invItem.count = Mathf.CeilToInt(invItem.count * Random.Range(0.05f, 0.3f));
                                briefcase.myItems.Add(invItem.item.index, invItem);
                            }
                        }
                    }
                }
            }
        }
    }
}