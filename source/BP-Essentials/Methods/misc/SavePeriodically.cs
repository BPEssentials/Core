using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Threading;
using System.Timers;

namespace BP_Essentials
{
    class SavePeriodically : EssentialsChatPlugin
    {
        public static void Run(object man)
        {
            try
            {
                var svManager = (SvManager)man;
                var Tmer = new System.Timers.Timer(); // TODO: Disposing the timer seems to break
                Tmer.Elapsed += (sender, e) => OnTime(svManager);
                Tmer.Interval = SaveTime * 1000;
                Tmer.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
        static void OnTime(object onetMan)
        {
            var svManager = (SvManager)onetMan;
            Debug.Log(SetTimeStamp.Run() + "[INFO] Saving game..");
            foreach (var shPlayer in svManager.players.Values)
                if (!shPlayer.svPlayer.IsServerside())
                {
                    if (shPlayer.GetPlaceIndex() >= 13) continue;
                    shPlayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, "<color=#DCDADA>Saving game.. This can take up to 5 seconds.</color>");
                    shPlayer.svPlayer.Save();
                }
        }
    }
}