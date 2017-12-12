using static BP_Essentials.EssentialsMethodsPlugin;
namespace BP_Essentials.Commands {
    public class Essentials : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message) {
            var player = (SvPlayer)oPlayer;
            string arg = GetArgument.Run(1, false, false, message).Trim().ToLower();
            if (arg == "ver" || arg == "version")
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Version: " + EssentialsVariablesPlugin.Version);
            }
            else if (arg == "info")
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Essentials Created by UserR00T & DeathByKorea & BP");
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Version " + EssentialsVariablesPlugin.Version);
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Type '/essentials help' for more info. ");
            }
            else if (arg == "help")
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Subcommands for /essentials:");
                player.SendToSelf(Channel.Unsequenced, (byte)10, " - help : Shows this menu.");
                player.SendToSelf(Channel.Unsequenced, (byte)10, " - info : Shows infomation about the developer.");
                player.SendToSelf(Channel.Unsequenced, (byte)10, " - ver/version : Shows version number.");
            }
            else if (arg == "cmd" || arg == "cmds" || arg == "command" || arg == "commands")
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Up to date help can be found at http://bit.do/BPEssentials");
            }
            else
            {
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Essentials Created by UserR00T & DeathByKorea & BP.");
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Version " + EssentialsVariablesPlugin.Version);
                player.SendToSelf(Channel.Unsequenced, (byte)10, "Type '/essentials help' for more info. ");
            }
            return true;

        }
    }
}