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
            var shPlayer = GetShBySv.Run(player);
            if (shPlayer.ownedApartment)
            {
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Selling apartment...</color>");
                SellApartment(shPlayer);
            }
            else
                player.SendToSelf(Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>You don't have a apartment to sell!</color>");
            return true;
        }
        public static void SellApartment(ShPlayer shPlayer)
        {
            shPlayer.TransferMoney(1, shPlayer.ownedApartment.value / 2, true);
            shPlayer.svPlayer.SendToSelf(Channel.Reliable, ClPacket.ApartmentOwner, new object[]{0,0});
            CleanupApartment.Run(shPlayer);
        }
    }
}