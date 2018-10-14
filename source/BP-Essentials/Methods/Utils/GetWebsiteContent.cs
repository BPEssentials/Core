using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;

namespace BP_Essentials
{
    class GetWebsiteContent
    {
        public static void WriteToFile(string fileName, string websiteLink)
        {
            var output = GetContentWebClient(websiteLink);
            if (output != null)
                File.WriteAllText(fileName, output);
        }
        public static void WriteToFile(string fileName, string websiteLink, Action<bool> callback)
        {
            SvMan.StartCoroutine(GetContentWWW(websiteLink, new Action<string>((output) => {
                if (output != null)
                    File.WriteAllText(fileName, output);
                callback?.Invoke(output != null);
            })));
        }

        public static string GetContentWebClient(string DownloadLink)
        {
            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead(DownloadLink);
                using (StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
        public static IEnumerator GetContentWWW(string link, Action<string> callback)
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
