using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Threading;

namespace BP_Essentials
{
    class ErrorLogging : EssentialsChatPlugin
    {
        public static void Run(Exception ex)
        {
            if (!File.Exists(ExeptionFile))
            {
                File.Create(ExeptionFile).Close();
            }
            Thread.Sleep(20);
            string[] content =
            {
                Environment.NewLine,
                "Expection START ---------------- Date: " + DateTime.Now,
                Environment.NewLine,
                "Error Message: " + ex.Message,
                "Stack Trace: " + ex.StackTrace,
                "Full error: " + ex,
                Environment.NewLine,
                "Expection STOP ---------------- Date: " + DateTime.Now
            };
            File.AppendAllLines(ExeptionFile, content);
            Debug.Log(ex);
            Debug.Log("[ERROR]   Essentials - An exception occured, Check the Exceptions file for more info.");
            Debug.Log("[ERROR]   Essentials - Or check the error above for more info,");
            Debug.Log("[ERROR]   Essentials - And it would be highly appreciated if you would send the error to the developers of this plugin!");
        }
    }
}
