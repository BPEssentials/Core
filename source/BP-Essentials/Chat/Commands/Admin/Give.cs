using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BP_Essentials.EssentialsVariablesPlugin;
using static BP_Essentials.EssentialsMethodsPlugin;

namespace BP_Essentials.Commands
{
    class Give
    {
        public static void Run(SvPlayer player, string message)
        {
            string arg1 = GetArgument.Run(1, false, false, message);
            string arg2 = GetArgument.Run(2, false, false, message);
            if (String.IsNullOrEmpty(arg1) || String.IsNullOrEmpty(arg2))
            {
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, ArgRequired);
                return;
            }
            bool Parsed = int.TryParse(arg1, out int arg1int);
            Parsed = int.TryParse(arg2, out int arg2int);
            if (Parsed)
            {
                if (arg1int > 0 && arg1int <= IDs_Items.Length)
                {
                    var shPlayer = player.player;
                    if (arg1.Length > 4)
                        shPlayer.TransferItem(1, arg1int, arg2int, true);
                    else
                        shPlayer.TransferItem(1, IDs_Items[arg1int - 1], arg2int, true);
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={infoColor}>Giving you item ID: </color><color={argColor}>{arg1}</color>");
                }
                else
                    player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Error: The ID must be between 1 and {IDs_Items.Length}.</color>");
            }
            else
                player.Send(SvSendType.Self, Channel.Unsequenced, ClPacket.GameMessage, $"<color={errorColor}>Error: Is that a valid number you provided as argument?</color>");
        }
    }
}
