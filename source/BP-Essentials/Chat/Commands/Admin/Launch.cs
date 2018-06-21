﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Launch : EssentialsChatPlugin
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (String.IsNullOrEmpty(arg1))
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
            else
            {
                bool playerfound = false;
                foreach (var shPlayer in FindObjectsOfType<ShPlayer>())
                    if (shPlayer.username == arg1 && shPlayer.IsRealPlayer() || shPlayer.ID.ToString() == arg1 && shPlayer.IsRealPlayer())
                    {
                        shPlayer.svPlayer.SvForce(new Vector3(0f, 6500f, 0f));
                        shPlayer.svPlayer.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>Off you go!</color>");
                        player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>You've launched </color><color={argColor}>{shPlayer.username}</color><color={infoColor}> into space!</color>");
                        playerfound = true;
                    }
                if (!playerfound)
                    player.SendToSelf(Channel.Reliable, ClPacket.GameMessage, NotFoundOnline);
            }
        }
    }
}
