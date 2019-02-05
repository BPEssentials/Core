using System;
using static BP_Essentials.HookMethods;
namespace BP_Essentials.Commands {
    public class Essentials {
        public static void Run(SvPlayer player, string message)
        {
            string arg = GetArgument.Run(1, false, false, message).Trim().ToLower();
			switch (arg)
			{
				case "ver":
				case "version":
					player.SendChatMessage("Version: " + Variables.Version);
					break;
				case "help":
					player.SendChatMessage("Subcommands for /essentials:");
					player.SendChatMessage(" - help : Shows this menu.");
					player.SendChatMessage(" - info : Shows infomation about the developer.");
					player.SendChatMessage(" - ver/version : Shows version number.");
					player.SendChatMessage(" - cmd/command : Gives a link to the website of BP-Essentials.");
					break;
				case "cmd":
				case "cmds":
				case "command":
				case "commands":
					player.SendChatMessage("Up to date help can be found at https://goo.gl/Rwhtjs");
					break;
				case "info":
				default:
					player.SendChatMessage("Essentials Created by UserR00T & DeathByKorea & BP.");
					player.SendChatMessage("Version " + Variables.Version);
					player.SendChatMessage("Type '/essentials help' for more info. ");
					break;
			}
		}
    }
}