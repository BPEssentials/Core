using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Net;

namespace BP_Essentials
{
    class DownloadFile : EssentialsChatPlugin
    {
        public static string Run(string DownloadLink)
        {
            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead(DownloadLink);
                using (StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}
