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
            var apartment = apartments[Math.Max(0, --homeNumber)];
            player.GetExtendedPlayer().ResetAndSavePosition(apartment.spawnPoint.position, apartment.spawnPoint.rotation, apartment.GetPlaceIndex);
        }
    }
}
