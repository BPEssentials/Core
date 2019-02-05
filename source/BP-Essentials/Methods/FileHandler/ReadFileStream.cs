using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;

namespace BP_Essentials
{
    class ReadStream
    {
        public static void Run(string fileName, List<string> output)
        {
            try
            {
                output.Clear();
				foreach (var line in File.ReadAllLines(fileName))
				{
					if (line.StartsWith("#", StringComparison.CurrentCulture))
						continue;
					output.Add(line);
				}
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}