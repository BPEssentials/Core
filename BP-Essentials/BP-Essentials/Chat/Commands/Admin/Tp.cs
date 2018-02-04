using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands {
    public class Tp : EssentialsChatPlugin{
        public static bool TpHere(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                string arg1 = GetArgument.Run(1, false, true, message);
                if (!string.IsNullOrEmpty(arg1))
                    Commands.ExecuteOnPlayer.Run(player, message, arg1);
                else
                    player.SendToSelf(Channel.Unsequenced, 10, ArgRequired);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }

        public static bool Run(object oPlayer, string message) {
            try
            {
                var player = (SvPlayer)oPlayer;
                string arg1 = GetArgument.Run(1, false, true, message);
                if (!string.IsNullOrEmpty(arg1))
                    Commands.ExecuteOnPlayer.Run(player, message, arg1);
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