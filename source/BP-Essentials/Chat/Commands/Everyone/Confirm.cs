using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Confirm
    {
        public static bool Run(SvPlayer player)
        {
            var shPlayer = player.player;
            if (shPlayer.ownedApartment)
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Selling apartment...</color>");
                SellApartment(shPlayer);
            }
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>You don't have a apartment to sell!</color>");
            return true;
        }
        public static void SellApartment(ShPlayer shPlayer)
        {
            shPlayer.TransferMoney(DeltaInv.AddToMe, shPlayer.ownedApartment.value / 2, true);
            shPlayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ApartmentOwner, new object[]{0,0});
            CleanupApartment.Run(shPlayer);
        }
    }
}