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
        public void Invoke(ShPlayer player, int homeNumber)
        {
            //make the apartments start at 1 (eg. /home 1 would point to apartments[0])
            homeNumber--;
            List<ShApartment> apartments = new List<ShApartment>(player.ownedApartments.Keys);
            try
            {
                player.GetExtendedPlayer().ResetAndSavePosition(apartments[homeNumber].GetPosition + new Vector3(1, 1, 1), apartments[homeNumber].GetRotation, apartments[homeNumber].GetPlaceIndex);
            }
            catch(System.IndexOutOfRangeException e)
            {
                player.TS("You_only_own", apartments.Count.ToString()) ;
            }
                }






    }
}
