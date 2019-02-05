using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Reflection;

namespace BP_Essentials
{
    class GetExtraCount : Variables
    {
        public static int Run(ShPlayer player, InventoryItem myItem)
        {
            try
            {
                if (player.job.info.rankItems.Length > player.rank)
                {
                    for (int i = player.rank; i >= 0; i--)
                        foreach (InventoryItem inventoryItem in player.job.info.rankItems[i].items)
                            if (myItem.item.index == inventoryItem.item.index)
                                return Mathf.Max(0, myItem.count - inventoryItem.count);
                }
                return myItem.count;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return 0;
        }
    }
}
