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
    class RemoveStringFromFile
    {
        public static void Run(string FileName, string RemoveString)
        {
            try
            {
                File.WriteAllLines(FileName, File.ReadLines(FileName).Where(s => !s.Contains(RemoveString)).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
