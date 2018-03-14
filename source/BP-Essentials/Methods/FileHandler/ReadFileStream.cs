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
    class ReadStream : EssentialsChatPlugin
    {
        public static void Run(string fileName, List<string> output)
        {
            try
            {
                foreach (var line in File.ReadAllLines(fileName))
                    if (line.StartsWith("#"))
                        continue;
                    else
                        output.Add(line);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
