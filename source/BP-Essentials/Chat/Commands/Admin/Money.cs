using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Money
    {
        public static void Run(SvPlayer player, string message)
        {
            string CorrSyntax = $"<color={argColor}>" + GetArgument.Run(0, false, false, message) + $"</color><color={errorColor}> [Player] [Amount]</color><color={warningColor}> (Incorrect or missing argument(s).)</color>";
            string arg1 = GetArgument.Run(1, false, true, message);
            string arg2 = message.Split(' ').Last().Trim();
            if (String.IsNullOrEmpty(arg1) || String.IsNullOrEmpty(arg2))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, CorrSyntax);
                return;
            }
            else
            {
                int lastIndex = arg1.LastIndexOf(" ");
                if (lastIndex != -1)
                    arg1 = arg1.Remove(lastIndex).Trim();
            }
            bool isNumeric = int.TryParse(arg2, out int arg2int);
            if (isNumeric)
            {
                bool found = false;
                foreach (var shPlayer in UnityEngine.Object.FindObjectsOfType<ShPlayer>())
                    if (shPlayer.username == arg1 && !shPlayer.svPlayer.IsServerside() || shPlayer.ID.ToString() == arg1 && !shPlayer.svPlayer.IsServerside())
                    {
                        shPlayer.TransferMoney(DeltaInv.AddToMe, arg2int, true);
                        player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Successfully gave</color><color={argColor}> " + shPlayer.username + " " + arg2int + $"</color><color={infoColor}>$</color>");
                        shPlayer.svPlayer.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={argColor}>" + player.playerData.username + $"</color><color={infoColor}> gave you </color><color={argColor}>" + arg2int + $"</color><color={infoColor}>$!</color>");
                        found = true;
                    }
                if (!found)
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, NotFoundOnline);
            }
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, CorrSyntax);
        }
    }
}
