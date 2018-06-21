using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Reflection;

namespace BP_Essentials
{
    class SetJob : EssentialsVariablesPlugin
    {
        public static void Run(ShPlayer shPlayer, byte jobIndex, bool AddItems, bool CollectCost)
        {
            try
            {
                typeof(SvPlayer).GetMethod("SvTrySetJob", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(shPlayer.svPlayer, new object[] { jobIndex, AddItems, CollectCost });
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
