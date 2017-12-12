using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

namespace BP_Essentials.Commands
{
    class CheckIp : EssentialsChatPlugin
    {
        // TODO
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var arg1 = GetArgument.Run(1, false, true, message);
                var player = (SvPlayer)oPlayer;
                var found = 0;
                //  player.SendToSelf(Channel.Unsequenced, (byte)10, "IP for player: " + arg1);
                var content = string.Empty;
                if (!String.IsNullOrWhiteSpace(arg1))
                {
                    foreach (var line in File.ReadAllLines(IpListFile))
                    {
                        if (line.Contains(arg1) || line.Contains(arg1 + ":"))
                        {
                            ++found;
                            // player.SendToSelf(Channel.Unsequenced, (byte)10, line.Substring(arg1.Length + 1));
                            content = line.Substring(arg1.Length + 1) + "\r\n" + content;
                        }
                    }

                    content = content + "\r\n\r\n" + arg1 + " occurred " + found + " times in the iplog file." + "\r\n";
                    player.SendToSelf(Channel.Reliable, (byte)50, content);
                    //player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " occurred " + found + " times in the iplog file.");
                }
                else
                    player.SendToSelf(Channel.Reliable, (byte)50, "A argument is required for this command.");

            }
            catch(Exception ex)
            {
                ErrorLogging.Run(ex);
            }
                return true;
        }
    }
}
