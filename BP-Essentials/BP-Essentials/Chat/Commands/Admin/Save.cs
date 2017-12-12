using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Threading;
using static BP_Essentials.EssentialsCorePlugin;

namespace BP_Essentials.Commands {
    public class Save : EssentialsChatPlugin {
        public static bool Run(object oPlayer) {
            var player = (SvPlayer)oPlayer;

            if (AdminsListPlayers.Contains(player.playerData.username))
            {
                var thread = new Thread(SaveNow.Run);
                thread.Start();
                return true;
            }
            player.SendToSelf(Channel.Unsequenced, (byte)10, MsgNoPerm);
            return true;
        }
    }
}