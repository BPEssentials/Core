using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Reflection;

namespace BP_Essentials
{
    class RegisterCommands
    {
        public static void Run(List<_Command> cmdlist)
        {
            Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Registering commands..");
            CommandList.Clear();
            foreach (var command in cmdlist)
            {
                if (DebugLevel >= 1)
                    Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Registering command: {command.CommandName}..");
                Action<SvPlayer, string> rMethod = null;
                try
                {
                    rMethod = (Action<SvPlayer, string>)
                                Delegate.CreateDelegate(typeof(Action<SvPlayer, string>),
                                Type.GetType($"BP_Essentials.Commands.{command.CommandName}")
                                .GetMethod(nameof(Run)));
                }
                catch (ArgumentException)
                {
                    rMethod = null;
                }
                if (command.CommandName == "ToggleReceiveStaffChat")
                    CmdStaffChatExecutableBy = command.ExecutableBy;
                if (command.CommandName == "Confirm")
                    CmdConfirm = command.Commands[0];
                if (command.CommandName == "ToggleChat")
                    CmdToggleChat = command.Commands[0];
                if (command.CommandName == "TpaAccept")
                    CmdTpaaccept = command.Commands[0];
                CommandList.Add(CommandList.Count + 1, new _CommandList {
                    RunMethod = rMethod,

                    commandGroup = command.ExecutableBy ?? "everyone",
                    commandName = command.CommandName,
                    commandCmds = command.Commands.Select(x=>$"{CmdCommandCharacter}{x}").ToList(),
                    commandDisabled = command.Disabled ?? false,
                    commandWantedAllowed = command.AllowWithCrimes ?? true,
                    commandHandcuffedAllowed = command.AllowWhileCuffed ?? true,
					commandWhileJailedAllowed = command.AllowWhileJailed ?? true
                });
            }
            Debug.Log($"{PlaceholderParser.ParseTimeStamp()} [INFO] Registered commands! ({CommandList.Count} commands loaded in.)");
        }
    }
}
