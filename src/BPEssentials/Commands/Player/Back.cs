using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;

namespace BPEssentials.Commands
{
    public class Back : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target)
        {
            var lastlocation = target.GetExtendedPlayer().LastLocation;
            if (!lastlocation.HasPositionSet())
            {
                player.TS("no_tp_location");
                return;
            }
            target.GetExtendedPlayer().ResetAndSavePosition(lastlocation.Position, lastlocation.Rotation, lastlocation.PlaceIndex);
            target.SendChatMessage("You've been teleported to your last known location.");
        }
    }
}
