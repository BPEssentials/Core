using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using System.Globalization;
using BrokeProtocol.Managers;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class TeleportCoordinates : BpeCommand
    {
        public void Invoke(ShPlayer player, float x, float y, float z, int placeIndex = 0)
        {
            if (placeIndex < 0 || placeIndex > (SceneManager.Instance.places.Count + SvManager.Instance.apartments.Count))
            {
                player.TS("TeleportCoordinates_invalidPlaceIndex", placeIndex.ToString());
                return;
            }
            player.GetExtendedPlayer().ResetAndSavePosition(new Vector3(x, y, z), player.GetRotation, placeIndex);
            player.TS("TeleportCoordinates", x.ToString(CultureInfo.InvariantCulture), y.ToString(CultureInfo.InvariantCulture), z.ToString(CultureInfo.InvariantCulture), placeIndex.ToString());
        }
    }
}
