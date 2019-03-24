using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.Reflection;

namespace BP_Essentials.Commands
{
    class _SetJob : SvPlayer
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, false, message);
            string arg2 = GetArgument.Run(2, false, true, message);
            byte arg1Parsed = 10;
            string msg = $"<color={infoColor}>Set </color><color={argColor}>{{0}}</color><color={infoColor}>'s Job to</color> <color={argColor}>{{1}}</color><color={infoColor}>.</color>";
            if (string.IsNullOrEmpty(arg2))
                arg2 = player.player.username;
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(NotValidArg);
                return;
            }
            bool parsedCorrectly = true;
            if (Jobs.Contains(arg1))
                arg1Parsed = Convert.ToByte((Array.IndexOf(Jobs, arg1)));
            else
                parsedCorrectly = byte.TryParse(arg1, out arg1Parsed);
            if (!parsedCorrectly)
            {
                player.SendChatMessage(NotValidArg);
                return;
            }
            if (arg1Parsed > Jobs.Length || arg1Parsed < 0)
            {
                player.SendChatMessage($"<color={errorColor}>Error: The value must be between 0 and {Jobs.Length}.</color>");
                return;
            }
            var currPlayer = GetShByStr.Run(arg2);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            player.SendChatMessage(string.Format(msg, currPlayer.username, Jobs[arg1Parsed]));
            SetJob.Run(currPlayer, arg1Parsed, true, false);
        }
    }
}
