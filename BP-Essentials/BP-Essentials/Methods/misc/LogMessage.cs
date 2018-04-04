using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;
using System.Threading;

namespace BP_Essentials
{
    class LogMessage : EssentialsChatPlugin
    {
        public static void Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (!message.StartsWith(CmdCommandCharacter))
                {
                    var mssge = SetTimeStamp.Run() + "[MESSAGE] " + player.playerData.username + ": " + message;
                    Debug.Log(mssge);
                    int tries = 0;
                    while (tries < 2)
                        try
                        {
                            File.AppendAllText(ChatLogFile, mssge + Environment.NewLine);
                            File.AppendAllText(LogFile, mssge + Environment.NewLine);
                            break;
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(50);
                            ++tries;
                        }
                }
                else if (message.StartsWith(CmdCommandCharacter))
                {
                    var mssge = (SetTimeStamp.Run() + "[COMMAND] " + player.playerData.username + ": " + message);
                    Debug.Log(mssge);
                    int tries = 0;
                    while (tries < 2)
                        try
                        {
                            File.AppendAllText(CommandLogFile, mssge + Environment.NewLine);
                            File.AppendAllText(LogFile, mssge + Environment.NewLine);
                            break;
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(50);
                            ++tries;
                        }

                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
