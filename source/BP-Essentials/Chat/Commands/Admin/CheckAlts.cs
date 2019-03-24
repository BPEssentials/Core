using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;

namespace BP_Essentials.Commands
{
    class CheckAlts
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, false, message);
            var found = 0;
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(GetArgument.Run(0, false, false, message) + "[IP/IGN] [Arg2] Eg " + GetArgument.Run(0, false, false, message) + " ip 127.0.0.1");
                return;
            }
            var arg2 = GetArgument.Run(2, false, true, message);
            if (arg1.Equals("ip", StringComparison.InvariantCultureIgnoreCase))
            {
                var content = "Possible accounts using the IP " + arg2 + ":\r\n\r\n";
                var builder = new StringBuilder();
                builder.Append(content);
                foreach (var line in File.ReadAllLines(IpListFile))
                {
                    if (line.EndsWith(arg2, StringComparison.CurrentCulture))
                    {
                        ++found;
                        builder.Append(line.Replace(": " + arg2, string.Empty) + "\r\n");
                    }
                }
                content = builder.ToString();
                content += "\r\n\r\n" + arg2 + " occurred " + found + " times in the iplog file." + "\r\n";
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ServerInfo, content);
            }
            else if (arg1.Equals("ign", StringComparison.InvariantCultureIgnoreCase) || (arg1.Equals(nameof(player), StringComparison.InvariantCultureIgnoreCase)))
            {
                var content = "Possible logins on the account " + arg2 + " using the following IP's:\r\n\r\n";
                var builder = new StringBuilder();
                builder.Append(content);
                foreach (var line in File.ReadAllLines(IpListFile))
                {
                    if (line.StartsWith(arg2 + ": ", StringComparison.CurrentCulture))
                    {
                        ++found;
                        builder.Append(line.Replace(arg2 + ": ", string.Empty) + "\r\n");
                    }
                }
                content = builder.ToString();
                content = content + "\r\n\r\n" + arg2 + " occurred " + found + " times in the iplog file." + "\r\n";
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ServerInfo, content);
            }
        }
    }
}
