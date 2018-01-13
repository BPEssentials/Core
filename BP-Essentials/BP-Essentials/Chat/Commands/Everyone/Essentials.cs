using System;
using static BP_Essentials.EssentialsMethodsPlugin;
namespace BP_Essentials.Commands {
    public class Essentials : EssentialsChatPlugin {
        public static bool Run(object oPlayer, string message) {
            try
            {
                var player = (SvPlayer)oPlayer;
                string arg = GetArgument.Run(1, false, false, message).Trim().ToLower();
                if (arg == "ver" || arg == "version")
                    player.SendToSelf(Channel.Unsequenced, 10, "Version: " + EssentialsVariablesPlugin.Version);
                else if (arg == "info")
                {
                    player.SendToSelf(Channel.Unsequenced, 10, "Essentials Created by UserR00T & DeathByKorea & BP");
                    player.SendToSelf(Channel.Unsequenced, 10, "Version " + EssentialsVariablesPlugin.Version);
                    player.SendToSelf(Channel.Unsequenced, 10, "Type '/essentials help' for more info. ");
                }
                else if (arg == "help")
                {
                    player.SendToSelf(Channel.Unsequenced, 10, "Subcommands for /essentials:");
                    player.SendToSelf(Channel.Unsequenced, 10, " - help : Shows this menu.");
                    player.SendToSelf(Channel.Unsequenced, 10, " - info : Shows infomation about the developer.");
                    player.SendToSelf(Channel.Unsequenced, 10, " - ver/version : Shows version number.");
                    player.SendToSelf(Channel.Unsequenced, 10, " - cmd/command : Gives a link to the website of BP-Essentials.");
                }
                else if (arg == "cmd" || arg == "cmds" || arg == "command" || arg == "commands")
                    player.SendToSelf(Channel.Unsequenced, 10, "Up to date help can be found at http://bit.do/BPEssentials");
                else
                {
                    player.SendToSelf(Channel.Unsequenced, 10, "Essentials Created by UserR00T & DeathByKorea & BP.");
                    player.SendToSelf(Channel.Unsequenced, 10, "Version " + EssentialsVariablesPlugin.Version);
                    player.SendToSelf(Channel.Unsequenced, 10, "Type '/essentials help' for more info. ");
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;

        }
    }
}