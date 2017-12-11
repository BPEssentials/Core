using static BP_Essentials.EssentialsVariablesPlugin;
using System;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands {
    public class Tp : EssentialsChatPlugin{
        public static bool TpHere(object oPlayer, string message) {
            var player = (SvPlayer)oPlayer;
            var tempMsg = message.Trim();
            if (tempMsg != CmdTpHere || tempMsg != CmdTpHere2)
            {
                var arg1 = String.Empty;
                if (tempMsg.StartsWith(CmdTpHere + " "))
                {
                    arg1 = tempMsg.Substring(CmdTpHere.Length + 1);
                }
                else if (tempMsg.StartsWith(CmdTpHere2 + " "))
                {
                    arg1 = tempMsg.Substring(CmdTpHere2.Length + 1);
                }
                Commands.ExecuteOnPlayer.Run(player, message, arg1);
            }
            else
                player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
            return true;
        }

        public static bool Run(object oPlayer, string message) {
            var player = (SvPlayer)oPlayer;
            var tempMsg = message.Trim();
            if (tempMsg != CmdTp)
            {
                var arg1 = tempMsg.Substring(CmdTp.Length + 1);
                Commands.ExecuteOnPlayer.Run(player, message, arg1);
            }
            else
                player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
            return true;
        }
    }
}