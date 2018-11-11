﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Threading;
using System.Reflection;

namespace BP_Essentials
{
    class ErrorLogging
    {
        public static void Run(Exception ex)
        {
            try
            {
                Thread.Sleep(20);
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(st.FrameCount - 1);
                Debug.Log("-------------------------------------------------------------------------------------------------------------------------------------------------------------");
                Debug.Log("[ERROR] - Exception!");
                Debug.Log("     ");
                Debug.Log("--------------------------------------------------------------------");
                Debug.Log("Exception Frame: " + frame);
                Debug.Log("Exception Method: " + frame.GetMethod());
                Debug.Log("Exception Line number: " + frame.GetFileLineNumber());
                Debug.Log("Exception Column number: " + frame.GetFileColumnNumber());
                Debug.Log("Exception File name: " + frame.GetFileName());
                Debug.Log("Exception Class name: " + ex.TargetSite.ReflectedType.Name);
                Debug.Log("Exception Namespace: " + ex.TargetSite.ReflectedType.Namespace);
                Debug.Log("--------------------------------------------------------------------");
                Debug.Log("     ");
                Debug.Log("     ");
                Debug.Log("[ERROR]   Essentials - Error message: " + ex);
                Debug.Log("     ");
                Debug.Log("     ");
                Debug.Log("[ERROR]   Essentials - An exception occured, Check the Exceptions file for more info.");
                Debug.Log("[ERROR]   Essentials - Or check the error above for more info,");
                Debug.Log("[ERROR]   Essentials - And it would be highly appreciated if you would send the error to the developers of this plugin!");
                Debug.Log("------------------------------------------------------------------------------------------------------------------------------------------------------------");

                string content =
                    $"\nException START ---------------- Date: {DateTime.Now}\n" +
                    $"Error Message: {ex.Message}\n" +
                    $"Full error: {ex}\n" +
                    $"Exception Class name: {ex.TargetSite.ReflectedType.Name}\n" +
                    $"Exception Method: {frame.GetMethod()}\n" +
                    $"Exception STOP ---------------- Date: {DateTime.Now}";
                File.AppendAllText(ExceptionFile, content);
            }
            catch (Exception exx)
            {
                Debug.Log("     ");
                Debug.Log("[ERROR]   Essentials - Error inside ErrorLogging. " + exx);
                Debug.Log("     ");
            }
        }
    }
}
