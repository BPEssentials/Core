using System;
using System.Collections.Generic;
using static BP_Essentials.EssentialsVariablesPlugin;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace BP_Essentials.Commands
{
    class GetLogs : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string logFile)
        {
            if (logFile != ChatLogFile) return;
            // My solution wasn't efficient, so stackoverflow it is!
            // Source: https://stackoverflow.com/questions/4619735/how-to-read-last-n-lines-of-log-file/42735132#42735132
            const int n = 100;
            int count = 0;
            string content;
            byte[] buffer = new byte[1];
            using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(0, SeekOrigin.End);
                while (count < n)
                {
                    fs.Seek(-1, SeekOrigin.Current);
                    fs.Read(buffer, 0, 1);
                    if (buffer[0] == '\n')
                        count++;
                    fs.Seek(-1, SeekOrigin.Current);
                }
                fs.Seek(1, SeekOrigin.Current);
                using (StreamReader sr = new StreamReader(fs))
                    content = sr.ReadToEnd();
            }
            var replace = new Dictionary<string, string>
                    {
                        {"\n","\n<color=#00ffffff>"},
                        {"[MESSAGE]","</color>"},
                        {"<", "<<b></b>"/* Escape Rich Text tags*/ }
                    };
            content = "<color=#00ffffff>" + new Regex(string.Join("|", replace.Keys.Select(k => Regex.Escape(k)))).Replace(content, m => replace[m.Value]) + "</color>";
            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>WARNING: This is a very unstable command and doesn't work all of the times.</color>");
            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>Limited to last <color={argColor}>100</color> messages.</color>");
            player.SendToSelf(Channel.Fragmented, ClPacket.ServerInfo, content);
        }
    }
}
