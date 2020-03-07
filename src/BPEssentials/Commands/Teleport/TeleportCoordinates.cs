using BPEssentials.Abstractions;
using BrokeProtocol.Entities;
using BPEssentials.ExtensionMethods;
using UnityEngine;
using System.Globalization;

namespace BPEssentials.Commands
{
    public class TeleportCoordinates : Command
    {
        public void Invoke(ShPlayer player, float x, float y, float z, int placeIndex = 0)
        {
            if (placeIndex < 0 || placeIndex > (Core.Instance.SvManager.fixedPlaces.Count + Core.Instance.SvManager.apartments.Count))
            {
                player.TS("TeleportCoordinates_invalidPlaceIndex", placeIndex.ToString());
                return;
            }
            player.GetExtendedPlayer().ResetAndSavePosition(new Vector3(x, y, z), player.GetRotation, placeIndex);
            player.TS("TeleportCoordinates", x.ToString(CultureInfo.InvariantCulture), y.ToString(CultureInfo.InvariantCulture), z.ToString(CultureInfo.InvariantCulture), placeIndex.ToString());
        }
    }
}
