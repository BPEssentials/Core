using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using System.Threading;
using static BP_Essentials.EssentialsCorePlugin;

namespace BP_Essentials.Commands {
    public class Save : EssentialsChatPlugin {
        public static bool Run(object oPlayer)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdSaveExecutableBy))
                {
                    var thread = new Thread(SaveNow.Run);
                    thread.Start();
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}