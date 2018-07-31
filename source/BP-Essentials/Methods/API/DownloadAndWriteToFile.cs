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
    class DownloadAndWriteToFile : EssentialsChatPlugin
    {
        public static void Run(string fileName, string websiteLink)
        {
            var output = DownloadFile.Run(websiteLink);
            if (output != null)
                File.WriteAllText(fileName, output);
        }
        public static void Run(string fileName, string websiteLink, Action<bool> callback)
        {
            SvMan.StartCoroutine(DownloadFile.Run(websiteLink, new Action<string>((output) => {
                if (output != null)
                    File.WriteAllText(fileName, output);
                callback?.Invoke(output != null);
            })));
        }
    }
}
