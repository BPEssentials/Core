using System;
using System.Collections.Generic;
using System.Timers;

namespace BPEssentials
{
    public class Announcer
    {
        public Timer Timer { get; }

        public List<string> Announcements { get; } = new List<string>();

        public int Index { get; private set; }

        public double Interval
        {
            get => Timer.Interval;
            set
            {
                Timer.Enabled = false;
                Timer.Interval = value;
                Timer.Enabled = true;
            }
        }

        public Announcer(double interval)
        {
            Timer = new Timer();
            Timer.Elapsed += (sender, e) => OnElapsed();
            Timer.Enabled = false;
            if (interval <= 0)
            {
                return;
            }
            Timer.Interval = interval;
            Timer.Enabled = true;
        }

        public Announcer(double interval, List<string> announcements) : this(interval)
        {
            Announcements = announcements;
        }

        public void OnElapsed()
        {
            if (Announcements?.Count == 0)
            {
                return;
            }
            if (++Index > Announcements.Count - 1)
            {
                Index = 0;
            }
            if (string.IsNullOrWhiteSpace(Announcements[Index]))
            {
                return;
            }
            var lines = Announcements[Index].Split(new[] { "\r\n", "\r", "\n", "\\r\\n", "\\r", "\\n" }, StringSplitOptions.None);
            Util.SendToAllEnabledChat(lines);
            Core.Instance.Logger.LogInfo("Announcement made...");
        }
    }
}
