﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

namespace BP_Essentials.Commands
{
    class CheckAlts : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdCheckAltsExecutableBy == "admins" || CmdCheckAltsExecutableBy == "everyone")
                {
                    var arg1 = GetArgument.Run(1, false, false, message);
                    var found = 0;
                    if (!String.IsNullOrEmpty(arg1))
                    {
                        var arg2 = GetArgument.Run(2, false, true, message);
                        if (arg1.Equals("ip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!arg2.StartsWith("::ffff:"))
                                arg2 = "::ffff:" + arg2;
                            var content = "Possible accounts using the IP " + arg2 + ":\r\n\r\n";
                            var builder = new StringBuilder();
                            builder.Append(content);
                            foreach (var line in File.ReadAllLines(IpListFile))
                            {
                                if (line.EndsWith(arg2))
                                {
                                    ++found;
                                    builder.Append(line.Replace(": " + arg2, String.Empty) + "\r\n");
                                }
                            }
                            content = builder.ToString();
                            content += "\r\n\r\n" + arg2 + " occurred " + found + " times in the iplog file." + "\r\n";
                            player.SendToSelf(Channel.Reliable, 50, content);
                        }
                        else if (arg1.Equals("ign", StringComparison.InvariantCultureIgnoreCase) || (arg1.Equals("player", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            var content = "Possible logins on the account " + arg2 + " using the following IP's:\r\n\r\n";
                            var builder = new StringBuilder();
                            builder.Append(content);
                            foreach (var line in File.ReadAllLines(IpListFile))
                            {
                                if (line.StartsWith(arg2 + ": "))
                                {
                                    ++found;
                                    builder.Append(line.Replace(arg2 + ": ", String.Empty) + "\r\n");
                                }
                            }
                            content = builder.ToString();
                            content = content + "\r\n\r\n" + arg2 + " occurred " + found + " times in the iplog file." + "\r\n";
                            player.SendToSelf(Channel.Reliable, 50, content);
                        }
                    }
                    else
                        player.SendToSelf(Channel.Reliable, 10, CmdCheckAlts + "[IP/IGN] [Arg2] Eg " + CmdCheckAlts + " ip 127.0.0.1");

                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
