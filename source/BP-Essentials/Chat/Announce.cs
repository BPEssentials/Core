using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Threading;

namespace BP_Essentials
{
    public class Announcer
	{
		public System.Timers.Timer Timer { get; private set; }
		public List<string> Announcements { get; set; } = new List<string>();
		public double Interval
		{
			get => Timer.Interval;
			set
			{
				Timer.Enabled = false;
				Timer.Interval = value * 1000;
				Timer.Enabled = true;
			}
		}
		public Announcer(List<string> announcements) : this()
		{
			Announcements = announcements;
		}
		public Announcer()
        {
            try
            {
				Timer = new System.Timers.Timer();

				Timer.Elapsed += (sender, e) => OnTime();
				if (TimeBetweenAnnounce == 0)
				{
					Timer.Enabled = false;
					return;
				}
				Timer.Interval = TimeBetweenAnnounce * 1000;
				Timer.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }

		void OnTime()
		{
			if (Announcements == null || Announcements.Count == 0)
				return;
			if (++AnnounceIndex > Announcements.Count - 1)
				AnnounceIndex = 0;
			if (string.IsNullOrWhiteSpace(Announcements[AnnounceIndex]))
				return;
			var lines = Announcements[AnnounceIndex].Split(new[] { "\r\n", "\r", "\n", "\\r\\n", "\\r", "\\n" }, StringSplitOptions.None);
			foreach (var player in SvMan.players)
			{
				foreach (var line in lines)
					player.Value.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, line);
			}
			Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Announcement made...");
		}
    }
}