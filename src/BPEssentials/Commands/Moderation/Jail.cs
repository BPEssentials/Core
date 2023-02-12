using System.Linq;
using BPCoreLib.ExtensionMethods;
using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.GameSource;
using BrokeProtocol.GameSource.Types;
using BrokeProtocol.Utility;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class Jail : BpeCommand
    {
        public void Invoke(ShPlayer player, ShPlayer target, float timeInSeconds)
        {
            ServerTrigger jail = LifeManager.jails.GetRandom();
            if (jail == null)
            {
                return;
            }
            if (target.IsDead || target.svPlayer.IsPrisoner())
            {
                return;
            }

            Transform getPositionT = jail.mainT;
            target.svPlayer.SvSetJob(BPAPI.Jobs[LifeCore.prisonerIndex], true, false);
            target.GetExtendedPlayer().ResetAndSavePosition(getPositionT.position, getPositionT.rotation, getPositionT.parent.GetSiblingIndex());
            target.LifePlayer().ClearCrimes();
            foreach (InventoryItem i in player.myItems.Values.ToArray())
            {
                if (!i.item.illegal)
                {
                    continue;
                }

                player.TransferItem(DeltaInv.RemoveFromMe, i.item.index, i.count);
            }
            if (LifeManager.pluginPlayers.TryGetValue(player, out LifeSourcePlayer pluginPlayer))
            {
                pluginPlayer.StartJailTimer(timeInSeconds);
            }

            player.TS("player_jail", target.username.CleanerMessage(), timeInSeconds);
            target.TS("target_jail", player.username.CleanerMessage(), timeInSeconds);
        }
    }
}
