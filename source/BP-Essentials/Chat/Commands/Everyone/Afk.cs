using System;
using static BP_Essentials.EssentialsVariablesPlugin;
using System.IO;
namespace BP_Essentials.Commands {
    public class Afk {
        public static void Run(SvPlayer player, string message)
        {
            ReadFile.Run(AfkListFile);
            if (AfkPlayers.Contains(player.playerData.username))
            {
                RemoveStringFromFile.Run(AfkListFile, player.playerData.username);
                ReadFile.Run(AfkListFile);
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>You are no longer AFK.</color>");
            }
            else
            {
                File.AppendAllText(AfkListFile, player.playerData.username + Environment.NewLine);
                AfkPlayers.Add(player.playerData.username);
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>You are now AFK.</color>");
            }
        }
    }
}