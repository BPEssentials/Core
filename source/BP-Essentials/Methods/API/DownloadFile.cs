using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Net;
using System.Collections;

namespace BP_Essentials
{
    class DownloadFile : EssentialsChatPlugin
    {
        static IEnumerator GetSiteContent(string link)
        {
            using (WWW www = new WWW(link))
            {
                if (www.error != null)
                {
                    Debug.Log($"{SetTimeStamp.Run()}[ERROR] {link} responded with HTTP error code: {www.error}!");
                    yield return null;
                }
                yield return www.text;
            }
        }
        public static string Run(string link)
        {
            return GetSiteContent(link).ToString();
        }
    }
}
