using System;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands {
    public class Ban : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message) {
            try
            {
                var player = (SvPlayer)oPlayer;
                string arg1 = GetArgument.Run(1, false, true, message);
                if (!string.IsNullOrWhiteSpace(arg1))
                    ExecuteOnPlayer.Run(player, message, arg1);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, ArgRequired);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}