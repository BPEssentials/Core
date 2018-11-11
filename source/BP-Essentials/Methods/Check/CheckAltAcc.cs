using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Threading;

namespace BP_Essentials
{
    class CheckAltAcc
    {
        public static void Run(SvPlayer player)
        {
            if (CheckAlt)
            {
                try
                {
                    Thread.Sleep(3000);
                    if (!string.IsNullOrEmpty(player.playerData.username.Trim()))
                        if (!player.player.admin && File.ReadAllText(IpListFile).Contains(player.connection.IP))
                        {
                            Debug.Log($"{SetTimeStamp.Run()}[WARNING] {player.player.username} Joined with a possible alt! IP: {player.connection.IP}");
                            player.svManager.AddBanned(player.player);
                            player.svManager.Disconnect(player.connection);
                        }
                }
                catch (Exception ex)
                {
                    ErrorLogging.Run(ex);
                }
            }
        }
    }
}
