using static BP_Essentials.Variables;
using System;
using static BP_Essentials.HookMethods;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BP_Essentials.Commands
{
    [Obsolete]
    public class TpLocation
    {
        [Obsolete]
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            foreach (var currKey in PlaceDictionary.Keys)
                if (currKey.Contains(arg1))
                {
                    player.ResetAndSavePosition(PlaceDictionary[currKey], player.player.GetRotation(), 0);
                    player.SendChatMessage($"<color={infoColor}>Teleported to</color> <color={argColor}>{currKey[1]}</color><color={infoColor}>.</color>");
                    return;
                }
            player.SendChatMessage($"<color={errorColor}>Invalid place name!</color>");
        }
    }
}