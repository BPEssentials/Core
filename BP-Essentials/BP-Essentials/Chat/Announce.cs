using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Threading;
using System.Timers;

namespace BP_Essentials.Chat
{
    class Announce : EssentialsCorePlugin
    {
        public static void Run(object man)
        {
            try
            {
                var netMan = (SvNetMan)man;
                using (System.Timers.Timer Tmer = new System.Timers.Timer())
                {
                    Tmer.Elapsed += (sender, e) => OnTime(netMan);
                    Tmer.Interval = TimeBetweenAnnounce * 1000;
                    Tmer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
        private static void OnTime(object onetMan)
        {
            var netMan = (SvNetMan)onetMan;
            foreach (var player in netMan.players)
                player.svPlayer.SendToSelf(Channel.Reliable, ClPacket.GameMessage, Announcements[AnnounceIndex]);
            Debug.Log(SetTimeStamp.Run() + "[INFO] Announcement made...");
            AnnounceIndex += 1;
            if (AnnounceIndex > Announcements.Length - 1)
                AnnounceIndex = 0;
        }
    }
}
