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
    class ReadCustomCommands : EssentialsChatPlugin
    {
        public static void Run()
        {
            string line;
            using (var file = new StreamReader(CustomCommandsFile))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.ToLower().StartsWith("#") || string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    else
                    {
                        if (line.ToLower().StartsWith("command: "))
                        {
                            EssentialsVariablesPlugin.CustomCommands.Add(CmdCommandCharacter + line.Substring(9));
                            line = file.ReadLine();
                            if (line.ToLower().StartsWith("response: "))
                            {
                                Responses.Add(line.Substring(10));
                            }
                        }
                    }
                }
                file.Close();
            }
        }
    }
}
