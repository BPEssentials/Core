using System;
using System.Threading;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands {
    public class ClearChat : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message) {
            try {
                var player = (SvPlayer) oPlayer;
                string arg1 = GetArgument.Run(1, false, false, message).Trim();
                if (arg1 == "all" || arg1 == "everyone") {
                    if (AdminsListPlayers.Contains(player.playerData.username)) {
                        player.SendToAll(Channel.Unsequenced, (byte) 10, "Clearing chat for everyone...");
                        Thread.Sleep(500);
                        for (var i = 0; i < 6; i++)
                            player.SendToAll(Channel.Unsequenced, (byte) 10, " ");
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte) 10, MsgNoPerm);
                }
                else {
                    player.SendToSelf(Channel.Unsequenced, (byte) 10, "Clearing the chat for yourself...");
                    Thread.Sleep(500);
                    for (var i = 0; i < 6; i++)
                        player.SendToSelf(Channel.Unsequenced, (byte) 10, " ");
                }
            }
            catch (Exception ex) {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
