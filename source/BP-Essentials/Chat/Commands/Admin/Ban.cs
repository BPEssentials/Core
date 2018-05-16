using System;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands {
    public class Ban : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message) {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (HasPermission.Run(player, CmdBanExecutableBy))
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (!string.IsNullOrEmpty(arg1))
                        ExecuteOnPlayer.Run(player, message, arg1);
                    else
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                }
                else
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}