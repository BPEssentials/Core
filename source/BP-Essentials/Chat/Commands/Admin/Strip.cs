using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Strip
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            // To be improved
            currPlayer.svPlayer.SvSetWearable(-1626497894);  //NullArmor
            currPlayer.svPlayer.SvSetWearable(673780802);    //NullBack
            currPlayer.svPlayer.SvSetWearable(-1638932793);  //NullBody
            currPlayer.svPlayer.SvSetWearable(1089711634);   //NullFace
            currPlayer.svPlayer.SvSetWearable(2064679354);   //NullFeet
            currPlayer.svPlayer.SvSetWearable(1174688158);   //NullHands
            currPlayer.svPlayer.SvSetWearable(-501996567);   //NullHead
            currPlayer.svPlayer.SvSetWearable(-1191209217);  //NullLegs
            foreach (var item in currPlayer.myItems.Values.ToList())
                if (item.item.GetType() == typeof(ShWearable))
                    currPlayer.TransferItem(DeltaInv.RemoveFromMe, item.item.index, currPlayer.MyItemCount(item.item.index), true);
            currPlayer.svPlayer.SendChatMessage($"<color={argColor}>{player.playerData.username}</color> <color={warningColor}>Removed your clothes.</color>");
            player.SendChatMessage($"<color={infoColor}>Removed </color><color={argColor}>{currPlayer.username}</color><color={infoColor}>'s clothes.</color>");
        }
    }
}
