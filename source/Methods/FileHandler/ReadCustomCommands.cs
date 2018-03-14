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
        public string command { get; set; }
        public string response { get; set; }
    }

    public class _RootObject
    {
        public List<CustomCommand> CustomCommands { get; set; }
    }
    class ReadCustomCommands : EssentialsChatPlugin
    {

        public static void Run()
        {
            try
            {
                _RootObject m = JsonConvert.DeserializeObject<_RootObject>(FilterComments.Run(CustomCommandsFile));
                foreach (var command in m.CustomCommands)
                {
                    CustomCommands.Add(CmdCommandCharacter + command.command);
                    Responses.Add(command.response);
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
