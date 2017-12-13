using System;
using System.IO;
using System.Threading;
using UnityEngine;
using static BP_Essentials.EssentialsMethodsPlugin;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials
{
    public class EssentialsCorePlugin {

        //Initialization
        [Hook("SvNetMan.StartServerNetwork")]
        public static void StartServerNetwork(SvNetMan netMan)
        {
            try
            {
                Reload.Run(true);
                if (EssentialsVariablesPlugin.Version != LocalVersion)
                {
                    Debug.Log("[ERROR] Essentials - Versions do not match!");
                    Debug.Log("[ERROR] Essentials - Essentials version:" + EssentialsVariablesPlugin.Version);
                    Debug.Log("[ERROR] Essentials - Settings file version" + LocalVersion);
                    Debug.Log("");
                    Debug.Log("");
                    Debug.Log("[ERROR] Essentials - Recreating settings file!");
                    if (File.Exists(SettingsFile + ".OLD"))
                    {
                        File.Delete(SettingsFile + ".OLD");
                    }
                    File.Move(SettingsFile, SettingsFile + ".OLD");
                    Reload.Run(true);
                }
                var thread = new Thread(SavePeriodically.Run);
                thread.Start();
                Debug.Log("-------------------------------------------------------------------------------");
                Debug.Log("    ");
                Debug.Log("[INFO] Essentials - version: " + LocalVersion + " Loaded in successfully!");
                Debug.Log("    ");
                Debug.Log("-------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Debug.Log("-------------------------------------------------------------------------------");
                Debug.Log("    ");
                Debug.Log("[ERROR]   Essentials - A file cannot be loaded in!");
                Debug.Log("[ERROR]   Essentials - Please check the error below for more info,");
                Debug.Log("[ERROR]   Essentials - And it would be highly appreciated if you would send the error to the developers of this plugin!");
                Debug.Log("    ");
                Debug.Log(ex);
                Debug.Log(ex.ToString());
                Debug.Log("-------------------------------------------------------------------------------");

            }

            if (Announcements.Length != 0)
            {
                var thread = new Thread(new ParameterizedThreadStart(Chat.Announce.Run));
                thread.Start(netMan);
                Debug.Log(SetTimeStamp.Run() + "[INFO] Announcer started successfully!");
            }
            else
                Debug.Log(SetTimeStamp.Run() + "[WARNING] No announcements found in the file!");
        }
    }
}