using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Info
    {
        public static void Run(SvPlayer player, string message)
        {
            var arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.Send(SvSendType.Self, Channel.Reliable, ClPacket.GameMessage, ArgRequired);
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            string[] contentarray = {
                "Username:              " +  currPlayer.username,
                "",
                "",
                "Job:                         " + Jobs[currPlayer.job.jobIndex],
                "Health:                    " + Math.Floor(currPlayer.health),
                "OwnsApartment:   " + (bool)currPlayer.ownedApartment,
                "Position:                 " + currPlayer.GetPosition().ToString(),
                "WantedLevel:         " + currPlayer.wantedLevel,
                "IsAdmin:                 " + currPlayer.admin,
                "BankBalance:         " + currPlayer.svPlayer.bankBalance,
                "ChatEnabled:         " + PlayerList[currPlayer.ID].ChatEnabled,
                "IP:                            " + currPlayer.svPlayer.connection.IP
            };
            var content = string.Join("\r\n", contentarray);
            player.Send(SvSendType.Self, Channel.Reliable, ClPacket.ServerInfo, content);
        }
    }
}
