using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class Home : BpeCommand
    {
        public void Invoke(ShPlayer player, int homeNumber = 1)
        {
            List<ShApartment> apartments = new List<ShApartment>(player.ownedApartments.Keys);
            if (apartments.Count == 0)
            {
                player.TS("no_appartments");
                return;
            }
            if (apartments.Count < homeNumber)
            {
                player.TS("you_only_own", apartments.Count.ToString());
                return;
            }

            ShApartment apartment = apartments[Math.Max(0, homeNumber - 1)];
            player.GetExtendedPlayer().ResetAndSavePosition(apartment.spawnPoint.position, apartment.spawnPoint.rotation, apartment.GetPlaceIndex());
        }
    }
}
