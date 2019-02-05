using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;
using System.Threading;

namespace BP_Essentials
{
    class LogMessage
    {
        public static void LocalMessage(SvPlayer player, string message)
        {
            Run(player, message, "[LOCAL-CHAT] ");
        }
        public static void Run(SvPlayer player, string message, string prefix = "")
        {
            try
            {
                var mssge = $"{PlaceholderParser.ParseTimeStamp()} {prefix}[{(message.StartsWith(CmdCommandCharacter, StringComparison.CurrentCulture) ? "COMMAND" : "MESSAGE")}] {player.playerData.username}: {message}";
                Debug.Log(mssge);
                int tries = 0;
                while (tries < 2)
                    try
                    {
                        if (!message.StartsWith(CmdCommandCharacter, StringComparison.CurrentCulture))
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
