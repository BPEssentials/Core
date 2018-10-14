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
    class GetIdList
    {
        public static bool Run(bool silent)
        {
            // yeah eh redo
            if (!silent)
                Debug.Log("Downloading newest ID list's and reloading them...");
            GetWebsiteContent.WriteToFile(IdListItemsFile, "http://www.UserR00T.com/dev/BPEssentials/idlist_items.txt", new Action<bool>((success) =>
            {
                if (success)
                    ReadFile.Run(IdListItemsFile);
                GetWebsiteContent.WriteToFile(IdListVehicleFile, "http://www.UserR00T.com/dev/BPEssentials/idlist_vehicles.txt", new Action<bool>((success2) =>
                {
                    if (success2)
                        ReadFile.Run(IdListVehicleFile);
                    if (!silent && success && success2)
                        Debug.Log($"[OK] Downloaded newest ID list's and reloaded them! ({IDs_Items.Length}(items) {IDs_Vehicles.Length}(vehicles) entries loaded in.)");
                }));
            }));
            return true;
        }
    }
}
