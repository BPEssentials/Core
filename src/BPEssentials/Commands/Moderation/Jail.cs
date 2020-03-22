using BPEssentials.Abstractions;
using BPEssentials.ExtensionMethods;
using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.Jobs;
using BrokeProtocol.Utility.Networking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace BPEssentials.Commands
{
	public class Jail : Command
	{
		public void Invoke(ShPlayer player, ShPlayer target, float timeInSeconds)
		{
			ShJail jail = target.manager.jails.FirstOrDefault();
			if (jail)
			{
				if (target.IsDead || target.job is Prisoner)
				{
					return;
				}
				Transform getPositionT = jail.GetPositionT;
				target.svPlayer.SvTrySetJob(JobIndex.Prisoner, true, false);
				target.GetExtendedPlayer().ResetAndSavePosition(getPositionT.position, getPositionT.rotation, jail.GetPlaceIndex);
				target.svPlayer.SvClearCrimes();
				target.RemoveItemsJail();
				target.StartCoroutine(target.svPlayer.JailTimer(timeInSeconds));
				target.svMovable.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowTimer, timeInSeconds);
				player.TS("player_jail", player.username.CleanerMessage(), player.username.CleanerMessage(), timeInSeconds.ToString(CultureInfo.InvariantCulture));
				player.TS("target_jail", player.username.CleanerMessage(), player.username.CleanerMessage(), timeInSeconds.ToString(CultureInfo.InvariantCulture));
			}
		}
	}
}