using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;
using System.IO;

namespace BP_Essentials.Commands
{
    public class GetKit
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            if (!Variables.KitsHandler.List.Any(x=>x.Name == arg1))
            {
                player.SendChatMessage($"<color={errorColor}>That kit doesn't exist.</color>");
                return;
            }
            var obj = Variables.KitsHandler.List.FirstOrDefault(x => x.Name == arg1);
            if (!HasPermission.Run(player, obj.ExecutableBy, false, player.player.job.jobIndex))
            {
                player.SendChatMessage($"<color={errorColor}>You do not have access to that kit.</color>");
                return;
            }
            if (obj.Disabled)
            {
                player.SendChatMessage($"<color={errorColor}>That kit is currently disabled.</color>");
                return;
            }
            if (obj.CurrentlyInCooldown.ContainsKey(player.player.username && obj.CurrentlyInCooldown[player.player.username] > 0)
            {
                player.SendChatMessage($"<color={errorColor}>You already used this kit. Please wait</color> <color={argColor}>{obj.CurrentlyInCooldown[player.player.username]}</color> <color={errorColor}>second(s) before executing this command again.</color>");
                return;
            }
            if (obj.Price > 0)
            {
                if (player.player.MyMoneyCount() < obj.Price)
                {
                    player.SendChatMessage($"<color={errorColor}>You do not have enough money to get this kit. (You have: {player.player.MyMoneyCount()} | Needed: {obj.Price})</color>");
                    return;
                }
                player.player.TransferMoney(DeltaInv.RemoveFromMe, obj.Price, true);
            }
            foreach (var item in obj.Items)
            {
                player.player.TransferItem(DeltaInv.AddToMe, item.Id, item.Amount, true);
            }
            if (obj.Delay > 0)
                SvMan.StartCoroutine(Variables.KitsHandler.StartCooldown(player.player.username, obj));
            player.SendChatMessage($"<color={infoColor}>You've been given the kit</color> <color={argColor}>{arg1}</color><color={infoColor}>.{(obj.Delay > 0 ? $" You can get this kit again in {obj.Delay} seconds." : "")}</color>");

        }
    }
}
