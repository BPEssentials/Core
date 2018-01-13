using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials
{
    class StringToVar : EssentialsChatPlugin
    {
        public static void Run(string s, string cmd1, string cmd2 = null, string ExeBy = null, bool? Disabled = null)
        {
            try
            {
                typeof(EssentialsVariablesPlugin).GetField(@"Cmd" + s).SetValue("Cmd" + s, CmdCommandCharacter + cmd1);
                if (!string.IsNullOrWhiteSpace(cmd2))
                    typeof(EssentialsVariablesPlugin).GetField(@"Cmd" + s + "2").SetValue("Cmd" + s + "2", CmdCommandCharacter + cmd2);
                if (!string.IsNullOrWhiteSpace(ExeBy))
                    typeof(EssentialsVariablesPlugin).GetField(@"Cmd" + s + "ExecutableBy").SetValue("Cmd" + s + "ExecutableBy", ExeBy);
                if (!string.IsNullOrWhiteSpace(Disabled.ToString()))
                    typeof(EssentialsVariablesPlugin).GetField(@"Cmd" + s + "Disabled").SetValue("Cmd" + s + "Disabled", Disabled);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
