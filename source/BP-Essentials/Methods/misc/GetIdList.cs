using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

namespace BP_Essentials
{
    class GetIdList : EssentialsChatPlugin
    {
        public static bool Run(bool silent)
        {
            if (silent)
            {
                File.WriteAllText(IdListFile, DownloadFile.Run("http://www.UserR00T.com/dev/BPEssentials/idlist.txt"));
                ReadFile.Run(IdListFile);
            }
            else
            {
                Debug.Log("Downloading newest ID list...");
                File.WriteAllText(IdListFile, DownloadFile.Run("http://www.UserR00T.com/dev/BPEssentials/idlist.txt"));
                Debug.Log("[OK] ID list downloaded");
                Debug.Log("Reloading ID list..");
                ReadFile.Run(IdListFile);
                Debug.Log("[OK] Downloaded newest ID list and reloaded it!");
            }
            return true;
        }
    }
}
