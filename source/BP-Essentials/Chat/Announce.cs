using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Threading;

namespace BP_Essentials.Chat
{
    class Announce : EssentialsCorePlugin
    {
        public static void Run(object man)
        {
            try
            {
                var svManager = (SvManager)man;
                _Timer.Elapsed += (sender, e) => OnTime(svManager);
                _Timer.Interval = TimeBetweenAnnounce * 1000;
                _Timer.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }

        private static void OnTime(object onetMan)
        {
            var svManager = (SvManager)onetMan;
            foreach (var player in svManager.players)
                foreach (var line in Announcements[AnnounceIndex].Split(new[] { "\\r\\n", "\\r", "\\n" }, StringSplitOptions.None))
                    player.Value.svPlayer.SendToSelf(Channel.Reliable, ClPacket.GameMessage, line);
            Debug.Log($"{SetTimeStamp.Run()}[INFO] Announcement made...");
            AnnounceIndex += 1;
            if (AnnounceIndex > Announcements.Length - 1)
                AnnounceIndex = 0;
        }
    }
}