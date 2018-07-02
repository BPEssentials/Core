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
        public static string Run(string str, SvPlayer player)
        {
            try
            {
                var shPlayer = player.player;
                var src = DateTime.Now;
                var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
                var placeHolderText = str;
                var minutes = hm.ToString("mm");
                var seconds = hm.ToString("ss");

                if (str.Contains("{YYYY}"))
                    placeHolderText = placeHolderText.Replace("{YYYY}", hm.ToString("yyyy"));
                if (str.Contains("{DD}"))
                    placeHolderText = placeHolderText.Replace("{DD}", hm.ToString("dd"));
                if (str.Contains("{DDDD}"))
                    placeHolderText = placeHolderText.Replace("{DDDD}", hm.ToString("dddd"));
                if (str.Contains("{MMMM}"))
                    placeHolderText = placeHolderText.Replace("{MMMM}", hm.ToString("MMMM"));
                if (str.Contains("{MM}"))
                    placeHolderText = placeHolderText.Replace("{MM}", hm.ToString("MM"));
                if (str.Contains("{H}"))
                    placeHolderText = placeHolderText.Replace("{H}", hm.ToString("HH"));
                if (str.Contains("{h}"))
                    placeHolderText = placeHolderText.Replace("{h}", hm.ToString("hh"));
                if (str.Contains("{M}") || str.Contains("{m}"))
                    placeHolderText = placeHolderText.Replace("{M}", minutes);
                if (str.Contains("{S}") || str.Contains("{s}"))
                    placeHolderText = placeHolderText.Replace("{S}", seconds);
                if (str.Contains("{T}"))
                    placeHolderText = placeHolderText.Replace("{T}", hm.ToString("tt"));
                if (str.ToLower().Contains("{username}"))
                    placeHolderText = placeHolderText.Replace("{username}", player.playerData.username);
                if (str.ToLower().Contains("{id}"))
                    placeHolderText = placeHolderText.Replace("{id}", $"{shPlayer.ID}");
                if (str.ToLower().Contains("{jobname}"))
                    placeHolderText = placeHolderText.Replace("{jobname}", shPlayer.job.info.jobName);
                if (str.ToLower().Contains("{jobindex}"))
                    placeHolderText = placeHolderText.Replace("{jobindex}", $"{shPlayer.job.jobIndex}");

                if (str.ToLower().Contains("{discordlink}"))
                    placeHolderText = placeHolderText.Replace("{discordlink}", MsgDiscord);
                if (str.ToLower().Contains("{infocolor}"))
                    placeHolderText = placeHolderText.Replace("{infocolor}", infoColor);
                if (str.ToLower().Contains("{warningcolor}"))
                    placeHolderText = placeHolderText.Replace("{warningcolor}", warningColor);
                if (str.ToLower().Contains("{errorcolor}"))
                    placeHolderText = placeHolderText.Replace("{errorcolor}", errorColor);
                if (str.ToLower().Contains("{argcolor}"))
                    placeHolderText = placeHolderText.Replace("{argcolor}", argColor);
                return placeHolderText;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
                return null;
            }
        }
    }
}
