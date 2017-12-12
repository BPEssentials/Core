using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands {
    public class Ban : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message) {
            var player = (SvPlayer)oPlayer;

            var tempMsg = message.Trim();
            if (tempMsg != CmdBan)
            {
                var arg1 = tempMsg.Substring(CmdBan.Length + 1);
                ExecuteOnPlayer.Run(player, message, arg1);
            }
            else
                player.SendToSelf(Channel.Unsequenced, (byte)10, "A argument is needed for this command.");
            return true;
        }
    }
}