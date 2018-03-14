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
    class RemoveStringFromFile : EssentialsChatPlugin
    {
        public static void Run(string FileName, string RemoveString)
        {
            try
            {
                string content = null;
                foreach (var line in File.ReadAllLines(FileName))
                {
                    if (!line.Contains(RemoveString))
                    {
                        content = content + line + Environment.NewLine;
                    }
                }
                File.WriteAllText(FileName, content);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
