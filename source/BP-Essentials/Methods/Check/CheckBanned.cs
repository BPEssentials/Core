using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Threading;
using System.IO;

namespace BP_Essentials
{
    class CheckBanned : Core
    {
        public static void Run(SvPlayer player)
        {
            if (CheckBannedEnabled)
            {
                try
                {
                    if (player.player.admin || string.IsNullOrEmpty(player.connection.IP.Trim()))
                        return;
                    foreach (var line in File.ReadAllLines(BansFile))
                        if (string.IsNullOrEmpty(line) || !line.StartsWith("# " + player.player.username, StringComparison.CurrentCulture))
                            continue;
                    Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [WARNING] {player.player.username} Joined while banned! IP: {player.connection.IP}");
                    player.svManager.AddBanned(player.player);
                    player.svManager.Disconnect(player.connection, DisconnectTypes.Banned);
                }
                catch (Exception ex)
                {
                    ErrorLogging.Run(ex);
                }
            }
        }
    }
}
