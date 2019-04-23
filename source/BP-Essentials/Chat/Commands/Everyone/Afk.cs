using System;
using static BP_Essentials.Variables;
using System.IO;
namespace BP_Essentials.Commands
{
    public class Afk
    {
        public static void Run(SvPlayer player, string message)
        {
            ReadFile.Run(AfkListFile);
            if (AfkPlayers.Contains(player.player.username))
            {
                RemoveStringFromFile.Run(AfkListFile, player.player.username);
                ReadFile.Run(AfkListFile);
                player.SendChatMessage($"<color={infoColor}>You are no longer AFK.</color>");
            }
            else
            {
                File.AppendAllText(AfkListFile, player.player.username + Environment.NewLine);
                AfkPlayers.Add(player.player.username);
                player.SendChatMessage($"<color={infoColor}>You are now AFK.</color>");
            }
        }
    }
}