using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BPEssentials.Models;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Back : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;
            LastLocation lastLocation = target.GetExtendedPlayer().LastLocation;
            if (!lastLocation.HasPositionSet())
            {
                player.TS("no_tp_location");
                return;
            }

            target.GetExtendedPlayer().ResetAndSavePosition(lastLocation.Position, lastLocation.Rotation, lastLocation.PlaceIndex);
            target.SendChatMessage("You've been teleported to your last known location.");
        }
    }
}
