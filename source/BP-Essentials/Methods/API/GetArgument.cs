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
                    message = message.Trim();
                    if (IncludeSpaces)
                    {
                        message = message.Trim();
                        if (!message.Contains(" "))
                            return "";
                        var args = message.Split(' ');
                        if (nr >= args.Length)
                            return "";
                        return message.Substring(message.IndexOf(args[nr])).TrimEnd();
                    }
                    else
                    {
                        if (!message.Contains(" "))
                            message = message + " ";
                        string[] args = message.Split(' ');
                        return nr <= args.Length ? args[nr] : "";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
                return "";
            }
        }
    }
}
