using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Money
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            string arg2 = message.Split(' ').Last().Trim();
            string correctSyntax = $"<color={argColor}>{GetArgument.Run(0, false, false, message)}</color> <color={errorColor}>[Player] [Amount]</color> <color={warningColor}>(Incorrect or missing argument(s).)</color>";
            if (string.IsNullOrEmpty(arg1) || string.IsNullOrEmpty(arg2))
            {
                player.SendChatMessage(correctSyntax);
                return;
            }
            int lastIndex = arg1.LastIndexOf(" ", StringComparison.CurrentCulture);
            if (lastIndex != -1)
                arg1 = arg1.Remove(lastIndex).Trim();
            bool parsedSuccessfully = int.TryParse(arg2, out var arg2int);
            if (!parsedSuccessfully)
            {
                player.SendChatMessage(correctSyntax);
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            currPlayer.TransferMoney(DeltaInv.AddToMe, arg2int, true);
            player.SendChatMessage($"<color={infoColor}>Successfully gave</color> <color={argColor}>{currPlayer.username} {arg2int}</color><color={infoColor}>$</color>");
            currPlayer.svPlayer.SendChatMessage($"<color={argColor}>{player.player.username}</color><color={infoColor}> gave you </color><color={argColor}>{arg2int}</color><color={infoColor}>$!</color>");
        }
    }
}
