using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Jobs;
using BrokeProtocol.Utility.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BPEssentials.Commands
{
    public class Jail : Command
    {
        public void Invoke(ShPlayer player, ShPlayer target, int timeInSeconds)
		{
			ShJail jail = player.manager.jails.FirstOrDefault();
			if (jail)
			{
				if (player.IsDead || player.job is Prisoner)
				{
					return;
				}
				Transform getPositionT = jail.GetPositionT;
				target.svPlayer.SvTrySetJob(JobIndex.Prisoner, true, false);
				target.GetExtendedPlayer().ResetAndSavePosition(getPositionT.position, getPositionT.rotation, jail.GetPlaceIndex);
				target.svPlayer.SvClearCrimes();
				target.svPlayer.player.RemoveItemsJail();
				target.svPlayer.StartCoroutine(target.svPlayer.JailTimer(timeInSeconds));
				target.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowTimer, timeInSeconds);
				player.TS("in_prison", player.username.CleanerMessage(), timeInSeconds.ToString());
			}
		}
    }
}
