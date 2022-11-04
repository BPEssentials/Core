using System.Linq;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using BrokeProtocol.GameSource.Types;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Networking;

namespace BPEssentials.Commands
{
    public class Jail : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target, float timeInSeconds)
        {
            var jail = LifeManager.jails.GetRandom();
            if (jail == null)
            {
                return;
            }
            if (target.IsDead || target.svPlayer.IsPrisoner())
            {
                return;
            }
            var getPositionT = jail.mainT;
            target.svPlayer.SvSetJob(BPAPI.Jobs[LifeCore.prisonerIndex], true, false);
            target.GetExtendedPlayer().ResetAndSavePosition(getPositionT.position, getPositionT.rotation, getPositionT.parent.GetSiblingIndex());
            target.LifePlayer().ClearCrimes();
            foreach (var i in player.myItems.Values.ToArray())
            {
                if (!i.item.illegal)
                {
                    continue;
                }

                player.TransferItem(DeltaInv.RemoveFromMe, i.item.index, i.count);
            }
            if (LifeManager.pluginPlayers.TryGetValue(player, out var pluginPlayer))
            {
                pluginPlayer.StartJailTimer(timeInSeconds);
            }
            player.TS("player_jail", target.username.CleanerMessage(), timeInSeconds);
            target.TS("target_jail", player.username.CleanerMessage(), timeInSeconds);
        }
    }
}
