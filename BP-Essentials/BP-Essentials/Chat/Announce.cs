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
            var netMan = (SvNetMan)man;
            while (true)
            {
                foreach (var player in netMan.players)
                    player.svPlayer.SendToSelf(Channel.Reliable, ClPacket.GameMessage, Announcements[AnnounceIndex]);
                Debug.Log(SetTimeStamp.Run() + "[INFO] Announcement made...");
                AnnounceIndex += 1;
                if (AnnounceIndex > Announcements.Length - 1)
                    AnnounceIndex = 0;
                Thread.Sleep(TimeBetweenAnnounce * 1000);
            }
        }
    }
}
