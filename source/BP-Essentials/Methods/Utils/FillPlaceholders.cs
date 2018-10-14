using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace BP_Essentials
{
    class FillPlaceholders
    {
        public static string Run(ShPlayer shplayer, string message, string playerMessage)
        {
            try
            {
                return message.Replace("{username}", new Regex("(<)").Replace(shplayer.username, "<<b></b>"))
                              .Replace("{id}", $"{shplayer.ID}")
                              .Replace("{jobindex}", $"{shplayer.job.jobIndex}")
                              .Replace("{jobname}", $"{Jobs[shplayer.job.jobIndex]}")
                              .Replace("{message}", new Regex("(<)").Replace(Chat.LangAndChatBlock.Run(playerMessage), "<<b></b>"));
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return null;
        }
    }
}
