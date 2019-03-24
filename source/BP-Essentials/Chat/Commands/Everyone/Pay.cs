using static BP_Essentials.Variables;
using System;
using System.Linq;
using UnityEngine;

namespace BP_Essentials.Commands
{
    public class Pay
    {
        public static void Run(SvPlayer player, string message)
        {
            string correctSyntax = $"<color={argColor}>{GetArgument.Run(0, false, false, message)}</color> <color={errorColor}>[Player] [Amount]</color> <color={warningColor}>(Incorrect or missing argument(s).)</color>";
            string arg1 = GetArgument.Run(1, false, true, message);
            string arg2 = message.Split(' ').Last().Trim();
            if (string.IsNullOrEmpty(GetArgument.Run(1, false, false, message)) || string.IsNullOrEmpty(arg2))
            {
                player.SendChatMessage(correctSyntax);
                return;
            }
            int lastIndex = arg1.LastIndexOf(" ", StringComparison.CurrentCulture);
            if (lastIndex != -1)
                arg1 = arg1.Remove(lastIndex).Trim();
            var parsedSuccessfully = int.TryParse(arg2, out int arg2Int);
            if (!parsedSuccessfully)
            {
                player.SendChatMessage(correctSyntax);
                return;
            }
            if (arg2Int <= 0)
            {
                player.SendChatMessage($"<color={errorColor}>Cannot transfer 0$ or less.</color>");
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }

            if (player.player.MyMoneyCount() < arg2Int)
            {
                player.SendChatMessage($"<color={errorColor}>Cannot transfer money, do you have</color> <color={argColor}>{arg2Int}</color><color={errorColor}>$ in your inventory?</color>");
                return;
            }
            player.player.TransferMoney(DeltaInv.RemoveFromMe, arg2Int, true);
            currPlayer.TransferMoney(DeltaInv.AddToMe, arg2Int, true);
            player.SendChatMessage($"<color={infoColor}>Successfully transfered</color> <color={argColor}>{arg2Int}</color><color={infoColor}>$ to </color><color={argColor}>{currPlayer.username}</color><color={infoColor}>!</color>");
            currPlayer.svPlayer.SendChatMessage($"<color={argColor}>{player.player.username}</color><color={infoColor}> gave you </color><color={argColor}>{arg2Int}</color><color={infoColor}>$!</color>");
        }
    }
}
