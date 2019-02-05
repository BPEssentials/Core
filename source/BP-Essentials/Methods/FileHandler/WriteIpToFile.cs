using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;
using System.Threading;

namespace BP_Essentials
{
    class WriteIpToFile : Core
    {
        public static void Run(SvPlayer player)
        {
            try
            {
                Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] [JOIN] {player.player.username} IP is: {player.connection.IP}");
                int tries = 0;
                while (tries < 2)
                    try
                    {
                        if (!File.ReadAllText(IpListFile).Contains(player.player.username + ": " + player.connection.IP))
                            File.AppendAllText(IpListFile, player.player.username + ": " + player.connection.IP + Environment.NewLine);
                        break;
                    }
                    catch (IOException)
                    {
                        ++tries;
                    }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
