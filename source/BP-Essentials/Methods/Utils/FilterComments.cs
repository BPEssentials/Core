using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Text.RegularExpressions;
using System.IO;

namespace BP_Essentials
{
    class FilterComments
    {

        public static string Run(string path)
        {
            string line;
            using (StreamReader file = new StreamReader(path))
            {
                var builder = new StringBuilder();
                // If anyone has a Regex that removes single comments (It should not remove https:// stuff etc because that contains // too)
                // And Block comments /* */ (And the multiple line block comment)
                while ((line = file.ReadLine()) != null)
                    if (!line.Trim().StartsWith("// ", StringComparison.Ordinal))
                        builder.Append(line);
                file.Close();
                return Regex.Replace(builder.ToString(), @"/\*(?:(?!\*/).)*\*/", String.Empty);
            }
        }
    }
}
