﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Heal : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message).Trim();
            string msg = $"<color={infoColor}>Healed </color><color={argColor}>{{0}}</color><color={infoColor}>.</color>";
            if (String.IsNullOrEmpty(arg1))
            {
                player.Heal(100);
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, String.Format(msg, "yourself"));
            }
            else
            {
                bool found = false;
                foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                    if (shPlayer.username == arg1 || shPlayer.ID.ToString() == arg1.ToString())
                        if (shPlayer.IsRealPlayer())
                        {
                            shPlayer.svPlayer.Heal(100);
                            player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, String.Format(msg, shPlayer.username));
                            found = true;
                        }
                if (!found)
                    player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
            }
        }
    }
}
