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
            var content = DownloadFile.Run(websiteLink);
            if (content != null)
                File.WriteAllText(fileName, content);
        }
    }
}
