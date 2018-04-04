using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Text.RegularExpressions;

namespace BP_Essentials
{
    class GetArgument : EssentialsCorePlugin
    {
        public static string Run(int nr, bool UseRegex, bool IncludeSpaces, string message)
        {
            try
            {
                if (UseRegex)
                {
                    var args = Regex.Matches(message, @"[\""].+?[\""]|[^ ]+")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToList();

                    return args[nr];
                }
                else
                {
                    if (IncludeSpaces)
                    {
                        string tmessage = message + " ";
                        string[] args = tmessage.Split(' ');
                        return tmessage.IndexOf(args[nr]) != 0 ? tmessage.Substring(tmessage.IndexOf(args[nr])).TrimEnd() : string.Empty;
                    }
                    else
                    {
                        string tmessage = message + " ";
                        string[] args = tmessage.Split(' ');
                        return args[nr] ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
                return null;
            }
        }
    }
}
