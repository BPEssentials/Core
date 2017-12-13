using System;
using static BP_Essentials.EssentialsVariablesPlugin;
using System.IO;
namespace BP_Essentials.Commands {
    public class Afk : EssentialsChatPlugin {
        public static bool Run(object oPlayer) {
                var player = (SvPlayer)oPlayer;
                ReadFile.Run(AfkListFile);
                if (AfkPlayers.Contains(player.playerData.username))
                {
                    RemoveStringFromFile.Run(AfkListFile, player.playerData.username);
                    ReadFile.Run(AfkListFile);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "You are no longer AFK");
                }
                else
                {
                    File.AppendAllText(AfkListFile, player.playerData.username + Environment.NewLine);
                    AfkPlayers.Add(player.playerData.username);
                    player.SendToSelf(Channel.Unsequenced, (byte)10, "You are now AFK");
                }
            return true;

        }
    }
}