using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class GetShByStr : EssentialsCorePlugin
    {
        public static ShPlayer Run(string player)
        {
            try
            {
                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.ID.ToString() == player || shPlayer.username == player)
                        return shPlayer;
                return null;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
                return null;
            }
        }
    }
}
