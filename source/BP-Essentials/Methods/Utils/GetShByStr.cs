using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials
{
    public class GetShByStr : Core
    {
        public static ShPlayer Run(string player, bool idOnly = false)
        {
            try
            {
                foreach (var shPlayer in SvMan.players.Values)
                    if ((shPlayer.ID.ToString() == player || shPlayer.username == player) && !idOnly || shPlayer.ID.ToString() == player && idOnly)
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
