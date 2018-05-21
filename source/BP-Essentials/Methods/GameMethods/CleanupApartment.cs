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
    class CleanupApartment : EssentialsVariablesPlugin
    {
        public static void Run(ShPlayer shPlayer)
        {
            try
            {
                typeof(SvPlayer).GetMethod(nameof(CleanupApartment), BindingFlags.NonPublic | BindingFlags.Instance).Invoke(shPlayer.svPlayer, new object[] { });
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
