using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Confirm
    {
        public static void Run(SvPlayer player, string message)
        {
            var shPlayer = player.player;
            if (!shPlayer.ownedApartment)
            {
                player.SendChatMessage($"<color={warningColor}>You don't have a apartment to sell!</color>");
                return;
            }
            if (shPlayer.InOwnApartment())
            {
                player.SendChatMessage($"<color={warningColor}>You cannot sell your apartment while you're in it.</color>");
                return;
            }
            player.SendChatMessage($"<color={infoColor}>Selling apartment...</color>");
            SellApartment(shPlayer);
        }
        public static void SellApartment(ShPlayer shPlayer)
        {
            shPlayer.TransferMoney(DeltaInv.AddToMe, shPlayer.ownedApartment.value / 2, true);
            shPlayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ApartmentOwner, new object[]{0,0});
            CleanupApartment.Run(shPlayer);
        }
    }
}