using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials
{
    class Save
    {
        public static void StartSaveTimer()
        {
            try
            {
                var Tmer = new System.Timers.Timer(); // TODO: Disposing the timer seems to break
                Tmer.Elapsed += (sender, e) => Run();
                Tmer.Interval = SaveTime * 1000;
                Tmer.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
        public static void Run()
        {
            try
            {
                Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Saving game..");
                foreach (var shPlayer in SvMan.players.Values.ToList())
                    if (!shPlayer.svPlayer.serverside)
                    {
                        shPlayer.svPlayer.SendChatMessage("<color=#DCDADA>Saving game.. This can take up to 5 seconds.</color>");
                        shPlayer.svPlayer.Save();
                    }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
