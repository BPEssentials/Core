using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.Variables;
using static BP_Essentials.HookMethods;

namespace BP_Essentials.Commands
{
    class Search
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (string.IsNullOrEmpty(arg1))
            {
                player.SendChatMessage(ArgRequired);
                return;
            }
            var currPlayer = GetShByStr.Run(arg1);
            if (currPlayer == null)
            {
                player.SendChatMessage(NotFoundOnline);
                return;
            }
            if (currPlayer == player.player)
            {
                player.SendChatMessage($"<color={errorColor}>You cannot search yourself.</color>");
                return;
            }
            if (currPlayer.IsDead())
            {
                player.SendChatMessage($"<color={errorColor}>You cannot search this player because he or she is dead.</color>");
                return;
            }
            if (currPlayer.otherEntity)
                currPlayer.svPlayer.SvStopInventory(true);
            currPlayer.viewers.Add(player.player);
            player.player.otherEntity = currPlayer;
            player.Send(SvSendType.Self, Channel.Fragmented, ClPacket.Searching, player.player.otherEntity.ID, player.player.otherEntity.SerializeMyItems());
            if (!currPlayer.svPlayer.serverside && currPlayer.viewers.Count == 1)
                currPlayer.svPlayer.Send(SvSendType.Self, Channel.Reliable, ClPacket.ShowSearchedInventory, new object[] { });
            player.SendChatMessage($"<color={infoColor}>Viewing inventory of</color> <color={argColor}>{currPlayer.username}</color>");
            currPlayer.svPlayer.SendChatMessage(AdminSearchingInv);
        }
    }
}
