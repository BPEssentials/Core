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
    class WriteIpToFile : EssentialsCorePlugin
    {
        public static void Run(object oPlayer)
        {
            try
            {
                Thread.Sleep(500);
                var player = (SvPlayer)oPlayer;
                Debug.Log(SetTimeStamp.Run() + "[INFO] " + "[JOIN] " + player.playerData.username + " IP is: " + player.netMan.GetAddress(player.connection));
                int tries = 0;
                while (tries < 2)
                    try
                    {
                        if (!File.ReadAllText(IpListFile).Contains(player.playerData.username + ": " + player.netMan.GetAddress(player.connection)))
                            File.AppendAllText(IpListFile, player.playerData.username + ": " + player.netMan.GetAddress(player.connection) + Environment.NewLine);
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(50);
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
