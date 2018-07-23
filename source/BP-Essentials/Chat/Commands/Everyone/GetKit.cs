using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;
using System.IO;

namespace BP_Essentials.Commands
{
    public class GetKit : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1.Trim()))
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            if (!listKits.Any(x=>x.Name == arg1))
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>That kit doesn't exist.</color>");
                return;
            }
            var obj = listKits.FirstOrDefault(x => x.Name == arg1);
            if (obj.Disabled)
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>That kit is currently disabled.</color>");
                return;
            }
            if (obj.CurrentlyInCooldown.ContainsKey(player.player.username))
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>You already used this kit. Please wait</color> <color={argColor}>{obj.CurrentlyInCooldown[player.player.username]}</color> <color={errorColor}>second(s) before executing this command again.</color>");
                return;
            }
            foreach (var item in obj.Items)
            {
                player.player.TransferItem(DeltaInv.AddToMe, item.Id, item.Amount, true);
            }
            player.StartCoroutine(Kits.KitCooldown(player, obj));
            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>You've been given the kit</color> <color={argColor}>{arg1}</color><color={infoColor}>.</color>");
        }
    }
}
