/*


    BP:Essentials

    Created by UserR00T, DBK, and BP.
    Currently only being worked on by UserR00T unfortunately. :(

    License: GPLv3.


*/


using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using static BP_Essentials.Variables;
namespace BP_Essentials
{
    public class Core
    {

        //Initialization
        [Hook("SvManager.StartServer")]
        public static void StartServer(SvManager svManager)
        {
            Debug.Log("[INFO] Essentials - Starting up..");
            try
            {
                SvMan = svManager;
                Methods.Utils.FunctionMenu.RegisterMenus();
                Reload.Run(true, null, true);
                CheckAutoReloadFile.Run(AutoReloader);
                if (Variables.Version != LocalVersion)
                {
                    Debug.Log("[ERROR] Essentials - Versions do not match!");
                    Debug.Log("[ERROR] Essentials - Essentials assembly version: " + Variables.Version);
                    Debug.Log("[ERROR] Essentials - Settings file version: " + LocalVersion);
                    Debug.Log("");
                    Debug.Log("");
                    Debug.Log("[ERROR] Essentials - Recreating settings file!");
                    string date = DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss");
                    if (File.Exists(SettingsFile + "." + date + ".OLD"))
                        File.Delete(SettingsFile + "." + date + ".OLD");
                    File.Move(SettingsFile, $"{SettingsFile}.{date}.OLD");
                    Reload.Run(true);
                }
                Save.StartSaveTimer();
                Variables.WarpHandler = new WarpHandler();
                Variables.WarpHandler.LoadAll(true);
                Variables.KitsHandler = new KitsHandler();
                Variables.KitsHandler.LoadAll(true);
                Debug.Log("-------------------------------------------------------------------------------");
                Debug.Log($"[INFO] Essentials - Version: {LocalVersion} {(IsPreRelease ? "[PRE-RELEASE] " : "")}Loaded in successfully!");
                Debug.Log("-------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Debug.Log("-------------------------------------------------------------------------------");
                Debug.Log("    ");
                Debug.Log("[ERROR]   Essentials - Whoopsie doopsie, something went wrong.");
                Debug.Log("[ERROR]   Essentials - Please check the error below for more info,");
                Debug.Log("[ERROR]   Essentials - And it would be highly appreciated if you would send the error to the developers of this plugin!");
                Debug.Log("    ");
                Debug.Log(ex);
                Debug.Log(ex.ToString());
                Debug.Log("-------------------------------------------------------------------------------");

            }
            Debug.Log("[INFO] Essentials - Ready.");

            if (Variables.Announcer.Announcements.Count == 0)
            {
                Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [WARNING] No announcements found in the file!");
                return;
            }
            Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Announcer started successfully!");
        }
    }
}