using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Confirm : EssentialsChatPlugin
    {
        public static bool Run(object oPlayer)
        {
            var player = (SvPlayer)oPlayer;
            foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                if (shPlayer.svPlayer == player && shPlayer.IsRealPlayer())
                    if (shPlayer.ownedApartment)
                    {
                        player.SendToSelf(Channel.Unsequenced, (byte)10, $"<color={infoColor}>Selling apartment...</color>");
                        Confirmed = true;
                        player.SvSellApartment();
                    }
                    else
                        player.SendToSelf(Channel.Unsequenced, (byte)10, $"<color={warningColor}>You don't have a apartment to sell!</color>");
            return true;
        }
    }
}
