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
    class CheckAltAcc : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player)
        {
            if (CheckAlt)
            {
                Thread.Sleep(3000);
                try
                {
                    if (!string.IsNullOrEmpty(player.playerData.username.Trim()))
                        if (File.ReadAllText(BansFile).Contains(player.svManager.GetAddress(player.connection)))
                        {
                            Debug.Log(SetTimeStamp.Run() + "[WARNING] " + player.playerData.username + " Joined with a possible alt! IP: " + player.svManager.GetAddress(player.connection));
                            foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                                if (shPlayer.svPlayer == player)
                                    if (shPlayer.IsRealPlayer())
                                    {
                                        player.svManager.AddBanned(shPlayer);
                                        player.svManager.Disconnect(player.connection);
                                    }

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
