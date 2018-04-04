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
    class GetLogs : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string logFile)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdLogsExecutableBy == "admins" || CmdLogsExecutableBy == "everyone")
                {
                    if (logFile != ChatLogFile) return true;
                    string content = null;
                    using (var reader = new StreamReader(logFile))
                    {
                        for (var i = 0; i < 31; i++)
                        {
                            string line = null;
                            if ((line = reader.ReadLine()) != null)
                                content = content + "\r\n" + line;
                            else
                                break;
                        }
                    }
                    player.SendToSelf(Channel.Unsequenced, 10, $"<color={warningColor}>WARNING: This is a very unstable command and doesn't work all of the times.</color>");
                    player.SendToSelf(Channel.Fragmented, 50, content);
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
