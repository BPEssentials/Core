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
        public static void Run(SvPlayer player, string message)
        {
            try
            {
                var mssge = $"{SetTimeStamp.Run()}[{(message.StartsWith(CmdCommandCharacter) ? "COMMAND" : "MESSAGE")}] {player.playerData.username}: {message}";
                Debug.Log(mssge);
                int tries = 0;
                while (tries < 2)
                    try
                    {
                        if (!message.StartsWith(CmdCommandCharacter))
                            File.AppendAllText(ChatLogFile, mssge + Environment.NewLine);
                        else
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
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
        public static void LogOther(string message)
        {
            try
            {
                Debug.Log(message);
                int tries = 0;
                while (tries < 2)
                    try
                    {
                        File.AppendAllText(LogFile, $"{message}{Environment.NewLine}");
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(50);
                        ++tries;
                    }

            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
        }
    }
}
