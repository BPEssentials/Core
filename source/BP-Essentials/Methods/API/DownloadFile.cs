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
        // Redo method(s)

        public static string Run(string DownloadLink)
        {
            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead(DownloadLink);
                using (StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }


        public static IEnumerator Run(string link, Action<string> callback)
        {
            using (var www = new WWW(link))
            {
                yield return www;
                if (www.error != null)
                {
                    Debug.Log($"{SetTimeStamp.Run()}[ERROR] {link} responded with HTTP error code: {www.error}!");
                    callback?.Invoke(null);
                    yield break;
                }
                callback?.Invoke(www.text);
                yield break;
            }
        }
    }
}
