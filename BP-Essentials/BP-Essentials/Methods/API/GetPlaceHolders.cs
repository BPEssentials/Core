using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class GetPlaceHolders : EssentialsCorePlugin
    {
        public static string Run(int i, object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            var src = DateTime.Now;
            var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
            var placeHolderText = Responses[i];
            var minutes = hm.ToString("mm");
            var seconds = hm.ToString("ss");

            if (Responses[i].Contains("{YYYY}"))
                placeHolderText = placeHolderText.Replace("{YYYY}", hm.ToString("yyyy"));
            if (Responses[i].Contains("{DD}"))
                placeHolderText = placeHolderText.Replace("{DD}", hm.ToString("dd"));
            if (Responses[i].Contains("{DDDD}"))
                placeHolderText = placeHolderText.Replace("{DDDD}", hm.ToString("dddd"));
            if (Responses[i].Contains("{MMMM}"))
                placeHolderText = placeHolderText.Replace("{MMMM}", hm.ToString("MMMM"));
            if (Responses[i].Contains("{MM}"))
                placeHolderText = placeHolderText.Replace("{MM}", hm.ToString("MM"));
            if (Responses[i].Contains("{H}"))
                placeHolderText = placeHolderText.Replace("{H}", hm.ToString("HH"));
            if (Responses[i].Contains("{h}"))
                placeHolderText = placeHolderText.Replace("{h}", hm.ToString("hh"));
            if (Responses[i].Contains("{M}") || Responses[i].Contains("{m}"))
                placeHolderText = placeHolderText.Replace("{M}", minutes);
            if (Responses[i].Contains("{S}") || Responses[i].Contains("{s}"))
                placeHolderText = placeHolderText.Replace("{S}", seconds);
            if (Responses[i].Contains("{T}"))
                placeHolderText = placeHolderText.Replace("{T}", hm.ToString("tt"));
            if (Responses[i].ToLower().Contains("{username}"))
                placeHolderText = placeHolderText.Replace("{username}", player.playerData.username);
            return placeHolderText;
        }
    }
}
