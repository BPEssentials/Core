﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Strip : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer, string message)
        {
            try
            {
                var player = (SvPlayer)oPlayer;
                if (AdminsListPlayers.Contains(player.playerData.username) && CmdStripExecutableBy == "admins" || CmdStripExecutableBy == "everyone")
                {
                    string arg1 = GetArgument.Run(1, false, true, message);
                    if (String.IsNullOrEmpty(arg1))
                        player.SendToSelf(Channel.Unsequenced, 10, ArgRequired);
                    else
                    {
                        bool playerfound = false;
                        foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                            if (shPlayer.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1 && shPlayer.IsRealPlayer())
                            {
                                // To be improved
                                shPlayer.svPlayer.SvSetWearable(-1626497894);  //NullArmor
                                shPlayer.svPlayer.SvSetWearable(673780802);    //NullBack
                                shPlayer.svPlayer.SvSetWearable(-1638932793);  //NullBody
                                shPlayer.svPlayer.SvSetWearable(1089711634);   //NullFace
                                shPlayer.svPlayer.SvSetWearable(2064679354);   //NullFeet
                                shPlayer.svPlayer.SvSetWearable(1174688158);   //NullHands
                                shPlayer.svPlayer.SvSetWearable(-501996567);   //NullHead
                                shPlayer.svPlayer.SvSetWearable(-1191209217);  //NullLegs
                                // ---
                                foreach (ShInventoryItem item in shPlayer.myItems.Values.ToList())
                                    if (item.item.GetType() == typeof(ShWearable))
                                        shPlayer.TransferItem(2, item.item.index, shPlayer.MyItemCount(item.item.index), true);
                                shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, 10, $"<color={argColor}>{player.playerData.username}</color> <color={warningColor}>Removed your clothes.</color>");
                                player.SendToSelf(Channel.Unsequenced, 10, $"<color={infoColor}>Removed </color><color={argColor}>{shPlayer.username}</color><color={infoColor}>'s clothes.</color>");
                                playerfound = true;
                            }
                        if (!playerfound)
                            player.SendToSelf(Channel.Reliable, 10, NotFoundOnline);

                    }
                }
                else
                    player.SendToSelf(Channel.Unsequenced, 10, MsgNoPerm);
            }
            catch (Exception ex)
            {
                ErrorLogging.Run(ex);
            }
            return true;
        }
    }
}
