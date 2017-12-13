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
    class CheckPlayer : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var arg1 = GetArgument.Run(1, false, true, message);
                var player = (SvPlayer)oPlayer;
                var found = 0;
                if (!String.IsNullOrWhiteSpace(arg1))
                {
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "Accounts for IP: " + arg1);
                    foreach (var line in File.ReadAllLines(IpListFile))
                    {
                        if (line.Contains(arg1) || line.Contains(" " + arg1))
                        {
                            ++found;
                            var arg1temp = line.Replace(": " + arg1, " ");
                            player.SendToSelf(Channel.Unsequenced, (byte)10, arg1temp);
                        }
                    }
                    player.SendToSelf(Channel.Unsequenced, (byte)10, arg1 + " occurred " + found + " times in the iplog file.");
                }
                else
                    player.SendToSelf(Channel.Reliable, (byte)50, "A argument is required for this command.");
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
