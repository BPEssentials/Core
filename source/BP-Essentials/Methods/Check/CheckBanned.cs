using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Threading;
using System.IO;

namespace BP_Essentials
{
    class CheckBanned : EssentialsCorePlugin
    {
        public static void Run(SvPlayer player)
        {
            if (CheckBannedEnabled)
            {
                try
                {
                    Thread.Sleep(3000);
                    if (!player.player.admin && !string.IsNullOrEmpty(player.svManager.GetAddress(player.connection).Trim()))
                        foreach (var line in File.ReadAllLines(BansFile))
                            if (line.StartsWith("# "+player.playerData.username))
                            {
                                Debug.Log($"{SetTimeStamp.Run()}[WARNING] {player.player.username} Joined while banned! IP: {player.svManager.GetAddress(player.connection)}");
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
