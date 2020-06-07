using BPEssentials.Utils;
using System;
using System.Collections.Generic;

namespace BPEssentials
{
    public class Announcer
    {
        public List<string> Announcements { get; } = new List<string>();

        public int Index { get; private set; }

        public Announcer(int interval)
        {
            Core.Instance.CooldownHandler.StartInfiniteTimer(interval, OnElapsed);
        }

        public Announcer(int interval, List<string> announcements) : this(interval)
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
            ChatUtils.SendToAllEnabledChat(lines);
            Core.Instance.Logger.LogInfo("Announcement made...");
        }
    }
}