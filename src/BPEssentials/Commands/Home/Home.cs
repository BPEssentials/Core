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
            if (apartments.Count == 0)
            {
                player.TS("No_Appartments");
                return;
            }
            else if (apartments.Count < homeNumber || homeNumber < 1)
            {

                player.TS("You_only_own", apartments.Count.ToString());
                return;
            }

            //make the apartments start at 1 (eg. /home 1 would point to apartments[0])
            homeNumber--;
            if (apartments[homeNumber].GetRotation.y <0.9)
            {
                player.GetExtendedPlayer().ResetAndSavePosition(apartments[homeNumber].GetPosition + new Vector3(1, 0, 2), apartments[homeNumber].GetRotation, apartments[homeNumber].GetPlaceIndex);
            }
            else
            {
                player.GetExtendedPlayer().ResetAndSavePosition(apartments[homeNumber].GetPosition + new Vector3(-1, 0, -1), apartments[homeNumber].GetRotation, apartments[homeNumber].GetPlaceIndex);
            }
        }
    }
}
