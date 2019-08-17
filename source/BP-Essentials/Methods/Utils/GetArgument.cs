using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Text.RegularExpressions;

namespace BP_Essentials
{
    public class GetArgument : Core
    {
        // redo?
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
                    return args[nr].TrimStart('"').TrimEnd('"');
                }
                else
                {
                    if (IncludeSpaces)
                    {
                        message = message.Trim();
                        if (nr == 0)
                            return message;
                        if (!message.Contains(" "))
                            return "";
                        var args = message.Split(' ');
                        if (nr > args.Length)
                            return "";
                        return message.Substring(message.IndexOf(args[nr])).TrimEnd();
                    }
                    else
                    {
                        var args = message.Trim().Split(' ');
                        if (nr > args.Length - 1)
                            return "";
                        return args[nr];
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
