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
    public class GoToWarp
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            if (!Variables.WarpHandler.List.Any(x=>x.Name == arg1))
            {
                player.SendChatMessage($"<color={errorColor}>That warp doesn't exist.</color>");
                return;
            }
            var obj = Variables.WarpHandler.List.FirstOrDefault(x => x.Name == arg1);
            if (!HasPermission.Run(player, obj.ExecutableBy, false, player.player.job.jobIndex))
            {
                player.SendChatMessage($"<color={errorColor}>You do not have access to that warp.</color>");
                return;
            }
            if (obj.Disabled)
            {
                player.SendChatMessage($"<color={errorColor}>That warp is currently disabled.</color>");
                return;
            }
            if (obj.CurrentlyInCooldown.ContainsKey(player.player.username))
            {
                player.SendChatMessage($"<color={errorColor}>You already went to that warp. Please wait</color> <color={argColor}>{obj.CurrentlyInCooldown[player.player.username]}</color> <color={errorColor}>second(s) before executing this command again.</color>");
                return;
            }
            if (obj.Price > 0)
            {
                if (player.player.MyMoneyCount() < obj.Price)
                {
                    player.SendChatMessage($"<color={errorColor}>You do not have enough money to pay for this warp. (You have: {player.player.MyMoneyCount()} | Needed: {obj.Price})</color>");
                    return;
                }
                player.player.TransferMoney(DeltaInv.RemoveFromMe, obj.Price, true);
            }
            player.ResetAndSavePosition(new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z), new Quaternion(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.Rotation.W), obj.Position.PlaceIndex);
            if (obj.Delay > 0)
                SvMan.StartCoroutine(Variables.WarpHandler.StartCooldown(player.player.username, obj));
			player.SendChatMessage($"<color={infoColor}>You've been teleported to the warp named</color> <color={argColor}>{arg1}</color><color={infoColor}>.{(obj.Delay > 0 ? $" You can teleport to this warp again in {obj.Delay} seconds." : "")}</color>");
        }
    }
}
