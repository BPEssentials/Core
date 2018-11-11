using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using Newtonsoft.Json;

namespace BP_Essentials
{
    public class CustomCommand
    {
        public string Command { get; set; }
        public string Response { get; set; }
    }
    class RootObject
    {
        public List<CustomCommand> CustomCommands { get; set; }
    }
    class ReadCustomCommands
    {

        public static void Run()
        {
            try
            {
                var m = JsonConvert.DeserializeObject<RootObject>(FilterComments.Run(CustomCommandsFile));
                CustomCommands = m.CustomCommands;
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
