using System;
using System.Collections.Generic;
using static BP_Essentials.EssentialsVariablesPlugin;
namespace BP_Essentials.Commands
{
    public class Clear
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, true, message);
            if (!string.IsNullOrEmpty(arg1))
            {
                var shPlayer = GetShByStr.Run(arg1);
                if (shPlayer == null)
                {
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
                    return;
                }
                var tempList = new List<InventoryItem>();
                foreach (var item in shPlayer.myItems.Values)
                    tempList.Add(item);
                for (int i = 0; i < tempList.Count; i++)
                    shPlayer.TransferItem(2, tempList[i].item.index, shPlayer.MyItemCount(tempList[i].item.index), true);
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>You cleared the inventory of</color> <color={argColor}>{shPlayer.username}</color><color={infoColor}>.</color>");
                shPlayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={warningColor}>Your inventory has been cleared by</color> <color={argColor}>{player.playerData.username}</color><color={warningColor}>.</color>");
            }
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
        }
    }
}