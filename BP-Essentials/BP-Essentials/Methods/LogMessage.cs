using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

namespace BP_Essentials
{
    class LogMessage : EssentialsChatPlugin
    {
        public static void Run(object oPlayer, string message)
        {
            var player = (SvPlayer)oPlayer;
            if (!message.StartsWith(CmdCommandCharacter))
            {
                var mssge = SetTimeStamp() + "[MESSAGE] " + player.playerData.username + ": " + message;
                Debug.Log(mssge);
                File.AppendAllText(ChatLogFile, mssge + Environment.NewLine);
                File.AppendAllText(LogFile, mssge + Environment.NewLine);
            }
            else if (message.StartsWith(CmdCommandCharacter))
            {
                var mssge = (SetTimeStamp() + "[COMMAND] " + player.playerData.username + ": " + message);
                Debug.Log(mssge);
                File.AppendAllText(CommandLogFile, mssge + Environment.NewLine);
                File.AppendAllText(LogFile, mssge + Environment.NewLine);
            }
        }
    }
}
