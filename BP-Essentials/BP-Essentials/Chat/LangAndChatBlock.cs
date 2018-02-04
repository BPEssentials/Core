using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.Text.RegularExpressions;

namespace BP_Essentials.Chat
{
    class LangAndChatBlock : EssentialsChatPlugin
    {
        public static string Run(object oPlayer, string message)
        {
            try
            {
                string tmessage = message.ToLower();
                var player = (SvPlayer)oPlayer;
                if (ChatBlock)
                {
                    const RegexOptions Options = RegexOptions.IgnoreCase;
                    IEnumerable<Regex> badWordMatchers = ChatBlockWords.ToArray().Select(x => new Regex(string.Format(PatternTemplate, x), Options));
                    string output = badWordMatchers.Aggregate(tmessage, (current, matcher) => matcher.Replace(current, CensoredText));
                    return output;
                }
                if (LanguageBlock)
                {
                    const RegexOptions Options = RegexOptions.IgnoreCase;
                    IEnumerable<Regex> badWordMatchers = LanguageBlockWords.ToArray().Select(x => new Regex(string.Format(PatternTemplate, x), Options));
                    string output = badWordMatchers.Aggregate(tmessage, (current, matcher) => matcher.Replace(current, CensoredText));
                    return output;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return message;
        }
    }
}
