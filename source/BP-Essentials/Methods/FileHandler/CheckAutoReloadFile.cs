using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BP_Essentials
{
    class CheckAutoReloadFile
    {
        public static void Run(string file)
        {
            try
            {
                var watcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(file),
                    Filter = Path.GetFileName(file),
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    EnableRaisingEvents = true
                };
                watcher.Changed += new FileSystemEventHandler((sender, e) =>
                {
                    Debug.Log($"{SetTimeStamp.Run()}[INFO] Found a change in file {file}, reloading all files...");
                    Debug.Log("    ");
                    Reload.Run(true);
                });
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
