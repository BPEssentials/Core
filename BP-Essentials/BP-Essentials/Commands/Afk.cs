using System;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsConfigPlugin;
using System.IO;
namespace BP_Essentials.Commands {
    public class Afk : EssentialsCorePlugin {
        public static bool Run(object oPlayer, string message) {
            
                var player = (SvPlayer)oPlayer;
                ReadFile(AfkListFile);
                if (AfkPlayers.Contains(player.playerData.username))
                {
                    RemoveStringFromFile(AfkListFile, player.playerData.username);
                    ReadFile(AfkListFile);
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