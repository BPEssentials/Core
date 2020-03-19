using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class Home : Command
    {
        public void Invoke(ShPlayer player, int homeNumber = 1)
        {

            var apartments = new List<ShApartment>(player.ownedApartments.Keys);
            if (apartments.Count < homeNumber)
            {

                player.TS("you_only_own", apartments.Count.ToString());
                return;
            }

            if (apartments.Count == 0)
            {
                player.TS("no_appartments");
                return;
            }
            var offset = new Vector3(-1, 0, -1);
            var apartment = apartments[Math.Max(0, --homeNumber)];
            if (apartment.GetRotation.y < 0.9)
            {
                offset = new Vector3(1, 0, 2);
            }
            player.GetExtendedPlayer().ResetAndSavePosition(apartment.GetPosition + offset, apartment.GetRotation, apartment.GetPlaceIndex);
        }
    }
}
