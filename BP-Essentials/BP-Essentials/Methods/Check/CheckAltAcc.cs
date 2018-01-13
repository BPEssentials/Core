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
        public static void Run(object oPlayer)
        {
            if (CheckAlt)
            {
                Thread.Sleep(3000);
                try
                {
                    var player = (SvPlayer)oPlayer;
                    if (File.ReadAllText(BansFile).Contains(player.netMan.GetAddress(player.connection)))
                    {
                        Debug.Log(SetTimeStamp.Run() + "[WARNING] " + player.playerData.username + " Joined with a possible alt! IP: " + player.netMan.GetAddress(player.connection));
                        foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                            if (shPlayer.svPlayer == player)
                                if (shPlayer.IsRealPlayer())
                                {
                                    player.netMan.AddBanned(shPlayer);
                                    player.netMan.Disconnect(player.connection);
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
