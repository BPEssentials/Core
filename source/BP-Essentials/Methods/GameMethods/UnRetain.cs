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
    class UnRetain : EssentialsVariablesPlugin
    {
        public static void Run(SvPlayer player)
        {
            try
            {
                typeof(SvPlayer).GetMethod(nameof(UnRetain), BindingFlags.NonPublic | BindingFlags.Instance).Invoke(player, new object[] {});
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
