using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class SetTimeStamp : EssentialsChatPlugin
    {
        public static string Run()
        {
            try
            {
                var src = DateTime.Now;
                var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
                var placeHolderText = TimestampFormat;
                var minutes = hm.ToString("mm");
                var seconds = hm.ToString("ss");

                if (TimestampFormat.Contains("{YYYY}"))
                    placeHolderText = placeHolderText.Replace("{YYYY}", hm.ToString("yyyy"));
                if (TimestampFormat.Contains("{DD}"))
                    placeHolderText = placeHolderText.Replace("{DD}", hm.ToString("dd"));
                if (TimestampFormat.Contains("{DDDD}"))
                    placeHolderText = placeHolderText.Replace("{DDDD}", hm.ToString("dddd"));
                if (TimestampFormat.Contains("{MMMM}"))
                    placeHolderText = placeHolderText.Replace("{MMMM}", hm.ToString("MMMM"));
                if (TimestampFormat.Contains("{MM}"))
                    placeHolderText = placeHolderText.Replace("{MM}", hm.ToString("MM"));
                if (TimestampFormat.Contains("{H}"))
                    placeHolderText = placeHolderText.Replace("{H}", hm.ToString("HH"));
                if (TimestampFormat.Contains("{h}"))
                    placeHolderText = placeHolderText.Replace("{h}", hm.ToString("hh"));
                if (TimestampFormat.Contains("{M}") || TimestampFormat.Contains("{m}"))
                    placeHolderText = placeHolderText.Replace("{M}", minutes);
                if (TimestampFormat.Contains("{S}") || TimestampFormat.Contains("{s}"))
                    placeHolderText = placeHolderText.Replace("{S}", seconds.ToString());
                if (TimestampFormat.Contains("{T}"))
                    placeHolderText = placeHolderText.Replace("{T}", hm.ToString("tt"));
                placeHolderText = placeHolderText + " ";
                return placeHolderText;
            }
            catch (Exception)
            {
                return "[Failed] ";
            }
        }
    }
}
