﻿using System;
using static BP_Essentials.EssentialsMethodsPlugin;
namespace BP_Essentials.Commands {
    public class Essentials : EssentialsChatPlugin {
        public static void Run(SvPlayer player, string message)
        {
            string arg = GetArgument.Run(1, false, false, message).Trim().ToLower();
            if (arg == "ver" || arg == "version")
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Version: " + EssentialsVariablesPlugin.Version);
            else if (arg == "info")
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Essentials Created by UserR00T & DeathByKorea & BP");
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Version " + EssentialsVariablesPlugin.Version);
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Type '/essentials help' for more info. ");
            }
            else if (arg == "help")
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Subcommands for /essentials:");
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, " - help : Shows this menu.");
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, " - info : Shows infomation about the developer.");
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, " - ver/version : Shows version number.");
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, " - cmd/command : Gives a link to the website of BP-Essentials.");
            }
            else if (arg == "cmd" || arg == "cmds" || arg == "command" || arg == "commands")
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Up to date help can be found at http://bit.do/BPEssentials");
            else
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Essentials Created by UserR00T & DeathByKorea & BP.");
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Version " + EssentialsVariablesPlugin.Version);
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, "Type '/essentials help' for more info. ");
            }
        }
    }
}